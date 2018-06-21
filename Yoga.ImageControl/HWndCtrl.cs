#define NativeCode

using System;
using System.Collections;
using HalconDotNet;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Yoga.Wrapper;
using System.Drawing;
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
    ///halcon窗体控制类
    /// </summary>
    public class HWndCtrl
    {
        private IntPtr winHandle;
        /// <summary> 
        /// 最大显示图形个数
        /// </summary>
        private const int MAX_NUM_OBJ_LIST = 50;

        private bool mousePressed = false;

        private bool isShowCross = false;
        private bool isShowMessage = true;
        private double startX, startY;
        private bool inMeasureLine;
        /// <summary>HALCON window</summary>
        private HWindowControl viewPort;

#if !NativeCode


        /// <summary> 
        /// Halcon图像窗口的显示对象集合,
        /// 超过了最大显示个数则起始位置图像会被移除显示列表
        /// </summary>
        private List<HObjectEntry> HObjList;

        /// <summary>
        /// Instance that describes the graphical context for the
        /// HALCON window. According on the graphical settings
        /// attached to each HALCON object, this graphical context list 
        /// is updated constantly.
        /// </summary>
        private GraphicsContext mGC;
#endif
        private ContextMenuStrip /**/        hv_MenuStrip;                                    //右键菜单控件
        // 窗体控件右键菜单内容
        ToolStripMenuItem fit_strip;

        ToolStripMenuItem fit_showImageOnly;
        ToolStripMenuItem saveImg_strip;
        ToolStripMenuItem saveWindow_strip;

        ToolStripMenuItem ShowHistogram_strip;
        ToolStripMenuItem showCross_shrip;

        ToolStripMenuItem showMessage_strip;
        ToolStripMenuItem measureLine_strip;
        /// <summary>
        /// Instance of ROIController, which manages ROI interaction
        /// </summary>
        private ROIController roiManager;
        private string backguoundColor = "black";
        /// <summary>
        /// 显示模式
        /// </summary>
        private ShowMode showMode;

        private ResultShow resultShow = ResultShow.处理后;
        /* Basic parameters, like dimension of window and displayed image part */
        private int windowWidth;
        private int windowHeight;
        private int imageWidth = 0;
        private int imageHeight = 0;
        private double zoomWndFactor;

#if !NativeCode
        private double currentTextSize = 0; 
#endif
        private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;


        // 设定图像的窗口显示部分
        private int zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_endCol;
        // 获取图像的当前显示部分                   
        private int current_beginRow, current_beginCol, current_endRow, current_endCol;

        /// <summary>
        /// 鼠标位置图片状态信息
        /// </summary>
        public string MousePosMessage { get; private set; }

        public int WindowWidth
        {
            get
            {
                return windowWidth;
            }

        }

        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }
        }

        public ResultShow ResultShow
        {
            get
            {
                return resultShow;
            }

            set
            {
                resultShow = value;
                Repaint();
            }
        }

        public int ImageWidth
        {
            get
            {
                return imageWidth;
            }
        }

        public int ImageHeight
        {
            get
            {
                return imageHeight;
            }
        }

        public double ZoomWndFactor
        {
            get
            {
                return zoomWndFactor;
            }

            private set
            {
                zoomWndFactor = value;
            }
        }

        public event EventHandler<ShowMessageEventArgs> ShowMessageEvent;

        /// <summary> 
        /// Initializes the image dimension, mouse delegation, and the 
        /// graphical context setup of the instance.
        /// </summary>
        /// <param name="view"> HALCON window </param>
        public HWndCtrl(HWindowControl view)
        {
            viewPort = view;
            winHandle = viewPort.HalconWindow;

#if NativeCode
            Wrapper.ShowUnit.ClearWindowData(winHandle);

#else
            view.HalconWindow.SetDraw("margin");
            view.HalconWindow.SetColor("blue");
            view.HalconWindow.SetLineWidth(1);
#endif
            windowWidth = viewPort.Size.Width;
            windowHeight = viewPort.Size.Height;

            ZoomWndFactor = (double)imageWidth / viewPort.Width;

            showMode = ShowMode.IncludeROI;

            viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
            viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
            viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);
            //新添加滚轮事件
            viewPort.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.mouseWheel);

