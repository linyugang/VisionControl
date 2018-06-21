#include "stdafx.h"
#include "NativeShowUnit.h"

#include<QHash>
#include"GraphicsContext.h"
#include"HObjectEntry.h"
#include<QDebug>
#include<QMutexLocker>
#include"Util.h"
using namespace HalconCpp;

class ShowUnitData
{
public:

	// ********************************************************************************
	/// <summary>
	/// 显示区域初始化,修改默认值
	/// </summary>
	/// <param name="windowsId"></param>
	/// <created>linyugang,2018/4/10</created>
	/// <changed>linyugang,2018/4/10</changed>
	// ********************************************************************************
	void InitWindows(int windowsId)
	{
		NativeShowUnit::ChangeGraphicSettings(windowsId, "Color", "blue");
		NativeShowUnit::ChangeGraphicSettings(windowsId, "DrawMode", "margin");
		NativeShowUnit::ChangeGraphicSettings(windowsId, "LineWidth", 1);

	}
	//显示对象的属性
	GraphicsContext gc;
	//显示对象列表
	QList<HObjectEntry > hObjList;
	//控件背景颜色
	HTuple backgroundColor;
	//
	int imageWidth = 0;
	int imageHeight = 0;
	double currentTextSize = 0;
};
QHash<int, ShowUnitData>ShowUnitDataAll;
const int MAX_NUM_OBJ_LIST = 50;

QMutex mutexObj;
void showLogo(HTuple winID)
{
	/*HWndMessage ms("HYCONN", 2, 2, 10, "#1296db");
	ms.dispMessage(winID, "window", 1);*/
}
ShowUnitData* getShowUnitData(const HTuple & winID)
{
	int key = winID.I();
	if (ShowUnitDataAll.contains(key) == false)
	{
		ShowUnitDataAll.insert(key, ShowUnitData());
		ShowUnitDataAll[key].InitWindows(key);
	}
	return &(ShowUnitDataAll[key]);
}

void NativeShowUnit::AddIconicVar(const HalconCpp::HTuple & winID, const HalconCpp::HImage & img)
{
	ShowUnitData* data = getShowUnitData(winID);
	if (img.IsInitialized() == false)
	{
		return;
	}
	double r = 0, c = 0;
	Hlong h = 0, w = 0;
	Hlong area;
	//当前使用重载来判断是否图像,如果是hobject的图像,无法进入此处重载
	try
	{
		area = img.GetDomain().AreaCenter(&r, &c);
		img.GetImageSize(&w, &h);
		//面积=长*宽 表示确实是图片
		if (area == (w *h))
		{
			data->hObjList.clear();
		}//if
	}
	catch (HOperatorException)
	{
		qDebug() << "it is not image";
	}
	HObject tt = static_cast<HObject>(img);
	AddIconicVar(winID, tt);
}

void NativeShowUnit::AddIconicVar(const HalconCpp::HTuple & winID, const HalconCpp::HObject & obj)
{
	//Notify("test");
	ShowUnitData* data = getShowUnitData(winID);
	if (obj.IsInitialized() == false)
	{
		return;
	}

	//
	HObjectEntry entry(obj, data->gc.graphicalSettings);
	addEntry(winID, entry);
}

void NativeShowUnit::ShowText(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & message, int row, int colunm, int size, const HalconCpp::HTuple & color, const HalconCpp::HTuple & coordSystem)
{
	try
	{
		HWndMessage ms(message, row, colunm, size, color);
		ms.dispMessage(winID, QString(coordSystem.S().Text()), 1);
	}
	catch (HOperatorException)
	{
		qDebug() << "it is not image";
	}
	catch (const std::exception&)
	{

	}
	
}

void NativeShowUnit::AddText(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & message,
	int row, int colunm, int size, const HalconCpp::HTuple & color)
{
	HWndMessage ms(message, row, colunm, size, color);
	HObjectEntry entry(ms);

	addEntry(winID, entry);
}

void NativeShowUnit::AddText(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & message, int row, int colunm)
{
	HWndMessage ms(message, row, colunm);
	HObjectEntry entry(ms);

	addEntry(winID, entry);
}

