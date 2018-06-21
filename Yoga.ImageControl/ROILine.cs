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
    /// 直线ROI
    /// </summary>
    public class ROILine : ROI
    {
        /// <summary>
        /// 起点行坐标
        /// </summary>
        double startRow;
        /// <summary>
        /// 起点列坐标
        /// </summary>
        double startCol;
        /// <summary>
        /// 终点行坐标
        /// </summary>
        double endRow;
        /// <summary>
        /// 终点列坐标
        /// </summary>
        double endCol;
        /// <summary>
        /// 中点行坐标
        /// </summary>
        double midRow;
        /// <summary>
        /// 中点列坐标
        /// </summary>
        double midCol;

        [NonSerialized]
        /// <summary>
        /// 直线上添加箭头显示
        /// </summary>
        private HXLDCont arrowHandleXLD;
        /// <summary>
        /// 直线ROI构造
        /// </summary>
        public ROILine()
        {
            //直线上的控制框个数
            NumHandles = 3;        // 端点2+中点1=3
            //默认激活的控制点为2--中点
            activeHandleIdx = 2;
            //直线箭头初始化
            arrowHandleXLD = new HXLDCont();
            arrowHandleXLD.GenEmptyObj();

            this.ROIType = ROIType.Line;
        }
        /// <summary>
        /// 依据起点与终点信息重构ROI
        /// </summary>
        /// <param name="roiDat">坐标信息</param>
        private void CreateROI(HTuple roiDat)
        {
            if (roiDat.Length == 4)
            {
                startRow = roiDat[0].D;
                startCol = roiDat[1].D;
                endRow = roiDat[2].D;
                endCol = roiDat[3].D;
                midRow = (startRow + endRow) / 2.0;
                midCol = (startCol + endCol) / 2.0;
                updateArrowHandle();
            }
        }

        public override void ReCreateROI()
        {
            updateArrowHandle();
        }
        /// <summary>在鼠标位置创建一个ROI</summary>
        public override void CreateROI(double midX, double midY)
        {

            int width = GetHandleWidth();

            //鼠标位置为直线中心
            midRow = midY;
            midCol = midX;

            startRow = midRow;
            startCol = midCol - width*5;
            endRow = midRow;
            endCol = midCol + width*5;

            updateArrowHandle();
        }

        /// <summary>绘制图形在窗口中</summary>
        public override void Draw(HalconDotNet.HWindow window)
        {
            //直线绘制
            window.DispLine(startRow, startCol, endRow, endCol);

            int width = GetHandleWidth();
            //直线起点的框
            window.DispRectangle2(startRow, startCol, 0, width, width);
            //直线终点的箭头
            window.DispObj(arrowHandleXLD);  //window.DispRectangle2( row2, col2, 0, 5, 5);
            //直线中点的框
            window.DispRectangle2(midRow, midCol, 0, width, width);

            //int r1, c1, r2, c2;
            //window.GetPart(out r1, out c1, out r2, out c2);
            //int width = r2 - r1;
            ////直线起点的框
            //window.DispRectangle2(startRow, startCol, 0, width / 50.0, width / 50.0);
            ////System.Diagnostics.Debug.WriteLine("{0}-{1}-{2}-{3}", r1, c1, r2, c2);
        }

        /// <summary>
        /// 求出鼠标坐标与ROI的最近控制点的距离
        /// </summary>
        /// <param name="x">鼠标坐标X</param>
        /// <param name="y">鼠标坐标Y</param>
        /// <returns>鼠标与ROI的控制框的最近距离值</returns>
        public override double DistToClosestHandle(double x, double y)
        {

            double max = 10000;
            double[] val = new double[NumHandles];

            val[0] = HMisc.DistancePp(y, x, startRow, startCol); // upper left 
            val[1] = HMisc.DistancePp(y, x, endRow, endCol); // upper right 
            val[2] = HMisc.DistancePp(y, x, midRow, midCol); // midpoint 

            for (int i = 0; i < NumHandles; i++)
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
        /// 在窗体上显示激活的控制点
        /// </summary>
        public override void DisplayActive(HalconDotNet.HWindow window)
        {
            int width = GetHandleWidth();
            switch (activeHandleIdx)
            {
                case 0:
                    window.DispRectangle2(startRow, startCol, 0, width, width);
                    break;
                case 1:
                    window.DispObj(arrowHandleXLD); //window.DispRectangle2(row2, col2, 0, 5, 5);
                    break;
                case 2:
                    window.DispRectangle2(midRow, midCol, 0, width, width);
                    break;
            }
        }

        /// <summary>
        /// 获取region
        /// </summary>
        /// <returns>region图形</returns>
        public override HRegion GetRegion()
        {
            HRegion region = new HRegion();
            region.GenRegionLine(startRow, startCol, endRow, endCol);
            return region;
        }
        /// <summary>
        /// 获取坐标点到图像起始点的距离
        /// </summary>
        /// <param name="row">坐标行 Y</param>
        /// <param name="col">坐标列 X</param>
        /// <returns>距离值</returns>
        public override double GetDistanceFromStartPoint(double row, double col)
        {
            double distance = HMisc.DistancePp(row, col, startRow, startCol);
            return distance;
        }
        /// <summary>
        /// 获取region的坐标信息
        /// </summary> 
        public override HTuple GetModelData()
        {
            return new HTuple(new double[] { startRow, startCol, endRow, endCol });
        }

        /// <summary> 
        ///依据坐标点移动region
        /// </summary>
        public override void MoveByHandle(double newX, double newY)
        {
            double lenR, lenC;
            //起始点-终点-中点三种方式修改直线region
            switch (activeHandleIdx)
            {
                case 0: // first end point
                    startRow = newY;
                    startCol = newX;

                    midRow = (startRow + endRow) / 2;
                    midCol = (startCol + endCol) / 2;
                    break;
                case 1: // last end point
                    endRow = newY;
                    endCol = newX;

                    midRow = (startRow + endRow) / 2;
                    midCol = (startCol + endCol) / 2;
                    break;
                case 2: // midpoint 
                    lenR = startRow - midRow;
                    lenC = startCol - midCol;

                    midRow = newY;
                    midCol = newX;

                    startRow = midRow + lenR;
                    startCol = midCol + lenC;
                    endRow = midRow - lenR;
                    endCol = midCol - lenC;
                    break;
            }
            updateArrowHandle();
        }
        /// <summary> 
        /// 辅助的箭头显示方法
        /// </summary>
        private void updateArrowHandle()
        {
            double length, dr, dc, halfHW;
            double rrow1, ccol1, rowP1, colP1, rowP2, colP2;

            int width = GetHandleWidth();
            double headLength = width;
            double headWidth = width;

            if (arrowHandleXLD == null)
                arrowHandleXLD = new HXLDCont();
            arrowHandleXLD.Dispose();
            arrowHandleXLD.GenEmptyObj();

            //箭头起始点为直线长度的0.8位置
            rrow1 = startRow + (endRow - startRow) * 0.8;
            ccol1 = startCol + (endCol - startCol) * 0.8;
            //测量箭头起始点到直线终点的距离
            length = HMisc.DistancePp(rrow1, ccol1, endRow, endCol);
            //如果距离为0说明直线长度为0
            if (length == 0)
                length = -1;

            dr = (endRow - rrow1) / length;
            dc = (endCol - ccol1) / length;

            halfHW = headWidth / 2.0;
            rowP1 = rrow1 + (length - headLength) * dr + halfHW * dc;
            rowP2 = rrow1 + (length - headLength) * dr - halfHW * dc;
            colP1 = ccol1 + (length - headLength) * dc - halfHW * dr;
            colP2 = ccol1 + (length - headLength) * dc + halfHW * dr;

            if (length == -1)
                arrowHandleXLD.GenContourPolygonXld(rrow1, ccol1);
            else
                arrowHandleXLD.GenContourPolygonXld(new HTuple(new double[] { rrow1, endRow, rowP1, endRow, rowP2, endRow }),
                                                    new HTuple(new double[] { ccol1, endCol, colP1, endCol, colP2, endCol }));
        }

    }
}
