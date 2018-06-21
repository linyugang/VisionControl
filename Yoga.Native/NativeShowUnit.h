#pragma once

#include "HalconCpp.h"
#include"HObjectEntry.h"
class __declspec(dllexport) NativeShowUnit
{
public:
	static void AddIconicVar(const HalconCpp::HTuple& winID, const HalconCpp::HImage& img);
	static void AddIconicVar(const HalconCpp::HTuple& winID, const HalconCpp::HObject& obj);

	static void ShowText(const HalconCpp::HTuple& winID, const HalconCpp::HTuple& message, int row, int colunm, int size, const HalconCpp::HTuple& color,  const HalconCpp::HTuple & coordSystem);
	static void AddText(const HalconCpp::HTuple& winID, const HalconCpp::HTuple& message, int row, int colunm, int size, const HalconCpp::HTuple& color);
	static void AddText(const HalconCpp::HTuple& winID, const HalconCpp::HTuple& message, int row, int colunm);
	
	
	static void ClearWindowData(const HalconCpp::HTuple& winID);
	static void ClearEntryList(const HalconCpp::HTuple& winID);
	static void Refresh(const HalconCpp::HTuple& winID, bool showImageOnly, bool isShowText, double scale);
	static void SaveImage(const HalconCpp::HTuple& winID, const HalconCpp::HTuple& path);
	static bool IsEmpty(const HalconCpp::HTuple& winID);
	static void ChangeGraphicSettings(const HalconCpp::HTuple& winID, const HalconCpp::HTuple& mode, const HalconCpp::HTuple& val);

	static void SetImageSize(const HalconCpp::HTuple & winID, double imageWidth, double imageHeight);
	static void SetBackgroundColor(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & color);
	static void ShowHat(const HalconCpp::HTuple& winID, bool isShowCross);
	static std::string GetPixMessage(const HalconCpp::HTuple& winID, HalconCpp::HTuple *currX, HalconCpp::HTuple*currY);
	static HalconCpp::HTuple  GetGrayHisto(const HalconCpp::HTuple& winID, const HalconCpp::HTuple& rectangle1);
private:
	static void addEntry(const HalconCpp::HTuple & winID, const HObjectEntry & entry);
};



