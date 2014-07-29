namespace Mosa.Tool.Launcher
{
	partial class MainForm
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
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.cbEmulator = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lbSourceDirectory = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnSource = new System.Windows.Forms.Button();
			this.lbSource = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbBootFileSystem = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbBootFormat = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cbPlatform = new System.Windows.Forms.ComboBox();
			this.btnDestination = new System.Windows.Forms.Button();
			this.cbImageFormat = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lbDestinationDirectory = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cbLinkerFormat = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbGenerateASMFile = new System.Windows.Forms.CheckBox();
			this.cbGenerateMapFile = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbEnableSSAOptimizations = new System.Windows.Forms.CheckBox();
			this.cbEnableSSA = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.richTextBox2 = new System.Windows.Forms.RichTextBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "*.exe";
			this.openFileDialog1.Filter = "Assemblies|*.exe";
			this.openFileDialog1.Title = "Select Assembly";
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(0, 6);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(680, 20);
			this.progressBar1.TabIndex = 3;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.ItemSize = new System.Drawing.Size(48, 18);
			this.tabControl1.Location = new System.Drawing.Point(0, 32);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(0, 0);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(680, 371);
			this.tabControl1.TabIndex = 22;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.groupBox6);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.groupBox5);
			this.tabPage1.Controls.Add(this.groupBox4);
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(672, 345);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Options";
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox6.Controls.Add(this.cbEmulator);
			this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox6.Location = new System.Drawing.Point(428, 212);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(234, 90);
			this.groupBox6.TabIndex = 28;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Virtual Emulator:";
			// 
			// cbEmulator
			// 
			this.cbEmulator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEmulator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEmulator.FormattingEnabled = true;
			this.cbEmulator.Items.AddRange(new object[] {
            "QEMU",
            "Bochs",
            "VMware",
            "PeterBochs"});
			this.cbEmulator.Location = new System.Drawing.Point(10, 19);
			this.cbEmulator.Name = "cbEmulator";
			this.cbEmulator.Size = new System.Drawing.Size(184, 21);
			this.cbEmulator.TabIndex = 21;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(428, 308);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(234, 23);
			this.button1.TabIndex = 27;
			this.button1.Text = "Compile and Launch";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.checkBox6);
			this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox5.Location = new System.Drawing.Point(255, 287);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(166, 46);
			this.groupBox5.TabIndex = 26;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Debugger:";
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBox6.Location = new System.Drawing.Point(6, 25);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(112, 17);
			this.checkBox6.TabIndex = 13;
			this.checkBox6.Text = "Launch Debugger";
			this.checkBox6.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.lbSourceDirectory);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.btnSource);
			this.groupBox4.Controls.Add(this.lbSource);
			this.groupBox4.Location = new System.Drawing.Point(5, 7);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(657, 75);
			this.groupBox4.TabIndex = 25;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Source:";
			// 
			// lbSourceDirectory
			// 
			this.lbSourceDirectory.AutoSize = true;
			this.lbSourceDirectory.Location = new System.Drawing.Point(119, 48);
			this.lbSourceDirectory.Name = "lbSourceDirectory";
			this.lbSourceDirectory.Size = new System.Drawing.Size(94, 13);
			this.lbSourceDirectory.TabIndex = 20;
			this.lbSourceDirectory.Text = "{Source Directory}";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(7, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(106, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Source Directory:";
			// 
			// btnSource
			// 
			this.btnSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSource.Location = new System.Drawing.Point(10, 19);
			this.btnSource.Name = "btnSource";
			this.btnSource.Size = new System.Drawing.Size(103, 23);
			this.btnSource.TabIndex = 18;
			this.btnSource.Text = "Source:";
			this.btnSource.UseVisualStyleBackColor = true;
			// 
			// lbSource
			// 
			this.lbSource.AutoSize = true;
			this.lbSource.Location = new System.Drawing.Point(119, 24);
			this.lbSource.Name = "lbSource";
			this.lbSource.Size = new System.Drawing.Size(49, 13);
			this.lbSource.TabIndex = 17;
			this.lbSource.Text = "{Source}";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.cbBootFileSystem);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.cbBootFormat);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.cbPlatform);
			this.groupBox3.Controls.Add(this.btnDestination);
			this.groupBox3.Controls.Add(this.cbImageFormat);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.lbDestinationDirectory);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.cbLinkerFormat);
			this.groupBox3.Location = new System.Drawing.Point(5, 88);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(657, 114);
			this.groupBox3.TabIndex = 24;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Output:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(193, 81);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(105, 13);
			this.label4.TabIndex = 34;
			this.label4.Text = "Boot File System:";
			// 
			// cbBootFileSystem
			// 
			this.cbBootFileSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBootFileSystem.FormattingEnabled = true;
			this.cbBootFileSystem.Items.AddRange(new object[] {
            "FAT16",
            "FAT24"});
			this.cbBootFileSystem.Location = new System.Drawing.Point(304, 78);
			this.cbBootFileSystem.Name = "cbBootFileSystem";
			this.cbBootFileSystem.Size = new System.Drawing.Size(101, 21);
			this.cbBootFileSystem.TabIndex = 33;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(193, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 13);
			this.label2.TabIndex = 32;
			this.label2.Text = "Boot Format:";
			// 
			// cbBootFormat
			// 
			this.cbBootFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBootFormat.FormattingEnabled = true;
			this.cbBootFormat.Items.AddRange(new object[] {
            "MultibootHeader v0.7"});
			this.cbBootFormat.Location = new System.Drawing.Point(278, 51);
			this.cbBootFormat.Name = "cbBootFormat";
			this.cbBootFormat.Size = new System.Drawing.Size(127, 21);
			this.cbBootFormat.TabIndex = 31;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(46, 54);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(57, 13);
			this.label5.TabIndex = 30;
			this.label5.Text = "Platform:";
			// 
			// cbPlatform
			// 
			this.cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPlatform.FormattingEnabled = true;
			this.cbPlatform.Items.AddRange(new object[] {
            "x86",
            "ARMv6"});
			this.cbPlatform.Location = new System.Drawing.Point(109, 51);
			this.cbPlatform.Name = "cbPlatform";
			this.cbPlatform.Size = new System.Drawing.Size(78, 21);
			this.cbPlatform.TabIndex = 29;
			// 
			// btnDestination
			// 
			this.btnDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDestination.Location = new System.Drawing.Point(6, 18);
			this.btnDestination.Name = "btnDestination";
			this.btnDestination.Size = new System.Drawing.Size(107, 23);
			this.btnDestination.TabIndex = 17;
			this.btnDestination.Text = "Destination:";
			this.btnDestination.UseVisualStyleBackColor = true;
			// 
			// cbImageFormat
			// 
			this.cbImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbImageFormat.FormattingEnabled = true;
			this.cbImageFormat.Items.AddRange(new object[] {
            "Image (.img)",
            "VMware (.vhd)",
            "Virtual Box (.vdi)"});
			this.cbImageFormat.Location = new System.Drawing.Point(504, 51);
			this.cbImageFormat.Name = "cbImageFormat";
			this.cbImageFormat.Size = new System.Drawing.Size(113, 21);
			this.cbImageFormat.TabIndex = 16;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Linker Format:";
			// 
			// lbDestinationDirectory
			// 
			this.lbDestinationDirectory.AutoSize = true;
			this.lbDestinationDirectory.Location = new System.Drawing.Point(119, 23);
			this.lbDestinationDirectory.Name = "lbDestinationDirectory";
			this.lbDestinationDirectory.Size = new System.Drawing.Size(68, 13);
			this.lbDestinationDirectory.TabIndex = 14;
			this.lbDestinationDirectory.Text = "{Destination}";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(411, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Image Format:";
			// 
			// cbLinkerFormat
			// 
			this.cbLinkerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLinkerFormat.FormattingEnabled = true;
			this.cbLinkerFormat.Items.AddRange(new object[] {
            "ELF32",
            "PE32"});
			this.cbLinkerFormat.Location = new System.Drawing.Point(109, 78);
			this.cbLinkerFormat.Name = "cbLinkerFormat";
			this.cbLinkerFormat.Size = new System.Drawing.Size(78, 21);
			this.cbLinkerFormat.TabIndex = 11;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cbGenerateASMFile);
			this.groupBox2.Controls.Add(this.cbGenerateMapFile);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(255, 211);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(166, 70);
			this.groupBox2.TabIndex = 23;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Diagnostic Output:";
			// 
			// cbGenerateASMFile
			// 
			this.cbGenerateASMFile.AutoSize = true;
			this.cbGenerateASMFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbGenerateASMFile.Location = new System.Drawing.Point(6, 42);
			this.cbGenerateASMFile.Name = "cbGenerateASMFile";
			this.cbGenerateASMFile.Size = new System.Drawing.Size(115, 17);
			this.cbGenerateASMFile.TabIndex = 14;
			this.cbGenerateASMFile.Text = "Generate ASM File";
			this.cbGenerateASMFile.UseVisualStyleBackColor = true;
			// 
			// cbGenerateMapFile
			// 
			this.cbGenerateMapFile.AutoSize = true;
			this.cbGenerateMapFile.Checked = true;
			this.cbGenerateMapFile.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbGenerateMapFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbGenerateMapFile.Location = new System.Drawing.Point(6, 19);
			this.cbGenerateMapFile.Name = "cbGenerateMapFile";
			this.cbGenerateMapFile.Size = new System.Drawing.Size(115, 17);
			this.cbGenerateMapFile.TabIndex = 13;
			this.cbGenerateMapFile.Text = "Generate MAP File";
			this.cbGenerateMapFile.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbEnableSSAOptimizations);
			this.groupBox1.Controls.Add(this.cbEnableSSA);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(8, 211);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(241, 122);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Optimizations:";
			// 
			// cbEnableSSAOptimizations
			// 
			this.cbEnableSSAOptimizations.AutoSize = true;
			this.cbEnableSSAOptimizations.Checked = true;
			this.cbEnableSSAOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableSSAOptimizations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEnableSSAOptimizations.Location = new System.Drawing.Point(6, 42);
			this.cbEnableSSAOptimizations.Name = "cbEnableSSAOptimizations";
			this.cbEnableSSAOptimizations.Size = new System.Drawing.Size(148, 17);
			this.cbEnableSSAOptimizations.TabIndex = 7;
			this.cbEnableSSAOptimizations.Text = "Enable SSA Optimizations";
			this.cbEnableSSAOptimizations.UseVisualStyleBackColor = true;
			// 
			// cbEnableSSA
			// 
			this.cbEnableSSA.AutoSize = true;
			this.cbEnableSSA.Checked = true;
			this.cbEnableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableSSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEnableSSA.Location = new System.Drawing.Point(6, 18);
			this.cbEnableSSA.Name = "cbEnableSSA";
			this.cbEnableSSA.Size = new System.Drawing.Size(208, 17);
			this.cbEnableSSA.TabIndex = 6;
			this.cbEnableSSA.Text = "Enable Static Single Assignment (SSA)";
			this.cbEnableSSA.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.richTextBox1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(672, 345);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Output";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Font = new System.Drawing.Font("Consolas", 8F);
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.richTextBox1.Size = new System.Drawing.Size(672, 342);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			this.richTextBox1.WordWrap = false;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.richTextBox2);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(672, 345);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Counters";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// richTextBox2
			// 
			this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox2.Font = new System.Drawing.Font("Consolas", 8F);
			this.richTextBox2.Location = new System.Drawing.Point(0, 1);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.richTextBox2.Size = new System.Drawing.Size(672, 342);
			this.richTextBox2.TabIndex = 1;
			this.richTextBox2.Text = "";
			this.richTextBox2.WordWrap = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 402);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.tabControl1);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "MOSA Launcher";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ComboBox cbEmulator;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label lbSourceDirectory;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnSource;
		private System.Windows.Forms.Label lbSource;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbBootFormat;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbPlatform;
		private System.Windows.Forms.Button btnDestination;
		private System.Windows.Forms.ComboBox cbImageFormat;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbDestinationDirectory;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbLinkerFormat;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbGenerateASMFile;
		private System.Windows.Forms.CheckBox cbGenerateMapFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbEnableSSAOptimizations;
		private System.Windows.Forms.CheckBox cbEnableSSA;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbBootFileSystem;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.RichTextBox richTextBox2;
	}
}