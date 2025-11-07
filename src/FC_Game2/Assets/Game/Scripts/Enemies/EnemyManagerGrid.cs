using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Game
{
    public class EnemyManagerGrid : MonoBehaviour
    {
        [Header("Enemies Hybrid Settings")]
        public float activationRadius = 10f;
        public float deactivationRadius = 15f;
        public GameObject enemyPrefab;
        public int checkInterval = 10;

        private int frameCount = 0;
        private Dictionary<int, GameObject> activeEnemies = new Dictionary<int, GameObject>();
        private EnemyData[] enemyCache;

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
        public int maxPerCell = 32;

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

        // Ensure sequential layout and alignment
        [StructLayout(LayoutKind.Sequential)]
        public struct EnemyData
        {
            public Vector3 position; // 12
            public Vector3 velocity; // 12
            public float health;     // 4
            public float padding;    // 4 -> 32 so far
            public int active;       // 4
            public int _pad1;        // 4
            public int _pad2;        // 4
            public int _pad3;        // 4 -> total 48 bytes (multiple of 16)
        }

        void Start()
        {
            if (computeShader == null) { Debug.LogError("Assign computeShader"); enabled = false; return; }
            if (enemyMesh == null || enemyMaterial == null) { Debug.LogError("Assign mesh & material"); enabled = false; return; }
            if (player == null) { Debug.LogError("Assign player"); enabled = false; return; }
            if (enemyPrefab == null) Debug.LogWarning("enemyPrefab not assigned (CPU instantiation will fail).");

            enemyCache = new EnemyData[enemyCount];

            kernelClear = computeShader.FindKernel("ClearCells");
            kernelBuild = computeShader.FindKernel("BuildGrid");
            kernelMove = computeShader.FindKernel("MoveEnemies");

            int stride = Marshal.SizeOf(typeof(EnemyData));
            enemyBuffer = new ComputeBuffer(enemyCount, stride);
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
                initial[i].active = 1;
                initial[i]._pad1 = initial[i]._pad2 = initial[i]._pad3 = 0;
            }
            enemyBuffer.SetData(initial);

            int numCells = gridWidth * gridHeight;
            cellCountsBuffer = new ComputeBuffer(numCells, sizeof(int));
            cellIndicesBuffer = new ComputeBuffer(numCells * maxPerCell, sizeof(int));

            int[] zeros = new int[numCells];
            cellCountsBuffer.SetData(zeros);

            uint[] args = new uint[5] { (uint)enemyMesh.GetIndexCount(0), (uint)enemyCount, (uint)enemyMesh.GetIndexStart(0), (uint)enemyMesh.GetBaseVertex(0), 0 };
            argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            argsBuffer.SetData(args);

            computeShader.SetBuffer(kernelClear, "cellCounts", cellCountsBuffer);

            computeShader.SetBuffer(kernelBuild, "enemies", enemyBuffer);
            computeShader.SetBuffer(kernelBuild, "cellCounts", cellCountsBuffer);
            computeShader.SetBuffer(kernelBuild, "cellIndices", cellIndicesBuffer);

            computeShader.SetBuffer(kernelMove, "enemies", enemyBuffer);
            computeShader.SetBuffer(kernelMove, "cellCounts", cellCountsBuffer);
            computeShader.SetBuffer(kernelMove, "cellIndices", cellIndicesBuffer);

            enemyMaterial.SetBuffer("enemies", enemyBuffer);

            computeShader.SetInt("gridOriginX", Mathf.FloorToInt(gridOrigin.x));
            computeShader.SetInt("gridOriginZ", Mathf.FloorToInt(gridOrigin.y));
            computeShader.SetInt("gridWidth", gridWidth);
            computeShader.SetInt("gridHeight", gridHeight);
            computeShader.SetFloat("cellSize", cellSize);
            computeShader.SetInt("maxPerCell", maxPerCell);

            computeShader.SetFloat("minDistanceForNormalize", minDistanceForNormalize);

            // optional debug: verify stride
            // Debug.Log("EnemyData stride = " + stride);
        }

        void Update()
        {
            // 1) Clear
            int numCells = gridWidth * gridHeight;
            int clearThreads = Mathf.CeilToInt(numCells / 256.0f);
            computeShader.Dispatch(kernelClear, clearThreads, 1, 1);

            // 2) Build grid
            int buildThreads = Mathf.CeilToInt(enemyCount / 256.0f);
            computeShader.Dispatch(kernelBuild, buildThreads, 1, 1);

            // 3) Move
            computeShader.SetVector("playerPos", player.position);
            computeShader.SetFloat("deltaTime", Time.deltaTime);
            computeShader.SetFloat("speed", speed);
            computeShader.SetFloat("repulsionRadius", repulsionRadius);
            computeShader.SetFloat("repulsionStrength", repulsionStrength);

            computeShader.Dispatch(kernelMove, buildThreads, 1, 1);

            // 4) Draw (we draw enemyCount instances; shader will discard inactive ones)
            Graphics.DrawMeshInstancedIndirect(enemyMesh, 0, enemyMaterial, renderBounds, argsBuffer);

            // 5) Hybrid sync every N frames
            frameCount++;
            if (frameCount % checkInterval == 0)
            {
                SyncHybridEnemies();
            }
        }

        void OnDestroy()
        {
            if (enemyBuffer != null) enemyBuffer.Release();
            if (cellCountsBuffer != null) cellCountsBuffer.Release();
            if (cellIndicesBuffer != null) cellIndicesBuffer.Release();
            if (argsBuffer != null) argsBuffer.Release();
        }

        public void DestroyEnemy(int id, GameObject go)
        {
            if (activeEnemies.ContainsKey(id))
                activeEnemies.Remove(id);
            if (go != null) Destroy(go);

            // mark inactive entry (optional: set active=0 just in case)
            enemyCache[id].active = 0;
            enemyCache[id]._pad1 = enemyCache[id]._pad2 = enemyCache[id]._pad3 = 0;
            enemyBuffer.SetData(enemyCache, id, id, 1);
        }

        void SyncHybridEnemies()
        {
            // Read GPU buffer once to sample positions + flags
            enemyBuffer.GetData(enemyCache);

            Vector3 playerPos = player.position;

            // 1) Activate CPU for entries near player
            for (int i = 0; i < enemyCount; i++)
            {
                if (activeEnemies.ContainsKey(i)) continue;
                if (enemyCache[i].active == 0) continue; // already disabled on GPU

                float dist = Vector3.Distance(enemyCache[i].position, playerPos);
                if (dist < activationRadius)
                {
                    SpawnEnemyObject(i);
                }
            }

            // 2) For each active CPU enemy: update its position into the GPU buffer (so the GPU has continuity)
            // and check if it should be returned to GPU
            List<int> toRemove = new List<int>();
            foreach (var kvp in activeEnemies)
            {
                int id = kvp.Key;
                GameObject go = kvp.Value;
                if (go == null) { toRemove.Add(id); continue; }

                float dist = Vector3.Distance(go.transform.position, playerPos);
                if (dist > deactivationRadius)
                {
                    ReturnEnemyToGPU(id, go);
                    toRemove.Add(id);
                }
                else
                {
                    // push position to GPU for continuity (single-element update)
                    enemyCache[id].position = go.transform.position;
                    enemyCache[id].velocity = Vector3.zero; // or compute if you want
                    // keep active = 0 while CPU controlled
                    enemyBuffer.SetData(enemyCache, id, id, 1);
                }
            }

            foreach (var id in toRemove)
                activeEnemies.Remove(id);

            // IMPORTANT: do NOT call enemyBuffer.SetData(enemyCache) global here — it will overwrite targeted updates.
        }

        void SpawnEnemyObject(int id)
        {
            Vector3 pos = enemyCache[id].position;
            GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
            go.name = "Enemy_" + id;

            // store and link
            var bridge = go.GetComponent<EnemyHybridBridge>();
            if (bridge == null)
                bridge = go.AddComponent<EnemyHybridBridge>();
            bridge.manager = this;
            bridge.id = id;

            activeEnemies[id] = go;

            // mark GPU entry inactive (only touching that single entry)
            enemyCache[id].active = 0;
            enemyCache[id]._pad1 = enemyCache[id]._pad2 = enemyCache[id]._pad3 = 0;
            // keep position consistent (optional)
            enemyCache[id].position = pos;
            enemyBuffer.SetData(enemyCache, id, id, 1);
        }

        void ReturnEnemyToGPU(int id, GameObject go)
        {
            if (go != null)
            {
                enemyCache[id].position = go.transform.position;
                Destroy(go);
            }

            enemyCache[id].active = 1;
            enemyCache[id]._pad1 = enemyCache[id]._pad2 = enemyCache[id]._pad3 = 0;
            // write single entry back
            enemyBuffer.SetData(enemyCache, id, id, 1);
        }
    }
}