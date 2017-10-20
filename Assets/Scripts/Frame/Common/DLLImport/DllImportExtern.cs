﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class DllImportExtern
{
	protected static Dictionary<string, Dll> mDllLibraryList = new Dictionary<string, Dll>();
	
	//将要执行的函数转换为委托
	public static Delegate Invoke(string library, string funcName, Type t)
	{
		if (mDllLibraryList.ContainsKey(library))
		{
			return mDllLibraryList[library].getFunction(funcName, t);
		}
		return null;
	}
	public void init()
	{
		registerDLL(Winmm.WINMM_DLL);
	}
	public void destroy()
	{
		foreach (var library in mDllLibraryList)
		{
			library.Value.destroy();
		}
		mDllLibraryList.Clear();
	}
	protected void registerDLL(string name)
	{
		Dll dll = new Dll();
		dll.init(name);
		mDllLibraryList.Add(dll.getName(), dll);
	}
}