using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using FCTools;

namespace Game
{
    public class EnemyManagerGrid : MonoBehaviour
    {
        [Header("Enemies Hybrid Settings")]
        public float m_activationRadius = 10f;
        public float m_deactivationRadius = 15f;
        public GameObject m_enemyPrefab;
        public int m_checkInterval = 10;

        private int m_frameCount = 0;
        private Dictionary<int, GameObject> m_activeEnemies = new Dictionary<int, GameObject>();
        private EnemyData[] m_enemyCache;

        [Header("Enemies")]
        public int m_enemyCount = 20000;
        [Range(0, 500)]
        public int m_maxActiveCPUEnemies = 100;
        public float m_enemyBaseHealth = 1f;

        public Mesh m_enemyMesh;
        public Material m_enemyMaterial;

        [Header("Compute")]
        public ComputeShader m_computeShader;
        public float m_speed = 3f;
        public float m_repulsionRadius = 1.2f;
        public float m_repulsionStrength = 4f;
        public float m_minDistanceForNormalize = 0.001f;

        [Header("Grid")]
        public Vector2 m_gridOrigin = new Vector2(-100, -100);
        public int m_gridWidth = 100;
        public int m_gridHeight = 100;
        public float m_cellSize = 2f;
        public int m_maxPerCell = 32;

        [Header("Rendering")]
        public Transform m_player;
        public Bounds m_renderBounds = new Bounds(Vector3.zero, Vector3.one * 1000f);


        private ObjectPooler m_enemiesPool;


        ComputeBuffer m_enemyBuffer;
        ComputeBuffer m_argsBuffer;
        ComputeBuffer m_cellCountsBuffer;
        ComputeBuffer m_cellIndicesBuffer;

        int m_kernelClear;
        int m_kernelBuild;
        int m_kernelMove;

        // Ensure sequential layout and alignment
        [StructLayout(LayoutKind.Sequential)]
        public struct EnemyData
        {
            public Vector3 position; // 12
            public Vector3 velocity; // 12
            public float health;     // 4
            public float padding;    // 4 -> 32 so far
            public int active;       // 4
            public float direction;        // 4 -> -1 or 1
            public int _pad2;        // 4
            public int _pad3;        // 4 -> total 48 bytes (multiple of 16)
        }

        void Start()
        {
            m_enemiesPool = new ObjectPooler(m_enemyPrefab, m_maxActiveCPUEnemies, transform, 1000);

            if (m_computeShader == null) { Debug.LogError("Assign computeShader"); enabled = false; return; }
            if (m_enemyMesh == null || m_enemyMaterial == null) { Debug.LogError("Assign mesh & material"); enabled = false; return; }
            if (m_player == null) { Debug.LogError("Assign player"); enabled = false; return; }
            if (m_enemyPrefab == null) Debug.LogWarning("enemyPrefab not assigned (CPU instantiation will fail).");

            m_enemyCache = new EnemyData[m_enemyCount];

            m_kernelClear = m_computeShader.FindKernel("ClearCells");
            m_kernelBuild = m_computeShader.FindKernel("BuildGrid");
            m_kernelMove = m_computeShader.FindKernel("MoveEnemies");

            int stride = Marshal.SizeOf(typeof(EnemyData));
            m_enemyBuffer = new ComputeBuffer(m_enemyCount, stride);
            var initial = new EnemyData[m_enemyCount];
            for (int i = 0; i < m_enemyCount; i++)
            {
                initial[i].position = new Vector3(
                    Random.Range(m_gridOrigin.x, m_gridOrigin.x + m_gridWidth * m_cellSize),
                    0f,
                    Random.Range(m_gridOrigin.y, m_gridOrigin.y + m_gridHeight * m_cellSize)
                );
                if (Vector3.Distance(Vector3.zero, initial[i].position) < 10)
                {
                    initial[i].position = new Vector3(99, 0f, 99);
                }
                initial[i].velocity = Vector3.zero;
                initial[i].health = m_enemyBaseHealth;
                initial[i].padding = 0f;
                initial[i].active = 1;
                initial[i].direction = 1;
                initial[i]._pad2 = initial[i]._pad3 = 0;
            }
            m_enemyBuffer.SetData(initial);

            int numCells = m_gridWidth * m_gridHeight;
            m_cellCountsBuffer = new ComputeBuffer(numCells, sizeof(int));
            m_cellIndicesBuffer = new ComputeBuffer(numCells * m_maxPerCell, sizeof(int));

            int[] zeros = new int[numCells];
            m_cellCountsBuffer.SetData(zeros);

            uint[] args = new uint[5] { (uint)m_enemyMesh.GetIndexCount(0), (uint)m_enemyCount, (uint)m_enemyMesh.GetIndexStart(0), (uint)m_enemyMesh.GetBaseVertex(0), 0 };
            m_argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            m_argsBuffer.SetData(args);

            m_computeShader.SetBuffer(m_kernelClear, "cellCounts", m_cellCountsBuffer);

            m_computeShader.SetBuffer(m_kernelBuild, "enemies", m_enemyBuffer);
            m_computeShader.SetBuffer(m_kernelBuild, "cellCounts", m_cellCountsBuffer);
            m_computeShader.SetBuffer(m_kernelBuild, "cellIndices", m_cellIndicesBuffer);

            m_computeShader.SetBuffer(m_kernelMove, "enemies", m_enemyBuffer);
            m_computeShader.SetBuffer(m_kernelMove, "cellCounts", m_cellCountsBuffer);
            m_computeShader.SetBuffer(m_kernelMove, "cellIndices", m_cellIndicesBuffer);

            m_enemyMaterial.SetBuffer("enemies", m_enemyBuffer);
            // Activer le mode buffer pour le matériau GPU
            m_enemyMaterial.EnableKeyword("_USEBUFFER_ON");

            m_computeShader.SetInt("gridOriginX", Mathf.FloorToInt(m_gridOrigin.x));
            m_computeShader.SetInt("gridOriginZ", Mathf.FloorToInt(m_gridOrigin.y));
            m_computeShader.SetInt("gridWidth", m_gridWidth);
            m_computeShader.SetInt("gridHeight", m_gridHeight);
            m_computeShader.SetFloat("cellSize", m_cellSize);
            m_computeShader.SetInt("maxPerCell", m_maxPerCell);

            m_computeShader.SetFloat("minDistanceForNormalize", m_minDistanceForNormalize);

            // optional debug: verify stride
            // Debug.Log("EnemyData stride = " + stride);
        }

