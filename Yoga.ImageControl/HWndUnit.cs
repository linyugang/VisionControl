using HalconDotNet;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yoga.ImageControl
{
    public partial class HWndUnit : UserControl
    {
        private HWndCtrl hWndCtrl;
        private string cameraName;
        public HWndUnit()
        {
            InitializeComponent();
            hWindowControl1.Size = this.Size;
            this.Disposed += HWndUnit_Disposed;
        }

        private void HWndUnit_Disposed(object sender, EventArgs e)
        {

            hWndCtrl.ClearWindowData();
        }

        public HWndCtrl HWndCtrl
        {
            get
            {
                if (hWndCtrl == null)
                {
                    hWindowControl1.Size = this.Size;
                    hWndCtrl = new HWndCtrl(hWindowControl1);
                    hWndCtrl.ShowMessageEvent += HWndCtrl_ShowMessageEvent;
                }
                return hWndCtrl;
            }
        }

        private void HWndCtrl_ShowMessageEvent(object sender, ShowMessageEventArgs e)
        {
            switch (e.MessageType)
            {
                case MessageType.MouseMessage:
                    if (lblMouseMessage.Visible == false)
                    {
                        lblMouseMessage.Visible = true;
                    }
                    lblMouseMessage.Text = hWndCtrl.MousePosMessage;
                    break;
                case MessageType.ShowOk:
                    if (labelResult.Visible==false)
                    {
                        labelResult.Visible = true;
                    }
                    labelResult.BackColor = Color.Green;
                    labelResult.Text = "OK";
                    break;
                case MessageType.ShowNg:
                    if (labelResult.Visible == false)
                    {
                        labelResult.Visible = true;
                    }
                    labelResult.BackColor = Color.Red;
                    labelResult.Text = "NG";
                    break;
                default:
                    break;
            }
        }

        public string CameraMessage
        {
            get
            {
                return cameraName;
            }
            set
            {
                this.lblName.Text = value;
                this.lblName.Visible = true;
                cameraName = value;
            }
        }

        private  string ToHexColor(Color color)
        {
            if (color.IsEmpty)
                return "#000000";
            string R = Convert.ToString(color.R, 16);
            if (R == "0")
                R = "00";
            string G = Convert.ToString(color.G, 16);
            if (G == "0")
                G = "00";
            string B = Convert.ToString(color.B, 16);
            if (B == "0")
                B = "00";
            string HexColor = "#" + R + G + B;
            return HexColor.ToUpper();
        }
        private void HWndUnit_SizeChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            HWndCtrl.ResetWindow();
            hWndCtrl.Repaint();
        }

        private void HWndUnit_Load(object sender, EventArgs e)
        {
            //Wrapper.ShowUnit.ShowHat(hWindowControl1.HalconWindow, true);
        }

    }
}
