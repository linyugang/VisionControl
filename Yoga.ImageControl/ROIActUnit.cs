using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yoga.ImageControl
{
    public partial class ROIActUnit : UserControl
    {
        private ROIController roiController;
        private bool useSearchRoi=true;
        public ROIActUnit()
        {
            InitializeComponent();
        }

        public ROIController RoiController
        {
            get
            {
                return roiController;
            }

            set
            {
                roiController = value;
            }
        }

        public bool UseSearchRoi
        {
            get
            {
                return useSearchRoi;
            }

            set
            {
                useSearchRoi = value;
                reduceRect1Button.Visible = useSearchRoi;
            }
        }

        private void addToROIButton_CheckedChanged(object sender, EventArgs e)
        {
            if (addToROIButton.Checked)
                RoiController.SetROISign(ROIOperation.Positive);
        }

        private void subFromROIButton_CheckedChanged(object sender, EventArgs e)
        {
            if (subFromROIButton.Checked)
                RoiController.SetROISign(ROIOperation.Negative);
        }

        private void rect1Button_Click(object sender, EventArgs e)
        {
            RoiController.SetROIShape(new ROIRectangle1());
        }

        private void rect2Button_Click(object sender, EventArgs e)
        {
            RoiController.SetROIShape(new ROIRectangle2());
        }

        private void circleButton_Click(object sender, EventArgs e)
        {
            RoiController.SetROIShape(new ROICircle());
        }

        private void reduceRect1Button_Click(object sender, EventArgs e)
        {
            RoiController.SetROIShapeNoOperator(new ROIRectangle2("搜索区域"));
        }

        private void delROIButton_Click(object sender, EventArgs e)
        {
            RoiController.RemoveActive();
        }

        private void delAllROIButton_Click(object sender, EventArgs e)
        {
            RoiController.Reset();
        }
    }
}
