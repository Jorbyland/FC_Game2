using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCTools
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class RoadMeshRenderer2D : MonoBehaviour
	{
		[System.Serializable]
		public class Settings
		{
			public float Width => m_width;
			[SerializeField] private float m_width;

			public Material RoadMaterial => m_roadMaterial;
			[SerializeField] private Material m_roadMaterial;
		}


		#region properties
		private Settings m_settings;
		private Mesh m_mesh;
		private MeshFilter m_meshFilter;
		private MeshRenderer m_meshRenderer;

		public Vector3[] Positions => m_positions;
		private Vector3[] m_positions;
		private Dictionary<int, Vector3[]> m_chunkPositions = new Dictionary<int, Vector3[]>();
		#endregion


		// Test 
		public Settings settings;
		public Vector3[] positions;

		void Start()
		{
			Setup(settings);
			AddPositions(0, positions.ToList());
		}
		// EndTest


		public void Setup(Settings a_settings)
		{
			m_settings = a_settings;
			m_meshRenderer = GetComponent<MeshRenderer>();
			m_meshRenderer.material = a_settings.RoadMaterial;
			m_meshFilter = GetComponent<MeshFilter>();
		}

		public void AddPositions(int a_chunkId, List<Vector3> a_positions)
		{
			if (a_positions.Count < 2) return;
			a_positions.RemoveAt(a_positions.Count - 1);
			if (!m_chunkPositions.ContainsKey(a_chunkId))
			{
				m_chunkPositions.Add(a_chunkId, a_positions.ToArray());
				UpdatePositions();
				UpdateMesh(m_positions);
			}
		}
		public void RemoveChunk(int a_chunkId)
		{
			if (m_chunkPositions.ContainsKey(a_chunkId))
			{
				m_chunkPositions.Remove(a_chunkId);
				UpdatePositions();
				UpdateMesh(m_positions);
			}
		}


		public void UpdateMesh(Vector3[] a_positions)
		{
			if (a_positions.Length < 2) return;
			m_mesh = new Mesh();

			m_mesh.vertices = CalculVertices(a_positions);
			m_mesh.triangles = CalculTriangles(m_mesh.vertices);
			m_mesh.normals = CalculNormals(m_mesh.vertices.Length);
			m_mesh.uv = CalculUVs(m_mesh.vertices);

			m_meshFilter.mesh = m_mesh;
		}

		private void UpdatePositions()
		{
			List<Vector3> newPositions = new List<Vector3>();
			foreach (var item in m_chunkPositions)
			{
				newPositions.AddRange(item.Value);
			}
			// newPositions = newPositions.OrderBy(v => v.z).ToList();
			m_positions = newPositions.ToArray();
		}
		private Vector3[] CalculVertices(Vector3[] a_positions)
		{
			Vector3 nextDir = Vector3.zero;
			Vector3 lastDir = Vector3.zero;
			Vector3 nextLeft = Vector3.zero;
			Vector3 lastLeft = Vector3.zero;
			List<Vector3> vertices = new List<Vector3>();
			for (int i = 0; i < a_positions.Length; i++)
			{
				int type = -1;
				if (i < a_positions.Length - 1)
				{
					nextDir = Vector3.Normalize(a_positions[i + 1] - a_positions[i]);
					nextLeft = Vector3.Cross(nextDir, Vector3.up).normalized;
					type = 0;
				}
				if (i > 0)
				{
					lastDir = Vector3.Normalize(a_positions[i - 1] - a_positions[i]);
					lastLeft = -Vector3.Cross(lastDir, Vector3.up).normalized;
					type = type == 0 ? 1 : 2;

				}
				switch (type)
				{
					case 0:
						vertices.Add(a_positions[i] + (nextLeft * m_settings.Width));
						vertices.Add(a_positions[i] + (-nextLeft * m_settings.Width));
						break;
					case 1:
						vertices.Add(a_positions[i] + Vector3.Lerp(nextLeft * m_settings.Width, lastLeft * m_settings.Width, 0.5f));
						vertices.Add(a_positions[i] + Vector3.Lerp(-nextLeft * m_settings.Width, -lastLeft * m_settings.Width, 0.5f));
						break;
					case 2:
						vertices.Add(a_positions[i] + (lastLeft * m_settings.Width));
						vertices.Add(a_positions[i] + (-lastLeft * m_settings.Width));
						break;
				}
			}
			return vertices.ToArray();
		}
		private int[] CalculTriangles(Vector3[] a_vertices)
		{
			List<int> triangles = new List<int>();
			int total = (int)(a_vertices.Length / 2f - 1);
			int index = 0;
			for (int i = 0; i < total; i++)
			{
				triangles.Add(index);
				triangles.Add(index + 2);
				triangles.Add(index + 1);
				triangles.Add(index + 1);
				triangles.Add(index + 2);
				triangles.Add(index + 3);
				index += 2;
			}
			return triangles.ToArray();
		}
		private Vector3[] CalculNormals(int a_verticeCount)
		{
			Vector3[] normals = new Vector3[a_verticeCount];
			for (int i = 0; i < a_verticeCount; i++)
			{
				normals[i] = Vector3.up;
			}
			return normals;
		}
		private Vector2[] CalculUVs(Vector3[] a_vertices)
		{
			Vector2[] uvs = new Vector2[a_vertices.Length];

			for (int i = 0; i < uvs.Length; i++)
			{
				uvs[i] = new Vector2(a_vertices[i].x, a_vertices[i].z);
			}
			return uvs;
		}

	}
}