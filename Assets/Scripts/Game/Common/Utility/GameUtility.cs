using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

public class GameUtility : GameBase
{
	protected static int mIDMaker;
	public void init()
	{
		;
	}
	public static float calcuteConfigExpression(GAME_DEFINE_STRING expDefine, float variableValue)
	{
		string variableStr = "(" + variableValue.ToString("F2") + ")";
		string expression = mGameConfig.getStringParam(expDefine);
		expression = expression.Replace("i", variableStr);
		float expressionValue = MathUtility.calculateFloat(expression);
		return expressionValue;
	}
	public static float stepFrequencyToMS(float frequency)
	{
		if(frequency <= 0.0f)
		{
			return 0.0f;
		}
		float speed = frequency / 60.0f * 4.0f;
		MathUtility.clamp(ref speed, 0.0f, 20.0f);
		// 假设每踩踏一圈,前进4米
		return speed;
	}
	public static float MSToStepFrequency(float speedMS)
	{
		if (speedMS < 0.0f)
		{
			return 0.0f;
		}
		return speedMS / 4.0f * 60.0f;
	}
	public static int makeID() { return ++mIDMaker; }
}