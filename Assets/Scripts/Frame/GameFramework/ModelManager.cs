using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;

public class ModelManager : GameBase
{
	protected Dictionary<string, GameObject> mModelPrefabList;
	protected Dictionary<string, GameObject> mModelInstanceList;
	public ModelManager()
	{
		mModelPrefabList = new Dictionary<string, GameObject>();
		mModelInstanceList = new Dictionary<string, GameObject>();
	}
	public void init()
	{
		;
	}
	public void destroy()
	{
		foreach (var item in mModelPrefabList)
		{
			GameObject.Destroy(item.Value);
		}
		foreach (var item in mModelInstanceList)
		{
			GameObject.Destroy(item.Value);
		}
		mModelPrefabList.Clear();
		mModelInstanceList.Clear();
	}
	public GameObject createModel(string fileWithPath, string modelName)
	{
		if(mModelInstanceList.ContainsKey(modelName))
		{
			return mModelInstanceList[modelName];
		}
		GameObject prefab = getModelPrefab(fileWithPath);
		if(prefab == null)
		{
			return null;
		}
		GameObject model = GameObject.Instantiate<GameObject>(prefab);
		model.name = modelName;
		mModelInstanceList.Add(modelName, model);
		return model;
	}
	public GameObject getModel(string modelName)
	{
		if (mModelInstanceList.ContainsKey(modelName))
		{
			return mModelInstanceList[modelName];
		}
		return null;
	}
	public void destroyModel(string modelName)
	{
		if (mModelInstanceList.ContainsKey(modelName))
		{
			GameObject.Destroy(mModelInstanceList[modelName]);
			mModelInstanceList.Remove(modelName);
		}
	}
	public void destroyModelPrefab(string fileWithPath)
	{
		if (mModelPrefabList.ContainsKey(fileWithPath))
		{
			GameObject.Destroy(mModelPrefabList[fileWithPath]);
			mModelPrefabList.Remove(fileWithPath);
		}
	}
	//----------------------------------------------------------------------------------------------------------------------------------------
	protected GameObject getModelPrefab(string fileWithPath, bool loadIfNull = true)
	{
		if(mModelPrefabList.ContainsKey(fileWithPath))
		{
			return mModelPrefabList[fileWithPath];
		}
		else if(loadIfNull)
		{
			return loadPrefab(fileWithPath);
		}
		return null;
	}
	protected GameObject loadPrefab(string fileWithPath)
	{
		GameObject prefab = mResourceManager.loadResource<GameObject>(fileWithPath, false);
		if(prefab == null)
		{
			UnityUtility.logInfo("can not find model : " + fileWithPath);
			return null;
		}
		mModelInstanceList.Add(fileWithPath, prefab);
		return prefab;
	}
}