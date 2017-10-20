using UnityEngine;
using System.Collections;

public class MainHallScene : GameScene
{
	public MainHallScene(GAME_SCENE_TYPE type, string name)
		:
		base(type, name)
	{
		mDestroyEngineScene = false;
	}
	public override void assignStartExitProcedure()
	{
		mStartProcedure = PROCEDURE_TYPE.PT_MAIN_HALL_START;
		mExitProcedure = PROCEDURE_TYPE.PT_MAIN_HALL_EXIT;
	}
	public override void createSceneProcedure()
	{
		addProcedure<MainHallSceneSceneProcedureStart>(PROCEDURE_TYPE.PT_MAIN_HALL_START);
		addProcedure<MainHallSceneSceneProcedureExit>(PROCEDURE_TYPE.PT_MAIN_HALL_EXIT);
		if (mSceneProcedureList.Count != PROCEDURE_TYPE.PT_MAIN_HALL_MAX - PROCEDURE_TYPE.PT_MAIN_HALL_MIN - 1)
		{
			UnityUtility.logError("not all procedure add to scene : " + mSceneType);
		}
	}
}
