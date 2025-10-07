using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tween
{
	public class Easing : MonoBehaviour
	{
		public static float LinearEaseInOut(float elapsedTime, float min, float max, float duration)
		{
			return max * elapsedTime / duration + min;
		}
	}
}