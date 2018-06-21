using System;

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
    [Serializable]
    public enum ROIType
    {
        /// <summary>
        /// 直线
        /// </summary>
        Line = 10,
        /// <summary>
        /// 圆
        /// </summary>
        Circle,
        /// <summary>
        /// 圆弧
        /// </summary>
        CircleArc,
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle1,
        /// <summary>
        /// 带角度矩形
        /// </summary>
        Rectangle2
    }
    /// <summary>
    /// ROI运算
    /// </summary>
    public enum ROIOperation
    {
        /// <summary>
        /// ROI求和模式
        /// </summary>
        Positive = 21,
        /// <summary>
        /// ROI求差模式
        /// </summary>
        Negative,
        /// <summary>
        /// ROI模式为无
        /// </summary>
        None,
    }
    public enum ViewMessage
    {
        /// <summary>Constant describing an update of the model region</summary>
        UpdateROI = 50,

        ChangedROISign,

        /// <summary>Constant describing an update of the model region</summary>
        MovingROI,
        DeletedActROI,
        DelectedAllROIs,

        ActivatedROI,

        MouseMove,
        CreatedROI,
        /// <summary>
        /// Constant describes delegate message to signal new image
        /// </summary>
        UpdateImage,
        /// <summary>
        /// Constant describes delegate message to signal error
        /// when reading an image from file
        /// </summary>
        ErrReadingImage,
        /// <summary> 
        /// Constant describes delegate message to signal error
        /// when defining a graphical context
        /// </summary>
        ErrDefiningGC
    }
    public enum ShowMode
    {
        /// <summary>
        /// 包含ROI显示
        /// </summary>
        IncludeROI = 1,
        /// <summary>
        /// 不包含ROI显示
        /// </summary>
        ExcludeROI
    }
    public enum ResultShow
    {
        原图,
        处理后
    }
}
