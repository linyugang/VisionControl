// Data conversion between HALCON/C++ and HALCON/.NET objects
//
// This class can be reused as is for your own interoperating project

#pragma once

#include "HalconCpp.h"
// Due to identical class names in C++ and .NET, we only
// declare the namespace for HALCON/.NET and use all C++
// classes with the fully qualified name (HalconCpp::X)
using namespace HalconDotNet;

class Conversion
{
public:

  // Conversions from HALCON/.NET to HALCON/C++
  static HalconCpp::HObject      ObjectToNative(HObject^ object);
  static HalconCpp::HImage       ImageToNative(HImage^ image);
  static HalconCpp::HRegion      RegionToNative(HRegion^ region);
  static HalconCpp::HXLD         XLDToNative(HXLD^ xld);
  static HalconCpp::HTuple       TupleToNative(HTuple^ tuple);

  // Conversions from HALCON/C++ to HALCON/.NET
  static HObject^ ObjectToManaged(const HalconCpp::HObject& object);
  static HImage^  ImageToManaged(const HalconCpp::HImage& image);
  static HRegion^ RegionToManaged(const HalconCpp::HRegion& region);
  static HXLD^    XLDToManaged(const HalconCpp::HXLD& xld);
  static HXLDCont^    HXLDContToManaged(const HalconCpp::HXLDCont& xld);
  static HTuple^  TupleToManaged(const HalconCpp::HTuple& tuple);
  static System::String^ StringToManged(const std::string& string);
private:

  // Support functionality for implementing the conversions
  static System::IntPtr  KeyToManaged(Hkey key);
  static Hkey            KeyToNative(System::IntPtr handle);
};
