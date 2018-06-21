#pragma once
#include "stdafx.h"  
//#include <assert.h>
#include<QTextCodec>
#define   GBK(s)   QTextCodec::codecForName("GBK")->toUnicode(s)
extern "C" __declspec(dllexport) typedef void(__stdcall* EventCallback)(char*);
class __declspec(dllexport) Util
{
public:
	Util();
	~Util();
	/// 注册回调函数  
	static void RegisterCallback(EventCallback callback);
	
	/// 调用注册的回调函数  
	static void Invoke(char* msg);
private:
	static EventCallback ms_callback;
};

