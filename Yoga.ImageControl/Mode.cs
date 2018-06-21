using System;
using System.Collections;
using HalconDotNet;

/*********************************************************************************************************
 * 
 *    说明：
 * 
 *    halcon图像显示控件的再次封装 
 *   20180621
 *       1.对于图像显示及其他操作全部封装到c++代码中避免c#对于hobject对象释放导致显示异常问题
 *       2.c++ cli代理对于其他自定义算法也可以按照此模式添加
 *       3.roi操作参考的是halcon官方实例
 *       4.c++代码用到了qt5
 *       5.开发环境为vs2015+halcon13+qt5.9.1
 *       
 *   作者:林玉刚   有任何疑问或建议请联系 linyugang@foxmail.com
 * 
 *********************************************************************************************************/


namespace Yoga.ImageControl
{
    /// <summary>
    /// halcon窗体属性设置类
    /// </summary>
    public struct Mode
	{

		/// <summary>
        /// 显示颜色模式
		/// </summary>        
		public const string COLOR	 = "Color";

		/// <summary>
        /// 显示颜色种类 (see dev_set_colored)
		/// </summary>
		public const string COLORED	 = "Colored";

		/// <summary>
        /// 显示线宽(see set_line_width)
		/// </summary>
		public const string LINEWIDTH = "LineWidth";

		/// <summary>
        /// 填充模式(see set_draw)
		/// </summary>
		public const string DRAWMODE  = "DrawMode";

		/// <summary>
		/// Graphical mode for the drawing shape (see set_shape)
		/// </summary>
		public const string SHAPE     = "Shape";

		/// <summary>
		/// Graphical mode for the LUT (lookup table) (see set_lut)
		/// </summary>
		public const string LUT       = "Lut";

		/// <summary>
		/// Graphical mode for the painting (see set_paint)
		/// </summary>
		public const string PAINT     = "Paint";

		/// <summary>
		/// 线条显示方式,实线/虚线等
		/// </summary>
		public const string LINESTYLE = "LineStyle";
	}//end of class
}//end of namespace
