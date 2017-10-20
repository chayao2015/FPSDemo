using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class SceneManager : GameBase
{
	public SceneManager()
	{
		;
	}
	public void init()
	{
		;
	}
	public void destroy()
	{
		;
	}
	public void loadScene(string sceneName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
}
