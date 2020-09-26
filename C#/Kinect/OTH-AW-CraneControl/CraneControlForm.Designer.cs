namespace OTH_AW_CraneControl
{
    partial class CraneControlForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gBControl = new System.Windows.Forms.GroupBox();
            this.cpbVelocity = new CraneControl.CustomProgressBar();
            this.pbDown = new System.Windows.Forms.PictureBox();
            this.pbRight = new System.Windows.Forms.PictureBox();
            this.pbLeft = new System.Windows.Forms.PictureBox();
            this.pbController = new System.Windows.Forms.PictureBox();
            this.pbUp = new System.Windows.Forms.PictureBox();
            this.pbAxis = new System.Windows.Forms.PictureBox();
            this.btSettings = new System.Windows.Forms.Button();
            this.pBCam = new System.Windows.Forms.PictureBox();
            this.btServerControll = new System.Windows.Forms.Button();
            this.btPause = new System.Windows.Forms.Button();
            this.lbAcceleration = new System.Windows.Forms.Label();
            this.lbPort = new System.Windows.Forms.Label();
            this.txAcceleration = new System.Windows.Forms.TextBox();
            this.txPort = new System.Windows.Forms.TextBox();
            this.cBkinectframe = new System.Windows.Forms.ComboBox();
            this.pBClient = new System.Windows.Forms.PictureBox();
            this.lbFrame = new System.Windows.Forms.Label();
            this.lbConnected = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gBControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cpbVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBCam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBClient)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gBControl);
            this.splitContainer1.Panel1.Controls.Add(this.btSettings);
            this.splitContainer1.Panel1.Controls.Add(this.pBCam);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btServerControll);
            this.splitContainer1.Panel2.Controls.Add(this.btPause);
            this.splitContainer1.Panel2.Controls.Add(this.lbAcceleration);
            this.splitContainer1.Panel2.Controls.Add(this.lbPort);
            this.splitContainer1.Panel2.Controls.Add(this.txAcceleration);
            this.splitContainer1.Panel2.Controls.Add(this.txPort);
            this.splitContainer1.Panel2.Controls.Add(this.cBkinectframe);
            this.splitContainer1.Panel2.Controls.Add(this.pBClient);
            this.splitContainer1.Panel2.Controls.Add(this.lbFrame);
            this.splitContainer1.Panel2.Controls.Add(this.lbConnected);
            this.splitContainer1.Size = new System.Drawing.Size(769, 678);
            this.splitContainer1.SplitterDistance = 645;
            this.splitContainer1.TabIndex = 0;
            // 
            // gBControl
            // 
            this.gBControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gBControl.BackColor = System.Drawing.Color.Transparent;
            this.gBControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gBControl.Controls.Add(this.cpbVelocity);
            this.gBControl.Controls.Add(this.pbDown);
            this.gBControl.Controls.Add(this.pbRight);
            this.gBControl.Controls.Add(this.pbLeft);
            this.gBControl.Controls.Add(this.pbController);
            this.gBControl.Controls.Add(this.pbUp);
            this.gBControl.Controls.Add(this.pbAxis);
            this.gBControl.Location = new System.Drawing.Point(3, 535);
            this.gBControl.Name = "gBControl";
            this.gBControl.Size = new System.Drawing.Size(110, 140);
            this.gBControl.TabIndex = 1;
            this.gBControl.TabStop = false;
            this.gBControl.Text = "Control";
            // 
            // cpbVelocity
            // 
            this.cpbVelocity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cpbVelocity.Location = new System.Drawing.Point(10, 110);
            this.cpbVelocity.max = 0;
            this.cpbVelocity.min = 0;
            this.cpbVelocity.Name = "cpbVelocity";
            this.cpbVelocity.progressBrush = null;
            this.cpbVelocity.progressFont = null;
            this.cpbVelocity.progressFontBrush = null;
            this.cpbVelocity.Size = new System.Drawing.Size(85, 25);
            this.cpbVelocity.TabIndex = 7;
            this.cpbVelocity.TabStop = false;
            // 
            // pbDown
            // 
            this.pbDown.BackgroundImage = global::CraneControl.Properties.Resources.arrow_down;
            this.pbDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbDown.Location = new System.Drawing.Point(40, 80);
            this.pbDown.Name = "pbDown";
            this.pbDown.Size = new System.Drawing.Size(25, 25);
            this.pbDown.TabIndex = 6;
            this.pbDown.TabStop = false;
            // 
            // pbRight
            // 
            this.pbRight.BackgroundImage = global::CraneControl.Properties.Resources.arrow_right;
            this.pbRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbRight.Location = new System.Drawing.Point(70, 50);
            this.pbRight.Name = "pbRight";
            this.pbRight.Size = new System.Drawing.Size(25, 25);
            this.pbRight.TabIndex = 5;
            this.pbRight.TabStop = false;
            // 
            // pbLeft
            // 
            this.pbLeft.BackgroundImage = global::CraneControl.Properties.Resources.arrow_left;
            this.pbLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbLeft.Location = new System.Drawing.Point(10, 50);
            this.pbLeft.Name = "pbLeft";
            this.pbLeft.Size = new System.Drawing.Size(25, 25);
            this.pbLeft.TabIndex = 3;
            this.pbLeft.TabStop = false;
            // 
            // pbController
            // 
            this.pbController.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbController.Location = new System.Drawing.Point(70, 20);
            this.pbController.Name = "pbController";
            this.pbController.Size = new System.Drawing.Size(25, 25);
            this.pbController.TabIndex = 2;
            this.pbController.TabStop = false;
            // 
            // pbUp
            // 
            this.pbUp.BackgroundImage = global::CraneControl.Properties.Resources.arrow_up;
            this.pbUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbUp.Location = new System.Drawing.Point(40, 20);
            this.pbUp.Name = "pbUp";
            this.pbUp.Size = new System.Drawing.Size(25, 25);
            this.pbUp.TabIndex = 1;
            this.pbUp.TabStop = false;
            // 
            // pbAxis
            // 
            this.pbAxis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbAxis.Location = new System.Drawing.Point(10, 20);
            this.pbAxis.Name = "pbAxis";
            this.pbAxis.Size = new System.Drawing.Size(25, 25);
            this.pbAxis.TabIndex = 0;
            this.pbAxis.TabStop = false;
            // 
            // btSettings
            // 
            this.btSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSettings.BackgroundImage = global::CraneControl.Properties.Resources.gear;
            this.btSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btSettings.Location = new System.Drawing.Point(593, 2);
            this.btSettings.Margin = new System.Windows.Forms.Padding(2);
            this.btSettings.Name = "btSettings";
            this.btSettings.Size = new System.Drawing.Size(50, 50);
            this.btSettings.TabIndex = 1;
            this.btSettings.UseVisualStyleBackColor = true;
            this.btSettings.Click += new System.EventHandler(this.btSettings_Click);
            // 
            // pBCam
            // 
            this.pBCam.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pBCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBCam.Location = new System.Drawing.Point(0, 0);
            this.pBCam.Name = "pBCam";
            this.pBCam.Size = new System.Drawing.Size(645, 678);
            this.pBCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBCam.TabIndex = 0;
            this.pBCam.TabStop = false;
            // 
            // btServerControll
            // 
            this.btServerControll.Location = new System.Drawing.Point(5, 270);
            this.btServerControll.Name = "btServerControll";
            this.btServerControll.Size = new System.Drawing.Size(100, 50);
            this.btServerControll.TabIndex = 9;
            this.btServerControll.Text = "Server off";
            this.btServerControll.UseVisualStyleBackColor = true;
            this.btServerControll.Click += new System.EventHandler(this.btServerControll_Click);
            // 
            // btPause
            // 
            this.btPause.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btPause.Location = new System.Drawing.Point(5, 210);
            this.btPause.Name = "btPause";
            this.btPause.Size = new System.Drawing.Size(100, 50);
            this.btPause.TabIndex = 8;
            this.btPause.Text = "Pause";
            this.btPause.UseVisualStyleBackColor = true;
            this.btPause.Click += new System.EventHandler(this.btPause_Click);
            // 
            // lbAcceleration
            // 
            this.lbAcceleration.AutoSize = true;
            this.lbAcceleration.Location = new System.Drawing.Point(5, 165);
            this.lbAcceleration.Name = "lbAcceleration";
            this.lbAcceleration.Size = new System.Drawing.Size(66, 13);
            this.lbAcceleration.TabIndex = 7;
            this.lbAcceleration.Text = "Acceleration";
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(5, 10);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(26, 13);
            this.lbPort.TabIndex = 0;
            this.lbPort.Text = "Port";
            // 
            // txAcceleration
            // 
            this.txAcceleration.Location = new System.Drawing.Point(5, 180);
            this.txAcceleration.Name = "txAcceleration";
            this.txAcceleration.Size = new System.Drawing.Size(100, 20);
            this.txAcceleration.TabIndex = 6;
            this.txAcceleration.TextChanged += new System.EventHandler(this.TxAcceleration_TextChanged);
            // 
            // txPort
            // 
            this.txPort.Location = new System.Drawing.Point(5, 25);
            this.txPort.Name = "txPort";
            this.txPort.Size = new System.Drawing.Size(100, 20);
            this.txPort.TabIndex = 1;
            this.txPort.TextChanged += new System.EventHandler(this.TxPort_TextChanged);
            // 
            // cBkinectframe
            // 
            this.cBkinectframe.FormattingEnabled = true;
            this.cBkinectframe.Location = new System.Drawing.Point(5, 140);
            this.cBkinectframe.Name = "cBkinectframe";
            this.cBkinectframe.Size = new System.Drawing.Size(100, 21);
            this.cBkinectframe.TabIndex = 5;
            this.cBkinectframe.SelectedIndexChanged += new System.EventHandler(this.cBkinectframe_SelectedIndexChanged);
            // 
            // pBClient
            // 
            this.pBClient.BackgroundImage = global::CraneControl.Properties.Resources.off;
            this.pBClient.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pBClient.Location = new System.Drawing.Point(5, 70);
            this.pBClient.Name = "pBClient";
            this.pBClient.Size = new System.Drawing.Size(50, 50);
            this.pBClient.TabIndex = 2;
            this.pBClient.TabStop = false;
            // 
            // lbFrame
            // 
            this.lbFrame.AutoSize = true;
            this.lbFrame.Location = new System.Drawing.Point(5, 125);
            this.lbFrame.Name = "lbFrame";
            this.lbFrame.Size = new System.Drawing.Size(33, 13);
            this.lbFrame.TabIndex = 4;
            this.lbFrame.Text = "frame";
            // 
            // lbConnected
            // 
            this.lbConnected.AutoSize = true;
            this.lbConnected.Location = new System.Drawing.Point(5, 50);
            this.lbConnected.Name = "lbConnected";
            this.lbConnected.Size = new System.Drawing.Size(88, 13);
            this.lbConnected.TabIndex = 3;
            this.lbConnected.Text = "Client Connected";
            // 
            // CraneControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(769, 678);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CraneControlForm";
            this.Text = "OTH-AW-CraneControl";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CranecontrolForm_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gBControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cpbVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBCam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBClient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btSettings;
        private System.Windows.Forms.PictureBox pBCam;
        private System.Windows.Forms.GroupBox gBControl;
        private System.Windows.Forms.TextBox txPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.PictureBox pbDown;
        private System.Windows.Forms.PictureBox pbRight;
        private System.Windows.Forms.PictureBox pbLeft;
        private System.Windows.Forms.PictureBox pbController;
        private System.Windows.Forms.PictureBox pbUp;
        private System.Windows.Forms.PictureBox pbAxis;
        private System.Windows.Forms.Button btServerControll;
        private System.Windows.Forms.Button btPause;
        private System.Windows.Forms.Label lbAcceleration;
        private System.Windows.Forms.TextBox txAcceleration;
        private System.Windows.Forms.ComboBox cBkinectframe;
        private System.Windows.Forms.Label lbFrame;
        private System.Windows.Forms.Label lbConnected;
        private System.Windows.Forms.PictureBox pBClient;
        private CraneControl.CustomProgressBar cpbVelocity;
    }
}

