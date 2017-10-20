using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterOther : CharacterNPC
{
	protected bool mMatchSpeed;			// 当前是否与教练的速度等级相匹配
	protected int mCurSpeedLevel;
	protected float mCurTimeCount;
	protected const float TIME_INTERVAL = 10.0f;
	protected float mLastMileage;

	public CharacterOther(CHARACTER_TYPE type, string name)
		:
		base(type, name)
	{
		mMatchSpeed = false; 
		mCurSpeedLevel = -1;	
	}
	public override void update(float elapsedTime)
	{
		base.update(elapsedTime);
	}

	// 由外界通知角色的速度等级是否匹配,减少不必要的判断
	public void setMatchSpeed(bool match) { mMatchSpeed = match; }
	public bool getMatchSpeed() { return mMatchSpeed; }
	public void setCurSpeedLevel(int level) { mCurSpeedLevel = level; }
}