void NativeShowUnit::ClearWindowData(const HalconCpp::HTuple & winID)
{
	//Util::Invoke("开始清除窗体数据");
	/*{
		QMutexLocker locker(&mutexObj);
	}*/
	ShowUnitData* data = getShowUnitData(winID);
	data->hObjList.clear();
	data->gc = GraphicsContext();
	data->backgroundColor = HTuple();
}

void NativeShowUnit::ClearEntryList(const HalconCpp::HTuple & winID)
{
	//Util::Invoke("开始清除实体数据");
	QMutexLocker locker(&mutexObj);
	ShowUnitData* data = getShowUnitData(winID);
	data->hObjList.clear();
	data->gc = GraphicsContext();
	//data->backgroundColor = HTuple();
}

void NativeShowUnit::Refresh(const HalconCpp::HTuple & winID, bool showImageOnly, bool isShowText, double scale)
{
	try
	{
		//Util::Invoke("显示刷新未开显示");
		QMutexLocker locker(&mutexObj);
		ShowUnitData* data = getShowUnitData(winID);
		//关闭显示刷新
		HSystem::SetSystem("flush_graphic", "false");
		//HalconCpp::GetWindowBackgroundImage
		SetDraw(winID, "margin");
		HalconCpp::ClearWindow(winID);
		data->gc.stateOfSettings.clear();
		int count = data->hObjList.count();
		for (int i = 0; i < count; i++)
		{
			if (i > 0 && showImageOnly)
			{
				break;
			}
			HObject objShow = data->hObjList[i].getHobj();
			HWndMessage messageShow = data->hObjList[i].getMessage();
			if (objShow.IsInitialized())
			{
				data->gc.applyContext(winID, data->hObjList[i].getGC());
				DispObj(objShow, winID);
			}
			else if (messageShow.IsInitialized() && isShowText)
			{
				data->currentTextSize = messageShow.dispMessage(winID, "image", scale, data->currentTextSize);
			}
		}
		showLogo(winID);
	}
	catch (const std::exception& ex)
	{
		QString dat= QString("图像刷新异常出现:%1").arg(ex.what());;
		Util::Invoke((dat.toLocal8Bit().data()));
		return;
	}
	catch (const HException& hex)
	{
		QString dat = QString("图像刷新异常出现:%1").arg(hex.ErrorMessage().Text());;
		Util::Invoke((dat.toLocal8Bit().data()));
		return;
	}
	
}

void NativeShowUnit::SaveImage(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & path)
{
	try
	{
		//Util::Invoke("开始保存图像");
		QMutexLocker locker(&mutexObj);
		ShowUnitData* data = getShowUnitData(winID);

		if (data->hObjList.count() == 0)
		{
			return;
		}
		HObject objShow = data->hObjList[0].getHobj();
		if (objShow.IsInitialized())
		{
			WriteImage(objShow, "bmp", 0, path);
		}
	}
	catch (HException)
	{
		Util::Invoke(("图像保存失败"));
		return;
	}
}

bool NativeShowUnit::IsEmpty(const HalconCpp::HTuple & winID)
{
	//Util::Invoke("判断是否为空");
	QMutexLocker locker(&mutexObj);
	ShowUnitData* data = getShowUnitData(winID);
	if (data->hObjList.count() == 0)
	{
		return true;
	}
	HObject objShow = data->hObjList[0].getHobj();
	if (objShow.IsInitialized() == false)
	{
		return true;
	}

	return false;
}

void NativeShowUnit::ChangeGraphicSettings(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & mode, const HalconCpp::HTuple & val)
{
	//Util::Invoke("开始修改gc");
	QMutexLocker locker(&mutexObj);
	ShowUnitData* data = getShowUnitData(winID);
	data->gc.setAttribute(QString(mode.S()), val);
}

void NativeShowUnit::SetImageSize(const HalconCpp::HTuple & winID, double imageWidth, double imageHeight)
{
	//Util::Invoke("开始设置图像尺寸");
	QMutexLocker locker(&mutexObj);
	ShowUnitData* data = getShowUnitData(winID);
	data->imageHeight = imageHeight;
	data->imageWidth = imageWidth;
}

