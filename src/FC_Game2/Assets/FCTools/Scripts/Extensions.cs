using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public static class Extensions
	{
		#region Vector2
		public static Vector2 SetX(this Vector2 vector, float x)
		{
			return new Vector2(x, vector.y);
		}
		public static Vector2 SetY(this Vector2 vector, float y)
		{
			return new Vector2(vector.x, y);
		}
		public static Vector2 GetClosestVector2From(this Vector2 vector, Vector2[] otherVectors)
		{
			if (otherVectors.Length == 0) throw new Exception("The list of other vectors is empty");
			var minDistance = Vector2.Distance(vector, otherVectors[0]);
			var minVector = otherVectors[0];
			for (var i = otherVectors.Length - 1; i > 0; i--)
			{
				var newDistance = Vector2.Distance(vector, otherVectors[i]);
				if (newDistance < minDistance)
				{
					minDistance = newDistance;
					minVector = otherVectors[i];
				}
			}
			return minVector;
		}
		#endregion

		#region Vector3
		public static Vector3 SetX(this Vector3 vector, float x)
		{
			return new Vector3(x, vector.y, vector.z);
		}
		public static Vector3 SetY(this Vector3 vector, float y)
		{
			return new Vector3(vector.x, y, vector.z);
		}
		public static Vector3 SetZ(this Vector3 vector, float z)
		{
			return new Vector3(vector.x, vector.y, z);
		}
		public static bool IsCoordNeighbor(this Vector3Int a_coord, Vector3Int a_other, int a_range)
		{
			return a_other.x >= a_coord.x - a_range
					&& a_other.x <= a_coord.x + a_range
					&& a_other.y >= a_coord.y - a_range
					&& a_other.y <= a_coord.y + a_range;
		}
		public static T GetClosestElement<T>(this Vector3 fromPos, T[] others, float a_minDist) where T : MonoBehaviour
		{
			if (others == null || others.Length == 0) return null;
			var minDistance = Vector3.Distance(fromPos, others[0].transform.position);
			T result = others[0];
			for (var i = others.Length - 1; i > 0; i--)
			{
				var newDistance = Vector3.Distance(fromPos, others[i].transform.position);
				if (newDistance < minDistance && newDistance > a_minDist)
				{
					minDistance = newDistance;
					result = others[i];
				}
			}
			return result;
		}
		public static GameObject GetClosestGameObject(this Vector3 fromPos, GameObject[] others, float a_minDist)
		{
			if (others == null || others.Length == 0) return null;
			var minDistance = Vector3.Distance(fromPos, others[0].transform.position);
			GameObject result = others[0];
			for (var i = others.Length - 1; i > 0; i--)
			{
				var newDistance = Vector3.Distance(fromPos, others[i].transform.position);
				if (newDistance < minDistance && newDistance > a_minDist)
				{
					minDistance = newDistance;
					result = others[i];
				}
			}
			return result;
		}
		public static Vector3 GetClosestPosition(this Vector3 fromPos, Vector3[] others, float a_minDist)
		{
			if (others == null || others.Length == 0) return Vector3.zero;
			var minDistance = Vector3.Distance(fromPos, others[0]);
			Vector3 result = others[0];
			for (var i = others.Length - 1; i > 0; i--)
			{
				var newDistance = Vector3.Distance(fromPos, others[i]);
				if (newDistance < minDistance && newDistance > a_minDist)
				{
					minDistance = newDistance;
					result = others[i];
				}
			}
			return result;
		}
		#endregion

		#region GameObject
		public static Collider GetClosestCollider(this GameObject go, Collider[] others)
		{
			if (others == null || others.Length == 0) return null;
			var minDistance = Vector3.Distance(go.transform.position, others[0].transform.position);
			Collider result = others[0];
			for (var i = others.Length - 1; i > 0; i--)
			{
				var newDistance = Vector3.Distance(go.transform.position, others[i].transform.position);
				if (newDistance < minDistance)
				{
					minDistance = newDistance;
					result = others[i];
				}
			}
			return result;
		}
		public static T GetClosestGameObject<T>(this GameObject go, T[] others) where T : MonoBehaviour
		{
			if (others == null || others.Length == 0) return null;
			var minDistance = Vector3.Distance(go.transform.position, others[0].transform.position);
			T result = others[0];
			for (var i = others.Length - 1; i > 0; i--)
			{
				var newDistance = Vector3.Distance(go.transform.position, others[i].transform.position);
				if (newDistance < minDistance)
				{
					minDistance = newDistance;
					result = others[i];
				}
			}
			return result;
		}
		public static T GetClosestGameObject<T>(this GameObject go, T[] others, float a_minDist) where T : MonoBehaviour
		{
			if (others == null || others.Length == 0) return null;
			var minDistance = Vector3.Distance(go.transform.position, others[0].transform.position);
			T result = others[0];
			for (var i = others.Length - 1; i > 0; i--)
			{
				var newDistance = Vector3.Distance(go.transform.position, others[i].transform.position);
				if (newDistance < minDistance && newDistance > a_minDist)
				{
					minDistance = newDistance;
					result = others[i];
				}
			}
			return result;
		}

		#endregion

		#region Time
		// public static string ToRemainingTime(this TimeSpan a_timeSpan)
		// {
		// 	if (a_timeSpan.Hours > 0)
		// 	{
		// 		return a_timeSpan.ToString(@"%h\h\ mm\m\ ss\s");
		// 	}
		// 	else if (a_timeSpan.Minutes > 0)
		// 	{
		// 		return a_timeSpan.ToString(@"%m\m\ ss\s");
		// 	}
		// 	else if (a_timeSpan.Seconds >= 10)
		// 	{
		// 		return a_timeSpan.ToString(@"%s\s");
		// 	}
		// 	else
		// 	{
		// 		return a_timeSpan.ToString(@"%s\.%f\s");
		// 	}
		// }
		public static string ToRemainingTime(this TimeSpan a_timeSpan)
		{
			if (a_timeSpan.Days > 0)
			{
				return a_timeSpan.ToString(@"%d\d\ %h\h");
				// return a_timeSpan.ToString(@"%d\d\ %h\h\ mm\m");
			}
			else if (a_timeSpan.Hours > 0)
			{
				return a_timeSpan.ToString(@"%h\h\ mm\m");
				// return a_timeSpan.ToString(@"%h\h\ mm\m\ ss\s");
			}
			else if (a_timeSpan.Minutes > 0)
			{
				return a_timeSpan.ToString(@"%m\m\ ss\s");
			}
			else if (a_timeSpan.Seconds >= 10)
			{
				return a_timeSpan.ToString(@"%s\s");
			}
			else
			{
				return a_timeSpan.ToString(@"%s\.%f\s");
			}
		}
		#endregion
		#region Formated Numbers
		public static string ToUINumber(this float a_value)
		{
			if (a_value < 1)
			{
				return a_value.ToString("0.###");
			}
			else if (a_value < 10)
			{
				return a_value.ToString("#.##");
			}
			else if (a_value < 100)
			{
				return a_value.ToString("#.#");
			}
			else if (a_value < 1000)
			{
				return a_value.ToString("F0");
			}
			else if (a_value < 10000)
			{
				return $"{(System.Math.Truncate(a_value / 1000f * 100) / 100).ToString("#.##")}K";
			}
			else if (a_value < 100000)
			{
				return $"{(System.Math.Truncate(a_value / 1000f * 10) / 10).ToString("#.#")}K";
				// return $"{(a_value/1000f).ToString("#.#")}K";
			}
			else if (a_value < 1000000)
			{
				return $"{System.Math.Truncate(a_value / 1000f).ToString("F0")}K";
				// return $"{(a_value/1000f).ToString("F0")}K";
			}
			else if (a_value < 10000000)
			{
				return $"{(System.Math.Truncate(a_value / 1000000f * 100) / 100).ToString("#.##")}M";
				// return $"{(a_value/1000000f).ToString("#.##")}M";
			}
			else if (a_value < 100000000)
			{
				return $"{(System.Math.Truncate(a_value / 1000000f * 1000000) / 1000000).ToString("#.#")}M";
				// return $"{(a_value/1000000f).ToString("#.#")}M";
			}
			else if (a_value < 1000000000)
			{
				return $"{System.Math.Truncate(a_value / 1000000f).ToString("F0")}M";
				// return $"{(a_value/1000000f).ToString("F0")}M";
			}
			else if (a_value < 10000000000)
			{
				return $"{(System.Math.Truncate(a_value / 1000000000f * 1000000) / 1000000).ToString("#.##")}B";
				// return $"{(a_value/1000000000f).ToString("#.##")}B";
			}
			else if (a_value < 100000000000)
			{
				return $"{(System.Math.Truncate(a_value / 1000000000f * 1000000) / 1000000).ToString("#.#")}B";
				// return $"{(a_value/1000000000f).ToString("#.#")}B";
			}
			else if (a_value < 1000000000000)
			{
				return $"{System.Math.Truncate(a_value / 1000000000f).ToString("F0")}B";
				// return $"{(a_value/1000000000f).ToString("F0")}B";
			}
			else
			{
				return a_value.ToString("#.#");
			}
		}
		public static string ToUINumber(this int a_value)
		{
			if (a_value < 1000)
			{
				return a_value.ToString("F0");
			}
			else if (a_value < 10000)
			{
				return $"{(System.Math.Truncate(a_value / 1000f * 100) / 100).ToString("#.##")}K";
			}
			else if (a_value < 100000)
			{
				return $"{(System.Math.Truncate(a_value / 1000f * 10) / 10).ToString("#.#")}K";
				// return $"{(a_value/1000f).ToString("#.#")}K";
			}
			else if (a_value < 1000000)
			{
				return $"{System.Math.Truncate(a_value / 1000f).ToString("F0")}K";
				// return $"{(a_value/1000f).ToString("F0")}K";
			}
			else if (a_value < 10000000)
			{
				return $"{(System.Math.Truncate(a_value / 1000000f * 100) / 100).ToString("#.##")}M";
				// return $"{(a_value/1000000f).ToString("#.##")}M";
			}
			else if (a_value < 100000000)
			{
				return $"{(System.Math.Truncate(a_value / 1000000f * 1000000) / 1000000).ToString("#.#")}M";
				// return $"{(a_value/1000000f).ToString("#.#")}M";
			}
			else if (a_value < 1000000000)
			{
				return $"{System.Math.Truncate(a_value / 1000000f).ToString("F0")}M";
				// return $"{(a_value/1000000f).ToString("F0")}M";
			}
			else
			{
				return a_value.ToString("#.#");
			}
		}
		#endregion

		#region Textures
		public static Texture2D Circle(this Texture2D tex, int x, int y, int r, Color color, float factor)
		{
			float rSquared = r * r;
			int minCordX = Mathf.Clamp(x - r, 0, tex.width);
			int maxCordX = Mathf.Clamp(x + r, 0, tex.width);
			int minCordY = Mathf.Clamp(y - r, 0, tex.height);
			int maxCordY = Mathf.Clamp(y + r, 0, tex.height);
			float lerp = 0;
			Vector2 center = new Vector2(x, y);

			for (int u = minCordX; u < maxCordX; u++)
			{
				for (int v = minCordY; v < maxCordY; v++)
				{
					if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
					{
						float distance = Vector2.Distance(center, new Vector2(u, v));
						lerp = (distance - factor) / (r - factor);
						Color lerpColor = Color.Lerp(color, tex.GetPixel(u, v), lerp);
						tex.SetPixel(u, v, lerpColor);
					}
					// else
					// {
					// 	tex.SetPixel(u, v, Color.black);
					// }
				}
			}

			return tex;
		}
		#endregion

		public static void Shuffle<T>(this List<T> ts)
		{
			var count = ts.Count;
			var last = count - 1;
			for (var i = 0; i < last; ++i)
			{
				var r = UnityEngine.Random.Range(i, count);
				var tmp = ts[i];
				ts[i] = ts[r];
				ts[r] = tmp;
			}
		}

		public static List<GameObject> DetectGameObjects(this Transform a_transform, LayerMask a_layerMask, float a_range)
		{
			Collider[] colliders = Physics.OverlapSphere(a_transform.position, a_range, a_layerMask);
			if (colliders.Length > 0)
			{
				List<GameObject> result = new List<GameObject>();
				for (int i = 0; i < colliders.Length; i++)
				{
					result.Add(colliders[i].gameObject);
				}
				return result;
			}
			return null;
		}
		public static List<GameObject> DetectGameObjects(this Vector3 a_position, LayerMask a_layerMask, float a_range)
		{
			List<GameObject> result = new List<GameObject>();
			Collider[] colliders = Physics.OverlapSphere(a_position, a_range, a_layerMask);
			if (colliders.Length > 0)
			{
				for (int i = 0; i < colliders.Length; i++)
				{
					result.Add(colliders[i].gameObject);
				}
				return result;
			}
			return result;
		}

		public static RaycastHit RaycastFromCamera(this Camera a_camera, Vector3 a_position, float a_maxDistance, LayerMask a_layerMask)
		{
			RaycastHit hit;
			Ray ray = a_camera.ScreenPointToRay(a_position);

			if (Physics.Raycast(ray, out hit, a_maxDistance, a_layerMask))
			{
				return hit;
			}
			return hit;
		}

		public static bool IsInBound(this Vector3 a_position, Vector3 a_boundPos, Vector2 a_bound)
		{
			return a_position.x > a_boundPos.x - a_bound.x && a_position.x < a_boundPos.x + a_bound.x
				&& a_position.z > a_boundPos.z - a_bound.y && a_position.z < a_boundPos.z + a_bound.y;
		}

	}
}