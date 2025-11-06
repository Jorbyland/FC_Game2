using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using System.Runtime.InteropServices;

public class EnemyManager : MonoBehaviour
{
    [Header("Settings")]
    public int enemyCount = 10000;
    public Mesh enemyMesh;
    public Material enemyMaterial;
    public ComputeShader computeShader;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 5f;
    public float repulsionRadius = 1.5f;
    public float repulsionStrength = 5f;
    public Transform player;

    ComputeBuffer enemyBuffer;
    ComputeBuffer argsBuffer;
    Bounds renderBounds;
    int kernelHandle;

    struct EnemyData
    {
        public Vector3 position;
        public Vector3 velocity;
        public float health;
    }

    void Start()
    {
        // Init enemies
        EnemyData[] data = new EnemyData[enemyCount];
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 rand = UnityEngine.Random.insideUnitCircle * 100f;
            data[i].position = new Vector3(rand.x, 0, rand.y);
            data[i].velocity = Vector3.zero;
            data[i].health = 100f;
        }

        enemyBuffer = new ComputeBuffer(enemyCount, Marshal.SizeOf(typeof(EnemyData)));
        enemyBuffer.SetData(data);

        // Set compute shader
        kernelHandle = computeShader.FindKernel("CSMain");
        computeShader.SetBuffer(kernelHandle, "enemies", enemyBuffer);
        computeShader.SetFloat("speed", moveSpeed);
        computeShader.SetFloat("repulsionRadius", repulsionRadius);
        computeShader.SetFloat("repulsionStrength", repulsionStrength);

        // Args buffer for instanced rendering
        uint[] args = new uint[5] { enemyMesh.GetIndexCount(0), (uint)enemyCount, 0, 0, 0 };
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        // Material setup
        enemyMaterial.SetBuffer("enemies", enemyBuffer);
        renderBounds = new Bounds(Vector3.zero, Vector3.one * 500f);
    }

    void Update()
    {
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.SetVector("playerPos", player.position);
        computeShader.Dispatch(kernelHandle, Mathf.CeilToInt(enemyCount / 256f), 1, 1);

        Graphics.DrawMeshInstancedIndirect(enemyMesh, 0, enemyMaterial, renderBounds, argsBuffer);

        HandleCloseEnemies();
    }

    [BurstCompile]
    struct DamageJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> positions;
        public NativeArray<float> healths;
        public Vector3 playerPos;
        public float attackRange;
        public float attackDamage;

        public void Execute(int i)
        {
            float dist = Vector3.Distance(playerPos, positions[i]);
            if (dist < attackRange)
            {
                // Ennemis proches attaquent le joueur
                // (à connecter à ton système de santé)
                Debug.Log("Damage to player from " + i);
            }
        }
    }

    void HandleCloseEnemies()
    {
        var data = new EnemyData[enemyCount];
        enemyBuffer.GetData(data);

        // Exemple simple : affichage feedback sur ennemis proches
        foreach (var e in data)
        {
            float dist = Vector3.Distance(player.position, e.position);
            if (dist < attackRange)
            {
                // Exemple : feedback simple
                Debug.DrawLine(player.position, e.position, Color.red);
            }
        }
    }

    void OnDestroy()
    {
        enemyBuffer?.Release();
        argsBuffer?.Release();
    }
}