using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace Yoga.ImageControl
{
    public partial class FunctionPlotUnit : UserControl
    {

        private FunctionPlot plotGraphWindow;


        public FunctionPlotUnit()
        {
            InitializeComponent();

            plotGraphWindow = new FunctionPlot(panelAxis, true);
            plotGraphWindow.SetAxisAdaption(FunctionPlot.AXIS_RANGE_FIXED, 255.0f);
        }
        public void SetAxisAdaption(int mode)
        {
            plotGraphWindow.SetAxisAdaption(mode);
        }
        public void SetLabel(string x, string y)
        {
            plotGraphWindow.SetLabel(x, y);
            lblX.Text = x;
            lblY.Text = y;
        }
        public void ComputeStatistics(HTuple grayVals)
        {
            HTuple tuple, val;
            int max = 0;

            if (grayVals != null && grayVals.Length > 1)
            {
                tuple = new HTuple(grayVals);

                val = tuple.TupleMean();
                labelMean.Text = val[0].D.ToString("f2");
                val = tuple.TupleDeviation();
                labelDeviation.Text = val[0].D.ToString("f2");

                val = tuple.TupleSortIndex();
                labelPeakX.Text = val[val.Length - 1].I + "";
                max = (int)tuple[val[val.Length - 1].I].D;
                labelPeak.Text = max + "";

                labelRange.Text = (int)tuple[0].D + " ... " + (int)tuple[tuple.Length - 1].D;
                labelRangeX.Text = "0 ... " + (tuple.Length - 1);
            }
            else
            {
                labelMean.Text = "0";
                labelDeviation.Text = "0";

                labelPeakX.Text = "0";
                labelPeak.Text = "0";

                labelRange.Text = "0 ... 0";
                labelRangeX.Text = "0 ... 0";
            }
        }
        /// <summary>Adjusts statistics of measure projection (line profile).</summary>
        public void ComputeStatistics(double[] grayVals)
        {
            if (grayVals != null && grayVals.Length > 1)
            {
                ComputeStatistics(new HTuple(grayVals));
            }
            else
            {
                ComputeStatistics((HTuple)null);
            }
        }
        /// <summary>
		///设置灰度曲线的值数组
		/// </summary>
		/// <param name="grayValues">
		/// 灰度曲线上y(灰度)的值
		/// </param>
		public void SetFunctionPlotValue(double[] grayValues)
        {
            plotGraphWindow.drawFunction(new HTuple(grayValues));
        }

        /// <summary>
		///设置灰度曲线的值数组
		/// </summary>
		/// <param name="grayValues">
		/// 灰度曲线上y(灰度)的值
		/// </param>
		public void SetFunctionPlotValue(HTuple grayValues)
        {
            plotGraphWindow.drawFunction(grayValues);
        }
        /// <summary>
        ///设置灰度曲线的值数组
        /// </summary>
        /// <param name="grayValues">
        /// 灰度曲线上y(灰度)的值
        /// </param>
        public void SetFunctionPlotValue(float[] grayValues)
        {
            plotGraphWindow.drawFunction(new HTuple(grayValues));
        }


        /// <summary>
        ///设置灰度曲线的值数组
        /// </summary>
        /// <param name="grayValues">
        /// 灰度曲线上y(灰度)的值
        /// </param>
        public void SetFunctionPlotValue(int[] grayValues)
        {
            plotGraphWindow.drawFunction(new HTuple(grayValues));
        }
    }
}
