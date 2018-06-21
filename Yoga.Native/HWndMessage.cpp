#include "stdafx.h"
#include "HWndMessage.h"
#  include "HalconCpp.h"
using namespace HalconCpp;
#include"Util.h"
HWndMessage::HWndMessage()
{
	isInitialized = false;
}

HWndMessage::HWndMessage(HTuple message, int row, int colunm, int size, HTuple color)
{
	this->message = message;
	this->size = size;
	this->row = row;
	this->colunm = colunm;
	this->color = color;
	isInitialized = true;
}

HWndMessage::HWndMessage(HTuple message, int row, int colunm)
{
	this->message = message;
	this->row = row;
	this->colunm = colunm;
	isInitialized = true;
}



HWndMessage::~HWndMessage()
{
}
double HWndMessage::dispMessage(HTuple winID, QString coordSystem, double zoom, double currentSize)
{
	double sizeTmp = size* zoom;
	if (sizeTmp != currentSize)
	{
		try
		{
			setDisplayFont(winID, sizeTmp, "serif", "true", "false");
		}
		catch (const std::exception&)
		{
				
		}
		catch (const HException& /*hex*/)
		{
			/*QString dat = GBK("图像刷新异常出现:")+QString("%1").arg(hex.ErrorMessage().Text());;
			Util::Invoke((dat.toLocal8Bit().data()));*/
		}
	}

	dispMessage(winID, message, coordSystem.toLocal8Bit().data(), row, colunm, color, "false");
	return sizeTmp;
}

void HWndMessage::dispMessage(HalconCpp::HTuple winID, QString coordSystem, double zoom)
{
	double sizeTmp = size* zoom;

	setDisplayFont(winID, sizeTmp, "serif", "true", "false");

	dispMessage(winID, message, coordSystem.toLocal8Bit().data(), row, colunm, color, "false");
}

bool HWndMessage::IsInitialized() const
{
	return isInitialized;
}

HWndMessage HWndMessage::clone() const
{
	HWndMessage tmp = HWndMessage(this->message, this->row, this->colunm,
		this->size, this->color);
	return tmp;
}

