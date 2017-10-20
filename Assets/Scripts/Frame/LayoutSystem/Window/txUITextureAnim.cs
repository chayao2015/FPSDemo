﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextureInfo
{
	public string  mName;
	public Texture mTexture;
	public TextureInfo(string name, Texture tex)
	{
		mName = name;
		mTexture = tex;
	}
}

public class txUITextureAnim : txUIStaticTexture
{
	protected List<TextureInfo>   mTextureNameList;
	protected int				  mStartIndex = 0;			// 序列帧的起始帧下标,默认为0,即从头开始
	protected int				  mEndIndex = -1;			// 序列帧的终止帧下标,默认为-1,即播放到尾部
	protected bool				  mPlayDirection = true;	// 播放方向,true为正向播放(从mStartIndex到mEndIndex),false为返向播放(从mEndIndex到mStartIndex)
	protected int				  mCurTextureIndex = 0;
	protected LOOP_MODE			  mLoopMode = LOOP_MODE.LM_ONCE;
	protected string			  mTextureSetName = "";
	protected float				  mCurTimeCount = 0.0f;
	protected PLAY_STATE		  mPlayState = PLAY_STATE.PS_STOP;
	protected float				  mInterval = 0.03f;	// 隔多少秒切换图片
	protected float				  mInverseInterval = 0.0f;
	protected bool				  mAutoHide = true;		// 是否在播放完毕后自动隐藏并且重置当前帧下标
	protected TextureAnimCallBack mPlayEndCallback = null;	// 一个序列播放完时的回调函数,只在非循环播放状态下有效
	protected object			  mPlayEndUserData;
	public txUITextureAnim()
	{
		mType = UI_OBJECT_TYPE.UBT_TEXTURE_ANIM;
		mTextureNameList = new List<TextureInfo>();
		mInverseInterval = 1.0f / mInterval;
	}
	public override void init(GameLayout layout, GameObject go)
	{
		base.init(layout, go);
		string textureName = getTextureName();
		int index = textureName.LastIndexOf('_');
		if(index >= 0)
		{
			string textureSetName = textureName.Substring(0, index);
			setTextureSet(textureSetName);
		}
	}
	public override void update(float elapsedTime)
	{
		base.update(elapsedTime);
		if (mPlayState == PLAY_STATE.PS_PLAY)
		{
			if (mTextureNameList.Count != 0)
			{
				if (mPlayDirection)
				{
					mCurTimeCount += elapsedTime;
					if (mCurTimeCount > mInterval)
					{
						// 一帧时间内可能会跳过多帧序列帧
						int elapsedFrames = (int)(mCurTimeCount / mInterval);
						mCurTimeCount -= elapsedFrames * mInterval;
						if (mCurTextureIndex + elapsedFrames <= getRealEndIndex())
						{
							mCurTextureIndex += elapsedFrames;
						}
						else
						{
							if (mLoopMode == LOOP_MODE.LM_ONCE)
							{
								// 非循环播放时播放完成后,自动隐藏时,隐藏窗口,并且停止播放
								if (mAutoHide)
								{
									setActive(false);
								}
								stop(mAutoHide, true, false);
							}
							// 普通循环,则将下标重置到起始下标
							else if (mLoopMode == LOOP_MODE.LM_LOOP)
							{
								mCurTextureIndex = mStartIndex;
							}
							// 来回循环,则将下标重置到终止下标,并且开始反向播放
							else if (mLoopMode == LOOP_MODE.LM_PINGPONG)
							{
								mCurTextureIndex = getRealEndIndex();
								mPlayDirection = !mPlayDirection;
							}
						}
						setTexture(mTextureNameList[mCurTextureIndex].mTexture);
					}
				}
				else
				{
					mCurTimeCount += elapsedTime;
					if (mCurTimeCount > mInterval)
					{
						// 一帧时间内可能会跳过多帧序列帧
						int elapsedFrames = (int)(mCurTimeCount / mInterval);
						mCurTimeCount -= elapsedFrames * mInterval;
						if (mCurTextureIndex - elapsedFrames >= mStartIndex)
						{
							mCurTextureIndex -= elapsedFrames;
						}
						else
						{
							if (mLoopMode == LOOP_MODE.LM_ONCE)
							{
								// 非循环播放时播放完成后,自动隐藏时,隐藏窗口,并且停止播放
								if (mAutoHide)
								{
									setActive(false);
								}
								stop(mAutoHide, true, false);
							}
							// 普通循环,则将下标重置到终止下标
							else if (mLoopMode == LOOP_MODE.LM_LOOP)
							{
								mCurTextureIndex = getRealEndIndex();
							}
							// 来回循环,则将下标重置到起始下标,并且开始正向播放
							else if (mLoopMode == LOOP_MODE.LM_PINGPONG)
							{
								mCurTextureIndex = mStartIndex;
								mPlayDirection = !mPlayDirection;
							}
						}
						setTexture(mTextureNameList[mCurTextureIndex].mTexture);
					}
				}
			}
			else
			{
				setTexture(null);
			}
		}
	}
	public LOOP_MODE getLoop() { return mLoopMode; }
	public string getTextureSetName() { return mTextureSetName; }
	public float getInterval() { return mInterval; }
	public int getStartIndex() { return mStartIndex; }
	public PLAY_STATE getPlayState() { return mPlayState; }
	public bool getPlayDirection() { return mPlayDirection; }
	public int getEndIndex() { return mEndIndex; }
	public int getTextureFrameCount() { return mTextureNameList.Count; }
	public bool getAutoHide() { return mAutoHide; }
	// 获得实际的终止下标,如果是自动获得,则返回最后一张的下标
	public int getRealEndIndex()
	{
		if (mEndIndex < 0)
		{
			return MathUtility.getMax(getTextureFrameCount() - 1, 0);
		}
		else
		{
			return mEndIndex;
		}
	}
	public void setLoop(LOOP_MODE loop)				{ mLoopMode = loop; }
	public void setInterval(float interval)			
	{
		mInterval = interval;
		mInverseInterval = 1.0f / mInterval;
	}
	public void setPlayDirection(bool direction)	{ mPlayDirection = direction; }
	public void setAutoHide(bool autoHide)			{ mAutoHide = autoHide; }
	public void setPlayState(PLAY_STATE state)
	{
		if (state == PLAY_STATE.PS_PAUSE)
		{
			pause();
		}
		else if (state == PLAY_STATE.PS_PLAY)
		{
			play();
		}
		else if (state == PLAY_STATE.PS_STOP)
		{
			stop();
		}
	}
	public void setStartIndex(int startIndex)		
	{
		mStartIndex = startIndex;
		MathUtility.clamp(ref mStartIndex, 0, getTextureFrameCount() - 1);
	}
	public void setEndIndex(int endIndex)			
	{ 
		mEndIndex = endIndex;
		if (mEndIndex >= 0)
		{
			MathUtility.clamp(ref mEndIndex, 0, getTextureFrameCount() - 1);
		}
	}
	public void setTextureSet(string textureSetName)
	{
		mTextureNameList.Clear();
		mTextureSetName = textureSetName;
		string path = CommonDefine.R_TEXTURE_ANIM_PATH + mTextureSetName;
		string preName = path + "/" + mTextureSetName + "_";
		for (int i = 1; ; ++i)
		{
			string name = preName + StringUtility.intToString(i);
			Texture tex = mResourceManager.loadResource<Texture>(name, false);
			if(tex == null)
			{
				break;
			}
			mTextureNameList.Add(new TextureInfo(name, tex));
		}

		// 重新判断起始下标和终止下标,确保下标不会越界
		MathUtility.clamp(ref mStartIndex, 0, getTextureFrameCount() - 1);
		if (mEndIndex >= 0)
		{
			MathUtility.clamp(ref mEndIndex, 0, getTextureFrameCount() - 1);
		}
		if (mTextureNameList.Count > 0 && mStartIndex >= 0 && mStartIndex < mTextureNameList.Count)
		{
			setTexture(mTextureNameList[mStartIndex].mTexture);
		}
	}
	public void stop(bool resetStartIndex = true, bool callback = true, bool isBreak = true)
	{
		mPlayState = PLAY_STATE.PS_STOP;
		if (resetStartIndex)
		{
			setCurFrameIndex(mStartIndex);
		}
		// 中断序列帧播放时调用回调函数,只在非循环播放时才调用
		if (callback && mLoopMode == LOOP_MODE.LM_ONCE)
		{
			callAndClearEndCallback(isBreak);
		}
		// 确保在停止后回调函数已经清空
		clearEndCallback();
	}
	public void play() { mPlayState = PLAY_STATE.PS_PLAY; }
	public void pause() { mPlayState = PLAY_STATE.PS_PAUSE; }
	public void setPlayEndCallback(TextureAnimCallBack callback, object userData = null)
	{
		callAndClearEndCallback(true);
		mPlayEndCallback = callback;
		mPlayEndUserData = userData;
	}
	public int getCurFrameIndex() { return mCurTextureIndex; }
	public void setCurFrameIndex(int index)
	{
		mCurTextureIndex = index;
		// 限制在起始帧和终止帧之间
		MathUtility.clamp(ref mCurTextureIndex, mStartIndex, getRealEndIndex());
		if (mCurTextureIndex >= 0 && mCurTextureIndex < mTextureNameList.Count)
		{
			setTexture(mTextureNameList[mCurTextureIndex].mTexture);
		}
		mCurTimeCount = 0.0f;
	}
	// 调用并且清空回调,清空是在调用之前
	protected void callAndClearEndCallback(bool isBreak)
	{
		TextureAnimCallBack curCallback = mPlayEndCallback;
		object curUserData = mPlayEndUserData;
		clearEndCallback();
		// 如果回调函数当前不为空,则是中断了更新
		if (curCallback != null)
		{
			curCallback(this, curUserData, isBreak);
		}
	}
	protected void clearEndCallback()
	{
		mPlayEndCallback = null;
		mPlayEndUserData = null;
	}
}