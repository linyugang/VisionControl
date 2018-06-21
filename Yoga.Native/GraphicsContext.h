#pragma once
#include "HalconCpp.h"
#include <QString>
#include <QVariantHash>
#include<QHash>
class GraphicsContext
{
public:
	static   QString GC_COLOR;
	static  QString GC_COLORED;
	static  QString GC_LINEWIDTH;
	static  QString GC_DRAWMODE;
	static  QString GC_SHAPE;
	static  QString GC_LUT;
	static  QString GC_PAINT;
	static  QString GC_LINESTYLE;
	QHash<QString, HalconCpp::HTuple> graphicalSettings;
	QHash<QString, HalconCpp::HTuple> stateOfSettings;

	GraphicsContext();
	void applyContext(const HalconCpp::HTuple &winID, const QHash<QString, HalconCpp::HTuple> &cContext);
	void setAttribute(const QString &key, const HalconCpp::HTuple &val);
};

