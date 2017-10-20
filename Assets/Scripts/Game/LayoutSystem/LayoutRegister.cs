using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LayoutRegister : GameBase
{
	public void registeAllLayout()
	{
		registeLayout(typeof(ScriptLogin), LAYOUT_TYPE.LT_LOGIN, "UILogin");
		if (mLayoutManager.getLayoutCount() < (int)LAYOUT_TYPE.LT_MAX)
		{
			UnityUtility.logError("error : not all script added! max count : " + (int)LAYOUT_TYPE.LT_MAX + ", added count :" + mLayoutManager.getLayoutCount());
		}
	}
	//----------------------------------------------------------------------------------------------------------------------------------------------------------------
	protected void registeLayout(Type script, LAYOUT_TYPE layout, string name)
	{
		mLayoutManager.registeLayout(script, layout, name);
	}
}