#include "stdafx.h"
#include "HObjectEntry.h"

HObjectEntry::HObjectEntry(const HalconCpp::HObject & obj, const QHash<QString, HalconCpp::HTuple>& gc)
{
	this->gc = gc;
	this->hobj = obj;
}

HObjectEntry::HObjectEntry(const HWndMessage & message)
{
	this->message = message;
}

HObjectEntry::~HObjectEntry()
{
}

QHash<QString, HalconCpp::HTuple> HObjectEntry::getGC() const
{
	return gc;
}


HalconCpp::HObject HObjectEntry::getHobj() const
{
	return hobj;
}

HWndMessage HObjectEntry::getMessage() const
{
	return message;
}

