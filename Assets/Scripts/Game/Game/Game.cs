using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;

public class Game : GameFramework
{
	protected GameUtility mGameUtility;
	protected GameConfig mGameConfig;
	public override void start()
	{
		base.start();
		mGameConfig = new GameConfig();
		mGameUtility = new GameUtility();
	}
	public override void notifyBase()
	{
		base.notifyBase();
		// 所有类都构造完成后通知GameBase
		GameBase frameBase = new GameBase();
		frameBase.notifyConstructDone();
	}
	public override void registe()
	{
		LayoutRegister layoutRegister = new LayoutRegister();
		layoutRegister.registeAllLayout();
		GameSceneRegister sceneRegister = new GameSceneRegister();
		sceneRegister.registerAllGameScene();
		CharacterRegister characterRegiste = new CharacterRegister();
		characterRegiste.registeAllCharacter();
	}
	public override void init()
	{
		base.init();
		mGameConfig.init();
		mGameUtility.init();
	}
	public override void launch()
	{
		base.launch();
		CommandGameSceneManagerEnter cmd = mCommandSystem.newCmd<CommandGameSceneManagerEnter>(false, false);
		cmd.mSceneType = GAME_SCENE_TYPE.GST_LOGIN;
		mCommandSystem.pushCommand(cmd, mGameSceneManager);
	}
	public override void update(float elapsedTime)
	{
		base.update(elapsedTime);
	}
	public override void destroy()
	{
		mGameConfig.destory();
		mGameConfig = null;
		mGameUtility = null;
		// 最后调用基类的destroy,确保资源被释放完毕
		base.destroy();
	}
	public GameConfig getGameConfig() { return mGameConfig; }
}