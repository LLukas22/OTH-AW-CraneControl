namespace TensorflowInstallationScript
{
	partial class MainForm
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
			this.btn_InstallPip = new System.Windows.Forms.Button();
			this.box_gpuSupport = new System.Windows.Forms.CheckBox();
			this.btn_UninstallPip = new System.Windows.Forms.Button();
			this.directoryTextBox = new System.Windows.Forms.TextBox();
			this.btn_browse = new System.Windows.Forms.Button();
			this.btn_DownloadOD = new System.Windows.Forms.Button();
			this.objectDetectionStateLabel = new System.Windows.Forms.Label();
			this.btn_downloadspecififcmodel = new System.Windows.Forms.Button();
			this.neuralNetLabel = new System.Windows.Forms.Label();
			this.btn_protobuf = new System.Windows.Forms.Button();
			this.protobufLabel = new System.Windows.Forms.Label();
			this.btn_Unzip = new System.Windows.Forms.Button();
			this.btn_CompileProtoc = new System.Windows.Forms.Button();
			this.btn_Installmodel = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.objectTextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			this.btn_ConfigurePipeline = new System.Windows.Forms.Button();
			this.btn_StartTraining = new System.Windows.Forms.Button();
			this.btn_ExportGraph = new System.Windows.Forms.Button();
			this.btn_Webcam = new System.Windows.Forms.Button();
			this.btn_Tesnorboard = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.STDOUT = new System.Windows.Forms.TextBox();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Clear = new System.Windows.Forms.Button();
			this.Dependencies = new System.Windows.Forms.GroupBox();
			this.Prerequisites_Label = new System.Windows.Forms.Label();
			this.boxDownloads = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.btn_ExportLiteGraph = new System.Windows.Forms.Button();
			this.Dependencies.SuspendLayout();
			this.boxDownloads.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_InstallPip
			// 
			this.btn_InstallPip.Location = new System.Drawing.Point(6, 22);
			this.btn_InstallPip.Name = "btn_InstallPip";
			this.btn_InstallPip.Size = new System.Drawing.Size(175, 23);
			this.btn_InstallPip.TabIndex = 0;
			this.btn_InstallPip.Text = "Install";
			this.btn_InstallPip.UseVisualStyleBackColor = true;
			this.btn_InstallPip.Click += new System.EventHandler(this.btn_InstallPip_Click);
			// 
			// box_gpuSupport
			// 
			this.box_gpuSupport.AutoSize = true;
			this.box_gpuSupport.Location = new System.Drawing.Point(368, 25);
			this.box_gpuSupport.Name = "box_gpuSupport";
			this.box_gpuSupport.Size = new System.Drawing.Size(118, 19);
			this.box_gpuSupport.TabIndex = 2;
			this.box_gpuSupport.Text = "GPU Acceleration";
			this.box_gpuSupport.UseVisualStyleBackColor = true;
			// 
			// btn_UninstallPip
			// 
			this.btn_UninstallPip.Location = new System.Drawing.Point(187, 22);
			this.btn_UninstallPip.Name = "btn_UninstallPip";
			this.btn_UninstallPip.Size = new System.Drawing.Size(175, 23);
			this.btn_UninstallPip.TabIndex = 3;
			this.btn_UninstallPip.Text = "Uninstall";
			this.btn_UninstallPip.UseVisualStyleBackColor = true;
			this.btn_UninstallPip.Click += new System.EventHandler(this.btn_UninstallPip_Click);
			// 
			// directoryTextBox
			// 
			this.directoryTextBox.Location = new System.Drawing.Point(6, 22);
			this.directoryTextBox.Name = "directoryTextBox";
			this.directoryTextBox.ReadOnly = true;
			this.directoryTextBox.Size = new System.Drawing.Size(267, 23);
			this.directoryTextBox.TabIndex = 6;
			// 
			// btn_browse
			// 
			this.btn_browse.Location = new System.Drawing.Point(270, 22);
			this.btn_browse.Name = "btn_browse";
			this.btn_browse.Size = new System.Drawing.Size(35, 23);
			this.btn_browse.TabIndex = 7;
			this.btn_browse.Text = "...";
			this.btn_browse.UseVisualStyleBackColor = true;
			this.btn_browse.Click += new System.EventHandler(this.btn_BrowseDirectory_Click);
			// 
			// btn_DownloadOD
			// 
			this.btn_DownloadOD.Location = new System.Drawing.Point(299, 17);
			this.btn_DownloadOD.Name = "btn_DownloadOD";
			this.btn_DownloadOD.Size = new System.Drawing.Size(136, 22);
			this.btn_DownloadOD.TabIndex = 8;
			this.btn_DownloadOD.Text = "Download";
			this.btn_DownloadOD.UseVisualStyleBackColor = true;
			this.btn_DownloadOD.Click += new System.EventHandler(this.btn_DownloadObjectDetectionAPI_Click);
			// 
			// objectDetectionStateLabel
			// 
			this.objectDetectionStateLabel.AutoSize = true;
			this.objectDetectionStateLabel.Location = new System.Drawing.Point(441, 23);
			this.objectDetectionStateLabel.Name = "objectDetectionStateLabel";
			this.objectDetectionStateLabel.Size = new System.Drawing.Size(80, 15);
			this.objectDetectionStateLabel.TabIndex = 9;
			this.objectDetectionStateLabel.Text = "State: Missing";
			// 
			// btn_downloadspecififcmodel
			// 
			this.btn_downloadspecififcmodel.Location = new System.Drawing.Point(299, 45);
			this.btn_downloadspecififcmodel.Name = "btn_downloadspecififcmodel";
			this.btn_downloadspecififcmodel.Size = new System.Drawing.Size(136, 24);
			this.btn_downloadspecififcmodel.TabIndex = 10;
			this.btn_downloadspecififcmodel.Text = "Download";
			this.btn_downloadspecififcmodel.UseVisualStyleBackColor = true;
			this.btn_downloadspecififcmodel.Click += new System.EventHandler(this.btn_DownloadNeuralNet_Click);
			// 
			// neuralNetLabel
			// 
			this.neuralNetLabel.AutoSize = true;
			this.neuralNetLabel.Location = new System.Drawing.Point(441, 50);
			this.neuralNetLabel.Name = "neuralNetLabel";
			this.neuralNetLabel.Size = new System.Drawing.Size(80, 15);
			this.neuralNetLabel.TabIndex = 11;
			this.neuralNetLabel.Text = "State: Missing";
			// 
			// btn_protobuf
			// 
			this.btn_protobuf.Location = new System.Drawing.Point(299, 76);
			this.btn_protobuf.Name = "btn_protobuf";
			this.btn_protobuf.Size = new System.Drawing.Size(136, 23);
			this.btn_protobuf.TabIndex = 12;
			this.btn_protobuf.Text = "Download";
			this.btn_protobuf.UseVisualStyleBackColor = true;
			this.btn_protobuf.Click += new System.EventHandler(this.btn_DownloadProtobuf_Click);
			// 
			// protobufLabel
			// 
			this.protobufLabel.AutoSize = true;
			this.protobufLabel.Location = new System.Drawing.Point(441, 76);
			this.protobufLabel.Name = "protobufLabel";
			this.protobufLabel.Size = new System.Drawing.Size(80, 15);
			this.protobufLabel.TabIndex = 13;
			this.protobufLabel.Text = "State: Missing";
			// 
			// btn_Unzip
			// 
			this.btn_Unzip.Location = new System.Drawing.Point(311, 21);
			this.btn_Unzip.Name = "btn_Unzip";
			this.btn_Unzip.Size = new System.Drawing.Size(75, 23);
			this.btn_Unzip.TabIndex = 15;
			this.btn_Unzip.Text = "Build";
			this.btn_Unzip.UseVisualStyleBackColor = true;
			this.btn_Unzip.Click += new System.EventHandler(this.btn_Unzip_Click);
			// 
			// btn_CompileProtoc
			// 
			this.btn_CompileProtoc.Location = new System.Drawing.Point(16, 18);
			this.btn_CompileProtoc.Name = "btn_CompileProtoc";
			this.btn_CompileProtoc.Size = new System.Drawing.Size(150, 23);
			this.btn_CompileProtoc.TabIndex = 17;
			this.btn_CompileProtoc.Text = "Compile *.protoc Files";
			this.btn_CompileProtoc.UseVisualStyleBackColor = true;
			this.btn_CompileProtoc.Click += new System.EventHandler(this.btn_CompileProtoc_Click);
			// 
			// btn_Installmodel
			// 
			this.btn_Installmodel.Location = new System.Drawing.Point(172, 18);
			this.btn_Installmodel.Name = "btn_Installmodel";
			this.btn_Installmodel.Size = new System.Drawing.Size(138, 23);
			this.btn_Installmodel.TabIndex = 18;
			this.btn_Installmodel.Text = "Execute setup.py";
			this.btn_Installmodel.UseVisualStyleBackColor = true;
			this.btn_Installmodel.Click += new System.EventHandler(this.btn_InstallObjectDetectionAPI_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(435, 105);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(99, 23);
			this.button1.TabIndex = 21;
			this.button1.Text = "Clear Cache";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.btn_DeleteDownloads_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(16, 105);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(140, 25);
			this.button2.TabIndex = 23;
			this.button2.Text = "Traindata";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.btn_XmlToCsv_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(293, 105);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(116, 25);
			this.button3.TabIndex = 25;
			this.button3.Text = "TF Records";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.btn_tfRecord_Click);
			// 
			// objectTextBox
			// 
			this.objectTextBox.Location = new System.Drawing.Point(17, 74);
			this.objectTextBox.Name = "objectTextBox";
			this.objectTextBox.Size = new System.Drawing.Size(530, 23);
			this.objectTextBox.TabIndex = 27;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 47);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(183, 15);
			this.label7.TabIndex = 28;
			this.label7.Text = "Objects to Detect divided by  \';\'  : ";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(162, 105);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(125, 23);
			this.button4.TabIndex = 29;
			this.button4.Text = "LabelMap";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.btn_labelmap_Click);
			// 
			// btn_ConfigurePipeline
			// 
			this.btn_ConfigurePipeline.Location = new System.Drawing.Point(415, 106);
			this.btn_ConfigurePipeline.Name = "btn_ConfigurePipeline";
			this.btn_ConfigurePipeline.Size = new System.Drawing.Size(123, 23);
			this.btn_ConfigurePipeline.TabIndex = 30;
			this.btn_ConfigurePipeline.Text = "Pipeline";
			this.btn_ConfigurePipeline.UseVisualStyleBackColor = true;
			this.btn_ConfigurePipeline.Click += new System.EventHandler(this.btn_Pipeline);
			// 
			// btn_StartTraining
			// 
			this.btn_StartTraining.Location = new System.Drawing.Point(18, 21);
			this.btn_StartTraining.Name = "btn_StartTraining";
			this.btn_StartTraining.Size = new System.Drawing.Size(138, 23);
			this.btn_StartTraining.TabIndex = 31;
			this.btn_StartTraining.Text = "Start Training!";
			this.btn_StartTraining.UseVisualStyleBackColor = true;
			this.btn_StartTraining.Click += new System.EventHandler(this.btn_Start_Click);
			// 
			// btn_ExportGraph
			// 
			this.btn_ExportGraph.Location = new System.Drawing.Point(12, 22);
			this.btn_ExportGraph.Name = "btn_ExportGraph";
			this.btn_ExportGraph.Size = new System.Drawing.Size(154, 23);
			this.btn_ExportGraph.TabIndex = 32;
			this.btn_ExportGraph.Text = "Export Graph";
			this.btn_ExportGraph.UseVisualStyleBackColor = true;
			this.btn_ExportGraph.Click += new System.EventHandler(this.btn_ExportGraph_Click);
			// 
			// btn_Webcam
			// 
			this.btn_Webcam.Location = new System.Drawing.Point(12, 51);
			this.btn_Webcam.Name = "btn_Webcam";
			this.btn_Webcam.Size = new System.Drawing.Size(154, 23);
			this.btn_Webcam.TabIndex = 34;
			this.btn_Webcam.Text = "Test on Webcam";
			this.btn_Webcam.UseVisualStyleBackColor = true;
			this.btn_Webcam.Click += new System.EventHandler(this.btn_WebcamDemo_Click);
			// 
			// btn_Tesnorboard
			// 
			this.btn_Tesnorboard.Location = new System.Drawing.Point(162, 21);
			this.btn_Tesnorboard.Name = "btn_Tesnorboard";
			this.btn_Tesnorboard.Size = new System.Drawing.Size(125, 23);
			this.btn_Tesnorboard.TabIndex = 35;
			this.btn_Tesnorboard.Text = "Open Tensorboard";
			this.btn_Tesnorboard.UseVisualStyleBackColor = true;
			this.btn_Tesnorboard.Click += new System.EventHandler(this.btn_Tesnorboard_Click);
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(392, 21);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(75, 23);
			this.button9.TabIndex = 37;
			this.button9.Text = "Open";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.btn_OpenFolderinExplorer_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(18, 13);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(98, 15);
			this.label9.TabIndex = 39;
			this.label9.Text = "Standard Output:";
			// 
			// STDOUT
			// 
			this.STDOUT.Location = new System.Drawing.Point(18, 29);
			this.STDOUT.Multiline = true;
			this.STDOUT.Name = "STDOUT";
			this.STDOUT.ReadOnly = true;
			this.STDOUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.STDOUT.Size = new System.Drawing.Size(520, 727);
			this.STDOUT.TabIndex = 41;
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Location = new System.Drawing.Point(463, 5);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_Cancel.TabIndex = 43;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// btn_Clear
			// 
			this.btn_Clear.Location = new System.Drawing.Point(382, 5);
			this.btn_Clear.Name = "btn_Clear";
			this.btn_Clear.Size = new System.Drawing.Size(75, 23);
			this.btn_Clear.TabIndex = 44;
			this.btn_Clear.Text = "Clear";
			this.btn_Clear.UseVisualStyleBackColor = true;
			this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
			// 
			// Dependencies
			// 
			this.Dependencies.Controls.Add(this.Prerequisites_Label);
			this.Dependencies.Controls.Add(this.box_gpuSupport);
			this.Dependencies.Controls.Add(this.btn_UninstallPip);
			this.Dependencies.Controls.Add(this.btn_InstallPip);
			this.Dependencies.Location = new System.Drawing.Point(562, 13);
			this.Dependencies.Name = "Dependencies";
			this.Dependencies.Size = new System.Drawing.Size(540, 56);
			this.Dependencies.TabIndex = 48;
			this.Dependencies.TabStop = false;
			this.Dependencies.Text = "Dependnecies : ";
			// 
			// Prerequisites_Label
			// 
			this.Prerequisites_Label.AutoSize = true;
			this.Prerequisites_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.Prerequisites_Label.Location = new System.Drawing.Point(458, 0);
			this.Prerequisites_Label.Name = "Prerequisites_Label";
			this.Prerequisites_Label.Size = new System.Drawing.Size(80, 15);
			this.Prerequisites_Label.TabIndex = 54;
			this.Prerequisites_Label.Text = "Prerequisites";
			this.Prerequisites_Label.Click += new System.EventHandler(this.Prerequisits_Label_Click);
			// 
			// boxDownloads
			// 
			this.boxDownloads.Controls.Add(this.label4);
			this.boxDownloads.Controls.Add(this.protobufLabel);
			this.boxDownloads.Controls.Add(this.neuralNetLabel);
			this.boxDownloads.Controls.Add(this.objectDetectionStateLabel);
			this.boxDownloads.Controls.Add(this.button1);
			this.boxDownloads.Controls.Add(this.label11);
			this.boxDownloads.Controls.Add(this.btn_protobuf);
			this.boxDownloads.Controls.Add(this.btn_DownloadOD);
			this.boxDownloads.Controls.Add(this.btn_downloadspecififcmodel);
			this.boxDownloads.Controls.Add(this.label1);
			this.boxDownloads.Location = new System.Drawing.Point(562, 75);
			this.boxDownloads.Name = "boxDownloads";
			this.boxDownloads.Size = new System.Drawing.Size(540, 134);
			this.boxDownloads.TabIndex = 49;
			this.boxDownloads.TabStop = false;
			this.boxDownloads.Text = "Downloads";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label4.Location = new System.Drawing.Point(6, 45);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 19);
			this.label4.TabIndex = 38;
			this.label4.Text = "Model:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label11.Location = new System.Drawing.Point(6, 74);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(77, 19);
			this.label11.TabIndex = 37;
			this.label11.Text = "Protobuf :";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(6, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 19);
			this.label1.TabIndex = 37;
			this.label1.Text = "Api :";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button9);
			this.groupBox1.Controls.Add(this.btn_Unzip);
			this.groupBox1.Controls.Add(this.btn_browse);
			this.groupBox1.Controls.Add(this.directoryTextBox);
			this.groupBox1.Location = new System.Drawing.Point(562, 215);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(540, 56);
			this.groupBox1.TabIndex = 38;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Environment";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btn_Installmodel);
			this.groupBox2.Controls.Add(this.btn_CompileProtoc);
			this.groupBox2.Location = new System.Drawing.Point(562, 277);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(540, 50);
			this.groupBox2.TabIndex = 50;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Installation";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.btn_ConfigurePipeline);
			this.groupBox3.Controls.Add(this.button3);
			this.groupBox3.Controls.Add(this.button4);
			this.groupBox3.Controls.Add(this.objectTextBox);
			this.groupBox3.Controls.Add(this.button2);
			this.groupBox3.Location = new System.Drawing.Point(562, 333);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(552, 136);
			this.groupBox3.TabIndex = 51;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Configuration";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(16, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(262, 19);
			this.label2.TabIndex = 37;
			this.label2.Text = "Put Labeled Images in ..\\Images\\Input";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.btn_StartTraining);
			this.groupBox4.Controls.Add(this.btn_Tesnorboard);
			this.groupBox4.Location = new System.Drawing.Point(562, 475);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(552, 53);
			this.groupBox4.TabIndex = 52;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Training";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.btn_ExportLiteGraph);
			this.groupBox5.Controls.Add(this.btn_Webcam);
			this.groupBox5.Controls.Add(this.btn_ExportGraph);
			this.groupBox5.Location = new System.Drawing.Point(562, 534);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(552, 88);
			this.groupBox5.TabIndex = 53;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Export";
			// 
			// btn_ExportLiteGraph
			// 
			this.btn_ExportLiteGraph.Location = new System.Drawing.Point(172, 22);
			this.btn_ExportLiteGraph.Name = "btn_ExportLiteGraph";
			this.btn_ExportLiteGraph.Size = new System.Drawing.Size(214, 23);
			this.btn_ExportLiteGraph.TabIndex = 35;
			this.btn_ExportLiteGraph.Text = "Export Lite Convertible Graph";
			this.btn_ExportLiteGraph.UseVisualStyleBackColor = true;
			this.btn_ExportLiteGraph.Click += new System.EventHandler(this.btn_ExportLiteGraph_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1121, 764);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.boxDownloads);
			this.Controls.Add(this.Dependencies);
			this.Controls.Add(this.btn_Clear);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.STDOUT);
			this.Controls.Add(this.label9);
			this.Name = "MainForm";
			this.Text = "TensorFlow";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Dependencies.ResumeLayout(false);
			this.Dependencies.PerformLayout();
			this.boxDownloads.ResumeLayout(false);
			this.boxDownloads.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btn_InstallPip;
		private System.Windows.Forms.CheckBox box_gpuSupport;
		private System.Windows.Forms.Button btn_UninstallPip;
		private System.Windows.Forms.TextBox directoryTextBox;
		private System.Windows.Forms.Button btn_browse;
		private System.Windows.Forms.Button btn;
		private System.Windows.Forms.Button btn_DownloadOD;
		private System.Windows.Forms.Label objectDetectionStateLabel;
		private System.Windows.Forms.Button btn_downloadspecififcmodel;
		private System.Windows.Forms.Label neuralNetLabel;
		private System.Windows.Forms.Button btn_protobuf;
		private System.Windows.Forms.Label protobufLabel;
		private System.Windows.Forms.Button btn_Unzip;
		private System.Windows.Forms.Button btn_CompileProtoc;
		private System.Windows.Forms.Button btn_Installmodel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox objectTextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button btn_ConfigurePipeline;
		private System.Windows.Forms.Button btn_StartTraining;
		private System.Windows.Forms.Button btn_ExportGraph;
		private System.Windows.Forms.Button btn_Webcam;
		private System.Windows.Forms.Button btn_Tesnorboard;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox STDOUT;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Button btn_Clear;
		private System.Windows.Forms.GroupBox Dependencies;
		private System.Windows.Forms.GroupBox boxDownloads;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label Prerequisites_Label;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btn_ExportLiteGraph;
	}
}