#if !NativeCode
            // graphical stack 
            HObjList = new List<HObjectEntry>();
            mGC = new GraphicsContext();
#endif
            fit_strip = new ToolStripMenuItem("适应窗口");
            fit_strip.Click += new EventHandler((s, e) => DispImageFit());


            fit_showImageOnly = new ToolStripMenuItem("显示原图/所有");
            fit_showImageOnly.Click += new EventHandler((s, e) => ShowImageOnly());

            saveImg_strip = new ToolStripMenuItem("保存原始图像");
            saveImg_strip.Click += new EventHandler((s, e) => SaveImage());

            saveWindow_strip = new ToolStripMenuItem("截图另存");
            saveWindow_strip.Click += new EventHandler((s, e) => SaveWindowDump());

            showCross_shrip = new ToolStripMenuItem("显示/隐藏十字");
            showCross_shrip.Click += new EventHandler((s, e) => ShowCross());


            showMessage_strip = new ToolStripMenuItem("显示/隐藏文字");
            showMessage_strip.Click += new EventHandler((s, e) => ShowMessage());


            ShowHistogram_strip = new ToolStripMenuItem("灰度直方图");
            ShowHistogram_strip.Click += ShowHistogram_strip_Click;

            measureLine_strip = new ToolStripMenuItem("距离测量");
            measureLine_strip.Click += MeasureLine_strip_Click;
            hv_MenuStrip = new ContextMenuStrip();
            hv_MenuStrip.Items.Add(fit_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());

            hv_MenuStrip.Items.Add(fit_showImageOnly);

            hv_MenuStrip.Items.Add(showCross_shrip);
            hv_MenuStrip.Items.Add(showMessage_strip);
            hv_MenuStrip.Items.Add(measureLine_strip);
            hv_MenuStrip.Items.Add(ShowHistogram_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());

            hv_MenuStrip.Items.Add(saveImg_strip);
            hv_MenuStrip.Items.Add(saveWindow_strip);

            viewPort.ContextMenuStrip = hv_MenuStrip;
            //m_CtrlHStatusLabelCtrl.BringToFront();
            //viewPort.ResumeLayout(false);
            //viewPort.PerformLayout();
            //HOperatorSet.SetSystem("filename_encoding", "utf8");
        }

        private void ShowHistogram_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();
#if NativeCode
            ShowUnit.ShowText(viewPort.HalconWindow, "鼠标左键点击并拉取矩形区域,鼠标右键完成", 20, 20, 20, "green", "window");
#else
            HWndMessage message = new HWndMessage("鼠标左键点击并拉取矩形区域,鼠标右键完成", 20, 20, 20, "green");
            message.DispMessage(viewPort.HalconWindow, "window", 1);
#endif
            inMeasureLine = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;
            //HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);
            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();

            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色


            window.DrawRectangle1(out r1, out c1, out r2, out c2);
            window.DispRectangle1(r1, c1, r2, c2);
            Form frm = new Form();

            FunctionPlotUnit pointUnit = new FunctionPlotUnit();
            Size size = pointUnit.Size;
            size.Height = (int)(size.Height + 50);
            size.Width = (int)(size.Width + 50);
            frm.Size = size;
            frm.Controls.Add(pointUnit);
            pointUnit.Dock = DockStyle.Fill;
            HTuple grayVals;
#if NativeCode
            grayVals = Wrapper.ShowUnit.GetGrayHisto(viewPort.HalconWindow, new HTuple(r1, c1, r2, c2));
#else
            grayVals = GetGrayHisto(new HTuple(r1, c1, r2, c2));
