#pragma once
#  include "HalconCpp.h"
#include <QString>
class HWndMessage
{
public:
	HWndMessage();
	HWndMessage(HalconCpp::HTuple message, int row, int colunm, int size, HalconCpp::HTuple color);
	HWndMessage(HalconCpp::HTuple message, int row, int colunm);
	~HWndMessage();
	double dispMessage(HalconCpp::HTuple winID, QString coordSystem, double zoom,double currentSize);
	void dispMessage(HalconCpp::HTuple winID, QString coordSystem, double zoom);
	bool IsInitialized() const;
	HWndMessage clone() const;
private:
	HalconCpp::HTuple message;
	int size = 16;
	int row;
	int colunm;
	HalconCpp::HTuple color = "green";
	bool isInitialized;
	void dispMessage(HalconCpp::HTuple hv_WindowHandle, HalconCpp::HTuple hv_String, HalconCpp::HTuple hv_CoordSystem,
		HalconCpp::HTuple hv_Row, HalconCpp::HTuple hv_Column, HalconCpp::HTuple hv_Color, HalconCpp::HTuple hv_Box);
	void setDisplayFont(HalconCpp::HTuple hv_WindowHandle, HalconCpp::HTuple hv_Size, HalconCpp::HTuple hv_Font, HalconCpp::HTuple hv_Bold,
		HalconCpp::HTuple hv_Slant);

};



