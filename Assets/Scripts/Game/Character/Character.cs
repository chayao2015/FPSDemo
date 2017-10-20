using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Character : MovableObject
{
	protected CHARACTER_TYPE	mCharacterType;// 角色类型
	protected CharacterData		mCharacterData;	//玩家数据
	public Character(CHARACTER_TYPE type, string name)
		:
		base(name)
	{
		mCharacterType = type;
		mCharacterData = null;
		// 需要将角色挂接到角色管理器下
	}
	public virtual void init(int id)
	{
		if (null == mCharacterData)
		{
			mCharacterData = new CharacterData();
		}
		mCharacterData.mGUID = id;
		mCharacterData.mName = mName;
		initComponents();
	}
	public override void initComponents()
	{
	}
	public override void update(float elaspedTime)
	{
		// 先更新自己的所有组件
		base.updateComponents(elaspedTime);
	}
	public virtual void notifyComponentChanged(GameComponent component) {}
	public CharacterData getCharacterData() { return mCharacterData; }
	public CHARACTER_TYPE getType() { return mCharacterType; }
}