#endif
            pointUnit.SetAxisAdaption(FunctionPlot.AXIS_RANGE_INCREASING);
            pointUnit.SetLabel("灰度值", "频率");
            pointUnit.SetFunctionPlotValue(grayVals);

            pointUnit.ComputeStatistics(grayVals);

            frm.ShowDialog();


            //window.DrawLine(out r1, out c1, out r2, out c2);
            //window.DispLine(r1, c1, r2, c2);


            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);


            //HOperatorSet.DistancePp(r1, c1, r2, c2, out dd);
            //double dr = Math.Abs(r2 - r1);
            //double dc = Math.Abs(c2 - c1);
            //MessageBox.Show(string.Format("直线距离{0:f2}px\rx轴距离{1:f2}px\ry轴距离{2:f2}px", dd.D, dc, dr), "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hv_MenuStrip.Visible = true;
            viewPort.ContextMenuStrip = hv_MenuStrip;
            inMeasureLine = false;
            Repaint();
        }
#if !NativeCode
        HTuple GetGrayHisto(HTuple rectangle1)
        {

            if (HObjList == null || HObjList.Count < 1)
            {
                return null;
            }
            HImage hv_image = HObjList[0].HObj as HImage;

            if (hv_image != null)
            {
                try
                {
                    HTuple hv_AbsoluteHisto, hv_RelativeHisto;

                    HTuple channel = hv_image.CountChannels();
                    HImage imgTmp = null;
                    if (channel == 3)
                    {
                        imgTmp = hv_image.Rgb1ToGray();
                    }
                    else
                    {
                        imgTmp = hv_image.Clone();
                    }
                    HRegion region = new HRegion();
                    region.GenRectangle1(rectangle1[0].D, rectangle1[1], rectangle1[2], rectangle1[3]);
                    hv_AbsoluteHisto = imgTmp.GrayHisto(region, out hv_RelativeHisto);
                    return hv_AbsoluteHisto;
                }
                catch (Exception)
                {

                }

            }
            return null;
        }
#endif
        private void ShowImageOnly()
        {
            if (ResultShow == ResultShow.原图)
            {
                ResultShow = ResultShow.处理后;
            }
            else
            {
                ResultShow = ResultShow.原图;
            }
        }

        public HImage DumpWindows()
        {
            return viewPort.HalconWindow.DumpWindowImage();
        }

        public void DrawPoint(out double x, out double y)
        {
            viewPort.Focus();
#if NativeCode
            Wrapper.ShowUnit.ShowText(viewPort.HalconWindow, "鼠标左键点击点位置,鼠标右键完成。", 20, 20, 20, "green", "window");
#else
            HWndMessage message = new HWndMessage("鼠标左键点击点位置,鼠标右键完成", 20, 20, 20, "green");
            message.DispMessage(viewPort.HalconWindow, "window", 1);
#endif
            inMeasureLine = true;
            viewPort.ContextMenuStrip = null;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawPoint(out y, out x);
            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);

            viewPort.ContextMenuStrip = hv_MenuStrip;
            inMeasureLine = false;
            Repaint();
        }
        private void MeasureLine_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();
#if NativeCode
            ShowUnit.ShowText(viewPort.HalconWindow, "鼠标点击两个位置后,单击鼠标右键完成。", 20, 20, 20, "green", "window");
#else
            HWndMessage message = new HWndMessage("鼠标点击两个位置后,单击鼠标右键完成。", 20, 20, 20, "green");
            message.DispMessage(viewPort.HalconWindow, "window", 1);
#endif
            inMeasureLine = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;
            HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawLine(out r1, out c1, out r2, out c2);
            window.DispLine(r1, c1, r2, c2);
            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);


            HOperatorSet.DistancePp(r1, c1, r2, c2, out dd);
            double dr = Math.Abs(r2 - r1);
            double dc = Math.Abs(c2 - c1);
            MessageBox.Show(string.Format("直线距离{0:f2}px\rx轴距离{1:f2}px\ry轴距离{2:f2}px", dd.D, dc, dr), "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hv_MenuStrip.Visible = true;
            viewPort.ContextMenuStrip = hv_MenuStrip;
            inMeasureLine = false;
            Repaint();
        }

        private void ShowMessage()
        {
            isShowMessage = !isShowMessage;
            Repaint();
        }

        private void ShowCross()
        {
            isShowCross = !isShowCross;
            Repaint();
        }

        private void DispImageFit()
        {
            ResetWindow();
            Repaint();
        }

        private void SaveWindowDump()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG图像|*.png|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                    return;

                //截取窗口图
                HOperatorSet.DumpWindow(viewPort.HalconWindow, "png best", sfd.FileName);
            }
        }

        private void SaveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(sfd.FileName))
                {
                    return;
                }
