using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameSceneRegister : GameBase
{
	public void registerAllGameScene()
	{
		registeGameScene(typeof(LoginScene), GAME_SCENE_TYPE.GST_LOGIN);
		registeGameScene(typeof(MainHallScene), GAME_SCENE_TYPE.GST_MAIN_HALL);
		if (mGameSceneManager.getSceneCount() != (int)GAME_SCENE_TYPE.GST_MAX)
		{
			UnityUtility.logError("not all scene registed!");
		}
	}
	//-------------------------------------------------------------------------------------------------------------
	protected void registeGameScene(Type scene, GAME_SCENE_TYPE type)
	{
		mGameSceneManager.registeGameScene(scene, type);
	}
}