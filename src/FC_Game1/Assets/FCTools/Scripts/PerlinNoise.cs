using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class PerlinNoise : MonoBehaviour
	{
		#region inspector

		#endregion

		#region properties

		#endregion

		public float GenerateNoise(Vector3 a_origin, int a_x, int a_z, float a_detailScale)
		{
			float xNoise = (a_x + a_origin.x) / a_detailScale;
			float zNoise = (a_z + a_origin.z) / a_detailScale;
			return Mathf.PerlinNoise(xNoise, zNoise);
		}
	}
}