using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yoga.ImageControl;
using HalconDotNet;
namespace Yoga.Test
{
    public partial class Form1 : Form
    {
        /// <summary>halcon窗体控制</summary>
        private HWndCtrl hWndCrtl;
        /// <summary>ROI 管理控制</summary>
		public ROIController ROIController;

        public Form1()
        {
            InitializeComponent();
            hWndCrtl = hWndUnit1.HWndCtrl;

            ROIController = new ROIController();

            ROIController.ROINotifyEvent += new EventHandler<ViewEventArgs>(UpdateViewData);
            roiActUnit1.RoiController = ROIController;
            hWndCrtl.useROIController(ROIController);

            ROIController.SetROISign(ROIOperation.Positive);

        }

        private void UpdateViewData(object sender, ViewEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HImage img = new HImage(@"D:\12.bmp");
            hWndCrtl.AddIconicVar(img);
            hWndCrtl.AddText("测试",20,20);
            hWndCrtl.Repaint();
        }
    }
}
