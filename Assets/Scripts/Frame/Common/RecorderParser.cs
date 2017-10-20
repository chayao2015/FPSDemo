﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RecorderParser
{
	protected short[]			mFrequencyData;			// 转换后的频域数据
	protected int				mFrequencyDataCount;	// 频域数据数量
	protected short[]			mAllPCMData;			// 总的PCM数据
	protected int				mAllPCMCount;			// 总PCM数据的数量,当前已有的PCM数据数量
	protected WavRecorder		mRecorder;				// 采集本地音频输入的录音机
	protected int				mRecorderDataBlockSize; // 音频输入数据一次解析的数据数量
	protected int				mCurDB;					// 当前音量大小
	protected int				mBlockBufferSize;		// 每次接收到的数据缓冲区的大小
	public RecorderParser()
	{
		mCurDB = -96;
		mAllPCMCount = 0;
		int sampleRate = 44100;
		// 采集音频输入
		mBlockBufferSize = (int)(sampleRate * 0.02f);
		mRecorder = new WavRecorder(mBlockBufferSize, sampleRate);
		mRecorder.setRecordCallback(onRecorderData);

		// 缓冲区大小固定为2048,确保比mBlockBufferSize大
		mRecorderDataBlockSize = 1024;
		mAllPCMData = new short[mRecorderDataBlockSize];

		// 频域缓冲区
		mFrequencyDataCount = mRecorderDataBlockSize;
		mFrequencyData = new short[mFrequencyDataCount];
	}
	public void destroy()
	{
		stopRecord();
	}
	public void update(float elapsedTime)
	{
		if (mRecorder != null)
		{
			mRecorder.update(elapsedTime);
		}
	}
	public short[] getFrequencyData()				{ return mFrequencyData; }
	public int getFrequencyDataCount()				{ return mFrequencyDataCount; }
	public int getCurDB()							{ return mCurDB; }
	public bool startRecord()
	{
		mAllPCMCount = 0;
		bool ret = mRecorder.startRecord(0);
		if (!ret)
		{
			return false;
		}
		return true;
	}

	public void stopRecord()
	{
		mRecorder.stopRecord();
	}

	public void setRecordData(short[] data, int dataSize)
	{
		// 检测数据量是否正确
		if (mBlockBufferSize != dataSize)
		{
			return;
		}
		// 由于缓冲区大小和每次获取的数据数量都是固定的,所以只要有数据就需要移动
		// 将已有的数据移到缓冲区头部,然后将新的数据加入尾部
		if (mAllPCMCount > 0)
		{
			BinaryUtility.memmove(mAllPCMData, 0, dataSize, mRecorderDataBlockSize - dataSize);
			BinaryUtility.memcpy(mAllPCMData, data, mRecorderDataBlockSize - dataSize, 0, dataSize);
		}
		else
		{
			BinaryUtility.memset(mAllPCMData, (short)0, mRecorderDataBlockSize);
			BinaryUtility.memcpy(mAllPCMData, data, 0, 0, dataSize);
			mAllPCMCount = dataSize;
		}
	}
	public void onRecorderData(short[] data, int dataSize)
	{
		setRecordData(data, dataSize);
	}
	public void updateDBFrequency()
	{
		// 计算音量大小和频域
		mCurDB = MathUtility.pcm_db_count(mAllPCMData, mRecorderDataBlockSize);
	}
};