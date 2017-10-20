using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 游戏枚举定义-----------------------------------------------------------------------------------------------
// 界面布局定义
public enum LAYOUT_TYPE
{
	LT_LOGIN,
	LT_MAX,
}
// 所有的音效定义
public enum SOUND_DEFINE
{
	SD_MIN = -1,
	SD_MAX,
};
// 数据库表格类型
public enum DATA_TYPE
{
	DT_GAME_SOUND,
	DT_MAX,
}

// 场景的类型
public enum GAME_SCENE_TYPE
{
	GST_LOGIN,
	GST_MAIN_HALL,
	GST_MAX,
};
// 游戏场景流程类型
public enum PROCEDURE_TYPE
{
	PT_NONE,

	PT_LOGIN_MIN,
	PT_LOGIN_START,
	PT_LOGIN_EXIT,
	PT_LOGIN_MAX,

	PT_MAIN_HALL_MIN,
	PT_MAIN_HALL_START,
	PT_MAIN_HALL_EXIT,
	PT_MAIN_HALL_MAX,
};
// 游戏中的公共变量定义
public enum GAME_DEFINE_FLOAT
{
	GDF_NONE,
	// 应用程序配置参数
	GDF_APPLICATION_MIN,
	GDF_FULL_SCREEN,				// 是否全屏,0为窗口模式,1为全屏,2为无边框窗口
	GDF_SCREEN_WIDTH,				// 分辨率的宽
	GDF_SCREEN_HEIGHT,				// 分辨率的高
	GDF_SCREEN_COUNT,				// 显示屏数量,用于多屏横向组合为高分辨率
	GDF_USE_FIXED_TIME,				// 是否将每帧的时间固定下来
	GDF_FIXED_TIME,					// 每帧的固定时间,单位秒
	GDF_APPLICATION_MAX,

	// 框架配置参数
	GDF_FRAME_MIN,
	GDF_SOCKET_PORT,                // socket端口
	GDF_BROADCAST_PORT,             // 广播端口
	GDF_SHOW_COMMAND_DEBUG_INFO,    // 是否输出显示命令调试信息,0为不显示,1为显示
	GDF_LOAD_RESOURCES,             // 游戏加载资源的路径,0代表在Resources中读取,1代表从AssetBundle中读取
	GDF_LOG_LEVEL,                  // 日志输出等级
	GDF_FRAME_MAX,

	// 游戏配置参数
	GDF_GAME_MIN,
	GDF_GAME_MAX,
};
public enum GAME_DEFINE_STRING
{
	GDS_NONE,
	// 应用程序配置参数
	GDS_APPLICATION_MIN,
	GDS_APPLICATION_MAX,
	// 框架配置参数
	GDS_FRAME_MIN,
	GDS_FRAME_MAX,
	// 游戏配置参数
	GDS_GAME_MIN,
	GDS_GAME_MAX,
};

// 游戏常量定义-------------------------------------------------------------------------------------------------------------
public class GameDefine : CommonDefine
{
	//-----------------------------------------------------------------------------------------------------------------
}