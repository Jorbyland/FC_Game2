using System.Collections;
using System.Collections.Generic;
using RivalVillages;
using RumbleGuardian;
using UnityEngine;

namespace FCTools.Tutorial
{
	public class Tutorial_FunctionsComponent : MonoBehaviour
	{
		#region inspector

		#endregion
		#region properties
		private Tutorial m_tutorial;
		// private TutorialFunction_LoadGame m_loadGame;
		// private TutorialFunction_MessageWithDelay m_welcome;
		// private TutorialFunction_ShowVillage m_showPlayerVillage;
		// private TutorialFunction_ShowResources m_showPlayerResources;
		// private TutorialFunction_ClicToContinue m_explainResources;
		// private TutorialFunction_ClicToContinue m_letItWork;
		// private TutorialFunction_WaitNewBuidling m_waitNewBuilding;
		// private TutorialFunction_EnableBottomContent m_displayBottomContent;
		// private TutorialFunction_DragAndDrop m_dragAndDropResource;
		// private TutorialFunction_EnableXP m_displayXPBar;
		// private TutorialFunction_LockGameAndClicOk m_gainXP;
		// private TutorialFunction_LockGameAndClicOk m_tryToFillTheXPBar;
		// private TutorialFunction_RefreshPlayerVillagersMission m_refreshPlayerVillagersMission;
		// private TutorialFunction_WaitPowerUp m_waitPowerUp;
		// private TutorialFunction_WaitDelay m_waitDelayAfterPowerUp;
		// private TutorialFunction_EnableProgressionBar m_showProgressionBar;
		// private TutorialFunction_LockGameAndClicOk m_explainProgression;
		// private TutorialFunction_EnableTimer m_showTimer;
		// private TutorialFunction_LockGameAndClicOk m_completeTheLevel;
		// private TutorialFunction_WaitGameOver m_onTimeOut;
		// private TutorialFunction_WaitMainMenu m_waitMainMenu;
		// private TutorialFunction_ClicTechTree m_clicTechTree;
		// private TutorialFunction_ClicFireAbility m_clicFireAbility;

		// private TutorialFunction_WaitToMove m_waitToMove;
		// private TutorialFunction_MoveToBuilding m_moveToBuilding;
		// private TutorialFunction_BuildBuilding m_buildBuilding;
		// private TutorialFunction_GoToFight m_goToFight;
		// private TutorialFunction_WaitEndOfGame m_waitEndOfGame;
		// private TutorialFunction_GenerateGold m_generateGold;

		#endregion

