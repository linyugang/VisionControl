#include "stdafx.h"
#include "UtilManaged.h"
#include"Util.h"

Yoga::Wrapper::UtilManaged::UtilManaged()
{

	// 从成员函数创建一个委托  
	this->nativeCallback = gcnew EventDelegate(this, &UtilManaged::Callback);

	// 保证委托不会被内存移动和垃圾回收掉  
	this->delegateHandle = GCHandle::Alloc(this->nativeCallback);

	// 转换为函数指针注册  
	IntPtr ptr = Marshal::GetFunctionPointerForDelegate(this->nativeCallback);
	Util::RegisterCallback(static_cast<EventCallback>(ptr.ToPointer()));
}

Yoga::Wrapper::UtilManaged::~UtilManaged()
{
	// 释放委托句柄  
	if (this->delegateHandle.IsAllocated)
		this->delegateHandle.Free();
}

void Yoga::Wrapper::UtilManaged::Callback(String^ msg)
{
	if (NetCallback!= nullptr)
	{
		NetCallback(msg);
	}
	//Console::WriteLine("托管命令行输出: {0}", i);
	//throw gcnew System::NotImplementedException();
}
