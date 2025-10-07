using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
	public class Entity_HitboxComponent : MonoBehaviour
	{
		#region inspector
		public Collider Collider => m_collider;
		[SerializeField] private Collider m_collider;
		#endregion

		#region properties
		public Entity Entity => m_entity;
		private Entity m_entity;
		#endregion

		public void Setup(Entity a_entity)
		{
			m_entity = a_entity;
		}
		public void Init()
		{

		}
	}
}