﻿using UnityEngine;
using System.Collections;

public class txUISlider : txUIObject
{	
	protected UISlider mSlider;
	public txUISlider()
	{
		mType = UI_OBJECT_TYPE.UBT_SLIDER;
	}
	public override void init(GameLayout layout, GameObject go)
	{
		base.init(layout, go);
		mSlider = mObject.GetComponent<UISlider>();
		if(mSlider == null)
		{
			mSlider = mObject.AddComponent<UISlider>();
		}
	}
	public float getSliderValue ()
	{
		if (mSlider == null)
		{
			return 0.0f;
		}
		return mSlider.value;
	}
	public void setSliderValue(float value)
	{
		if (mSlider == null)
		{
			return;
		}
		MathUtility.clamp(ref value, 0.0f, 1.0f);
		mSlider.value = value;
	}
	public void setSliderValueChange(EventDelegate.Callback mUislider) 
	{
		if (mSlider == null)
		{
			return;
		}
		EventDelegate.Add(mSlider.onChange, mUislider);
	}
	public override void setAlpha(float alpha)
	{
		if (mSlider == null)
		{
			return;
		}
		mSlider.alpha = alpha;
	}
	public override float getAlpha()
	{
		if (mSlider == null)
		{
			return 0.0f;
		}
		return mSlider.alpha;
	}
}