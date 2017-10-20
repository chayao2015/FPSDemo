﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class ApplicationConfig : ConfigBase
{
	public override void writeConfig()
	{
		FileUtility.writeFile(CommonDefine.F_CONFIG_PATH + "ApplicationSetting.txt", generateFloatFile());
	}
	//---------------------------------------------------------------------------------------------------------------------------------
	protected override void addFloat()
	{
		addFloatParam(GAME_DEFINE_FLOAT.GDF_FULL_SCREEN);
		addFloatParam(GAME_DEFINE_FLOAT.GDF_SCREEN_WIDTH);
		addFloatParam(GAME_DEFINE_FLOAT.GDF_SCREEN_HEIGHT);
		addFloatParam(GAME_DEFINE_FLOAT.GDF_SCREEN_COUNT);
		addFloatParam(GAME_DEFINE_FLOAT.GDF_USE_FIXED_TIME);
		addFloatParam(GAME_DEFINE_FLOAT.GDF_FIXED_TIME);
		if (mFloatNameToDefine.Count != (int)GAME_DEFINE_FLOAT.GDF_APPLICATION_MAX - (int)GAME_DEFINE_FLOAT.GDF_APPLICATION_MIN - 1)
		{
			UnityUtility.logError("not all float parameter added!");
		}
	}
	protected override void addString()
	{
		if (mStringNameToDefine.Count != (int)GAME_DEFINE_STRING.GDS_APPLICATION_MAX - (int)GAME_DEFINE_STRING.GDS_APPLICATION_MIN - 1)
		{
			UnityUtility.logError("not all string parameter added!");
		}
	}
	protected override void readConfig()
	{
		readFile(CommonDefine.F_CONFIG_PATH + "ApplicationSetting.txt", true);
	}
}