void HWndMessage::dispMessage(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
	HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
{

	// Local iconic variables

	// Local control variables
	HTuple  hv_GenParamName, hv_GenParamValue;

	//This procedure displays text in a graphics window.
	//
	//Input parameters:
	//WindowHandle: The WindowHandle of the graphics window, where
	//   the message should be displayed
	//String: A tuple of strings containing the text message to be displayed
	//CoordSystem: If set to 'window', the text position is given
	//   with respect to the window coordinate system.
	//   If set to 'image', image coordinates are used.
	//   (This may be useful in zoomed images.)
	//Row: The row coordinate of the desired text position
	//   A tuple of values is allowed to display text at different
	//   positions.
	//Column: The column coordinate of the desired text position
	//   A tuple of values is allowed to display text at different
	//   positions.
	//Color: defines the color of the text as string.
	//   If set to [], '' or 'auto' the currently set color is used.
	//   If a tuple of strings is passed, the colors are used cyclically...
	//   - if |Row| == |Column| == 1: for each new textline
	//   = else for each text position.
	//Box: If Box[0] is set to 'true', the text is written within an orange box.
	//     If set to' false', no box is displayed.
	//     If set to a color string (e.g. 'white', '#FF00CC', etc.),
	//       the text is written in a box of that color.
	//     An optional second value for Box (Box[1]) controls if a shadow is displayed:
	//       'true' -> display a shadow in a default color
	//       'false' -> display no shadow
	//       otherwise -> use given string as color string for the shadow color
	//
	//It is possible to display multiple text strings in a single call.
	//In this case, some restrictions apply:
	//- Multiple text positions can be defined by specifying a tuple
	//  with multiple Row and/or Column coordinates, i.e.:
	//  - |Row| == n, |Column| == n
	//  - |Row| == n, |Column| == 1
	//  - |Row| == 1, |Column| == n
	//- If |Row| == |Column| == 1,
	//  each element of String is display in a new textline.
	//- If multiple positions or specified, the number of Strings
	//  must match the number of positions, i.e.:
	//  - Either |String| == n (each string is displayed at the
	//                          corresponding position),
	//  - or     |String| == 1 (The string is displayed n times).
	//
	//
	//Convert the parameters for disp_text.
	if (0 != (HTuple(hv_Row == HTuple()).TupleOr(hv_Column == HTuple())))
	{
		return;
	}
	if (0 != (hv_Row == -1))
	{
		hv_Row = 12;
	}
	if (0 != (hv_Column == -1))
	{
		hv_Column = 12;
	}
	//
	//Convert the parameter Box to generic parameters.
	hv_GenParamName = HTuple();
	hv_GenParamValue = HTuple();
	if (0 != ((hv_Box.TupleLength()) > 0))
	{
		if (0 != (HTuple(hv_Box[0]) == HTuple("false")))
		{
			//Display no box
			hv_GenParamName = hv_GenParamName.TupleConcat("box");
			hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
		}
		else if (0 != (HTuple(hv_Box[0]) != HTuple("true")))
		{
			//Set a color other than the default.
			hv_GenParamName = hv_GenParamName.TupleConcat("box_color");
			hv_GenParamValue = hv_GenParamValue.TupleConcat(HTuple(hv_Box[0]));
		}
	}
	if (0 != ((hv_Box.TupleLength()) > 1))
	{
		if (0 != (HTuple(hv_Box[1]) == HTuple("false")))
		{
			//Display no shadow.
			hv_GenParamName = hv_GenParamName.TupleConcat("shadow");
			hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
		}
		else if (0 != (HTuple(hv_Box[1]) != HTuple("true")))
		{
			//Set a shadow color other than the default.
			hv_GenParamName = hv_GenParamName.TupleConcat("shadow_color");
			hv_GenParamValue = hv_GenParamValue.TupleConcat(HTuple(hv_Box[1]));
		}
	}
	//Restore default CoordSystem behavior.
	if (0 != (hv_CoordSystem != HTuple("window")))
	{
		hv_CoordSystem = "image";
	}
	//
	if (0 != (hv_Color == HTuple("")))
	{
		//disp_text does not accept an empty string for Color.
		hv_Color = HTuple();
	}
	//
	DispText(hv_WindowHandle, hv_String, hv_CoordSystem, hv_Row, hv_Column, hv_Color,
		hv_GenParamName, hv_GenParamValue);
	return;
}

void HWndMessage::setDisplayFont(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold,
	HTuple hv_Slant)
{

	// Local iconic variables

	// Local control variables
	HTuple  hv_OS, hv_Fonts, hv_Style, hv_Exception;
	HTuple  hv_AvailableFonts, hv_Fdx, hv_Indices;

	//This procedure sets the text font of the current window with
	//the specified attributes.
	//
	//Input parameters:
	//WindowHandle: The graphics window for which the font will be set
	//Size: The font size. If Size=-1, the default of 16 is used.
	//Bold: If set to 'true', a bold font is used
	//Slant: If set to 'true', a slanted font is used
	//
	GetSystem("operating_system", &hv_OS);
	// dev_get_preferences(...); only in hdevelop
	// dev_set_preferences(...); only in hdevelop
	if (0 != (HTuple(hv_Size == HTuple()).TupleOr(hv_Size == -1)))
	{
		hv_Size = 16;
	}
	if (0 != ((hv_OS.TupleSubstr(0, 2)) == HTuple("Win")))
	{
		//Restore previous behaviour
		hv_Size = (1.13677*hv_Size).TupleInt();
	}
	if (0 != (hv_Font == HTuple("Courier")))
	{
		hv_Fonts.Clear();
		hv_Fonts[0] = "Courier";
		hv_Fonts[1] = "Courier 10 Pitch";
		hv_Fonts[2] = "Courier New";
		hv_Fonts[3] = "CourierNew";
	}
	else if (0 != (hv_Font == HTuple("mono")))
	{
		hv_Fonts.Clear();
		hv_Fonts[0] = "Consolas";
		hv_Fonts[1] = "Menlo";
		hv_Fonts[2] = "Courier";
		hv_Fonts[3] = "Courier 10 Pitch";
		hv_Fonts[4] = "FreeMono";
	}
	else if (0 != (hv_Font == HTuple("sans")))
	{
		hv_Fonts.Clear();
		hv_Fonts[0] = "Luxi Sans";
		hv_Fonts[1] = "DejaVu Sans";
		hv_Fonts[2] = "FreeSans";
		hv_Fonts[3] = "Arial";
	}
	else if (0 != (hv_Font == HTuple("serif")))
	{
		hv_Fonts.Clear();
		hv_Fonts[0] = "Times New Roman";
		hv_Fonts[1] = "Luxi Serif";
		hv_Fonts[2] = "DejaVu Serif";
		hv_Fonts[3] = "FreeSerif";
		hv_Fonts[4] = "Utopia";
	}
	else
	{
		hv_Fonts = hv_Font;
	}
	hv_Style = "";
	if (0 != (hv_Bold == HTuple("true")))
	{
		hv_Style += HTuple("Bold");
	}
	else if (0 != (hv_Bold != HTuple("false")))
	{
		hv_Exception = "Wrong value of control parameter Bold";
		throw HalconCpp::HException(hv_Exception);
	}
	if (0 != (hv_Slant == HTuple("true")))
	{
		hv_Style += HTuple("Italic");
	}
	else if (0 != (hv_Slant != HTuple("false")))
	{
		hv_Exception = "Wrong value of control parameter Slant";
		throw HalconCpp::HException(hv_Exception);
	}
	if (0 != (hv_Style == HTuple("")))
	{
		hv_Style = "Normal";
	}
	QueryFont(hv_WindowHandle, &hv_AvailableFonts);
	hv_Font = "";
	{
		HTuple end_val48 = (hv_Fonts.TupleLength()) - 1;
		HTuple step_val48 = 1;
		for (hv_Fdx = 0; hv_Fdx.Continue(end_val48, step_val48); hv_Fdx += step_val48)
		{
			hv_Indices = hv_AvailableFonts.TupleFind(HTuple(hv_Fonts[hv_Fdx]));
			if (0 != ((hv_Indices.TupleLength()) > 0))
			{
				if (0 != (HTuple(hv_Indices[0]) >= 0))
				{
					hv_Font = HTuple(hv_Fonts[hv_Fdx]);
					break;
				}
			}
		}
	}
	if (0 != (hv_Font == HTuple("")))
	{
		throw HalconCpp::HException("Wrong value of control parameter Font");
	}
	hv_Font = (((hv_Font + "-") + hv_Style) + "-") + hv_Size;
	SetFont(hv_WindowHandle, hv_Font);
	// dev_set_preferences(...); only in hdevelop
	return;
}