#if NativeCode
                Wrapper.ShowUnit.SaveImage(viewPort.HalconWindow, sfd.FileName);

#else
                if (HObjList == null || HObjList.Count < 1)
                {
                    return;
                }
                HImage hv_image = HObjList[0].HObj as HImage;
                if (hv_image != null && hv_image.IsInitialized())
                {
                    HOperatorSet.WriteImage(hv_image, "bmp", 0, sfd.FileName);
                }
#endif

            }
        }

        private void TriggerShowMessageEvent(ShowMessageEventArgs e)
        {
            if (ShowMessageEvent != null)
            {
                ShowMessageEvent(this, e);
            }
        }

        private void mouseWheel(object sender, HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            double Row, Column;
            int Button;
            try
            {
                viewPort.HalconWindow.GetMpositionSubPix(out Row, out Column, out Button);
            }
            catch (HalconException)
            {
                return;
            }

            double mode = 1;
            if (e.Delta > 0)
            {
                mode = 1;
            }
            else
            {
                mode = -1;
            }
            DispImageZoom(mode, Row, Column);
        }


        /// <summary>
        /// 注册窗体的ROI控制类
        /// </summary>
        /// <param name="rC"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        public void useROIController(ROIController rC)
        {
            roiManager = rC;
            rC.setViewController(this);
        }


        /// <summary>
        /// 设置是否显示roi
        /// </summary>
        /// <param name="mode"></param>
        public void SetDispLevel(ShowMode mode)
        {
            showMode = mode;
        }
        void DispImageZoom(double mode, double Mouse_row, double Mouse_col)
        {
            try
            {
                viewPort.HalconWindow.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);
            }
            catch
            {
                return;
            }

            if (mode > 0)                 // 放大图像
            {
                zoom_beginRow = (int)(current_beginRow + (Mouse_row - current_beginRow) * 0.300d);
                zoom_beginCol = (int)(current_beginCol + (Mouse_col - current_beginCol) * 0.300d);
                zoom_endRow = (int)(current_endRow - (current_endRow - Mouse_row) * 0.300d);
                zoom_endCol = (int)(current_endCol - (current_endCol - Mouse_col) * 0.300d);
            }
            else                // 缩小图像
            {
                zoom_beginRow = (int)(Mouse_row - (Mouse_row - current_beginRow) / 0.700d);
                zoom_beginCol = (int)(Mouse_col - (Mouse_col - current_beginCol) / 0.700d);
                zoom_endRow = (int)(Mouse_row + (current_endRow - Mouse_row) / 0.700d);
                zoom_endCol = (int)(Mouse_col + (current_endCol - Mouse_col) / 0.700d);
            }

            try
            {
                int hw_width, hw_height;
                hw_width = viewPort.WindowSize.Width;
                hw_height = viewPort.WindowSize.Height;

                bool _isOutOfArea = true;
                bool _isOutOfSize = true;
                bool _isOutOfPixel = true;  //避免像素过大

                _isOutOfArea = zoom_beginRow >= imageHeight || zoom_endRow <= 0 || zoom_beginCol >= imageWidth || zoom_endCol < 0;
                _isOutOfSize = (zoom_endRow - zoom_beginRow) > imageHeight * 20 || (zoom_endCol - zoom_beginCol) > imageWidth * 20;
                _isOutOfPixel = hw_height / (zoom_endRow - zoom_beginRow) > 500 || hw_width / (zoom_endCol - zoom_beginCol) > 500;

                if (_isOutOfArea || _isOutOfSize || _isOutOfPixel)
                {
                    return;
                }

                //viewPort.HalconWindow.ClearWindow();

                viewPort.HalconWindow.SetPaint(new HTuple("default"));
                //              保持图像显示比例
                viewPort.HalconWindow.SetPart(zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_beginCol + (zoom_endRow - zoom_beginRow) * hw_width / hw_height);

                int w = (zoom_endRow - zoom_beginRow) * hw_width / hw_height;
                int w0 = current_endCol - current_beginCol;
                double scale = (double)w / w0;
                ZoomWndFactor *= scale;
                Repaint();

            }
            catch         //ex.Message;
            {
                //DispImageFit();
            }
        }
        /*******************************************************************/
        private void moveImage(double motionX, double motionY)
        {
            viewPort.HalconWindow.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);

            ImgRow1 = current_beginRow - (int)motionY;
            ImgRow2 = current_endRow - (int)motionY;

            ImgCol1 = current_beginCol - (int)motionX;
            ImgCol2 = current_endCol - (int)motionX;

            viewPort.HalconWindow.SetPart((int)ImgRow1, (int)ImgCol1,
                (int)ImgRow2, (int)ImgCol2);
            Repaint();
        }


        /// <summary>
        /// Resets all parameters that concern the HALCON window display 
        /// setup to their initial values and clears the ROI list.
        /// </summary>
        public void ResetAll()
        {
            ImgRow1 = 0;
            ImgCol1 = 0;
            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            ZoomWndFactor = (double)imageWidth / viewPort.Width;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Width = imageWidth;
            rect.Height = imageHeight;
            viewPort.ImagePart = rect;


            if (roiManager != null)
                roiManager.Reset();
        }
        /// <summary>
        /// 窗口图像显示区域重置,不刷新图像
        /// </summary>
		public void ResetWindow()
        {
            if (imageHeight == 0)
            {
                return;
            }
            //判断行缩放还是列缩放
            double scaleC = (double)imageWidth / viewPort.Width;
            double scaleR = (double)imageHeight / viewPort.Height;

            double w, h;
            if (scaleC < scaleR)
            {

                h = viewPort.Height * scaleR;
                w = viewPort.Width * scaleR;
                ImgRow1 = 0;
                ImgCol1 = (imageWidth - w) / 2.0;
            }
            else
            {
                h = viewPort.Height * scaleC;
                w = viewPort.Width * scaleC;

                ImgRow1 = (imageHeight - h) / 2.0;
                ImgCol1 = 0;

            }

            ImgRow2 = ImgRow1 + h - 1;
            ImgCol2 = ImgCol1 + w - 1;

            ZoomWndFactor = w / viewPort.Width;
            viewPort.HalconWindow.SetPart((int)ImgRow1, (int)ImgCol1, (int)ImgRow2, (int)ImgCol2);
        }


        /*************************************************************************/
        /*      			 Event handling for mouse	   	                     */
        /*************************************************************************/
        private void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            mousePressed = true;
            int state;
            double x, y;
            try
            {
                viewPort.HalconWindow.GetMpositionSubPix(out y, out x, out state);
            }
            catch (HalconException)
            {
                return;
            }
            int activeROIidx = -1;

            if (roiManager != null && (showMode == ShowMode.IncludeROI))
            {
                activeROIidx = roiManager.MouseDownAction(x, y);
            }

            if (activeROIidx == -1)
            {
                startX = x;
                startY = y;
            }
        }

        /*******************************************************************/
        private void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            mousePressed = false;

            if (roiManager != null
                && (roiManager.ActiveRoiIdx != -1)
                && (showMode == ShowMode.IncludeROI))
            {
                roiManager.TiggerROINotifyEvent(new ViewEventArgs(ViewMessage.UpdateROI));
            }
        }
        public void ShowOK()
        {
            TriggerShowMessageEvent(new ShowMessageEventArgs(MessageType.ShowOk));
        }
        public void ShowNg()
        {
            TriggerShowMessageEvent(new ShowMessageEventArgs(MessageType.ShowNg));
        }
        /*******************************************************************/
        private void mouseMoved(object sender, HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            double motionX, motionY;
#if NativeCode
            if (Wrapper.ShowUnit.IsEmpty(viewPort.HalconWindow))
            {
                return;
            }

#else
#endif


            double currX, currY;
            HTuple currX1 = 0, currY1 = 0;
            try
            {
#if NativeCode
                string message = Wrapper.ShowUnit.GetPixMessage(viewPort.HalconWindow, out currX1, out currY1);
#else
                if (HObjList.Count < 1 || HObjList[0].HObj == null || (HObjList[0].HObj is HImage) == false)
                {
                    return;
                }
                int state;
                viewPort.HalconWindow.GetMpositionSubPix(out currY, out currX, out state);
                HImage hv_image = HObjList[0].HObj as HImage;

                string str_value = "";
                string str_position = "";
                bool _isXOut = true, _isYOut = true;
                int channel_count;
                string str_imgSize = string.Format("{0}*{1}", imageHeight, imageWidth);
                channel_count = hv_image.CountChannels();

                str_position = string.Format("|{0:F0}*{1:F0}", currY, currX);
                _isXOut = (currX < 0 || currX >= imageWidth);
                _isYOut = (currY < 0 || currY >= imageHeight);

                if (!_isXOut && !_isYOut)
                {
                    if ((int)channel_count == 1)
                    {
                        double grayVal;
                        grayVal = hv_image.GetGrayval((int)currY, (int)currX);
                        str_value = String.Format("|{0}", grayVal);
                    }
                    else if ((int)channel_count == 3)
                    {
                        double grayValRed, grayValGreen, grayValBlue;

                        HImage _RedChannel, _GreenChannel, _BlueChannel;

                        _RedChannel = hv_image.AccessChannel(1);
                        _GreenChannel = hv_image.AccessChannel(2);
                        _BlueChannel = hv_image.AccessChannel(3);

                        grayValRed = _RedChannel.GetGrayval((int)currY, (int)currX);
                        grayValGreen = _GreenChannel.GetGrayval((int)currY, (int)currX);
                        grayValBlue = _BlueChannel.GetGrayval((int)currY, (int)currX);
                        str_value = String.Format("| R:{0}, G:{1}, B:{2})", grayValRed, grayValGreen, grayValBlue);
                    }
                    else
                    {
                        str_value = "";
                    }
                }
                string message = str_imgSize + str_position + str_value;
#endif
                if (message.Length > 0)
                {
                    MousePosMessage = message;
                    TriggerShowMessageEvent(new ShowMessageEventArgs(message));
                }
                else
                {
                    return;
                }
                if (!mousePressed)
                    return;
                if (currX1.Length != 1 || currY1.Length != 1)
                {
                    return;
                }
#if NativeCode
                currX = currX1;
                currY = currY1;
#endif
                if (roiManager != null &&
                    (roiManager.ActiveRoiIdx != -1) && (showMode == ShowMode.IncludeROI))
                {

                    roiManager.MouseMoveAction(currX, currY);
                }
                else
                {
                    //qDebug()<<"xx.D():"<<xx.;
                    motionX = ((currX - startX));
                    motionY = ((currY - startY));

                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = currX - motionX;
                        startY = currY - motionY;
                    }
                }
            }
            catch (HOperatorException)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        ///控件图像对象刷新
        /// </summary>
        public void Repaint()
        {
            repaint(viewPort.HalconWindow);
        }

        /// <summary>
        /// 重绘halcon窗口
        /// </summary>
        private void repaint(HWindow window)
        {
            if (window.IsInitialized() == false)
            {
                return;
            }
            double scale1 = (1.0) / ZoomWndFactor;
#if NativeCode
            Wrapper.ShowUnit.Refresh(window, ResultShow == ResultShow.原图 ? true : false,
                isShowMessage, scale1);

#else
            ShowObject(window);
#endif

            int scale = (int)((double)(viewPort.Width) * ZoomWndFactor);

            if (roiManager != null && (showMode == ShowMode.IncludeROI))
                roiManager.PaintData(window, scale, scale1);
            //显示十字架等

#if NativeCode
            Wrapper.ShowUnit.ShowHat(window, isShowCross);

#else
            ShowHat(window);
#endif
        }
