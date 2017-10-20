﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class txUIStaticTextureMaskCut : txUIStaticTexture
{
	protected Texture mMask;
	protected Vector2 mMaskSize;
	public txUIStaticTextureMaskCut()
	{
		;
	}
	public override void init(GameLayout layout, GameObject go)
	{
		base.init(layout, go);
	}
	public void setMaskTexture(Texture mask) { mMask = mask; }
	public void setMaskSize(Vector2 size) { mMaskSize = size; }
	//---------------------------------------------------------------------------------------------------
	protected override void applyShader(Material mat)
	{
		base.applyShader(mat);
		if (mat != null && mat.shader != null)
		{
			string shaderName = mat.shader.name;
			if (shaderName == "MaskCut")
			{
				mat.SetTexture("_MaskTex", mMask);
				mat.SetFloat("_SizeX", mMaskSize.x);
				mat.SetFloat("_SizeY", mMaskSize.y);
			}
		}
	}
}