
#include "stdafx.h"

#include "ShowUnit.h"
#include"NativeShowUnit.h"
void Yoga::Wrapper::ShowUnit::AddIconicVar(HTuple ^ winID, HImage ^ img)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HImage n_img = Conversion::ImageToNative(img);
	NativeShowUnit::AddIconicVar(n_winID, n_img);
}
void Yoga::Wrapper::ShowUnit::AddIconicVar(HTuple ^ winID, HObject ^ obj)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HObject n_obj = Conversion::ObjectToNative(obj);
	NativeShowUnit::AddIconicVar(n_winID, n_obj);
}

void Yoga::Wrapper::ShowUnit::ShowText(HTuple ^ winID, HTuple ^ message, int row, int colunm, int size, HTuple ^ color, HTuple^ coordSystem)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_message = Conversion::TupleToNative(message);
	HalconCpp::HTuple n_color = Conversion::TupleToNative(color);
	HalconCpp::HTuple n_coordSystem = Conversion::TupleToNative(coordSystem);
	NativeShowUnit::ShowText(n_winID, n_message, row, colunm, size, n_color, n_coordSystem);
}

void Yoga::Wrapper::ShowUnit::AddText(HTuple ^ winID, HTuple ^ message, int row, int colunm, int size, HTuple ^ color)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_message = Conversion::TupleToNative(message);
	HalconCpp::HTuple n_color = Conversion::TupleToNative(color);
	NativeShowUnit::AddText(n_winID, n_message, row, colunm, size, n_color);
}

void Yoga::Wrapper::ShowUnit::AddText(HTuple ^ winID, HTuple ^ message, int row, int colunm)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_message = Conversion::TupleToNative(message);
	NativeShowUnit::AddText(n_winID, n_message, row, colunm);
}

void Yoga::Wrapper::ShowUnit::ClearWindowData(HTuple ^ winID)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	NativeShowUnit::ClearWindowData(n_winID);
}

void Yoga::Wrapper::ShowUnit::ClearEntryList(HTuple ^ winID)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	NativeShowUnit::ClearEntryList(n_winID);
}

void Yoga::Wrapper::ShowUnit::Refresh(HTuple ^ winID, bool showImageOnly, bool isShowText, double scale)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	NativeShowUnit::Refresh(n_winID, showImageOnly, isShowText, scale);
}

void Yoga::Wrapper::ShowUnit::SaveImage(HTuple ^ winID, HTuple ^ path)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_path = Conversion::TupleToNative(path);
	NativeShowUnit::SaveImage(n_winID, n_path);
}

bool Yoga::Wrapper::ShowUnit::IsEmpty(HTuple ^ winID)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);

	return NativeShowUnit::IsEmpty(n_winID);
}

void Yoga::Wrapper::ShowUnit::ChangeGraphicSettings(HTuple ^ winID, HTuple ^ mode, HTuple ^ val)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_mode = Conversion::TupleToNative(mode);
	HalconCpp::HTuple n_val = Conversion::TupleToNative(val);
	NativeShowUnit::ChangeGraphicSettings(n_winID, n_mode, n_val);
}

void Yoga::Wrapper::ShowUnit::SetImageSize(HTuple ^ winID, double imageWidth, double imageHeight)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	NativeShowUnit::SetImageSize(n_winID, imageWidth, imageHeight);
}

void Yoga::Wrapper::ShowUnit::SetBackgroundColor(HTuple ^ winID, HTuple ^ color)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_color = Conversion::TupleToNative(color);
	NativeShowUnit::SetBackgroundColor(n_winID, n_color);
}

void Yoga::Wrapper::ShowUnit::ShowHat(HTuple ^ winID, bool isShowCross)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	NativeShowUnit::ShowHat(n_winID, isShowCross);
}
System::String^ Yoga::Wrapper::ShowUnit::GetPixMessage(HTuple ^ winID, [Out]HTuple^%   currX, [Out]HTuple^% currY)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	std::string n_data;
	HalconCpp::HTuple n_x, n_y;
	//double x, y;
	n_data = NativeShowUnit::GetPixMessage(n_winID, &n_x, &n_y);
	currX = Conversion::TupleToManaged(n_x);
	currY = Conversion::TupleToManaged(n_y);
	System::String^data = Conversion::StringToManged(n_data);
	return data;
}

HTuple ^ Yoga::Wrapper::ShowUnit::GetGrayHisto(HTuple ^ winID, HTuple ^ rectangle1)
{
	HalconCpp::HTuple n_winID = Conversion::TupleToNative(winID);
	HalconCpp::HTuple n_rectangle1 = Conversion::TupleToNative(rectangle1);
	HalconCpp::HTuple n_data;
	n_data = NativeShowUnit::GetGrayHisto(n_winID, n_rectangle1);
	HTuple ^data = Conversion::TupleToManaged(n_data);
	return data;
}