#if !NativeCode
        void ShowObject(HWindow window)
        {
            if (window.IsInitialized() == false)
            {
                return;
            }
            //关闭显示刷新
            HSystem.SetSystem("flush_graphic", "false");
            //窗体图像清空
            window.ClearWindow();
            mGC.stateOfSettings.Clear();
            try
            {
                //HTuple showStart;
                //HOperatorSet.CountSeconds(out showStart);

                int count1 = 0;
                foreach (var item in HObjList)
                {
                    if (ResultShow == ResultShow.原图 && count1 > 0)
                    {
                        break;
                    }
                    if (item.HObj != null && item.HObj.IsInitialized())
                    {

                        mGC.ApplyContext(window, item.gContext);
                        window.DispObj(item.HObj);

                    }
                    else if (item.Message != null && isShowMessage)
                    {
                        //item.Message.DispMessage(window, "image", ((double)imageWidth / (double)(viewPort.Width)) / ZoomWndFactor);
                        double sizeTmp = item.Message.CahangeDisplayFontSize(window, (1.0) / ZoomWndFactor, currentTextSize);
                        currentTextSize = sizeTmp;
                        item.Message.DispMessage(window, "image");

                    }
                    count1++;
                }
                //HTuple showEnd;
                //HOperatorSet.CountSeconds(out showEnd);
                //double timeShow = (showEnd - showStart) * 1000.0;
                //Util.Notify(string.Format("内部显示图像用时{0:f2}ms", timeShow));
            }
            catch (Exception)
            {; }
        }
        void ShowHat(HWindow window)
        {
            HSystem.SetSystem("flush_graphic", "true");
            if (isShowCross)
            {
                //获取当前显示信息
                HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
                int hv_lineWidth;

                window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

                hv_lineWidth = (int)window.GetLineWidth();
                string hv_Draw = window.GetDraw();
                window.SetLineWidth(1);//设置线宽
                window.SetLineStyle(new HTuple());
                window.SetColor("green");//十字架显示颜色
                double CrossCol = (double)imageWidth / 2.0, CrossRow = (double)imageHeight / 2.0;
                double borderWidth = (double)imageWidth / 50.0;
                CrossCol = (double)imageWidth / 2.0;
                CrossRow = (double)imageHeight / 2.0;
                //竖线1
                //window.DispLine(0, CrossCol, CrossRow - 50, CrossCol);

                //window.DispLine(CrossRow + 50, CrossCol, imageHeight, CrossCol);

                window.DispPolygon(new HTuple(0, CrossRow - 50), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow + 50, ImageHeight), new HTuple(CrossCol, CrossCol));


                //中心点
                window.DispPolygon(new HTuple(CrossRow - 2, CrossRow + 2), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol - 2, CrossCol + 2));

                //横线

                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(0, CrossCol - 50));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol + 50, ImageWidth));


                //window.DispLine(CrossRow, 0, CrossRow, CrossCol - 50);
                //window.DispLine(CrossRow, CrossCol + 50, CrossRow, imageWidth);

                //恢复窗口显示信息
                window.SetRgb(hv_Red, hv_Green, hv_Blue);
                window.SetLineWidth(hv_lineWidth);
                window.SetDraw(hv_Draw);
            }
            else
            {
                window.SetColor(backguoundColor);
                window.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
        }

