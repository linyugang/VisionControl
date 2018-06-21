using System;
using HalconDotNet;
using System.Diagnostics;
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
    /// <summary>
    /// ROI类型的基类
    /// </summary>    
    public class ROI
    {
        /// <summary>
        /// 要显示roi的图像宽度
        /// </summary>
        private int imageWidth;
        /// <summary>
        /// 图形上的控制框个数
        /// </summary>
		protected int NumHandles;
        /// <summary>
        /// 图形上的激活控制框序号
        /// </summary>
		protected int activeHandleIdx;
        /// <summary>
        /// ROI类型
        /// </summary>
        public ROIType ROIType { get; protected set; }
        /// <summary>
        /// Flag to define the ROI to be 'positive' or 'negative'.
        /// </summary>
        protected ROIOperation operatorFlag;

        protected string info;
        public ROIOperation OperatorFlag
        {
            get
            {
                return operatorFlag;
            }
            set
            {
                operatorFlag = value;

                switch (operatorFlag)
                {
                    case ROIOperation.Positive:
                        FlagLineStyle = posOperation;
                        break;
                    case ROIOperation.Negative:
                        FlagLineStyle = negOperation;
                        break;
                    default:
                        FlagLineStyle = posOperation;
                        break;
                }
            }

        }
        /// <summary>ROI线显示方式设置</summary>
        public HTuple FlagLineStyle { get; set; }


        public double TxtScale { get; set; }
        public int ImageWidth
        {
            get
            {
                if (imageWidth == 0)
                {
                    imageWidth = 500;
                }
                return imageWidth;
            }

            set
            {
                imageWidth = value;
            }
        }

        /// <summary>
        /// "+"方式直接直线
        /// </summary>
        protected HTuple posOperation = new HTuple();
        /// <summary>
        /// "-"方式的虚线
        /// </summary>
		protected HTuple negOperation = new HTuple(new int[] { 8, 8 });

        /// <summary>Constructor of abstract ROI class.</summary>
        public ROI() { }

        /// <summary>
        /// 依据roi信息重新生成rio辅助图形
        /// </summary>
        public virtual void ReCreateROI() { }
        /// <summary>在鼠标位置创建ROI</summary>
        /// <param name="midX">
        /// 鼠标列坐标
        /// </param>
        /// <param name="midY">
        /// 鼠标行坐标
        /// </param>
        public virtual void CreateROI(double midX, double midY) { }

        /// <summary>Paints the ROI into the supplied window.</summary>
        /// <param name="window">HALCON window</param>
        public virtual void Draw(HalconDotNet.HWindow window) { }

        /// <summary>
        /// 求出鼠标坐标与ROI的最近控制点的距离
        /// </summary>
        /// <param name="x">鼠标坐标X</param>
        /// <param name="y">鼠标坐标Y</param>
        /// <returns>鼠标与ROI的控制框的最近距离值</returns>
        public virtual double DistToClosestHandle(double x, double y)
        {
            return 0.0;
        }

        /// <summary> 
        /// Paints the active handle of the ROI object into the supplied window. 
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void DisplayActive(HalconDotNet.HWindow window) { }

        /// <summary> 
        /// Recalculates the shape of the ROI. Translation is 
        /// performed at the active handle of the ROI object 
        /// for the image coordinate (x,y).
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        public virtual void MoveByHandle(double x, double y) { }

        /// <summary>获取roi描述的region</summary>
        public virtual HRegion GetRegion()
        {
            return null;
        }

        public virtual double GetDistanceFromStartPoint(double row, double col)
        {
            return 0.0;
        }
        /// <summary>
        /// 获取ROI的描述点坐标信息.
        /// </summary> 
        public virtual HTuple GetModelData()
        {
            return null;
        }

        /// <summary>Number of handles defined for the ROI.</summary>
        /// <returns>Number of handles</returns>
        public int GetNumHandles()
        {
            return NumHandles;
        }

        /// <summary>Gets the active handle of the ROI.</summary>
        /// <returns>Index of the active handle (from the handle list)</returns>
        public int GetActHandleIdx()
        {
            return activeHandleIdx;
        }

        public int GetHandleWidth()
        {
            //Debug.WriteLine("widht{0}", ImageWidth);
            int dat = ImageWidth / 80;
            if (dat < 3)
            {
                dat = 3;
            }
            return dat;
        }
    }//end of class
}//end of namespace
