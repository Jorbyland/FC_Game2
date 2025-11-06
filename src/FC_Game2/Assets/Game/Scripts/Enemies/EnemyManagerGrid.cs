using UnityEngine;
using System.Runtime.InteropServices;

public class EnemyManagerGrid : MonoBehaviour
{
    [Header("Enemies")]
    public int enemyCount = 20000;
    public Mesh enemyMesh;
    public Material enemyMaterial;

    [Header("Compute")]
    public ComputeShader computeShader;
    public float speed = 3f;
    public float repulsionRadius = 1.2f;
    public float repulsionStrength = 4f;
    public float minDistanceForNormalize = 0.001f;

    [Header("Grid")]
    public Vector2 gridOrigin = new Vector2(-100, -100);
    public int gridWidth = 100;
    public int gridHeight = 100;
    public float cellSize = 2f;
    public int maxPerCell = 32; // tune: how many indices stored per cell

    [Header("Rendering")]
    public Transform player;
    public Bounds renderBounds = new Bounds(Vector3.zero, Vector3.one * 1000f);

    ComputeBuffer enemyBuffer;
    ComputeBuffer argsBuffer;
    ComputeBuffer cellCountsBuffer;
    ComputeBuffer cellIndicesBuffer;

    int kernelClear;
    int kernelBuild;
    int kernelMove;

    struct EnemyData
    {
        public Vector3 position;
        public Vector3 velocity;
        public float health;
        public float padding; // align to 16 bytes
    }

    void Start()
    {
        if (computeShader == null) { Debug.LogError("Assign computeShader"); enabled = false; return; }
        if (enemyMesh == null || enemyMaterial == null) { Debug.LogError("Assign mesh & material"); enabled = false; return; }
        if (player == null) { Debug.LogError("Assign player"); enabled = false; return; }

        kernelClear = computeShader.FindKernel("ClearCells");
        kernelBuild = computeShader.FindKernel("BuildGrid");
        kernelMove = computeShader.FindKernel("MoveEnemies");

        // enemy buffer
        enemyBuffer = new ComputeBuffer(enemyCount, Marshal.SizeOf(typeof(EnemyData)));
        var initial = new EnemyData[enemyCount];
        for (int i = 0; i < enemyCount; i++)
        {
            initial[i].position = new Vector3(
                Random.Range(gridOrigin.x, gridOrigin.x + gridWidth * cellSize),
                0f,
                Random.Range(gridOrigin.y, gridOrigin.y + gridHeight * cellSize)
            );
            initial[i].velocity = Vector3.zero;
            initial[i].health = 100f;
            initial[i].padding = 0f;
        }
        enemyBuffer.SetData(initial);

        // grid buffers
        int numCells = gridWidth * gridHeight;
        cellCountsBuffer = new ComputeBuffer(numCells, sizeof(int));
        // cellIndices size = numCells * maxPerCell
        cellIndicesBuffer = new ComputeBuffer(numCells * maxPerCell, sizeof(int));

        // zero counts initially
        int[] zeros = new int[numCells];
        cellCountsBuffer.SetData(zeros);

        // args for instanced draw
        uint[] args = new uint[5] { enemyMesh.GetIndexCount(0), (uint)enemyCount, enemyMesh.GetIndexStart(0), enemyMesh.GetBaseVertex(0), 0 };
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        // bind buffers to compute shader
        computeShader.SetBuffer(kernelClear, "cellCounts", cellCountsBuffer);

        computeShader.SetBuffer(kernelBuild, "enemies", enemyBuffer);
        computeShader.SetBuffer(kernelBuild, "cellCounts", cellCountsBuffer);
        computeShader.SetBuffer(kernelBuild, "cellIndices", cellIndicesBuffer);

        computeShader.SetBuffer(kernelMove, "enemies", enemyBuffer);
        computeShader.SetBuffer(kernelMove, "cellCounts", cellCountsBuffer);
        computeShader.SetBuffer(kernelMove, "cellIndices", cellIndicesBuffer);

        // bind to material for rendering (read-only in shader)
        enemyMaterial.SetBuffer("enemies", enemyBuffer);

        // set static grid params
        computeShader.SetInt("gridOriginX", Mathf.FloorToInt(gridOrigin.x));
        computeShader.SetInt("gridOriginZ", Mathf.FloorToInt(gridOrigin.y));
        computeShader.SetInt("gridWidth", gridWidth);
        computeShader.SetInt("gridHeight", gridHeight);
        computeShader.SetFloat("cellSize", cellSize);
        computeShader.SetInt("maxPerCell", maxPerCell);

        computeShader.SetFloat("minDistanceForNormalize", minDistanceForNormalize);
    }

    void Update()
    {
        // 1) Clear cell counts
        int numCells = gridWidth * gridHeight;
        int clearThreads = Mathf.CeilToInt(numCells / 256.0f);
        computeShader.Dispatch(kernelClear, clearThreads, 1, 1);

        // 2) Build grid (atomic adds)
        int buildThreads = Mathf.CeilToInt(enemyCount / 256.0f);
        computeShader.Dispatch(kernelBuild, buildThreads, 1, 1);

        // 3) Move enemies (read neighbor lists)
        computeShader.SetVector("playerPos", player.position);
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.SetFloat("speed", speed);
        computeShader.SetFloat("repulsionRadius", repulsionRadius);
        computeShader.SetFloat("repulsionStrength", repulsionStrength);

        computeShader.Dispatch(kernelMove, buildThreads, 1, 1);

        // 4) Draw
        Graphics.DrawMeshInstancedIndirect(enemyMesh, 0, enemyMaterial, renderBounds, argsBuffer);
    }

    void OnDestroy()
    {
        if (enemyBuffer != null) enemyBuffer.Release();
        if (cellCountsBuffer != null) cellCountsBuffer.Release();
        if (cellIndicesBuffer != null) cellIndicesBuffer.Release();
        if (argsBuffer != null) argsBuffer.Release();
    }
}