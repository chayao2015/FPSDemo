﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
#if UNITY_STANDALONE_WIN
using System.Windows.Forms;
#endif
using UnityEngine;

// 日志等级
public enum LOG_LEVEL
{
	LL_FORCE,   // 强制显示
	LL_HIGH,    // 高
	LL_NORMAL,  // 正常
	LL_MAX,
}

public class UnityUtility : GameBase
{
	protected static GameCamera mForeEffectCamera;
	protected static GameCamera mBackEffectCamera;
	protected static LOG_LEVEL mLogLevel;
	public void init()
	{
		mForeEffectCamera = mCameraManager.getCamera("UIForeEffectCamera");
		mBackEffectCamera = mCameraManager.getCamera("UIBackEffectCamera");
#if UNITY_EDITOR
		setLogLevel(LOG_LEVEL.LL_NORMAL);
#else
		setLogLevel((LOG_LEVEL)(int)mFrameConfig.getFloatParam(GAME_DEFINE_FLOAT.GDF_LOG_LEVEL));
#endif
	}
	public static void setLogLevel(LOG_LEVEL level)
	{
		mLogLevel = level;
		logInfo("log level : " + mLogLevel, LOG_LEVEL.LL_FORCE);
	}
	public static LOG_LEVEL getLogLevel()
	{
		return mLogLevel;
	}
	public static void logError(string info, bool isMainThread = true)
	{
		if (isMainThread)
		{
			messageBox(info, true);
		}
		Debug.LogError("error : " + info);
	}
	// force表示是否强制输出日志
	public static void logInfo(string info, LOG_LEVEL level = LOG_LEVEL.LL_NORMAL)
	{
		if ((int)level <= (int)mLogLevel)
		{
			Debug.Log(getTime() + " : " + info);
		}
	}
	public static string getTime()
	{
		DateTime time = DateTime.Now;
		return time.Hour + ":" + time.Minute + ":" + time.Second + ":" + time.Millisecond;
	}
	public static void copyTextToClipbord(string str)
	{
#if UNITY_STANDALONE_WIN
		Clipboard.SetText(str);
#endif
	}
	public static string getTextFromClipboard()
	{
#if UNITY_STANDALONE_WIN
		return Clipboard.GetText();
#else
		return "";
#endif
	}
	public static void messageBox(string info, bool errorOrInfo)
	{
		string title = errorOrInfo ? "错误" : "提示";
		// 在编辑器中显示对话框
#if UNITY_EDITOR
		UnityEditor.EditorUtility.DisplayDialog(title, info, "确认");
#elif UNITY_STANDALONE_WIN
		// 游戏运行过程中显示窗口提示框
		MessageBox.Show(info, title, MessageBoxButtons.OK, errorOrInfo ? MessageBoxIcon.Error : MessageBoxIcon.Information);
#endif
	}
	public static Ray getRay(Vector2 screenPos)
	{
		if (mLayoutManager.getUIRootObject() == null)
		{
			return new Ray();
		}
		Camera camera = mCameraManager.getUICamera().getCamera();
		return camera.ScreenPointToRay(screenPos);
	}
	public static List<txUIButton> raycast(Ray ray, SortedDictionary<int, List<txUIButton>> buttonList, int maxCount = 0)
	{
		bool cast = true;
		List<txUIButton> retList = new List<txUIButton>();
		RaycastHit hit = new RaycastHit();
		foreach (var box in buttonList)
		{
			int count = box.Value.Count;
			for (int i = 0; i < count; ++i)
			{
				txUIButton button = box.Value[i];
				if (button.getHandleInput() && button.Raycast(ray, out hit, 10000.0f))
				{
					retList.Add(button);
					// 如果射线不能穿透当前按钮,或者已经达到最大数量,则不再继续
					if (!button.getPassRay() || maxCount > 0 && retList.Count >= maxCount)
					{
						cast = false;
						break;
					}
				}
			}
			if (!cast)
			{
				break;
			}
		}
		return retList;
	}
	public static GameObject getGameObject(GameObject parent, string name)
	{
		GameObject go = null;
		if (parent == null)
		{
			go = GameObject.Find(name);
		}
		else
		{
			Transform trans = parent.transform.Find(name);
			if (trans != null)
			{
				go = trans.gameObject;
			}
		}
		return go;
	}
	public static GameObject cloneObject(GameObject oriObj, string name)
	{
		GameObject obj = GameObject.Instantiate(oriObj);
		obj.name = name;
		return obj;
	}
	// 根据预设名实例化
	public static GameObject instantiatePrefab(GameObject parent, string prefabName, string name, Vector3 scale, Vector3 rot, Vector3 pos)
	{
		GameObject prefab = mResourceManager.loadResource<GameObject>(prefabName, true);
		if (prefab == null)
		{
			logError("error : can not find prefab : " + prefabName);
			return null;
		}
		GameObject obj = instantiatePrefab(parent, prefab, name, scale, rot, pos);
		return obj;
	}
	// prefabName为Resource下的相对路径
	public static GameObject instantiatePrefab(GameObject parent, string prefabName)
	{
		string name = StringUtility.getFileName(prefabName);
		return instantiatePrefab(parent, prefabName, name, Vector3.one, Vector3.zero, Vector3.zero);
	}

