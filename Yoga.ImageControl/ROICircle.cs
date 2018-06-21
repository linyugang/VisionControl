using System;
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
    [Serializable]
	/// <summary>
	/// 圆形region
	/// </summary>
	public class ROICircle : ROI
	{

		private double radius;     //半径
		private double row1, col1;  // 圆上的点
        private double midR, midC;  // 圆心


        public ROICircle()
		{
            //控制点2个
			NumHandles = 2; // one at corner of circle + midpoint

            //默认控制点为圆心
			activeHandleIdx = 1;

            this.ROIType = ROIType.Circle;
		}
        /// <summary>依据鼠标所在位置生成一个圆形region</summary>
        public override void CreateROI(double midX, double midY)
		{
            //圆心为鼠标所在点
			midR = midY;
			midC = midX;


            int width = GetHandleWidth();
            radius = width*10.0;
            //圆上的点
			row1 = midR;
			col1 = midC + radius;
		}

		/// <summary>绘制圆形坐标在窗体上</summary>
		/// <param name="window">HALCON 窗体</param>
		public override void Draw(HalconDotNet.HWindow window)
		{
            //显示圆
			window.DispCircle(midR, midC, radius);

            int width = GetHandleWidth();
            //显示两个控制框
			window.DispRectangle2(row1, col1, 0, width, width);
			window.DispRectangle2(midR, midC, 0, width, width);
            
        }

        /// <summary>
        /// 求出鼠标与最近控制点的距离
        /// </summary>
        /// <param name="x">鼠标x</param>
        /// <param name="y">鼠标y</param>
        /// <returns>距离值</returns>
		public override double DistToClosestHandle(double x, double y)
		{
			double max = 10000;
			double [] val = new double[NumHandles];

			val[0] = HMisc.DistancePp(y, x, row1, col1); // border handle 
			val[1] = HMisc.DistancePp(y, x, midR, midC); // midpoint 

			for (int i=0; i < NumHandles; i++)
			{
				if (val[i] < max)
				{
					max = val[i];
					activeHandleIdx = i;
				}
			}// end of for 
			return val[activeHandleIdx];
		}

		/// <summary> 
		///在窗体上显示激活的控制图形
		/// </summary>
		public override void DisplayActive(HalconDotNet.HWindow window)
		{
            int width = GetHandleWidth();
			switch (activeHandleIdx)
			{
				case 0://圆上的点
					window.DispRectangle2(row1, col1, 0, width, width);
					break;
				case 1://圆心
					window.DispRectangle2(midR, midC, 0, width, width);
					break;
			}
		}

		/// <summary>返回当前的region图形数据</summary>
		public override HRegion GetRegion()
		{
			HRegion region = new HRegion();
			region.GenCircle(midR, midC, radius);
			return region;
		}
        /// <summary>
        /// 返回点与圆心的距离
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
		public override double GetDistanceFromStartPoint(double row, double col)
		{
			double sRow = midR; // assumption: we have an angle starting at 0.0
			double sCol = midC + 1 * radius;

			double angle = HMisc.AngleLl(midR, midC, sRow, sCol, midR, midC, row, col);

			if (angle < 0)
				angle += 2 * Math.PI;

			return (radius * angle);
		}

		/// <summary>
		/// 获取圆形region的参数数据
		/// </summary> 
		public override HTuple GetModelData()
		{
			return new HTuple(new double[] { midR, midC, radius });
		}

		/// <summary> 
		/// 依据控制点及鼠标交互修改region
		/// </summary>
		public override void MoveByHandle(double newX, double newY)
		{
			HTuple distance;
			double shiftX,shiftY;

			switch (activeHandleIdx)
			{
				case 0: // 鼠标在圆上的一点时

					row1 = newY;
					col1 = newX;
					HOperatorSet.DistancePp(new HTuple(row1), new HTuple(col1),
											new HTuple(midR), new HTuple(midC),
											out distance);

					radius = distance[0].D;
					break;
				case 1: // 鼠标在圆心上时

					shiftY = midR - newY;
					shiftX = midC - newX;

					midR = newY;
					midC = newX;

					row1 -= shiftY;
					col1 -= shiftX;
					break;
			}
		}
	}//end of class
}//end of namespace
