#include "stdafx.h"
#include "GraphicsContext.h"
#  include "HalconCpp.h"
using namespace HalconCpp;

QString  GraphicsContext::GC_COLOR = "Color";
QString  GraphicsContext::GC_COLORED = "Colored";
QString  GraphicsContext::GC_LINEWIDTH = "LineWidth";
QString  GraphicsContext::GC_DRAWMODE = "DrawMode";
QString  GraphicsContext::GC_SHAPE = "Shape";
QString  GraphicsContext::GC_LUT = "Lut";
QString  GraphicsContext::GC_PAINT = "Paint";
QString  GraphicsContext::GC_LINESTYLE = "LineStyle";


GraphicsContext::GraphicsContext()
{
}

void GraphicsContext::applyContext(const HalconCpp::HTuple & winID, const QHash<QString, HalconCpp::HTuple>& cContext)
{
	using namespace HalconCpp;
	HTuple valH;

	QList<QString> allKey = cContext.keys();
	foreach(QString key, allKey)
	{
		//如果属性相同就跳出循环
		if (stateOfSettings.contains(key) && stateOfSettings[key] == cContext[key])
		{
			continue;
		}
		valH = cContext[key];
		if (key == GC_COLOR)
		{
			SetColor(winID, valH);
		}
		else if (key == GC_COLORED)
		{
			SetColored(winID, valH);
		}
		else if (key == GC_DRAWMODE)
		{
			SetDraw(winID, valH);
		}
		else if (key == GC_LINEWIDTH)
		{
			SetLineWidth(winID, valH);
		}
		else if (key == GC_LUT)
		{
			SetLut(winID, valH);
		}
		else if (key == GC_PAINT)
		{
			SetPaint(winID, valH);
		}
		else if (key == GC_SHAPE)
		{
			SetShape(winID, valH);
		}
		else if (key == GC_LINESTYLE)
		{
			SetLineStyle(winID, valH);
		}
		//将当前属性添加到系统状态中
		if (stateOfSettings.contains(key))
		{
			stateOfSettings[key] = valH;
		}
		else
		{
			stateOfSettings.insert(key, valH);
		}
	}
}

void GraphicsContext::setAttribute(const QString & key, const HalconCpp::HTuple & val)
{
	if (graphicalSettings.contains(key))
		graphicalSettings[key] = val;
	else
		graphicalSettings.insert(key, val);
}