	// 根据预设对象实例化
	public static GameObject instantiatePrefab(GameObject parent, GameObject prefab)
	{
		string name = StringUtility.getFileName(prefab.name);
		return instantiatePrefab(parent, prefab, name, Vector3.one, Vector3.zero, Vector3.zero);
	}
	// parent为实例化后挂接的父节点
	// prefabName为预设名,带Resources下相对路径
	// name为实例化后的名字
	// 其他三个是实例化后本地的变换
	public static GameObject instantiatePrefab(GameObject parent, GameObject prefab, string name, Vector3 scale, Vector3 rot, Vector3 pos)
	{
		GameObject obj = GameObject.Instantiate(prefab);
		setNormalProperty(ref obj, parent, name, scale, rot, pos);
		return obj;
	}

	public static void setNormalProperty(ref GameObject obj, GameObject parent, string name, Vector3 scale, Vector3 rot, Vector3 pos)
	{
		obj.transform.parent = parent.transform;
		obj.transform.localPosition = pos;
		obj.transform.localEulerAngles = rot;
		obj.transform.localScale = scale;
		obj.transform.name = name;
	}
	public static T createInstance<T>(Type classType, object[] param) where T : class
	{
		T instance = Activator.CreateInstance(classType, param) as T;
		return instance;
	}
	public static Vector2 screenPosToEffectPos(Vector2 screenPos, Vector2 parentWorldPos, Vector2 parentWorldScale, float effectDepth, bool isBack, bool isRelative = true)
	{
		GameCamera camera = isBack ? mBackEffectCamera : mForeEffectCamera;
		if (isRelative)
		{
			// 转到世界坐标系下,首先计算UI根节点的缩放值
			Vector3 scale = mLayoutManager.getUIRoot().getScale();
			screenPos.x = screenPos.x / scale.x;
			screenPos.y = screenPos.y / scale.y;
			parentWorldPos.x = parentWorldPos.x / scale.x;
			parentWorldPos.y = parentWorldPos.y / scale.y;
		}
		Vector2 pos = (effectDepth / -camera.getPosition().z + 1) * screenPos - parentWorldPos;
		return new Vector2(pos.x / parentWorldScale.x, pos.y / parentWorldScale.y);
	}
	public static void setGameObjectLayer(txUIObject obj, string layerName)
	{
		int layer = LayerMask.NameToLayer(layerName);
		if (layer < 0 || layer >= 32)
		{
			return;
		}
		GameObject go = obj.mObject;
		go.layer = layer;
		foreach (Transform t in go.transform.GetComponentsInChildren<Transform>(true))
		{
			t.gameObject.layer = layer;
		}
	}
	public static int getLineNum(int preFrameCount = 1)
	{
	    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(preFrameCount, true);
	    return st.GetFrame(0).GetFileLineNumber();
	}
	public static string getCurSourceFileName(int preFrameCount = 1)
	{
	    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(preFrameCount, true);
	    return st.GetFrame(0).GetFileName();
	}
}