        void Update()
        {
            // Limite CPU instance by speed factor
            int currentActive = m_activeEnemies.Count;
            // float t = Mathf.Clamp01((float)currentActive / maxActiveCPUEnemies);
            // float speedFactor = Mathf.Lerp(1f, 0f, t);
            float t = Mathf.Pow(Mathf.Clamp01((float)currentActive / m_maxActiveCPUEnemies), 2f);
            float speedFactor = Mathf.Lerp(1f, 0.2f, t); // ne descend jamais à 0

            // Envoi du facteur dans le compute shader
            m_computeShader.SetFloat("speedFactor", speedFactor);

            // 1) Clear
            int numCells = m_gridWidth * m_gridHeight;
            int clearThreads = Mathf.CeilToInt(numCells / 256.0f);
            m_computeShader.Dispatch(m_kernelClear, clearThreads, 1, 1);

            // 2) Build grid
            int buildThreads = Mathf.CeilToInt(m_enemyCount / 256.0f);
            m_computeShader.Dispatch(m_kernelBuild, buildThreads, 1, 1);

            // 3) Move
            m_computeShader.SetVector("playerPos", m_player.position);
            m_computeShader.SetFloat("deltaTime", Time.deltaTime);
            m_computeShader.SetFloat("speed", m_speed);
            m_computeShader.SetFloat("repulsionRadius", m_repulsionRadius);
            m_computeShader.SetFloat("repulsionStrength", m_repulsionStrength);

            m_computeShader.Dispatch(m_kernelMove, buildThreads, 1, 1);

            // 4) Draw (we draw enemyCount instances; shader will discard inactive ones)
            Graphics.DrawMeshInstancedIndirect(m_enemyMesh, 0, m_enemyMaterial, m_renderBounds, m_argsBuffer);

            // 5) Hybrid sync every N frames
            m_frameCount++;
            if (m_frameCount % m_checkInterval == 0)
            {
                SyncHybridEnemies();
            }
        }
        void LateUpdate()
        {
            if (m_player == null) return;
            
            Vector3 playerPos = m_player.position;
            
            // Mettre à jour le matériau GPU
            if (m_enemyMaterial)
                m_enemyMaterial.SetVector("_PlayerPosition", playerPos);
            
            // Mettre à jour les matériaux des ennemis CPU actifs
            foreach (var kvp in m_activeEnemies)
            {
                if (kvp.Value == null) continue;
                
                MeshRenderer renderer = kvp.Value.GetComponent<MeshRenderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.SetVector("_PlayerPosition", playerPos);
                }
            }
        }

        void OnDestroy()
        {
            if (m_enemyBuffer != null) m_enemyBuffer.Release();
            if (m_cellCountsBuffer != null) m_cellCountsBuffer.Release();
            if (m_cellIndicesBuffer != null) m_cellIndicesBuffer.Release();
            if (m_argsBuffer != null) m_argsBuffer.Release();
        }