#endif
        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/
        public void AddText(string message, int row, int colunm, int size, HTuple color)
        {
#if NativeCode
            Wrapper.ShowUnit.AddText(viewPort.HalconWindow, message, row, colunm, size, color);

#else
            AddIconicVar(new HWndMessage(message, row, colunm, size, color));
#endif

        }

        public void AddText(string message, int row, int colunm)
        {
#if NativeCode
            Wrapper.ShowUnit.AddText(viewPort.HalconWindow, message, row, colunm);

#else
            AddIconicVar(new HWndMessage(message, row, colunm));
#endif

        }
#if !NativeCode
        public void AddIconicVar(HWndMessage message)
        {
            HObjectEntry entry;
            if (message == null)
            {
                return;
            }

            entry = new HObjectEntry(message);

            HObjList.Add(entry);

            if (HObjList.Count > MAX_NUM_OBJ_LIST)
                HObjList.RemoveAt(1);
        }
#endif
        /// <summary>
        /// 添加图像变量
        /// </summary>
        /// <param name="obj"></param>
        public void AddIconicVar(HObject obj)
        {
            if (obj == null)
                return;
            //图片数据就更新长宽信息
            if (obj is HImage && obj.IsInitialized())
            {
                double r, c;
                int h, w, area;
                string s;

                area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);
                ((HImage)obj).GetImagePointer1(out s, out w, out h);

                //图像无论是否Domain(), w及h不会变化
                if ((h != imageHeight) || (w != imageWidth))
                {
#if NativeCode
                    Wrapper.ShowUnit.SetImageSize(viewPort.HalconWindow, w, h);

#else
                    //viewPort.HalconWindow.SetWindowParam("background_color", backguoundColor);
#endif

                    imageHeight = h;
                    imageWidth = w;
                    ResetWindow();
                }

                //面积=长*宽 表示确实是图片
                if (area == (w * h))
                {
                    //viewPort.HalconWindow.AttachBackgroundToWindow((HImage)obj);

                    //HTuple winid = 1;
                    //Wrapper.ShowUnit.ClearEntryList(viewPort.HalconWindow);
                    //ResetWindow();
                    ClearList();
                    isShowMessage = true;
                    resultShow = ResultShow.处理后;
                    //if ((h != imageHeight) || (w != imageWidth))
                    //{
                    //    imageHeight = h;
                    //    imageWidth = w;
                    //    Wrapper.ShowUnit.ClearEntryList(viewPort.HalconWindow);
                    //    ResetWindow();
                    //}
                }
            }

