#pragma once
#include"Util.h"

using namespace System;
using namespace System::Runtime::InteropServices;

class Util;
namespace Yoga
{
	namespace Wrapper {
		
		public delegate void EventDelegate(String^ msg);

		public ref class UtilManaged
		{
		public:
			UtilManaged();
			~UtilManaged();
			EventDelegate^ NetCallback;
		private:
			void Callback(String^ msg);
			GCHandle delegateHandle;
			EventDelegate^ nativeCallback;
		};
	}
}