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
	/// 无角度矩形ROI类
	/// </summary>
	public class ROIRectangle1 : ROI
	{

		private double row1, col1;   // 左上
		private double row2, col2;   // 右下
		private double midR, midC;   // 中心 


		/// <summary>Constructor</summary>
		public ROIRectangle1()
		{

			NumHandles = 5; // 4 corner points + midpoint
			activeHandleIdx = 4;

            this.ROIType = ROIType.Rectangle1;
		}

		/// <summary>Creates a new ROI instance at the mouse position</summary>
		/// <param name="midX">
		/// x (=column) coordinate for interactive ROI
		/// </param>
		/// <param name="midY">
		/// y (=row) coordinate for interactive ROI
		/// </param>
		public override void CreateROI(double midX, double midY)
		{

            int width = GetHandleWidth() * 5;
            midR = midY;
			midC = midX;

			row1 = midR - width;
			col1 = midC - width;
			row2 = midR + width;
			col2 = midC + width;
		}

		/// <summary>Paints the ROI into the supplied window</summary>
		/// <param name="window">HALCON window</param>
		public override void Draw(HalconDotNet.HWindow window)
		{
			window.DispRectangle1(row1, col1, row2, col2);

            int width = GetHandleWidth();
			window.DispRectangle2(row1, col1, 0, width, width);
			window.DispRectangle2(row1, col2, 0, width, width);
			window.DispRectangle2(row2, col2, 0, width, width);
			window.DispRectangle2(row2, col1, 0, width, width);
			window.DispRectangle2(midR, midC, 0, width, width);
		}

		/// <summary> 
		/// Returns the distance of the ROI handle being
		/// closest to the image point(x,y)
		/// </summary>
		/// <param name="x">x (=column) coordinate</param>
		/// <param name="y">y (=row) coordinate</param>
		/// <returns> 
		/// Distance of the closest ROI handle.
		/// </returns>
		public override double DistToClosestHandle(double x, double y)
		{

			double max = 10000;
			double [] val = new double[NumHandles];

			midR = ((row2 - row1) / 2) + row1;
			midC = ((col2 - col1) / 2) + col1;

			val[0] = HMisc.DistancePp(y, x, row1, col1); // upper left 
			val[1] = HMisc.DistancePp(y, x, row1, col2); // upper right 
			val[2] = HMisc.DistancePp(y, x, row2, col2); // lower right 
			val[3] = HMisc.DistancePp(y, x, row2, col1); // lower left 
			val[4] = HMisc.DistancePp(y, x, midR, midC); // midpoint 

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
		/// Paints the active handle of the ROI object into the supplied window
		/// </summary>
		/// <param name="window">HALCON window</param>
		public override void DisplayActive(HalconDotNet.HWindow window)
		{

            int width = GetHandleWidth();
			switch (activeHandleIdx)
			{
				case 0:
					window.DispRectangle2(row1, col1, 0, width, width);
					break;
				case 1:
					window.DispRectangle2(row1, col2, 0, width, width);
					break;
				case 2:
					window.DispRectangle2(row2, col2, 0, width, width);
					break;
				case 3:
					window.DispRectangle2(row2, col1, 0, width, width);
					break;
				case 4:
					window.DispRectangle2(midR, midC, 0, width, width);
					break;
			}
		}

		/// <summary>Gets the HALCON region described by the ROI</summary>
		public override HRegion GetRegion()
		{
			HRegion region = new HRegion();
			region.GenRectangle1(row1, col1, row2, col2);
			return region;
		}

		/// <summary>
		/// Gets the model information described by 
		/// the interactive ROI
		/// </summary> 
		public override HTuple GetModelData()
		{
			return new HTuple(new double[] { row1, col1, row2, col2 });
		}


		/// <summary> 
		/// Recalculates the shape of the ROI instance. Translation is 
		/// performed at the active handle of the ROI object 
		/// for the image coordinate (x,y)
		/// </summary>
		/// <param name="newX">x mouse coordinate</param>
		/// <param name="newY">y mouse coordinate</param>
		public override void MoveByHandle(double newX, double newY)
		{
			double len1, len2;
			double tmp;

			switch (activeHandleIdx)
			{
				case 0: // upper left 
					row1 = newY;
					col1 = newX;
					break;
				case 1: // upper right 
					row1 = newY;
					col2 = newX;
					break;
				case 2: // lower right 
					row2 = newY;
					col2 = newX;
					break;
				case 3: // lower left
					row2 = newY;
					col1 = newX;
					break;
				case 4: // midpoint 
					len1 = ((row2 - row1) / 2);
					len2 = ((col2 - col1) / 2);

					row1 = newY - len1;
					row2 = newY + len1;

					col1 = newX - len2;
					col2 = newX + len2;

					break;
			}

			if (row2 <= row1)
			{
				tmp = row1;
				row1 = row2;
				row2 = tmp;
			}

			if (col2 <= col1)
			{
				tmp = col1;
				col1 = col2;
				col2 = tmp;
			}

			midR = ((row2 - row1) / 2) + row1;
			midC = ((col2 - col1) / 2) + col1;

		}//end of method
	}//end of class
}//end of namespace
