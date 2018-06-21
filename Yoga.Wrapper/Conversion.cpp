#include "StdAfx.h"
#include "Conversion.h"

using namespace System::Runtime::InteropServices;
#include <msclr\marshal_cppstd.h>  
using namespace msclr::interop;
// Conversions from HALCON/.NET to HALCON/C++

HalconCpp::HObject Conversion::ObjectToNative(HObject^ object)
{
	if (object == nullptr)
	{
		return HalconCpp::HObject();
	}
	return HalconCpp::HObject(KeyToNative(object->Key), TRUE);
}

HalconCpp::HImage Conversion::ImageToNative(HImage^ image)
{
	if (image==nullptr)
	{
		return HalconCpp::HImage();
	}
	return HalconCpp::HImage(KeyToNative(image->Key));
}

HalconCpp::HRegion Conversion::RegionToNative(HRegion^ region)
{
	System::Object^tmp = nullptr;
	if (region == tmp)
	{
		return HalconCpp::HRegion();
	}
	return HalconCpp::HRegion(KeyToNative(region->Key));
}

HalconCpp::HXLD Conversion::XLDToNative(HXLD^ xld)
{
	if (xld == nullptr)
	{
		return HalconCpp::HXLD();
	}
	return HalconCpp::HXLD(KeyToNative(xld->Key));
}

HalconCpp::HTuple Conversion::TupleToNative(HTuple^ tuple)
{
	System::Object^tmp = nullptr;
	if (tuple == tmp)
	{
		return HalconCpp::HTuple();
	}
	HalconCpp::HTuple result(tuple->Length, 0.0);

	for (int i = 0; i < tuple->Length; i++)
	{
		switch (tuple[i]->Type)
		{
		case HTupleType::INTEGER:
		case HTupleType::LONG:
			result[i] = (Hlong)tuple[i]->IP.ToPointer();
			break;
		case HTupleType::DOUBLE:
			result[i] = tuple[i]->D;
			break;
		case HTupleType::STRING:
			result[i] = (char*)Marshal::StringToHGlobalAnsi(tuple[i]->S).ToPointer();
			break;
		}
	}

	return result;
}



// Conversions from HALCON/C++ to HALCON/.NET

HObject^ Conversion::ObjectToManaged(const HalconCpp::HObject& object)
{
	return gcnew HObject(KeyToManaged(object.Key()), true);
}

HImage^ Conversion::ImageToManaged(const HalconCpp::HImage& image)
{
	return gcnew HImage(KeyToManaged(image.Key()), true);
}

HRegion^ Conversion::RegionToManaged(const HalconCpp::HRegion& region)
{
	return gcnew HRegion(KeyToManaged(region.Key()), true);
}

HXLD^ Conversion::XLDToManaged(const HalconCpp::HXLD& xld)
{
	return gcnew HXLD(KeyToManaged(xld.Key()), true);
}

HXLDCont ^ Conversion::HXLDContToManaged(const HalconCpp::HXLDCont & xld)
{
	return gcnew HXLDCont(KeyToManaged(xld.Key()), true);
}

HTuple^ Conversion::TupleToManaged(const HalconCpp::HTuple& tuple)
{
	// This is not optimized but data transfer between language
	// interfaces should also not occur very frequently.

	HTuple^ result = HTuple::TupleGenConst(tuple.Length(), 0.0);
	//HTuple^ result = gcnew HTuple();
	for (int i = 0; i < tuple.Length(); i++)
	{
		switch (tuple[i].Type())
		{
		case LONG_PAR:
			result[i]->IP = System::IntPtr((void*)tuple[i].L());
			break;
		case DOUBLE_PAR:
			result[i]->D = tuple[i].D();
			break;
		case STRING_PAR:
			result[i]->S = gcnew System::String(tuple[i].S());
			break;
		}
	}

	return result;
}

System::String ^ Conversion::StringToManged(const std::string & string)
{
	System::String^ result = marshal_as<System::String^>(string);
	return result;
}


// Support functionality for implementing the conversions

System::IntPtr Conversion::KeyToManaged(Hkey key)
{
	return System::IntPtr((void*)key);
}

Hkey Conversion::KeyToNative(System::IntPtr handle)
{
	return (Hkey)handle.ToPointer();
}
