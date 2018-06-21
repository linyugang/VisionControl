using HalconDotNet;
using System.Collections;

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
    /// This class is an auxiliary class, which is used to 
    /// link a graphical context to an HALCON object. The graphical 
    /// context is described by a hashtable, which contains a list of
    /// graphical modes (e.g GC_COLOR, GC_LINEWIDTH and GC_PAINT) 
    /// and their corresponding values (e.g "blue", "4", "3D-plot"). These
    /// graphical states are applied to the window before displaying the
    /// object.
    /// </summary>
    public class HObjectEntry
	{
		/// <summary>HObject的键值集合</summary>
		public Hashtable	gContext;

		/// <summary>halcon图形对象</summary>
		public HObject		HObj;

        public HWndMessage Message;

		/// <summary>构造函数</summary>
		/// <param name="obj">
		/// 图像对象. 
		/// </param>
		/// <param name="gc"> 
		/// Hashlist of graphical states that are applied before the object
		/// is displayed. 
		/// </param>
		public HObjectEntry(HObject obj, Hashtable gc)
		{
			gContext = gc;
			HObj = obj;
		}
        public HObjectEntry(HWndMessage message)
        {
            this.Message = message;
        }
		/// <summary>
		/// 清除实体
		/// </summary>
		public void Clear()
		{
			gContext.Clear();
			HObj.Dispose();
		}

	}
}
