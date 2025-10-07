using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace FCTools
{
	public class HapticManager : MonoBehaviour
	{
		static private HapticManager _HM;
		public enum Type {
			SOFT, 
			LIGHT, 
			MEDIUM, 
			HEAVY, 
			RIGID,
			SUCCESS,
			FAILUR,
			SELECTION
		}

		#region inspector
			[SerializeField] private MMF_Player m_softImpact;
			[SerializeField] private MMF_Player m_lightImpact;
			[SerializeField] private MMF_Player m_mediumImpact;
			[SerializeField] private MMF_Player m_hevyImpact;
			[SerializeField] private MMF_Player m_rigidImpact;
			[SerializeField] private MMF_Player m_successImpact;
			[SerializeField] private MMF_Player m_failurImpact;
			[SerializeField] private MMF_Player m_selectionImpact;
		#endregion

		#region properties

		#endregion

		void Awake(){
			_HM = this;
		}

		public void Start()
		{
				m_softImpact.Initialization();
				m_lightImpact.Initialization();
				m_mediumImpact.Initialization();
				m_hevyImpact.Initialization();
				m_rigidImpact.Initialization();
				m_successImpact.Initialization();
				m_failurImpact.Initialization();
				m_selectionImpact.Initialization();
		}
		static public void Play(Type a_type)
		{
			switch(a_type){
				case Type.SOFT:
				_HM.m_softImpact.PlayFeedbacks();
				break;
				case Type.LIGHT:
				_HM.m_lightImpact.PlayFeedbacks();
				break;
				case Type.MEDIUM:
				_HM.m_mediumImpact.PlayFeedbacks();
				break;
				case Type.HEAVY:
				_HM.m_hevyImpact.PlayFeedbacks();
				break;
				case Type.RIGID:
				_HM.m_rigidImpact.PlayFeedbacks();
				break;
				case Type.SELECTION:
				_HM.m_selectionImpact.PlayFeedbacks();
				break;
				case Type.SUCCESS:
				_HM.m_successImpact.PlayFeedbacks();
				break;
				case Type.FAILUR:
				_HM.m_failurImpact.PlayFeedbacks();
				break;
			}
		}
	}
}