		public void Setup(Tutorial a_tutorial)
		{
			m_tutorial = a_tutorial;
			// m_loadGame = gameObject.AddComponent<TutorialFunction_LoadGame>();
			// m_loadGame.Setup();
			// m_welcome = gameObject.AddComponent<TutorialFunction_MessageWithDelay>();
			// m_welcome.Setup();
			// m_showPlayerVillage = gameObject.AddComponent<TutorialFunction_ShowVillage>();
			// m_showPlayerVillage.Setup();
			// m_showPlayerResources = gameObject.AddComponent<TutorialFunction_ShowResources>();
			// m_showPlayerResources.Setup();
			// m_explainResources = gameObject.AddComponent<TutorialFunction_ClicToContinue>();
			// m_explainResources.Setup();
			// m_letItWork = gameObject.AddComponent<TutorialFunction_ClicToContinue>();
			// m_letItWork.Setup();
			// m_refreshPlayerVillagersMission = gameObject.AddComponent<TutorialFunction_RefreshPlayerVillagersMission>();
			// m_refreshPlayerVillagersMission.Setup();
			// m_waitNewBuilding = gameObject.AddComponent<TutorialFunction_WaitNewBuidling>();
			// m_waitNewBuilding.Setup();
			// m_displayBottomContent = gameObject.AddComponent<TutorialFunction_EnableBottomContent>();
			// m_displayBottomContent.Setup();
			// m_dragAndDropResource = gameObject.AddComponent<TutorialFunction_DragAndDrop>();
			// m_dragAndDropResource.Setup();
			// m_displayXPBar = gameObject.AddComponent<TutorialFunction_EnableXP>();
			// m_displayXPBar.Setup();
			// m_gainXP = gameObject.AddComponent<TutorialFunction_LockGameAndClicOk>();
			// m_gainXP.Setup();
			// m_tryToFillTheXPBar = gameObject.AddComponent<TutorialFunction_LockGameAndClicOk>();
			// m_tryToFillTheXPBar.Setup();
			// m_waitPowerUp = gameObject.AddComponent<TutorialFunction_WaitPowerUp>();
			// m_waitPowerUp.Setup();
			// m_waitDelayAfterPowerUp = gameObject.AddComponent<TutorialFunction_WaitDelay>();
			// m_waitDelayAfterPowerUp.Setup();
			// m_showProgressionBar = gameObject.AddComponent<TutorialFunction_EnableProgressionBar>();
			// m_showProgressionBar.Setup();
			// m_explainProgression = gameObject.AddComponent<TutorialFunction_LockGameAndClicOk>();
			// m_explainProgression.Setup();
			// m_showTimer = gameObject.AddComponent<TutorialFunction_EnableTimer>();
			// m_showTimer.Setup();
			// m_completeTheLevel = gameObject.AddComponent<TutorialFunction_LockGameAndClicOk>();
			// m_completeTheLevel.Setup();
			// m_onTimeOut = gameObject.AddComponent<TutorialFunction_WaitGameOver>();
			// m_onTimeOut.Setup();
			// m_waitMainMenu = gameObject.AddComponent<TutorialFunction_WaitMainMenu>();
			// m_waitMainMenu.Setup();
			// m_clicTechTree = gameObject.AddComponent<TutorialFunction_ClicTechTree>();
			// m_clicTechTree.Setup();
			// m_clicFireAbility = gameObject.AddComponent<TutorialFunction_ClicFireAbility>();
			// m_clicFireAbility.Setup();




		}
		public void Init()
		{

		}
		public void Execute(TutorialStepScriptable a_tutorialFunctionSO, int a_stepId)
		{
			// switch (a_tutorialFunctionSO.FunctionId)
			// {
			// 	case 0:
			// 		m_loadGame.Init(a_stepId, CompleteStep);
			// 		return;
			// 	case 1:
			// 		m_welcome.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 2:
			// 		m_showPlayerVillage.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 3:
			// 		m_showPlayerResources.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 4:
			// 		m_explainResources.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 5:
			// 		m_letItWork.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 6:
			// 		m_refreshPlayerVillagersMission.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 7:
			// 		m_waitNewBuilding.Init(a_stepId, CompleteStep);
			// 		return;
			// 	case 8:
			// 		m_displayBottomContent.Init(a_stepId, CompleteStep);
			// 		return;
			// 	case 9:
			// 		m_dragAndDropResource.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 10:
			// 		m_displayXPBar.Init(a_stepId, CompleteStep);
			// 		return;
			// 	case 11:
			// 		m_gainXP.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 12:
			// 		m_tryToFillTheXPBar.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 13:
			// 		m_waitPowerUp.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 14:
			// 		m_waitDelayAfterPowerUp.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 15:
			// 		m_showProgressionBar.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 16:
			// 		m_explainProgression.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 17:
			// 		m_showTimer.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 18:
			// 		m_completeTheLevel.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 19:
			// 		m_onTimeOut.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 20:
			// 		m_waitMainMenu.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 21:
			// 		m_clicTechTree.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// 	case 22:
			// 		m_clicFireAbility.Init(a_stepId, CompleteStep, a_tutorialFunctionSO.Message);
			// 		return;
			// }
		}

		private void CompleteStep(int a_stepId, bool a_saveStep)
		{
			m_tutorial.DataComponent.CompletStep(a_stepId, a_saveStep);
		}

		private void ResetTutorial()
		{

		}
	}
}