        public void DestroyEnemy(int id, GameObject go)
        {
            if (m_activeEnemies.ContainsKey(id))
                m_activeEnemies.Remove(id);
            if (go != null)
                go.gameObject.SetActive(false);

            // mark inactive entry (optional: set active=0 just in case)
            m_enemyCache[id].active = 0;
            m_enemyCache[id]._pad2 = m_enemyCache[id]._pad3 = 0;
            m_enemyBuffer.SetData(m_enemyCache, id, id, 1);
        }

        void SyncHybridEnemies()
        {
            // Read GPU buffer once to sample positions + flags
            m_enemyBuffer.GetData(m_enemyCache);

            Vector3 playerPos = m_player.position;

            // 1) D'abord, désactiver les ennemis CPU trop éloignés
            List<int> toRemove = new List<int>();
            foreach (var kvp in m_activeEnemies)
            {
                int id = kvp.Key;
                GameObject go = kvp.Value;
                if (go == null) { toRemove.Add(id); continue; }

                float dist = Vector3.Distance(go.transform.position, playerPos);
                if (dist > m_deactivationRadius)
                {
                    ReturnEnemyToGPU(id, go);
                    toRemove.Add(id);
                }
                else
                {
                    // push position to GPU for continuity (single-element update)
                    m_enemyCache[id].position = go.transform.position;
                    m_enemyCache[id].velocity = Vector3.zero; // or compute if you want
                    // keep active = 0 while CPU controlled
                    m_enemyBuffer.SetData(m_enemyCache, id, id, 1);
                }
            }

            foreach (var id in toRemove)
                m_activeEnemies.Remove(id);

            // 2) Activer les nouveaux ennemis CPU proches du joueur
            // Si la limite est atteinte, remplacer les ennemis CPU les plus éloignés
            for (int i = 0; i < m_enemyCount; i++)
            {
                if (m_activeEnemies.ContainsKey(i)) continue;
                if (m_enemyCache[i].active == 0) continue; // already disabled on GPU

                float dist = Vector3.Distance(m_enemyCache[i].position, playerPos);
                if (dist < m_activationRadius)
                {
                    if (m_activeEnemies.Count < m_maxActiveCPUEnemies)
                    {
                        // On a de la place, on active directement
                        SpawnEnemyObject(i);
                    }
                    else
                    {
                        // Limite atteinte : trouver l'ennemi CPU le plus éloigné et le remplacer
                        int furthestEnemyId = FindFurthestCPUEnemy(playerPos);
                        if (furthestEnemyId != -1)
                        {
                            float furthestDist = Vector3.Distance(m_activeEnemies[furthestEnemyId].transform.position, playerPos);
                            
                            // Remplacer seulement si le nouvel ennemi est plus proche que le plus éloigné
                            if (dist < furthestDist)
                            {
                                // Retourner l'ennemi le plus éloigné en GPU
                                ReturnEnemyToGPU(furthestEnemyId, m_activeEnemies[furthestEnemyId]);
                                m_activeEnemies.Remove(furthestEnemyId);
                                
                                // Activer le nouvel ennemi
                                SpawnEnemyObject(i);
                            }
                        }
                    }
                }
            }

            // IMPORTANT: do NOT call enemyBuffer.SetData(enemyCache) global here — it will overwrite targeted updates.
        }

        private int FindFurthestCPUEnemy(Vector3 playerPos)
        {
            int furthestId = -1;
            float furthestDist = -1f;

            foreach (var kvp in m_activeEnemies)
            {
                if (kvp.Value == null) continue;

                float dist = Vector3.Distance(kvp.Value.transform.position, playerPos);
                if (dist > furthestDist)
                {
                    furthestDist = dist;
                    furthestId = kvp.Key;
                }
            }

            return furthestId;
        }

        void SpawnEnemyObject(int id)
        {
            Vector3 pos = m_enemyCache[id].position;
            GameObject go = m_enemiesPool.GetItem();
            go.transform.position = pos;
            go.name = "Enemy_" + id;

            // store and link
            var enemy = go.GetComponent<Enemy>();
            enemy.Setup(this, id, m_enemyCache[id], m_player);
            enemy.Init();

            m_activeEnemies[id] = go;

            // mark GPU entry inactive (only touching that single entry)
            m_enemyCache[id].active = 0;
            m_enemyCache[id]._pad2 = m_enemyCache[id]._pad3 = 0;
            // keep position consistent (optional)
            m_enemyCache[id].position = pos;
            m_enemyBuffer.SetData(m_enemyCache, id, id, 1);
        }

        void ReturnEnemyToGPU(int id, GameObject go)
        {
            if (go != null)
            {
                m_enemyCache[id].position = go.transform.position;
                go.SetActive(false);
            }

            m_enemyCache[id].active = 1;
            m_enemyCache[id]._pad2 = m_enemyCache[id]._pad3 = 0;
            m_enemyCache[id].direction = 1;
            // write single entry back
            m_enemyBuffer.SetData(m_enemyCache, id, id, 1);
        }
    }
}