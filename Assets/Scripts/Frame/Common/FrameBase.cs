﻿using UnityEngine;
using System.Collections;

// 管理类初始化完成调用
// 这个父类的添加是方便代码的书写
public class FrameBase
{
	public static GameFramework			mGameFramework			= null;
	public static CommandSystem			mCommandSystem			= null;
	public static AudioManager			mAudioManager			= null;
	public static GameSceneManager		mGameSceneManager		= null;
	public static CharacterManager		mCharacterManager		= null;
	public static GameLayoutManager		mLayoutManager			= null;
	public static KeyFrameManager		mKeyFrameManager		= null;
	public static GlobalTouchSystem		mGlobalTouchSystem		= null;
	public static ShaderManager			mShaderManager			= null;
	public static DataBase				mDataBase				= null;
	public static CameraManager			mCameraManager			= null;
	public static ResourceManager		mResourceManager		= null;
	public static LayoutPrefabManager	mLayoutPrefabManager	= null;
	public static ApplicationConfig		mApplicationConfig		= null;
	public static FrameConfig			mFrameConfig			= null;
	public static ModelManager			mModelManager			= null;
	public virtual void notifyConstructDone()
	{
		if (mGameFramework == null)
		{
			mGameFramework = GameFramework.instance;
			mCommandSystem = mGameFramework.getCommandSystem();
			mAudioManager = mGameFramework.getAudioManager();
			mGameSceneManager = mGameFramework.getGameSceneManager();
			mCharacterManager = mGameFramework.getCharacterManager();
			mLayoutManager = mGameFramework.getLayoutManager();
			mKeyFrameManager = mGameFramework.getKeyFrameManager();
			mGlobalTouchSystem = mGameFramework.getGlobalTouchSystem();
			mShaderManager = mGameFramework.getShaderManager();
			mDataBase = mGameFramework.getDataBase();
			mCameraManager = mGameFramework.getCameraManager();
			mResourceManager = mGameFramework.getResourceManager();
			mLayoutPrefabManager = mGameFramework.getLayoutPrefabManager();
			mApplicationConfig = mGameFramework.getApplicationConfig();
			mFrameConfig = mGameFramework.getFrameConfig();
			mModelManager = mGameFramework.getModelManager();
		}
	}
}