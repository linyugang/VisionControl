#pragma once
#include "qstring.h"
#include "HalconCpp.h"
#include "HWndMessage.h"
#include <QVariantHash>
#include<QHash>
using namespace std;
class HObjectEntry
{
public:
	HObjectEntry(const HalconCpp::HObject &obj, const QHash<QString, HalconCpp::HTuple> &gc);
	HObjectEntry(const HWndMessage &message);
	~HObjectEntry();
	QHash<QString, HalconCpp::HTuple> getGC() const;
	HalconCpp::HObject getHobj() const;

	HWndMessage getMessage() const;

private:
	QHash<QString, HalconCpp::HTuple>  gc;
	HalconCpp::HObject  hobj;
	HWndMessage message;
};

