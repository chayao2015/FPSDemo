using UnityEngine;
using System.Collections;

public class LoginScene : GameScene
{
	public LoginScene(GAME_SCENE_TYPE type, string name)
		:
		base(type, name)
	{
		mDestroyEngineScene = false;
	}
	public override void assignStartExitProcedure()
	{
		mStartProcedure = PROCEDURE_TYPE.PT_LOGIN_START;
		mExitProcedure = PROCEDURE_TYPE.PT_LOGIN_EXIT;
	}
	public override void createSceneProcedure()
	{
		addProcedure<LoginSceneProcedureStart>(PROCEDURE_TYPE.PT_LOGIN_START);
		addProcedure<LoginSceneProcedureExit>(PROCEDURE_TYPE.PT_LOGIN_EXIT);
		if(mSceneProcedureList.Count != PROCEDURE_TYPE.PT_LOGIN_MAX - PROCEDURE_TYPE.PT_LOGIN_MIN - 1)
		{
			UnityUtility.logError("not all procedure add to scene : " + mSceneType);
		}
	}
}
