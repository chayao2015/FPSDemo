﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class Curve
{
	public GameObject mObject;
	public AnimationCurve mCurve;
}

public class KeyFrameManager : GameBase
{
	protected GameObject mManagerObject;
	protected Dictionary<string, Curve> mCurveList;
	protected int mLoadedCount;	// 已加载的关键帧数量
	public KeyFrameManager()
	{
		mCurveList = new Dictionary<string, Curve>();
		mLoadedCount = 0;
	}
	public AnimationCurve getKeyFrame(string name)
	{
		name = name.ToLower();
		if (mCurveList.ContainsKey(name))
		{
			return mCurveList[name].mCurve;
		}
		else 
		{
			if (mCurveList.ContainsKey(name.ToLower()))
			{
				return mCurveList[name.ToLower()].mCurve;
			}	
		}
		return null;
	}
	public void init()
	{
		// 查找关键帧管理器物体
		mManagerObject = UnityUtility.getGameObject(mGameFramework.getGameFrameObject(), "KeyFrameManager");
		if (mManagerObject == null)
		{
			UnityUtility.logError("error: can not find KeyFrameManager!");
			return;
		}
	}
	// 加载所有KeyFrame下的关键帧
	public void loadAll(bool async)
	{
		string path = CommonDefine.R_KEY_FRAME_PATH;
		List<string> fileList = mResourceManager.getFileList(path);
		int fileCount = fileList.Count;
		for (int i = 0; i < fileCount; ++i)
		{
			string fileNameNoSuffix = fileList[i];
			mCurveList.Add(fileNameNoSuffix.ToLower(), null);
			if (async)
			{
				mResourceManager.loadResourceAsync<GameObject>(path + fileNameNoSuffix, onKeyFrameLoaded, true);
			}
			else
			{
				GameObject prefab = mResourceManager.loadResource<GameObject>(path + fileNameNoSuffix, true);
				onKeyFrameLoaded(prefab, null);
			}
		}
	}
	public void destroy()
	{
		// 将实例化出的所有物体销毁
		foreach(var item in mCurveList)
		{
			GameObject.Destroy(item.Value.mObject);
		}
		mCurveList.Clear();
		mLoadedCount = 0;
		GameObject.Destroy(mManagerObject);
		mManagerObject = null;
	}
	public bool isLoadDone()
	{
		return mLoadedCount == mCurveList.Count;
	}
	public float getLoadedPercent()
	{
		return (float)mLoadedCount / (float)mCurveList.Count;
	}
	//------------------------------------------------------------------------------------------------------------------
	protected void onKeyFrameLoaded(UnityEngine.Object res, object userData)
	{
		GameObject keyFrameObject = UnityUtility.instantiatePrefab(mManagerObject, res as GameObject);
		// 查找关键帧曲线,加入列表中
		TweenScale tweenScale = keyFrameObject.GetComponent<TweenScale>();
		if (tweenScale == null)
		{
			UnityUtility.logError("object in KeyFrame folder must has TweenScale!");
			return;
		}
		AnimationCurve animCurve = tweenScale.animationCurve;
		if (animCurve != null)
		{
			Curve curve = new Curve();
			curve.mObject = keyFrameObject;
			curve.mCurve = animCurve;
			mCurveList[res.name.ToLower()] = curve;
			++mLoadedCount;
		}
		else
		{
			UnityUtility.logError("object in KeyFrame folder must has TweenScale and AnimationCurve!");
		}
	}
}
