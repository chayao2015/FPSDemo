using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LoginSceneProcedureStart : SceneProcedure
{
	public LoginSceneProcedureStart()
	{ }
	public LoginSceneProcedureStart(PROCEDURE_TYPE type, GameScene gameScene)
		:
		base(type, gameScene)
	{
		;
	}
	protected override void onInit(SceneProcedure lastProcedure, string intent)
	{
		LayoutTools.LOAD_LAYOUT_SHOW(LAYOUT_TYPE.LT_LOGIN, 0);
	}
	protected override void onUpdate(float elapsedTime)
	{
		;
	}
	protected override void onExit(SceneProcedure nextProcedure)
	{
		LayoutTools.UNLOAD_LAYOUT(LAYOUT_TYPE.LT_LOGIN);
	}
	protected override void onKeyProcess(float elapsedTime)
	{
		;
	}
}