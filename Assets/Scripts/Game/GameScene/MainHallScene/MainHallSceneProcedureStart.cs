using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainHallSceneSceneProcedureStart : SceneProcedure
{
	public MainHallSceneSceneProcedureStart()
	{ }
	public MainHallSceneSceneProcedureStart(PROCEDURE_TYPE type, GameScene gameScene)
		:
		base(type, gameScene)
	{
		;
	}
	protected override void onInit(SceneProcedure lastProcedure, string intent)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Temple");
		GameObject character = mModelManager.createModel(GameDefine.R_MODEL_CHARACTER + "US_SOLDIER01", "role");
		character.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		//Animation anim = character.GetComponent<Animation>();
		//anim.Play();
		return;
	}
	protected override void onUpdate(float elapsedTime)
	{
		;
	}
	protected override void onExit(SceneProcedure nextProcedure)
	{
		;
	}
	protected override void onKeyProcess(float elapsedTime)
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			GameObject character = mModelManager.createModel(GameDefine.R_MODEL_CHARACTER + "US_SOLDIER01", "role");
			character.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
		}
	}
}