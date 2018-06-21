namespace Yoga.Test
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hWndUnit1 = new Yoga.ImageControl.HWndUnit();
            this.roiActUnit1 = new Yoga.ImageControl.ROIActUnit();
            this.SuspendLayout();
            // 
            // hWndUnit1
            // 
            this.hWndUnit1.BackColor = System.Drawing.SystemColors.Control;
            this.hWndUnit1.CameraMessage = null;
            this.hWndUnit1.Location = new System.Drawing.Point(23, 120);
            this.hWndUnit1.MinimumSize = new System.Drawing.Size(10, 10);
            this.hWndUnit1.Name = "hWndUnit1";
            this.hWndUnit1.Size = new System.Drawing.Size(311, 305);
            this.hWndUnit1.TabIndex = 0;
            // 
            // roiActUnit1
            // 
            this.roiActUnit1.Location = new System.Drawing.Point(376, 218);
            this.roiActUnit1.Name = "roiActUnit1";
            this.roiActUnit1.RoiController = null;
            this.roiActUnit1.Size = new System.Drawing.Size(202, 155);
            this.roiActUnit1.TabIndex = 1;
            this.roiActUnit1.UseSearchRoi = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 437);
            this.Controls.Add(this.roiActUnit1);
            this.Controls.Add(this.hWndUnit1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageControl.HWndUnit hWndUnit1;
        private ImageControl.ROIActUnit roiActUnit1;
    }
}