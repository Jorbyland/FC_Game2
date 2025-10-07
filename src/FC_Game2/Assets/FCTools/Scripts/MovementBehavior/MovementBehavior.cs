using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.MovementBehavior
{
	public abstract class MovementBehavior : MonoBehaviour
	{
		protected Vector3[] directions;

		public virtual void Setup(Vector3[] a_directions)
		{
			directions = a_directions;
		}

		public abstract float[] Compute();
	}
}