#if NativeCode
            Wrapper.ShowUnit.AddIconicVar(viewPort.HalconWindow, obj);

#else
            HObjectEntry entry;
            entry = new HObjectEntry(obj, mGC.CopyContextList());
            //entry = new HObjectEntry(obj, new Hashtable());
            HObjList.Add(entry);

            if (HObjList.Count > MAX_NUM_OBJ_LIST)
                HObjList.RemoveAt(1);
#endif

        }


        /// <summary>
        /// 清除图像列表
        /// </summary>
        public void ClearList()
        {
#if NativeCode
            Wrapper.ShowUnit.ClearEntryList(winHandle);
#else
            HObjList.Clear();
#endif

        }


        public void ClearWindowData()
        {
#if NativeCode
            //控件释放时候会调用,不能在这里获取控件handle
            Wrapper.ShowUnit.ClearWindowData(winHandle);
#else
            HObjList.Clear();
#endif
        }

        /// <summary>
        /// 修改运行的状态
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="val"></param>
        public void ChangeGraphicSettings(HTuple mode, HTuple val)
        {
#if NativeCode
            Wrapper.ShowUnit.ChangeGraphicSettings(viewPort.HalconWindow, mode, val);

#else
            switch (mode.S)
            {
                case GraphicsContext.GC_COLOR:
                    mGC.SetColorAttribute(val);
                    break;
                case GraphicsContext.GC_DRAWMODE:
                    mGC.SetDrawModeAttribute(val);
                    break;
                case GraphicsContext.GC_LUT:
                    mGC.SetLutAttribute(val);
                    break;
                case GraphicsContext.GC_PAINT:
                    mGC.SetPaintAttribute(val);
                    break;
                case GraphicsContext.GC_SHAPE:
                    mGC.SetShapeAttribute(val);
                    break;
                case GraphicsContext.GC_COLORED:
                    mGC.SetColoredAttribute(val);
                    break;
                case GraphicsContext.GC_LINEWIDTH:
                    mGC.SetLineWidthAttribute(val);
                    break;
                case GraphicsContext.GC_LINESTYLE:
                    mGC.SetLineStyleAttribute(val);
                    break;
                default:
                    break;
            }
#endif

        }
        public void SetBackgroundColor(string color)
        {
            backguoundColor = color;
#if NativeCode
            Wrapper.ShowUnit.SetBackgroundColor(viewPort.HalconWindow, color);

#else
            viewPort.HalconWindow.SetWindowParam("background_color", backguoundColor);
#endif


            Repaint();
        }
    }//end of class
}//end of namespace
