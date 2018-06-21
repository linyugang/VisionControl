namespace Yoga.ImageControl
{
    partial class ROIActUnit
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxCreateROI = new System.Windows.Forms.GroupBox();
            this.reduceRect1Button = new System.Windows.Forms.Button();
            this.circleButton = new System.Windows.Forms.Button();
            this.delROIButton = new System.Windows.Forms.Button();
            this.delAllROIButton = new System.Windows.Forms.Button();
            this.rect2Button = new System.Windows.Forms.Button();
            this.subFromROIButton = new System.Windows.Forms.RadioButton();
            this.addToROIButton = new System.Windows.Forms.RadioButton();
            this.rect1Button = new System.Windows.Forms.Button();
            this.groupBoxCreateROI.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCreateROI
            // 
            this.groupBoxCreateROI.Controls.Add(this.reduceRect1Button);
            this.groupBoxCreateROI.Controls.Add(this.circleButton);
            this.groupBoxCreateROI.Controls.Add(this.delROIButton);
            this.groupBoxCreateROI.Controls.Add(this.delAllROIButton);
            this.groupBoxCreateROI.Controls.Add(this.rect2Button);
            this.groupBoxCreateROI.Controls.Add(this.subFromROIButton);
            this.groupBoxCreateROI.Controls.Add(this.addToROIButton);
            this.groupBoxCreateROI.Controls.Add(this.rect1Button);
            this.groupBoxCreateROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCreateROI.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCreateROI.Name = "groupBoxCreateROI";
            this.groupBoxCreateROI.Size = new System.Drawing.Size(202, 155);
            this.groupBoxCreateROI.TabIndex = 96;
            this.groupBoxCreateROI.TabStop = false;
            this.groupBoxCreateROI.Text = "创建ROI";
            // 
            // reduceRect1Button
            // 
            this.reduceRect1Button.Location = new System.Drawing.Point(101, 58);
            this.reduceRect1Button.Name = "reduceRect1Button";
            this.reduceRect1Button.Size = new System.Drawing.Size(80, 26);
            this.reduceRect1Button.TabIndex = 13;
            this.reduceRect1Button.Text = "搜索矩形";
            this.reduceRect1Button.Click += new System.EventHandler(this.reduceRect1Button_Click);
            // 
            // circleButton
            // 
            this.circleButton.Location = new System.Drawing.Point(6, 117);
            this.circleButton.Name = "circleButton";
            this.circleButton.Size = new System.Drawing.Size(80, 26);
            this.circleButton.TabIndex = 12;
            this.circleButton.Text = "圆形";
            this.circleButton.Click += new System.EventHandler(this.circleButton_Click);
            // 
            // delROIButton
            // 
            this.delROIButton.Location = new System.Drawing.Point(96, 90);
            this.delROIButton.Name = "delROIButton";
            this.delROIButton.Size = new System.Drawing.Size(86, 26);
            this.delROIButton.TabIndex = 11;
            this.delROIButton.Text = "删除激活ROI";
            this.delROIButton.Click += new System.EventHandler(this.delROIButton_Click);
            // 
            // delAllROIButton
            // 
            this.delAllROIButton.Location = new System.Drawing.Point(96, 117);
            this.delAllROIButton.Name = "delAllROIButton";
            this.delAllROIButton.Size = new System.Drawing.Size(86, 26);
            this.delAllROIButton.TabIndex = 10;
            this.delAllROIButton.Text = "删除所有ROI";
            this.delAllROIButton.Click += new System.EventHandler(this.delAllROIButton_Click);
            // 
            // rect2Button
            // 
            this.rect2Button.Location = new System.Drawing.Point(6, 88);
            this.rect2Button.Name = "rect2Button";
            this.rect2Button.Size = new System.Drawing.Size(80, 26);
            this.rect2Button.TabIndex = 9;
            this.rect2Button.Text = "带角度矩形";
            this.rect2Button.Click += new System.EventHandler(this.rect2Button_Click);
            // 
            // subFromROIButton
            // 
            this.subFromROIButton.Location = new System.Drawing.Point(125, 17);
            this.subFromROIButton.Name = "subFromROIButton";
            this.subFromROIButton.Size = new System.Drawing.Size(57, 43);
            this.subFromROIButton.TabIndex = 8;
            this.subFromROIButton.Text = "(-)";
            this.subFromROIButton.CheckedChanged += new System.EventHandler(this.subFromROIButton_CheckedChanged);
            // 
            // addToROIButton
            // 
            this.addToROIButton.Checked = true;
            this.addToROIButton.Location = new System.Drawing.Point(38, 17);
            this.addToROIButton.Name = "addToROIButton";
            this.addToROIButton.Size = new System.Drawing.Size(48, 43);
            this.addToROIButton.TabIndex = 7;
            this.addToROIButton.TabStop = true;
            this.addToROIButton.Text = "(+)";
            this.addToROIButton.CheckedChanged += new System.EventHandler(this.addToROIButton_CheckedChanged);
            // 
            // rect1Button
            // 
            this.rect1Button.Location = new System.Drawing.Point(6, 59);
            this.rect1Button.Name = "rect1Button";
            this.rect1Button.Size = new System.Drawing.Size(80, 26);
            this.rect1Button.TabIndex = 5;
            this.rect1Button.Text = "矩形";
            this.rect1Button.Click += new System.EventHandler(this.rect1Button_Click);
            // 
            // ROIActUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCreateROI);
            this.Name = "ROIActUnit";
            this.Size = new System.Drawing.Size(202, 155);
            this.groupBoxCreateROI.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCreateROI;
        private System.Windows.Forms.Button reduceRect1Button;
        private System.Windows.Forms.Button circleButton;
        private System.Windows.Forms.Button delROIButton;
        private System.Windows.Forms.Button delAllROIButton;
        private System.Windows.Forms.Button rect2Button;
        private System.Windows.Forms.RadioButton subFromROIButton;
        private System.Windows.Forms.RadioButton addToROIButton;
        private System.Windows.Forms.Button rect1Button;
    }
}
