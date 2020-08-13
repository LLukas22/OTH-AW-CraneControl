namespace DatasetStretcher
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxInput = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btn_Input = new System.Windows.Forms.Button();
			this.btn_Output = new System.Windows.Forms.Button();
			this.textBoxOutput = new System.Windows.Forms.TextBox();
			this.btn_Load = new System.Windows.Forms.Button();
			this.btn_Start = new System.Windows.Forms.Button();
			this.grayscale_chbx = new System.Windows.Forms.CheckBox();
			this.whiteNoisechbx = new System.Windows.Forms.CheckBox();
			this.blurchbx = new System.Windows.Forms.CheckBox();
			this.chropchbx = new System.Windows.Forms.CheckBox();
			this.chropIts = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.MAkro_Chbx = new System.Windows.Forms.CheckBox();
			this.ckb_Classifier = new System.Windows.Forms.CheckBox();
			this.txt_Classifier = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.chropIts)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxInput
			// 
			this.textBoxInput.Location = new System.Drawing.Point(23, 21);
			this.textBoxInput.Name = "textBoxInput";
			this.textBoxInput.Size = new System.Drawing.Size(229, 23);
			this.textBoxInput.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(23, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Input";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(23, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Output";
			// 
			// btn_Input
			// 
			this.btn_Input.Location = new System.Drawing.Point(249, 21);
			this.btn_Input.Name = "btn_Input";
			this.btn_Input.Size = new System.Drawing.Size(34, 23);
			this.btn_Input.TabIndex = 3;
			this.btn_Input.Text = "...";
			this.btn_Input.UseVisualStyleBackColor = true;
			this.btn_Input.Click += new System.EventHandler(this.btn_Input_Click);
			// 
			// btn_Output
			// 
			this.btn_Output.Location = new System.Drawing.Point(249, 62);
			this.btn_Output.Name = "btn_Output";
			this.btn_Output.Size = new System.Drawing.Size(34, 23);
			this.btn_Output.TabIndex = 3;
			this.btn_Output.Text = "...";
			this.btn_Output.UseVisualStyleBackColor = true;
			this.btn_Output.Click += new System.EventHandler(this.btn_Output_Click);
			// 
			// textBoxOutput
			// 
			this.textBoxOutput.Location = new System.Drawing.Point(23, 63);
			this.textBoxOutput.Name = "textBoxOutput";
			this.textBoxOutput.Size = new System.Drawing.Size(229, 23);
			this.textBoxOutput.TabIndex = 0;
			// 
			// btn_Load
			// 
			this.btn_Load.Location = new System.Drawing.Point(12, 335);
			this.btn_Load.Name = "btn_Load";
			this.btn_Load.Size = new System.Drawing.Size(75, 23);
			this.btn_Load.TabIndex = 4;
			this.btn_Load.Text = "Load Images";
			this.btn_Load.UseVisualStyleBackColor = true;
			this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
			// 
			// btn_Start
			// 
			this.btn_Start.Location = new System.Drawing.Point(179, 334);
			this.btn_Start.Name = "btn_Start";
			this.btn_Start.Size = new System.Drawing.Size(105, 23);
			this.btn_Start.TabIndex = 5;
			this.btn_Start.Text = "Generate Images";
			this.btn_Start.UseVisualStyleBackColor = true;
			this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
			// 
			// grayscale_chbx
			// 
			this.grayscale_chbx.AutoSize = true;
			this.grayscale_chbx.Checked = true;
			this.grayscale_chbx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.grayscale_chbx.Location = new System.Drawing.Point(23, 106);
			this.grayscale_chbx.Name = "grayscale_chbx";
			this.grayscale_chbx.Size = new System.Drawing.Size(77, 19);
			this.grayscale_chbx.TabIndex = 6;
			this.grayscale_chbx.Text = "GrayScale";
			this.grayscale_chbx.UseVisualStyleBackColor = true;
			// 
			// whiteNoisechbx
			// 
			this.whiteNoisechbx.AutoSize = true;
			this.whiteNoisechbx.Checked = true;
			this.whiteNoisechbx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.whiteNoisechbx.Location = new System.Drawing.Point(23, 131);
			this.whiteNoisechbx.Name = "whiteNoisechbx";
			this.whiteNoisechbx.Size = new System.Drawing.Size(87, 19);
			this.whiteNoisechbx.TabIndex = 7;
			this.whiteNoisechbx.Text = "WhiteNoise";
			this.whiteNoisechbx.UseVisualStyleBackColor = true;
			// 
			// blurchbx
			// 
			this.blurchbx.AutoSize = true;
			this.blurchbx.Checked = true;
			this.blurchbx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.blurchbx.Location = new System.Drawing.Point(23, 156);
			this.blurchbx.Name = "blurchbx";
			this.blurchbx.Size = new System.Drawing.Size(47, 19);
			this.blurchbx.TabIndex = 8;
			this.blurchbx.Text = "Blur";
			this.blurchbx.UseVisualStyleBackColor = true;
			// 
			// chropchbx
			// 
			this.chropchbx.AutoSize = true;
			this.chropchbx.Location = new System.Drawing.Point(23, 225);
			this.chropchbx.Name = "chropchbx";
			this.chropchbx.Size = new System.Drawing.Size(59, 19);
			this.chropchbx.TabIndex = 9;
			this.chropchbx.Text = "Chrop";
			this.chropchbx.UseVisualStyleBackColor = true;
			// 
			// chropIts
			// 
			this.chropIts.Location = new System.Drawing.Point(143, 224);
			this.chropIts.Name = "chropIts";
			this.chropIts.Size = new System.Drawing.Size(120, 23);
			this.chropIts.TabIndex = 10;
			this.chropIts.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(143, 206);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(94, 15);
			this.label3.TabIndex = 11;
			this.label3.Text = "Chrop-Iterations";
			// 
			// MAkro_Chbx
			// 
			this.MAkro_Chbx.AutoSize = true;
			this.MAkro_Chbx.Location = new System.Drawing.Point(23, 181);
			this.MAkro_Chbx.Name = "MAkro_Chbx";
			this.MAkro_Chbx.Size = new System.Drawing.Size(60, 19);
			this.MAkro_Chbx.TabIndex = 12;
			this.MAkro_Chbx.Text = "Makro";
			this.MAkro_Chbx.UseVisualStyleBackColor = true;
			// 
			// ckb_Classifier
			// 
			this.ckb_Classifier.AutoSize = true;
			this.ckb_Classifier.Location = new System.Drawing.Point(23, 267);
			this.ckb_Classifier.Name = "ckb_Classifier";
			this.ckb_Classifier.Size = new System.Drawing.Size(114, 19);
			this.ckb_Classifier.TabIndex = 13;
			this.ckb_Classifier.Text = "ChangeClassifier";
			this.ckb_Classifier.UseVisualStyleBackColor = true;
			// 
			// txt_Classifier
			// 
			this.txt_Classifier.Location = new System.Drawing.Point(143, 265);
			this.txt_Classifier.Name = "txt_Classifier";
			this.txt_Classifier.Size = new System.Drawing.Size(120, 23);
			this.txt_Classifier.TabIndex = 14;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(295, 370);
			this.Controls.Add(this.txt_Classifier);
			this.Controls.Add(this.ckb_Classifier);
			this.Controls.Add(this.MAkro_Chbx);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.chropIts);
			this.Controls.Add(this.chropchbx);
			this.Controls.Add(this.blurchbx);
			this.Controls.Add(this.whiteNoisechbx);
			this.Controls.Add(this.grayscale_chbx);
			this.Controls.Add(this.btn_Start);
			this.Controls.Add(this.btn_Load);
			this.Controls.Add(this.textBoxOutput);
			this.Controls.Add(this.btn_Output);
			this.Controls.Add(this.btn_Input);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxInput);
			this.Name = "Form1";
			this.Text = "DatasetStretcher";
			((System.ComponentModel.ISupportInitialize)(this.chropIts)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxInput;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btn_Input;
		private System.Windows.Forms.Button btn_Output;
		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.Button btn_Load;
		private System.Windows.Forms.Button btn_Start;
		private System.Windows.Forms.CheckBox grayscale_chbx;
		private System.Windows.Forms.CheckBox whiteNoisechbx;
		private System.Windows.Forms.CheckBox blurchbx;
		private System.Windows.Forms.CheckBox chropchbx;
		private System.Windows.Forms.NumericUpDown chropIts;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox MAkro_Chbx;
		private System.Windows.Forms.CheckBox ckb_Classifier;
		private System.Windows.Forms.TextBox txt_Classifier;
	}
}

