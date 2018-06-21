#include "stdafx.h"
#include "Util.h"
EventCallback Util::ms_callback = 0;
Util::Util()
{
}


Util::~Util()
{
}
void Util::RegisterCallback(EventCallback callback)
{
	//assert(0 != callback);
	ms_callback = callback;
}

void Util::Invoke(char* msg)
{
	//assert(0 != ms_callback);
	if (0 != ms_callback)
	{
		ms_callback(msg);
	}
	
}
