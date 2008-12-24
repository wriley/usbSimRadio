namespace usbSimRadio
{
    partial class usbSimRadioForm
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
            this.buttonExit = new System.Windows.Forms.Button();
            this.listBoxDevices = new System.Windows.Forms.ListBox();
            this.labelDevicePath = new System.Windows.Forms.Label();
            this.FreqActive1 = new System.Windows.Forms.NumericUpDown();
            this.FreqActive2 = new System.Windows.Forms.NumericUpDown();
            this.labelActive = new System.Windows.Forms.Label();
            this.labelStandby = new System.Windows.Forms.Label();
            this.FreqStandby2 = new System.Windows.Forms.NumericUpDown();
            this.FreqStandby1 = new System.Windows.Forms.NumericUpDown();
            this.buttonRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FreqActive1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreqActive2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreqStandby2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreqStandby1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(12, 322);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "&Quit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // listBoxDevices
            // 
            this.listBoxDevices.FormattingEnabled = true;
            this.listBoxDevices.Location = new System.Drawing.Point(279, 12);
            this.listBoxDevices.Name = "listBoxDevices";
            this.listBoxDevices.ScrollAlwaysVisible = true;
            this.listBoxDevices.Size = new System.Drawing.Size(199, 95);
            this.listBoxDevices.Sorted = true;
            this.listBoxDevices.TabIndex = 1;
            this.listBoxDevices.SelectedIndexChanged += new System.EventHandler(this.listBoxDevices_SelectedIndexChanged);
            // 
            // labelDevicePath
            // 
            this.labelDevicePath.AutoSize = true;
            this.labelDevicePath.Location = new System.Drawing.Point(301, 335);
            this.labelDevicePath.Name = "labelDevicePath";
            this.labelDevicePath.Size = new System.Drawing.Size(63, 13);
            this.labelDevicePath.TabIndex = 2;
            this.labelDevicePath.Text = "DevicePath";
            // 
            // FreqActive1
            // 
            this.FreqActive1.Location = new System.Drawing.Point(64, 7);
            this.FreqActive1.Maximum = new decimal(new int[] {
            136,
            0,
            0,
            0});
            this.FreqActive1.Minimum = new decimal(new int[] {
            118,
            0,
            0,
            0});
            this.FreqActive1.Name = "FreqActive1";
            this.FreqActive1.ReadOnly = true;
            this.FreqActive1.Size = new System.Drawing.Size(45, 20);
            this.FreqActive1.TabIndex = 3;
            this.FreqActive1.Value = new decimal(new int[] {
            118,
            0,
            0,
            0});
            this.FreqActive1.ValueChanged += new System.EventHandler(this.FreqActive1_ValueChanged);
            // 
            // FreqActive2
            // 
            this.FreqActive2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.FreqActive2.Location = new System.Drawing.Point(115, 7);
            this.FreqActive2.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.FreqActive2.Name = "FreqActive2";
            this.FreqActive2.ReadOnly = true;
            this.FreqActive2.Size = new System.Drawing.Size(45, 20);
            this.FreqActive2.TabIndex = 4;
            this.FreqActive2.ValueChanged += new System.EventHandler(this.FreqActive2_ValueChanged);
            // 
            // labelActive
            // 
            this.labelActive.AutoSize = true;
            this.labelActive.Location = new System.Drawing.Point(12, 9);
            this.labelActive.Name = "labelActive";
            this.labelActive.Size = new System.Drawing.Size(37, 13);
            this.labelActive.TabIndex = 5;
            this.labelActive.Text = "Active";
            // 
            // labelStandby
            // 
            this.labelStandby.AutoSize = true;
            this.labelStandby.Location = new System.Drawing.Point(12, 35);
            this.labelStandby.Name = "labelStandby";
            this.labelStandby.Size = new System.Drawing.Size(46, 13);
            this.labelStandby.TabIndex = 8;
            this.labelStandby.Text = "Standby";
            // 
            // FreqStandby2
            // 
            this.FreqStandby2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.FreqStandby2.Location = new System.Drawing.Point(115, 33);
            this.FreqStandby2.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.FreqStandby2.Name = "FreqStandby2";
            this.FreqStandby2.ReadOnly = true;
            this.FreqStandby2.Size = new System.Drawing.Size(45, 20);
            this.FreqStandby2.TabIndex = 7;
            this.FreqStandby2.ValueChanged += new System.EventHandler(this.FreqStandby2_ValueChanged);
            // 
            // FreqStandby1
            // 
            this.FreqStandby1.Location = new System.Drawing.Point(64, 33);
            this.FreqStandby1.Maximum = new decimal(new int[] {
            136,
            0,
            0,
            0});
            this.FreqStandby1.Minimum = new decimal(new int[] {
            118,
            0,
            0,
            0});
            this.FreqStandby1.Name = "FreqStandby1";
            this.FreqStandby1.ReadOnly = true;
            this.FreqStandby1.Size = new System.Drawing.Size(45, 20);
            this.FreqStandby1.TabIndex = 6;
            this.FreqStandby1.Value = new decimal(new int[] {
            118,
            0,
            0,
            0});
            this.FreqStandby1.ValueChanged += new System.EventHandler(this.FreqStandby1_ValueChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(403, 113);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 9;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // usbSimRadioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 357);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.labelStandby);
            this.Controls.Add(this.FreqStandby2);
            this.Controls.Add(this.FreqStandby1);
            this.Controls.Add(this.labelActive);
            this.Controls.Add(this.FreqActive2);
            this.Controls.Add(this.FreqActive1);
            this.Controls.Add(this.labelDevicePath);
            this.Controls.Add(this.listBoxDevices);
            this.Controls.Add(this.buttonExit);
            this.Name = "usbSimRadioForm";
            this.Text = "usbSimRadio";
            this.Load += new System.EventHandler(this.usbSimRadioForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FreqActive1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreqActive2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreqStandby2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FreqStandby1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ListBox listBoxDevices;
        private System.Windows.Forms.Label labelDevicePath;
        private System.Windows.Forms.NumericUpDown FreqActive1;
        private System.Windows.Forms.NumericUpDown FreqActive2;
        private System.Windows.Forms.Label labelActive;
        private System.Windows.Forms.Label labelStandby;
        private System.Windows.Forms.NumericUpDown FreqStandby2;
        private System.Windows.Forms.NumericUpDown FreqStandby1;
        private System.Windows.Forms.Button buttonRefresh;
    }
}

