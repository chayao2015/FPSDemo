﻿
//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public delegate void RecordCallback(short[] data, int dataCount);

public class WavRecorder : GameBase
{
	protected RecordCallback mRecordCallback;
	protected int mSampleRate;					// 音频采样率
	protected GameObject mRecorderObject;
	protected AudioClip mClip;
	protected int maxRecordTime = 500;
	protected string[] mDeviceList;
	protected int mStartDevice = -1;
	protected int mLastPosition;
	protected short[] mReceivedData;
	protected int mBufferSize;
	protected int mCurDataCount;
	public WavRecorder(int bufferSize, int sampleRate = 44100)
	{
		mSampleRate = sampleRate;
		mRecorderObject = UnityUtility.getGameObject(mGameFramework.getGameFrameObject(), "Recorder");
		mDeviceList = Microphone.devices;
		mCurDataCount = 0;
		mBufferSize = bufferSize;
		mReceivedData = new short[mBufferSize];
	}
	public void setRecordCallback(RecordCallback callback)
	{
		mRecordCallback = callback;
	}
	public bool startRecord(int startDevice)
	{
		try
		{
			stopRecord();
			if (startDevice < 0 || startDevice >= mDeviceList.Length)
			{
				return false;
			}
			mStartDevice = startDevice;
			mClip = Microphone.Start(mDeviceList[mStartDevice], false, maxRecordTime, mSampleRate);
		}
		catch (Exception e)
		{
			UnityUtility.logInfo("error in record! : " + e.Message, LOG_LEVEL.LL_FORCE);
			return false;
		}
		return true;
	}
	public void stopRecord()
	{
		if (mStartDevice >= 0 && mStartDevice < mDeviceList.Length)
		{
			Microphone.End(mDeviceList[mStartDevice]);
			mStartDevice = -1;
		}
	}
	public void update(float elapsedTime)
	{
		// 如果正在录音
		if (Microphone.IsRecording(mDeviceList[mStartDevice]))
		{
			int curPos = Microphone.GetPosition(mDeviceList[mStartDevice]);
			if (curPos - mLastPosition > 0 && mRecordCallback != null)
			{
				int dataCount = curPos - mLastPosition;
				float[] data = new float[curPos - mLastPosition];
				mClip.GetData(data, mLastPosition);
				short[] pcmData = new short[dataCount];
				int allValue = 0;
				for (int i = 0; i < dataCount; ++i)
				{
					pcmData[i] = (short)(data[i] * 0x7fff);
					allValue += pcmData[i];
				}
				receiveData(pcmData, curPos - mLastPosition);
				mLastPosition = curPos;
			}
		}
		else 
		{
			if (mStartDevice!=-1)
			{
				mClip = Microphone.Start(mDeviceList[mStartDevice], false, maxRecordTime, mSampleRate);
				mLastPosition = 0;
			}
		}
	}
	//----------------------------------------------------------------------------------------------------------------------------------
	protected void receiveData(short[] data, int dataCount)
	{
		// 缓冲区还能直接放下数据
		if(mCurDataCount + dataCount <= mBufferSize)
		{
			BinaryUtility.memcpy(mReceivedData, data, mCurDataCount, 0, dataCount);
			mCurDataCount += dataCount;
		}
		// 数据量超出缓冲区
		else
		{
			int copyDataCount = mBufferSize - mCurDataCount;
			BinaryUtility.memcpy(mReceivedData, data, mCurDataCount, 0, copyDataCount);
			// 数据存满后通知回调
			mRecordCallback(mReceivedData, mBufferSize);
			// 清空缓冲区,存储新的数据
			int remainCount = dataCount - copyDataCount;
			// 缓冲区无法存放剩下的数据,则将多余的数据丢弃
			if (remainCount > mBufferSize)
			{
				BinaryUtility.memcpy(mReceivedData, data, 0, copyDataCount, mBufferSize - copyDataCount);
				mCurDataCount = mBufferSize - copyDataCount;
			}
			else
			{
				BinaryUtility.memcpy(mReceivedData, data, 0, copyDataCount, remainCount);
				mCurDataCount = remainCount;
			}
		}
	}
}