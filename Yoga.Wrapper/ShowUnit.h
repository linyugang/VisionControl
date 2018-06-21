#pragma once

#include "Conversion.h"
using namespace HalconDotNet;
using namespace System::Runtime::InteropServices;
using namespace System::Runtime::Serialization;
using namespace System;
namespace Yoga {
	namespace Wrapper {
		public ref class ShowUnit
		{
		public:

			/// ********************************************************************************
			/// <summary>
			/// 添加图像变量到显示窗体,需要刷新才能显示
			/// </summary>
			/// <param name="winID">显示控件id</param>
			/// <param name="img">图像对象</param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void AddIconicVar(HTuple^ winID, HImage^ img);
			/// ********************************************************************************
			/// <summary>
			/// 添加图形变量到显示窗体,需要刷新才能显示
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="obj"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void AddIconicVar(HTuple^ winID, HObject^ obj);

			/// ********************************************************************************
			/// <summary>
			/// 立即显示文字信息
			/// </summary>
			/// <param name="winID">窗口id</param>
			/// <param name="message"></param>
			/// <param name="row"></param>
			/// <param name="colunm"></param>
			/// <param name="size"></param>
			/// <param name="color"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void ShowText(HTuple^ winID, HTuple^ message, int row, int colunm, int size, HTuple^ color, HTuple^ coordSystem);
			/// ********************************************************************************
			/// <summary>
			/// 添加文字对象到显示窗体
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="message"></param>
			/// <param name="row"></param>
			/// <param name="colunm"></param>
			/// <param name="size"></param>
			/// <param name="color"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void AddText(HTuple^ winID, HTuple^ message, int row, int colunm, int size, HTuple^ color);
			// ********************************************************************************
			/// <summary>
			/// 添加文字对象到显示窗体
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="message"></param>
			/// <param name="row"></param>
			/// <param name="colunm"></param>
			/// <created>linyugang,2018/4/10</created>
			/// <changed>linyugang,2018/4/10</changed>
			// ********************************************************************************
			static void AddText(HTuple^ winID, HTuple^ message, int row, int colunm);			
			/// ********************************************************************************
			/// <summary>
			/// 清除显示窗体所对应的所有数据
			/// </summary>
			/// <param name="winID"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void ClearWindowData(HTuple^ winID);
			/// ********************************************************************************
			/// <summary>
			/// 清除显示窗体内的显示对象 不包含roi 
			/// </summary>
			/// <param name="winID"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void ClearEntryList(HTuple^ winID);
			/// ********************************************************************************
			/// <summary>
			/// 刷新显示窗体-可以添加多个显示对象后调用,鼠标操作会自动触发该方法
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="showImageOnly"></param>
			/// <param name="isShowText"></param>
			/// <param name="scale"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void Refresh(HTuple^ winID, bool showImageOnly, bool isShowText, double scale);
			/// ********************************************************************************
			/// <summary>
			/// 保存窗体内的原始图像对象
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="path"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void SaveImage(HTuple^  winID, HTuple^  path);
			/// ********************************************************************************
			/// <summary>
			/// 窗体内的图像是否为空
			/// </summary>
			/// <param name="winID"></param>
			/// <returns></returns>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static bool IsEmpty(HTuple^  winID);

			/// ********************************************************************************
			/// <summary>
			/// 修改窗体对应的显示效果
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="mode"></param>
			/// <param name="val"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void ChangeGraphicSettings(HTuple^ winID, HTuple^ mode, HTuple^ val);

			/// ********************************************************************************
			/// <summary>
			/// 设置显示控件内的图像尺寸
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="imageWidth"></param>
			/// <param name="imageHeight"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void SetImageSize(HTuple^ winID, double imageWidth, double imageHeight);
			/// ********************************************************************************
			/// <summary>
			/// 设置背景颜色
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="color"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void SetBackgroundColor(HTuple^ winID, HTuple^ color);
			/// ********************************************************************************
			/// <summary>
			/// 显示最后的十字架等
			/// </summary>
			/// <param name="winID"></param>
			/// <param name="isShowCross"></param>
			/// <created>linyugang,2018/3/4</created>
			/// <changed>linyugang,2018/3/4</changed>
			/// ********************************************************************************
			static void ShowHat(HTuple^ winID, bool isShowCross);

			// ********************************************************************************
			/// <summary>
			/// 获取当前鼠标位置信息
			/// </summary>
			/// <param name="winID">图像窗口</param>
			/// <param name="currX">当前x坐标</param>
			/// <param name="currY">当前y坐标</param>
			/// <returns>包含坐标及灰度信息等的文本</returns>
			/// <created>linyugang,2018/4/10</created>
			/// <changed>linyugang,2018/4/10</changed>
			// ********************************************************************************
			
			static System::String^ GetPixMessage(HTuple^ winID, [Out]HTuple^%   currX, [Out]HTuple^% currY);
			// ********************************************************************************
			/// <summary>
			/// 获取窗口内图像指定矩形区域内的直方图数据
			/// </summary>
			/// <param name="winID">窗口id</param>
			/// <param name="rectangle1">矩形坐标 rectangle1</param>
			/// <returns>灰度直方图 若无直方图信息则为空htuple</returns>
			/// <created>linyugang,2018/4/11</created>
			/// <changed>linyugang,2018/4/11</changed>
			// ********************************************************************************
			static HTuple^  GetGrayHisto(HTuple^ winID, HTuple^ rectangle1);
		};
	}
}