void NativeShowUnit::SetBackgroundColor(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & color)
{
	ShowUnitData* data = getShowUnitData(winID);
	data->backgroundColor = color;
	//HTuple color = data.backgroundColor;
	//Notify(data.backgroundColor.S().Text());
	SetWindowParam(winID, "background_color", data->backgroundColor);
}

void NativeShowUnit::ShowHat(const HalconCpp::HTuple & winID, bool isShowCross)
{
	try
	{
		//Util::Invoke("开始显示结果hat");
		QMutexLocker locker(&mutexObj);
		ShowUnitData* data = getShowUnitData(winID);

		HSystem::SetSystem("flush_graphic", "true");


		//获取当前显示信息
		HTuple hv_Red, hv_Green, hv_Blue;
		HTuple hv_lineWidth;

		GetRgb(winID, &hv_Red, &hv_Green, &hv_Blue);
		GetLineWidth(winID, &hv_lineWidth);
		HTuple hv_Draw;
		GetDraw(winID, &hv_Draw);
		if (isShowCross == false || data->imageWidth == 0 || data->imageHeight == 0)
		{
			//初始化
			if (data->backgroundColor.Length() == 0)
			{
				//Notify(QString("init id=%1").arg(winID.I()));
				SetBackgroundColor(winID, "#262626");
			}
			SetColor(winID, data->backgroundColor);
			DispLine(winID, -100.0, -100.0, -101.0, -101.0);

		}
		else
		{
			SetDraw(winID, "margin");
			SetLineWidth(winID, 1);//设置线宽
			SetLineStyle(winID, HTuple());
			SetColor(winID, "green");//十字架显示颜色
			double CrossCol = (double)(data->imageWidth / 2),
				CrossRow = (double)(data->imageHeight / 2);
			//竖线
			HTuple row, col;
			row[0] = 0;
			row[1] = CrossRow - 50;
			col[0] = CrossCol;
			col[1] = CrossCol;
			DispPolygon(winID, row, col);

			row[0] = CrossRow + 50;
			row[1] = data->imageHeight;
			col[0] = CrossCol;
			col[1] = CrossCol;
			DispPolygon(winID, row, col);


			//中心点
			row[0] = CrossRow - 2;
			row[1] = CrossRow + 2;
			col[0] = CrossCol;
			col[1] = CrossCol;
			DispPolygon(winID, row, col);

			row[0] = CrossRow;
			row[1] = CrossRow;
			col[0] = CrossCol - 2;
			col[1] = CrossCol + 2;
			DispPolygon(winID, row, col);

			//DispPolygon(winID, CrossRow, CrossCol);
			//横线
			row[0] = CrossRow;
			row[1] = CrossRow;
			col[0] = 0;
			col[1] = CrossCol - 50;
			DispPolygon(winID, row, col);

			row[0] = CrossRow;
			row[1] = CrossRow;
			col[0] = CrossCol + 50;
			col[1] = data->imageWidth;
			DispPolygon(winID, row, col);
		}
		//恢复窗口显示信息
		SetRgb(winID, hv_Red, hv_Green, hv_Blue);
		SetLineWidth(winID, hv_lineWidth);
		SetDraw(winID, hv_Draw);
	}
	catch (const HException& e)
	{
		QString ta;
		ta.toLocal8Bit().data();
		Util::Invoke((QString("图像刷新失败:%1").arg(e.ErrorMessage().Text())).toLocal8Bit().data());
		return;
	}
	
}

