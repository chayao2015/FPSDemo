using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ScriptLogin : LayoutScript
{
	protected txUIStaticSprite mBackground;
	protected txUIEditbox mAccountEdit;
	protected txUIEditbox mPasswordEdit;
	protected txUIButton mLoginButton;
	protected txUIButton mRegisterButton;
	protected txUIButton mQuitButton;
	public ScriptLogin(LAYOUT_TYPE type, string name, GameLayout layout)
		:
		base(type, name, layout)
	{
		;
	}
	public override void assignWindow()
	{
		mBackground = newObject<txUIStaticSprite>("Background");
		mAccountEdit = newObject<txUIEditbox>(mBackground, "AccountEdit");
		mPasswordEdit = newObject<txUIEditbox>(mBackground, "PasswordEdit");
		mLoginButton = newObject<txUIButton>(mBackground, "LoginButton");
		mRegisterButton = newObject<txUIButton>(mBackground, "RegisterButton");
		mQuitButton = newObject<txUIButton>(mBackground, "QuitButton");
	}
	public override void init()
	{
		mLoginButton.setClickCallback(onLoginClick);
		mLoginButton.setPressCallback(onButtonPress);
		mRegisterButton.setClickCallback(onRegisterClick);
		mRegisterButton.setPressCallback(onButtonPress);
		mQuitButton.setClickCallback(onQuitClick);
		mQuitButton.setPressCallback(onButtonPress);
	}
	public override void onReset()
	{
		;
	}
	public override void onShow(bool immediately, string param)
	{
		;
	}
	public override void onHide(bool immediately, string param)
	{
		;
	}
	public override void update(float elapsedTime)
	{
		;
	}
	//---------------------------------------------------------------------------------------------------------------------
	protected void onLoginClick(GameObject obj)
	{
		CommandGameSceneManagerEnter cmd = mCommandSystem.newCmd<CommandGameSceneManagerEnter>();
		cmd.mSceneType = GAME_SCENE_TYPE.GST_MAIN_HALL;
		mCommandSystem.pushCommand(cmd, mGameSceneManager);
	}
	protected void onButtonPress(GameObject button, bool press)
	{
		txUIObject obj = mLayout.getUIObject(button);
		LayoutTools.SCALE_WINDOW(obj, obj.getScale(), press ? new Vector2(1.2f, 1.2f) : Vector2.one, 0.2f);
	}
	protected void onRegisterClick(GameObject button)
	{
		CommandGameSceneManagerEnter cmd = mCommandSystem.newCmd<CommandGameSceneManagerEnter>();
		cmd.mSceneType = GAME_SCENE_TYPE.GST_MAIN_HALL;
		mCommandSystem.pushCommand(cmd, mGameSceneManager);
	}
	protected void onQuitClick(GameObject button)
	{
		mGameFramework.stop();
	}
}