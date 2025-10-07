using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Menus
{
	public class Menu : MonoBehaviour
	{
		#region inspector
		#endregion

		#region properties

		#endregion

		public virtual void Setup()
		{
		}

		public virtual void Open()
		{
			gameObject.SetActive(true);
		}

		public virtual void Close()
		{
			gameObject.SetActive(false);
		}

		public void CloseImmediate()
		{
			gameObject.SetActive(false);
		}
	}
}