std::string NativeShowUnit::GetPixMessage(const HalconCpp::HTuple & winID, HalconCpp::HTuple *currX, HalconCpp::HTuple*currY)
{
	try
	{
		//Util::Invoke("开始获取图像信息");
		QMutexLocker locker(&mutexObj);
		ShowUnitData* data = getShowUnitData(winID);
		HTuple y, x, state;
		GetMpositionSubPix(winID, currY, currX, &state);
		double x1 = currX->D();
		double y1 = currY->D();
		/*currX = &x;
		currY = &y1;*/
		if (data->hObjList.count() < 1)
		{
			return std::string();
		}
		HObject objShow = data->hObjList[0].getHobj();
		if (objShow.IsInitialized())
		{

			QString str_value = "";
			QString str_position = "";
			bool _isXOut = true, _isYOut = true;
			int channel_count;
			QString str_imgSize = QString("size:%1*%2").arg(data->imageWidth).arg(data->imageHeight);

			HTuple count;
			HalconCpp::CountChannels(objShow, &count);
			channel_count = count.I();
			str_position = QString("|row:%1 col:%2").arg((int)y1).arg((int)x1);;
			_isXOut = (x1 < 0 || x1 >= data->imageWidth);
			_isYOut = (y1 < 0 || y1 >= data->imageHeight);

			if (!_isXOut && !_isYOut)
			{
				if ((int)channel_count == 1)
				{
					double grayVal;
					HTuple gv;
					HalconCpp::GetGrayval(objShow, y1, x1, &gv);
					grayVal = gv.D();
					str_value = QString("|gray:%1").arg((int)grayVal);
				}
				else if ((int)channel_count == 3)
				{
					double grayValRed, grayValGreen, grayValBlue;

					HTuple gvr, gvg, gvb;
					HObject _RedChannel, _GreenChannel, _BlueChannel;
					AccessChannel(objShow, &_RedChannel, 1);
					AccessChannel(objShow, &_GreenChannel, 2);
					AccessChannel(objShow, &_BlueChannel, 3);

					HalconCpp::GetGrayval(_RedChannel, y1, x1, &gvr);
					HalconCpp::GetGrayval(_GreenChannel, y1, x1, &gvg);
					HalconCpp::GetGrayval(_BlueChannel, y1, x1, &gvb);

					grayValRed = gvr.D();
					grayValGreen = gvg.D();
					grayValBlue = gvb.D();
					str_value = QString("|rgb:%1,%2,%3").arg((int)grayValRed).arg((int)grayValGreen).arg((int)grayValBlue);
				}
				else
				{
					str_value = "";
				}
			}
			QString MousePosMessage = str_imgSize + str_position + str_value;
			return std::string(MousePosMessage.toLocal8Bit().data());
		}
	}
	catch (HOperatorException)
	{
		return std::string();
	}
	catch (HException)
	{
		return std::string();
	}
	return std::string();
}
HalconCpp::HTuple NativeShowUnit::GetGrayHisto(const HalconCpp::HTuple & winID, const HalconCpp::HTuple & rectangle1)
{
	ShowUnitData* data = getShowUnitData(winID);
	if (data->hObjList.count() < 1)
	{
		return HalconCpp::HTuple();
	}
	if (rectangle1.Length() != 4)
	{
		return HalconCpp::HTuple();
	}

	QMutexLocker locker(&mutexObj);
	HObject objShow = data->hObjList[0].getHobj();
	if (objShow.IsInitialized())
	{
		try
		{
			HObject ho_Rectangle;
			HTuple  hv_AbsoluteHisto, hv_RelativeHisto;

			HTuple channel;
			HalconCpp::CountChannels(objShow,&channel);
			if (channel == 3)
			{
				HalconCpp::Rgb1ToGray(objShow,&objShow);
			}
			HalconCpp::GenRectangle1(&ho_Rectangle, rectangle1[0], rectangle1[1], rectangle1[2], rectangle1[3]);
			HalconCpp::GrayHisto(ho_Rectangle, objShow, &hv_AbsoluteHisto, &hv_RelativeHisto);
			return hv_AbsoluteHisto;
		}
		catch (HOperatorException)
		{
			return HalconCpp::HTuple();
		}
	}
	return HalconCpp::HTuple();
}
void NativeShowUnit::addEntry(const HalconCpp::HTuple & winID, const HObjectEntry & entry)
{
	//Util::Invoke("1开始添加数据");
	QMutexLocker locker(&mutexObj);
	ShowUnitData* data = getShowUnitData(winID);
	data->hObjList.append(entry);
	if (data->hObjList.count() > MAX_NUM_OBJ_LIST)
	{
		data->hObjList.removeAt(1);
	}
}


