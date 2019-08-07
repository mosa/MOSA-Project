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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.progressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tbApplicationLocations = new MetroFramework.Controls.MetroTabControl();
            this.tabOptions = new MetroFramework.Controls.MetroTabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.cbEnableMethodScanner = new MetroFramework.Controls.MetroCheckBox();
            this.cbCompilerUsesMultipleThreads = new MetroFramework.Controls.MetroCheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbGenerateCompilerTime = new MetroFramework.Controls.MetroCheckBox();
            this.cbGenerateDebugInfoFile = new MetroFramework.Controls.MetroCheckBox();
            this.cbGenerateASMFile = new MetroFramework.Controls.MetroCheckBox();
            this.cbGenerateMapFile = new MetroFramework.Controls.MetroCheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button1 = new MetroFramework.Controls.MetroButton();
            this.label6 = new MetroFramework.Controls.MetroLabel();
            this.nmMemory = new System.Windows.Forms.NumericUpDown();
            this.cbExitOnLaunch = new MetroFramework.Controls.MetroCheckBox();
            this.cbEmulator = new MetroFramework.Controls.MetroComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbSourceDirectory = new MetroFramework.Controls.MetroLabel();
            this.label7 = new MetroFramework.Controls.MetroLabel();
            this.btnSource = new MetroFramework.Controls.MetroButton();
            this.lbSource = new MetroFramework.Controls.MetroLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbMode = new MetroFramework.Controls.MetroTextBox();
            this.cbVBEVideo = new MetroFramework.Controls.MetroCheckBox();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.cbBootLoader = new MetroFramework.Controls.MetroComboBox();
            this.label4 = new MetroFramework.Controls.MetroLabel();
            this.cbBootFileSystem = new MetroFramework.Controls.MetroComboBox();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.cbBootFormat = new MetroFramework.Controls.MetroComboBox();
            this.label5 = new MetroFramework.Controls.MetroLabel();
            this.cbPlatform = new MetroFramework.Controls.MetroComboBox();
            this.btnDestination = new MetroFramework.Controls.MetroButton();
            this.cbImageFormat = new MetroFramework.Controls.MetroComboBox();
            this.lbDestinationDirectory = new MetroFramework.Controls.MetroLabel();
            this.label3 = new MetroFramework.Controls.MetroLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbBitTracker = new MetroFramework.Controls.MetroCheckBox();
            this.cbValueNumbering = new MetroFramework.Controls.MetroCheckBox();
            this.cbTwoPassOptimizations = new MetroFramework.Controls.MetroCheckBox();
            this.cbIRLongExpansion = new MetroFramework.Controls.MetroCheckBox();
            this.cbInlinedMethods = new MetroFramework.Controls.MetroCheckBox();
            this.cbEnableSparseConditionalConstantPropagation = new MetroFramework.Controls.MetroCheckBox();
            this.cbEnableIROptimizations = new MetroFramework.Controls.MetroCheckBox();
            this.cbEnableSSA = new MetroFramework.Controls.MetroCheckBox();
            this.tabAdvanced = new MetroFramework.Controls.MetroTabPage();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.cbGenerateNASMFile = new MetroFramework.Controls.MetroCheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbDebugConnectionOption = new MetroFramework.Controls.MetroComboBox();
            this.label8 = new MetroFramework.Controls.MetroLabel();
            this.cbMosaDebugger = new MetroFramework.Controls.MetroCheckBox();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.cbLaunchMosaDebugger = new MetroFramework.Controls.MetroCheckBox();
            this.cbLaunchGDB = new MetroFramework.Controls.MetroCheckBox();
            this.cbEnableQemuGDB = new MetroFramework.Controls.MetroCheckBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.cbPlugKorlib = new MetroFramework.Controls.MetroCheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label9 = new MetroFramework.Controls.MetroLabel();
            this.tbBaseAddress = new MetroFramework.Controls.MetroTextBox();
            this.cbRelocationTable = new MetroFramework.Controls.MetroCheckBox();
            this.cbEmitSymbolTable = new MetroFramework.Controls.MetroCheckBox();
            this.tabFiles = new System.Windows.Forms.TabPage();
            this.panelAdditionalFiles = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnRemoveFiles = new MetroFramework.Controls.MetroButton();
            this.btnAddFiles = new MetroFramework.Controls.MetroButton();
            this.tabApplicationLocations = new MetroFramework.Controls.MetroTabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.button7 = new MetroFramework.Controls.MetroButton();
            this.lbmkisofsExecutable = new MetroFramework.Controls.MetroLabel();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.button4 = new MetroFramework.Controls.MetroButton();
            this.lbVMwarePlayerExecutable = new MetroFramework.Controls.MetroLabel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.button6 = new MetroFramework.Controls.MetroButton();
            this.lbBOCHSExecutable = new MetroFramework.Controls.MetroLabel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button5 = new MetroFramework.Controls.MetroButton();
            this.lbNDISASMExecutable = new MetroFramework.Controls.MetroLabel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lbQEMUImgApplication = new MetroFramework.Controls.MetroLabel();
            this.button8 = new MetroFramework.Controls.MetroButton();
            this.lbQEMUBIOSDirectory = new MetroFramework.Controls.MetroLabel();
            this.button3 = new MetroFramework.Controls.MetroButton();
            this.button2 = new MetroFramework.Controls.MetroButton();
            this.lbQEMUExecutable = new MetroFramework.Controls.MetroLabel();
            this.tabCounters = new MetroFramework.Controls.MetroTabPage();
            this.rtbCounters = new System.Windows.Forms.RichTextBox();
            this.tabOutput = new MetroFramework.Controls.MetroTabPage();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbOsName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.tbApplicationLocations.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMemory)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.tabFiles.SuspendLayout();
            this.panelAdditionalFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabApplicationLocations.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabCounters.SuspendLayout();
            this.tabOutput.SuspendLayout();
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
            this.progressBar1.Location = new System.Drawing.Point(0, 78);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(887, 25);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = MetroFramework.MetroColorStyle.Blue;
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // tbApplicationLocations
            // 
            this.tbApplicationLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApplicationLocations.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbApplicationLocations.Controls.Add(this.tabOptions);
            this.tbApplicationLocations.Controls.Add(this.tabAdvanced);
            this.tbApplicationLocations.Controls.Add(this.tabFiles);
            this.tbApplicationLocations.Controls.Add(this.tabApplicationLocations);
            this.tbApplicationLocations.Controls.Add(this.tabCounters);
            this.tbApplicationLocations.Controls.Add(this.tabOutput);
            this.tbApplicationLocations.ItemSize = new System.Drawing.Size(48, 18);
            this.tbApplicationLocations.Location = new System.Drawing.Point(0, 106);
            this.tbApplicationLocations.Margin = new System.Windows.Forms.Padding(0);
            this.tbApplicationLocations.Multiline = true;
            this.tbApplicationLocations.Name = "tbApplicationLocations";
            this.tbApplicationLocations.SelectedIndex = 1;
            this.tbApplicationLocations.Size = new System.Drawing.Size(887, 549);
            this.tbApplicationLocations.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbApplicationLocations.TabIndex = 22;
            this.tbApplicationLocations.UseSelectable = true;
            // 
            // tabOptions
            // 
            this.tabOptions.BackColor = System.Drawing.SystemColors.Control;
            this.tabOptions.Controls.Add(this.groupBox12);
            this.tabOptions.Controls.Add(this.statusStrip1);
            this.tabOptions.Controls.Add(this.groupBox2);
            this.tabOptions.Controls.Add(this.groupBox6);
            this.tabOptions.Controls.Add(this.groupBox4);
            this.tabOptions.Controls.Add(this.groupBox3);
            this.tabOptions.Controls.Add(this.groupBox1);
            this.tabOptions.HorizontalScrollbarBarColor = true;
            this.tabOptions.HorizontalScrollbarHighlightOnWheel = false;
            this.tabOptions.HorizontalScrollbarSize = 12;
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Margin = new System.Windows.Forms.Padding(0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new System.Drawing.Size(879, 523);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Compile Options";
            this.tabOptions.VerticalScrollbarBarColor = true;
            this.tabOptions.VerticalScrollbarHighlightOnWheel = false;
            this.tabOptions.VerticalScrollbarSize = 13;
            // 
            // groupBox12
            // 
            this.groupBox12.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox12.Controls.Add(this.cbEnableMethodScanner);
            this.groupBox12.Controls.Add(this.cbCompilerUsesMultipleThreads);
            this.groupBox12.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.Location = new System.Drawing.Point(352, 384);
            this.groupBox12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox12.Size = new System.Drawing.Size(211, 102);
            this.groupBox12.TabIndex = 30;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Misc Options:";
            // 
            // cbEnableMethodScanner
            // 
            this.cbEnableMethodScanner.AutoSize = true;
            this.cbEnableMethodScanner.Checked = true;
            this.cbEnableMethodScanner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableMethodScanner.Location = new System.Drawing.Point(9, 49);
            this.cbEnableMethodScanner.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableMethodScanner.Name = "cbEnableMethodScanner";
            this.cbEnableMethodScanner.Size = new System.Drawing.Size(163, 17);
            this.cbEnableMethodScanner.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEnableMethodScanner.TabIndex = 14;
            this.cbEnableMethodScanner.Text = "Enable Method Scanner";
            this.cbEnableMethodScanner.Theme = MetroFramework.MetroThemeStyle.Light;
            this.cbEnableMethodScanner.UseCustomBackColor = true;
            this.cbEnableMethodScanner.UseSelectable = true;
            // 
            // cbCompilerUsesMultipleThreads
            // 
            this.cbCompilerUsesMultipleThreads.AutoSize = true;
            this.cbCompilerUsesMultipleThreads.Checked = true;
            this.cbCompilerUsesMultipleThreads.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCompilerUsesMultipleThreads.Location = new System.Drawing.Point(9, 23);
            this.cbCompilerUsesMultipleThreads.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCompilerUsesMultipleThreads.Name = "cbCompilerUsesMultipleThreads";
            this.cbCompilerUsesMultipleThreads.Size = new System.Drawing.Size(148, 17);
            this.cbCompilerUsesMultipleThreads.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbCompilerUsesMultipleThreads.TabIndex = 13;
            this.cbCompilerUsesMultipleThreads.Text = "Use Multiple Threads";
            this.cbCompilerUsesMultipleThreads.Theme = MetroFramework.MetroThemeStyle.Light;
            this.cbCompilerUsesMultipleThreads.UseCustomBackColor = true;
            this.cbCompilerUsesMultipleThreads.UseSelectable = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 501);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(879, 22);
            this.statusStrip1.TabIndex = 32;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatusLabel
            // 
            this.tsStatusLabel.Name = "tsStatusLabel";
            this.tsStatusLabel.Size = new System.Drawing.Size(0, 16);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Controls.Add(this.cbGenerateCompilerTime);
            this.groupBox2.Controls.Add(this.cbGenerateDebugInfoFile);
            this.groupBox2.Controls.Add(this.cbGenerateASMFile);
            this.groupBox2.Controls.Add(this.cbGenerateMapFile);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(352, 250);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(211, 130);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Diagnostic Output:";
            // 
            // cbGenerateCompilerTime
            // 
            this.cbGenerateCompilerTime.AutoSize = true;
            this.cbGenerateCompilerTime.Location = new System.Drawing.Point(8, 75);
            this.cbGenerateCompilerTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbGenerateCompilerTime.Name = "cbGenerateCompilerTime";
            this.cbGenerateCompilerTime.Size = new System.Drawing.Size(127, 17);
            this.cbGenerateCompilerTime.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbGenerateCompilerTime.TabIndex = 49;
            this.cbGenerateCompilerTime.Text = "Compile Time File";
            this.cbGenerateCompilerTime.UseCustomBackColor = true;
            this.cbGenerateCompilerTime.UseSelectable = true;
            // 
            // cbGenerateDebugInfoFile
            // 
            this.cbGenerateDebugInfoFile.AutoSize = true;
            this.cbGenerateDebugInfoFile.Location = new System.Drawing.Point(8, 101);
            this.cbGenerateDebugInfoFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbGenerateDebugInfoFile.Name = "cbGenerateDebugInfoFile";
            this.cbGenerateDebugInfoFile.Size = new System.Drawing.Size(86, 17);
            this.cbGenerateDebugInfoFile.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbGenerateDebugInfoFile.TabIndex = 48;
            this.cbGenerateDebugInfoFile.Text = "Debug File";
            this.cbGenerateDebugInfoFile.UseCustomBackColor = true;
            this.cbGenerateDebugInfoFile.UseSelectable = true;
            // 
            // cbGenerateASMFile
            // 
            this.cbGenerateASMFile.AutoSize = true;
            this.cbGenerateASMFile.Location = new System.Drawing.Point(8, 49);
            this.cbGenerateASMFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbGenerateASMFile.Name = "cbGenerateASMFile";
            this.cbGenerateASMFile.Size = new System.Drawing.Size(74, 17);
            this.cbGenerateASMFile.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbGenerateASMFile.TabIndex = 15;
            this.cbGenerateASMFile.Text = "ASM File";
            this.cbGenerateASMFile.UseCustomBackColor = true;
            this.cbGenerateASMFile.UseSelectable = true;
            // 
            // cbGenerateMapFile
            // 
            this.cbGenerateMapFile.AutoSize = true;
            this.cbGenerateMapFile.Location = new System.Drawing.Point(8, 23);
            this.cbGenerateMapFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbGenerateMapFile.Name = "cbGenerateMapFile";
            this.cbGenerateMapFile.Size = new System.Drawing.Size(74, 17);
            this.cbGenerateMapFile.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbGenerateMapFile.TabIndex = 13;
            this.cbGenerateMapFile.Text = "MAP File";
            this.cbGenerateMapFile.UseCustomBackColor = true;
            this.cbGenerateMapFile.UseSelectable = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.nmMemory);
            this.groupBox6.Controls.Add(this.cbExitOnLaunch);
            this.groupBox6.Controls.Add(this.cbEmulator);
            this.groupBox6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(571, 250);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(292, 236);
            this.groupBox6.TabIndex = 28;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Virtual Machine Emulator:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 201);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 28);
            this.button1.TabIndex = 28;
            this.button1.Text = "Compile and Run (F5)";
            this.button1.UseSelectable = true;
            this.button1.Click += new System.EventHandler(this.Btn1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label6.Location = new System.Drawing.Point(9, 64);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 20);
            this.label6.TabIndex = 24;
            this.label6.Text = "Memory:";
            this.label6.UseCustomBackColor = true;
            // 
            // nmMemory
            // 
            this.nmMemory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmMemory.Location = new System.Drawing.Point(101, 63);
            this.nmMemory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nmMemory.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nmMemory.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nmMemory.Name = "nmMemory";
            this.nmMemory.Size = new System.Drawing.Size(75, 23);
            this.nmMemory.TabIndex = 23;
            this.nmMemory.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // cbExitOnLaunch
            // 
            this.cbExitOnLaunch.AutoSize = true;
            this.cbExitOnLaunch.Location = new System.Drawing.Point(13, 175);
            this.cbExitOnLaunch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbExitOnLaunch.Name = "cbExitOnLaunch";
            this.cbExitOnLaunch.Size = new System.Drawing.Size(119, 17);
            this.cbExitOnLaunch.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbExitOnLaunch.TabIndex = 22;
            this.cbExitOnLaunch.Text = "Exit on Emulator";
            this.cbExitOnLaunch.UseCustomBackColor = true;
            this.cbExitOnLaunch.UseSelectable = true;
            // 
            // cbEmulator
            // 
            this.cbEmulator.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbEmulator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEmulator.FormattingEnabled = true;
            this.cbEmulator.ItemHeight = 24;
            this.cbEmulator.Items.AddRange(new object[] {
            "QEMU",
            "Bochs",
            "VMware"});
            this.cbEmulator.Location = new System.Drawing.Point(13, 23);
            this.cbEmulator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEmulator.Name = "cbEmulator";
            this.cbEmulator.Size = new System.Drawing.Size(227, 30);
            this.cbEmulator.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEmulator.TabIndex = 21;
            this.cbEmulator.UseCustomBackColor = true;
            this.cbEmulator.UseSelectable = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox4.Controls.Add(this.lbSourceDirectory);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.btnSource);
            this.groupBox4.Controls.Add(this.lbSource);
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(7, 9);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(856, 86);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Source:";
            // 
            // lbSourceDirectory
            // 
            this.lbSourceDirectory.AutoSize = true;
            this.lbSourceDirectory.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbSourceDirectory.Location = new System.Drawing.Point(159, 58);
            this.lbSourceDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSourceDirectory.Name = "lbSourceDirectory";
            this.lbSourceDirectory.Size = new System.Drawing.Size(122, 20);
            this.lbSourceDirectory.TabIndex = 20;
            this.lbSourceDirectory.Text = "{Source Directory}";
            this.lbSourceDirectory.UseCustomBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label7.Location = new System.Drawing.Point(8, 58);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "Source Directory:";
            this.label7.UseCustomBackColor = true;
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(13, 23);
            this.btnSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(137, 28);
            this.btnSource.TabIndex = 18;
            this.btnSource.Text = "Source:";
            this.btnSource.UseSelectable = true;
            this.btnSource.Click += new System.EventHandler(this.BtnSource_Click);
            // 
            // lbSource
            // 
            this.lbSource.AutoSize = true;
            this.lbSource.Location = new System.Drawing.Point(159, 26);
            this.lbSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(60, 20);
            this.lbSource.TabIndex = 17;
            this.lbSource.Text = "{Source}";
            this.lbSource.UseCustomBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Controls.Add(this.tbMode);
            this.groupBox3.Controls.Add(this.cbVBEVideo);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbBootLoader);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.cbBootFileSystem);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cbBootFormat);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cbPlatform);
            this.groupBox3.Controls.Add(this.btnDestination);
            this.groupBox3.Controls.Add(this.cbImageFormat);
            this.groupBox3.Controls.Add(this.lbDestinationDirectory);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(7, 101);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(856, 142);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output:";
            // 
            // tbMode
            // 
            // 
            // 
            // 
            this.tbMode.CustomButton.Image = null;
            this.tbMode.CustomButton.Location = new System.Drawing.Point(127, 1);
            this.tbMode.CustomButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbMode.CustomButton.Name = "";
            this.tbMode.CustomButton.Size = new System.Drawing.Size(31, 28);
            this.tbMode.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbMode.CustomButton.TabIndex = 1;
            this.tbMode.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbMode.CustomButton.UseSelectable = true;
            this.tbMode.CustomButton.Visible = false;
            this.tbMode.Enabled = false;
            this.tbMode.Lines = new string[] {
        "{Mode}"};
            this.tbMode.Location = new System.Drawing.Point(713, 102);
            this.tbMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbMode.MaxLength = 32767;
            this.tbMode.Name = "tbMode";
            this.tbMode.PasswordChar = '\0';
            this.tbMode.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbMode.SelectedText = "";
            this.tbMode.SelectionLength = 0;
            this.tbMode.SelectionStart = 0;
            this.tbMode.ShortcutsEnabled = true;
            this.tbMode.Size = new System.Drawing.Size(119, 25);
            this.tbMode.TabIndex = 38;
            this.tbMode.Text = "{Mode}";
            this.tbMode.UseSelectable = true;
            this.tbMode.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbMode.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cbVBEVideo
            // 
            this.cbVBEVideo.AutoSize = true;
            this.cbVBEVideo.Location = new System.Drawing.Point(549, 106);
            this.cbVBEVideo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbVBEVideo.Name = "cbVBEVideo";
            this.cbVBEVideo.Size = new System.Drawing.Size(130, 17);
            this.cbVBEVideo.TabIndex = 37;
            this.cbVBEVideo.Text = "Enable VBE Video:";
            this.cbVBEVideo.UseCustomBackColor = true;
            this.cbVBEVideo.UseSelectable = true;
            this.cbVBEVideo.CheckedChanged += new System.EventHandler(this.CbVBEVideo_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label1.Location = new System.Drawing.Point(239, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "Boot Loader:";
            this.label1.UseCustomBackColor = true;
            // 
            // cbBootLoader
            // 
            this.cbBootLoader.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbBootLoader.FormattingEnabled = true;
            this.cbBootLoader.ItemHeight = 24;
            this.cbBootLoader.Items.AddRange(new object[] {
            "Syslinux 3.72",
            "Syslinux 6.03",
            "Grub 0.97",
            "Grub 2.00"});
            this.cbBootLoader.Location = new System.Drawing.Point(364, 96);
            this.cbBootLoader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbBootLoader.Name = "cbBootLoader";
            this.cbBootLoader.Size = new System.Drawing.Size(168, 30);
            this.cbBootLoader.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbBootLoader.TabIndex = 35;
            this.cbBootLoader.UseCustomBackColor = true;
            this.cbBootLoader.UseSelectable = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label4.Location = new System.Drawing.Point(3, 98);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 20);
            this.label4.TabIndex = 34;
            this.label4.Text = "File System:";
            this.label4.UseCustomBackColor = true;
            // 
            // cbBootFileSystem
            // 
            this.cbBootFileSystem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbBootFileSystem.FormattingEnabled = true;
            this.cbBootFileSystem.ItemHeight = 24;
            this.cbBootFileSystem.Items.AddRange(new object[] {
            "FAT12",
            "FAT16"});
            this.cbBootFileSystem.Location = new System.Drawing.Point(117, 96);
            this.cbBootFileSystem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbBootFileSystem.Name = "cbBootFileSystem";
            this.cbBootFileSystem.Size = new System.Drawing.Size(116, 30);
            this.cbBootFileSystem.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbBootFileSystem.TabIndex = 33;
            this.cbBootFileSystem.UseCustomBackColor = true;
            this.cbBootFileSystem.UseSelectable = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label2.Location = new System.Drawing.Point(240, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 20);
            this.label2.TabIndex = 32;
            this.label2.Text = "Boot Format:";
            this.label2.UseCustomBackColor = true;
            // 
            // cbBootFormat
            // 
            this.cbBootFormat.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbBootFormat.FormattingEnabled = true;
            this.cbBootFormat.ItemHeight = 24;
            this.cbBootFormat.Items.AddRange(new object[] {
            "Multiboot v1"});
            this.cbBootFormat.Location = new System.Drawing.Point(364, 58);
            this.cbBootFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbBootFormat.Name = "cbBootFormat";
            this.cbBootFormat.Size = new System.Drawing.Size(168, 30);
            this.cbBootFormat.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbBootFormat.TabIndex = 31;
            this.cbBootFormat.UseCustomBackColor = true;
            this.cbBootFormat.UseSelectable = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label5.Location = new System.Drawing.Point(27, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 20);
            this.label5.TabIndex = 30;
            this.label5.Text = "Platform:";
            this.label5.UseCustomBackColor = true;
            // 
            // cbPlatform
            // 
            this.cbPlatform.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbPlatform.FormattingEnabled = true;
            this.cbPlatform.ItemHeight = 24;
            this.cbPlatform.Items.AddRange(new object[] {
            "x86",
            "ARMv6"});
            this.cbPlatform.Location = new System.Drawing.Point(117, 58);
            this.cbPlatform.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPlatform.Name = "cbPlatform";
            this.cbPlatform.Size = new System.Drawing.Size(116, 30);
            this.cbPlatform.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbPlatform.TabIndex = 29;
            this.cbPlatform.UseCustomBackColor = true;
            this.cbPlatform.UseSelectable = true;
            // 
            // btnDestination
            // 
            this.btnDestination.Location = new System.Drawing.Point(8, 22);
            this.btnDestination.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDestination.Name = "btnDestination";
            this.btnDestination.Size = new System.Drawing.Size(143, 28);
            this.btnDestination.TabIndex = 17;
            this.btnDestination.Text = "Destination:";
            this.btnDestination.UseSelectable = true;
            this.btnDestination.Click += new System.EventHandler(this.BtnDestination_Click);
            // 
            // cbImageFormat
            // 
            this.cbImageFormat.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbImageFormat.FormattingEnabled = true;
            this.cbImageFormat.ItemHeight = 24;
            this.cbImageFormat.Items.AddRange(new object[] {
            "IMG (.img)",
            "ISO Image (.iso)",
            "Microsoft (.vhd)",
            "Virtual Box (.vdi)",
            "VMware (.vmdk)"});
            this.cbImageFormat.Location = new System.Drawing.Point(680, 58);
            this.cbImageFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbImageFormat.Name = "cbImageFormat";
            this.cbImageFormat.Size = new System.Drawing.Size(151, 30);
            this.cbImageFormat.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbImageFormat.TabIndex = 16;
            this.cbImageFormat.UseCustomBackColor = true;
            this.cbImageFormat.UseSelectable = true;
            // 
            // lbDestinationDirectory
            // 
            this.lbDestinationDirectory.AutoSize = true;
            this.lbDestinationDirectory.Location = new System.Drawing.Point(159, 25);
            this.lbDestinationDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDestinationDirectory.Name = "lbDestinationDirectory";
            this.lbDestinationDirectory.Size = new System.Drawing.Size(148, 20);
            this.lbDestinationDirectory.TabIndex = 14;
            this.lbDestinationDirectory.Text = "{Destination Directory}";
            this.lbDestinationDirectory.UseCustomBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label3.Location = new System.Drawing.Point(541, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Image Format:";
            this.label3.UseCustomBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.cbBitTracker);
            this.groupBox1.Controls.Add(this.cbValueNumbering);
            this.groupBox1.Controls.Add(this.cbTwoPassOptimizations);
            this.groupBox1.Controls.Add(this.cbIRLongExpansion);
            this.groupBox1.Controls.Add(this.cbInlinedMethods);
            this.groupBox1.Controls.Add(this.cbEnableSparseConditionalConstantPropagation);
            this.groupBox1.Controls.Add(this.cbEnableIROptimizations);
            this.groupBox1.Controls.Add(this.cbEnableSSA);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 250);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(337, 236);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optimizations:";
            // 
            // cbBitTracker
            // 
            this.cbBitTracker.AutoSize = true;
            this.cbBitTracker.Checked = true;
            this.cbBitTracker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBitTracker.Location = new System.Drawing.Point(8, 203);
            this.cbBitTracker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbBitTracker.Name = "cbBitTracker";
            this.cbBitTracker.Size = new System.Drawing.Size(84, 17);
            this.cbBitTracker.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbBitTracker.TabIndex = 13;
            this.cbBitTracker.Text = "Bit Tracker";
            this.cbBitTracker.UseCustomBackColor = true;
            this.cbBitTracker.UseSelectable = true;
            // 
            // cbValueNumbering
            // 
            this.cbValueNumbering.AutoSize = true;
            this.cbValueNumbering.Checked = true;
            this.cbValueNumbering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbValueNumbering.Location = new System.Drawing.Point(8, 177);
            this.cbValueNumbering.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbValueNumbering.Name = "cbValueNumbering";
            this.cbValueNumbering.Size = new System.Drawing.Size(125, 17);
            this.cbValueNumbering.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbValueNumbering.TabIndex = 12;
            this.cbValueNumbering.Text = "Value Numbering";
            this.cbValueNumbering.UseCustomBackColor = true;
            this.cbValueNumbering.UseSelectable = true;
            // 
            // cbTwoPassOptimizations
            // 
            this.cbTwoPassOptimizations.AutoSize = true;
            this.cbTwoPassOptimizations.Checked = true;
            this.cbTwoPassOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTwoPassOptimizations.Location = new System.Drawing.Point(8, 151);
            this.cbTwoPassOptimizations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTwoPassOptimizations.Name = "cbTwoPassOptimizations";
            this.cbTwoPassOptimizations.Size = new System.Drawing.Size(161, 17);
            this.cbTwoPassOptimizations.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbTwoPassOptimizations.TabIndex = 11;
            this.cbTwoPassOptimizations.Text = "Two Pass Optimizations";
            this.cbTwoPassOptimizations.UseCustomBackColor = true;
            this.cbTwoPassOptimizations.UseSelectable = true;
            // 
            // cbIRLongExpansion
            // 
            this.cbIRLongExpansion.AutoSize = true;
            this.cbIRLongExpansion.Checked = true;
            this.cbIRLongExpansion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIRLongExpansion.Location = new System.Drawing.Point(8, 126);
            this.cbIRLongExpansion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbIRLongExpansion.Name = "cbIRLongExpansion";
            this.cbIRLongExpansion.Size = new System.Drawing.Size(131, 17);
            this.cbIRLongExpansion.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbIRLongExpansion.TabIndex = 10;
            this.cbIRLongExpansion.Text = "IR Long Expansion";
            this.cbIRLongExpansion.UseCustomBackColor = true;
            this.cbIRLongExpansion.UseSelectable = true;
            // 
            // cbInlinedMethods
            // 
            this.cbInlinedMethods.AutoSize = true;
            this.cbInlinedMethods.Checked = true;
            this.cbInlinedMethods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInlinedMethods.Location = new System.Drawing.Point(8, 100);
            this.cbInlinedMethods.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbInlinedMethods.Name = "cbInlinedMethods";
            this.cbInlinedMethods.Size = new System.Drawing.Size(118, 17);
            this.cbInlinedMethods.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbInlinedMethods.TabIndex = 9;
            this.cbInlinedMethods.Text = "Inlined Methods";
            this.cbInlinedMethods.UseCustomBackColor = true;
            this.cbInlinedMethods.UseSelectable = true;
            // 
            // cbEnableSparseConditionalConstantPropagation
            // 
            this.cbEnableSparseConditionalConstantPropagation.AutoSize = true;
            this.cbEnableSparseConditionalConstantPropagation.Checked = true;
            this.cbEnableSparseConditionalConstantPropagation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableSparseConditionalConstantPropagation.Location = new System.Drawing.Point(8, 74);
            this.cbEnableSparseConditionalConstantPropagation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableSparseConditionalConstantPropagation.Name = "cbEnableSparseConditionalConstantPropagation";
            this.cbEnableSparseConditionalConstantPropagation.Size = new System.Drawing.Size(265, 17);
            this.cbEnableSparseConditionalConstantPropagation.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEnableSparseConditionalConstantPropagation.TabIndex = 8;
            this.cbEnableSparseConditionalConstantPropagation.Text = "Sparse Conditional Constant Propagation";
            this.cbEnableSparseConditionalConstantPropagation.UseCustomBackColor = true;
            this.cbEnableSparseConditionalConstantPropagation.UseSelectable = true;
            // 
            // cbEnableIROptimizations
            // 
            this.cbEnableIROptimizations.AutoSize = true;
            this.cbEnableIROptimizations.Checked = true;
            this.cbEnableIROptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableIROptimizations.Location = new System.Drawing.Point(8, 48);
            this.cbEnableIROptimizations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableIROptimizations.Name = "cbEnableIROptimizations";
            this.cbEnableIROptimizations.Size = new System.Drawing.Size(119, 17);
            this.cbEnableIROptimizations.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEnableIROptimizations.TabIndex = 7;
            this.cbEnableIROptimizations.Text = "IR Optimizations";
            this.cbEnableIROptimizations.UseCustomBackColor = true;
            this.cbEnableIROptimizations.UseSelectable = true;
            // 
            // cbEnableSSA
            // 
            this.cbEnableSSA.AutoSize = true;
            this.cbEnableSSA.Checked = true;
            this.cbEnableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableSSA.Location = new System.Drawing.Point(8, 22);
            this.cbEnableSSA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableSSA.Name = "cbEnableSSA";
            this.cbEnableSSA.Size = new System.Drawing.Size(199, 17);
            this.cbEnableSSA.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEnableSSA.TabIndex = 6;
            this.cbEnableSSA.Text = "Static Single Assignment (SSA)";
            this.cbEnableSSA.UseCustomBackColor = true;
            this.cbEnableSSA.UseSelectable = true;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.groupBox16);
            this.tabAdvanced.Controls.Add(this.groupBox5);
            this.tabAdvanced.Controls.Add(this.groupBox15);
            this.tabAdvanced.Controls.Add(this.groupBox14);
            this.tabAdvanced.Controls.Add(this.groupBox13);
            this.tabAdvanced.HorizontalScrollbarBarColor = true;
            this.tabAdvanced.HorizontalScrollbarHighlightOnWheel = false;
            this.tabAdvanced.HorizontalScrollbarSize = 12;
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabAdvanced.Size = new System.Drawing.Size(879, 523);
            this.tabAdvanced.TabIndex = 4;
            this.tabAdvanced.Text = "Advanced Options";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            this.tabAdvanced.VerticalScrollbarBarColor = true;
            this.tabAdvanced.VerticalScrollbarHighlightOnWheel = false;
            this.tabAdvanced.VerticalScrollbarSize = 13;
            // 
            // groupBox16
            // 
            this.groupBox16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox16.Controls.Add(this.cbGenerateNASMFile);
            this.groupBox16.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox16.Location = new System.Drawing.Point(293, 182);
            this.groupBox16.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox16.Size = new System.Drawing.Size(275, 138);
            this.groupBox16.TabIndex = 49;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Diagnostic Output:";
            // 
            // cbGenerateNASMFile
            // 
            this.cbGenerateNASMFile.AutoSize = true;
            this.cbGenerateNASMFile.Location = new System.Drawing.Point(8, 26);
            this.cbGenerateNASMFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbGenerateNASMFile.Name = "cbGenerateNASMFile";
            this.cbGenerateNASMFile.Size = new System.Drawing.Size(84, 17);
            this.cbGenerateNASMFile.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbGenerateNASMFile.TabIndex = 48;
            this.cbGenerateNASMFile.Text = "NASM File";
            this.cbGenerateNASMFile.UseCustomBackColor = true;
            this.cbGenerateNASMFile.UseSelectable = true;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox5.Controls.Add(this.cbDebugConnectionOption);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.cbMosaDebugger);
            this.groupBox5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(11, 182);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(275, 138);
            this.groupBox5.TabIndex = 48;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Debugger:";
            // 
            // cbDebugConnectionOption
            // 
            this.cbDebugConnectionOption.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbDebugConnectionOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDebugConnectionOption.FormattingEnabled = true;
            this.cbDebugConnectionOption.ItemHeight = 24;
            this.cbDebugConnectionOption.Items.AddRange(new object[] {
            "None",
            "Pipe",
            "TCP Server",
            "TCP Client"});
            this.cbDebugConnectionOption.Location = new System.Drawing.Point(9, 79);
            this.cbDebugConnectionOption.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDebugConnectionOption.Name = "cbDebugConnectionOption";
            this.cbDebugConnectionOption.Size = new System.Drawing.Size(244, 30);
            this.cbDebugConnectionOption.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbDebugConnectionOption.TabIndex = 36;
            this.cbDebugConnectionOption.UseCustomBackColor = true;
            this.cbDebugConnectionOption.UseSelectable = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label8.Location = new System.Drawing.Point(8, 52);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 20);
            this.label8.TabIndex = 35;
            this.label8.Text = "Debug Connection:";
            this.label8.UseCustomBackColor = true;
            // 
            // cbMosaDebugger
            // 
            this.cbMosaDebugger.AutoSize = true;
            this.cbMosaDebugger.Enabled = false;
            this.cbMosaDebugger.Location = new System.Drawing.Point(9, 28);
            this.cbMosaDebugger.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMosaDebugger.Name = "cbMosaDebugger";
            this.cbMosaDebugger.Size = new System.Drawing.Size(124, 17);
            this.cbMosaDebugger.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbMosaDebugger.TabIndex = 14;
            this.cbMosaDebugger.Text = "MOSA Debugger";
            this.cbMosaDebugger.UseCustomBackColor = true;
            this.cbMosaDebugger.UseSelectable = true;
            // 
            // groupBox15
            // 
            this.groupBox15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox15.Controls.Add(this.cbLaunchMosaDebugger);
            this.groupBox15.Controls.Add(this.cbLaunchGDB);
            this.groupBox15.Controls.Add(this.cbEnableQemuGDB);
            this.groupBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox15.Location = new System.Drawing.Point(11, 22);
            this.groupBox15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox15.Size = new System.Drawing.Size(275, 153);
            this.groupBox15.TabIndex = 44;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Debugger:";
            // 
            // cbLaunchMosaDebugger
            // 
            this.cbLaunchMosaDebugger.AutoSize = true;
            this.cbLaunchMosaDebugger.Checked = true;
            this.cbLaunchMosaDebugger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLaunchMosaDebugger.Location = new System.Drawing.Point(8, 89);
            this.cbLaunchMosaDebugger.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLaunchMosaDebugger.Name = "cbLaunchMosaDebugger";
            this.cbLaunchMosaDebugger.Size = new System.Drawing.Size(168, 17);
            this.cbLaunchMosaDebugger.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbLaunchMosaDebugger.TabIndex = 8;
            this.cbLaunchMosaDebugger.Text = "Launch MOSA Debugger";
            this.cbLaunchMosaDebugger.UseCustomBackColor = true;
            this.cbLaunchMosaDebugger.UseSelectable = true;
            // 
            // cbLaunchGDB
            // 
            this.cbLaunchGDB.AutoSize = true;
            this.cbLaunchGDB.Checked = true;
            this.cbLaunchGDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLaunchGDB.Location = new System.Drawing.Point(8, 63);
            this.cbLaunchGDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLaunchGDB.Name = "cbLaunchGDB";
            this.cbLaunchGDB.Size = new System.Drawing.Size(93, 17);
            this.cbLaunchGDB.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbLaunchGDB.TabIndex = 7;
            this.cbLaunchGDB.Text = "Launch GDB";
            this.cbLaunchGDB.UseCustomBackColor = true;
            this.cbLaunchGDB.UseSelectable = true;
            // 
            // cbEnableQemuGDB
            // 
            this.cbEnableQemuGDB.AutoSize = true;
            this.cbEnableQemuGDB.Checked = true;
            this.cbEnableQemuGDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableQemuGDB.Location = new System.Drawing.Point(8, 34);
            this.cbEnableQemuGDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableQemuGDB.Name = "cbEnableQemuGDB";
            this.cbEnableQemuGDB.Size = new System.Drawing.Size(134, 17);
            this.cbEnableQemuGDB.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEnableQemuGDB.TabIndex = 6;
            this.cbEnableQemuGDB.Text = "Enable QEMU GDB";
            this.cbEnableQemuGDB.UseCustomBackColor = true;
            this.cbEnableQemuGDB.UseSelectable = true;
            // 
            // groupBox14
            // 
            this.groupBox14.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox14.Controls.Add(this.metroLabel1);
            this.groupBox14.Controls.Add(this.tbOsName);
            this.groupBox14.Controls.Add(this.cbPlugKorlib);
            this.groupBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox14.Location = new System.Drawing.Point(576, 22);
            this.groupBox14.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox14.Size = new System.Drawing.Size(275, 153);
            this.groupBox14.TabIndex = 43;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Other:";
            // 
            // cbPlugKorlib
            // 
            this.cbPlugKorlib.AutoSize = true;
            this.cbPlugKorlib.Checked = true;
            this.cbPlugKorlib.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPlugKorlib.Location = new System.Drawing.Point(8, 23);
            this.cbPlugKorlib.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPlugKorlib.Name = "cbPlugKorlib";
            this.cbPlugKorlib.Size = new System.Drawing.Size(88, 17);
            this.cbPlugKorlib.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbPlugKorlib.TabIndex = 7;
            this.cbPlugKorlib.Text = "Plug Korlib";
            this.cbPlugKorlib.UseCustomBackColor = true;
            this.cbPlugKorlib.UseSelectable = true;
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox13.Controls.Add(this.label9);
            this.groupBox13.Controls.Add(this.tbBaseAddress);
            this.groupBox13.Controls.Add(this.cbRelocationTable);
            this.groupBox13.Controls.Add(this.cbEmitSymbolTable);
            this.groupBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox13.Location = new System.Drawing.Point(293, 22);
            this.groupBox13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox13.Size = new System.Drawing.Size(275, 153);
            this.groupBox13.TabIndex = 23;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Linker Options:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label9.Location = new System.Drawing.Point(8, 112);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 20);
            this.label9.TabIndex = 42;
            this.label9.Text = "Base Address:";
            this.label9.UseCustomBackColor = true;
            // 
            // tbBaseAddress
            // 
            // 
            // 
            // 
            this.tbBaseAddress.CustomButton.Image = null;
            this.tbBaseAddress.CustomButton.Location = new System.Drawing.Point(127, 1);
            this.tbBaseAddress.CustomButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbBaseAddress.CustomButton.Name = "";
            this.tbBaseAddress.CustomButton.Size = new System.Drawing.Size(31, 28);
            this.tbBaseAddress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbBaseAddress.CustomButton.TabIndex = 1;
            this.tbBaseAddress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbBaseAddress.CustomButton.UseSelectable = true;
            this.tbBaseAddress.CustomButton.Visible = false;
            this.tbBaseAddress.Lines = new string[] {
        "0x00400000"};
            this.tbBaseAddress.Location = new System.Drawing.Point(136, 111);
            this.tbBaseAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbBaseAddress.MaxLength = 32767;
            this.tbBaseAddress.Name = "tbBaseAddress";
            this.tbBaseAddress.PasswordChar = '\0';
            this.tbBaseAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbBaseAddress.SelectedText = "";
            this.tbBaseAddress.SelectionLength = 0;
            this.tbBaseAddress.SelectionStart = 0;
            this.tbBaseAddress.ShortcutsEnabled = true;
            this.tbBaseAddress.Size = new System.Drawing.Size(119, 25);
            this.tbBaseAddress.TabIndex = 41;
            this.tbBaseAddress.Text = "0x00400000";
            this.tbBaseAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBaseAddress.UseSelectable = true;
            this.tbBaseAddress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbBaseAddress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cbRelocationTable
            // 
            this.cbRelocationTable.AutoSize = true;
            this.cbRelocationTable.Checked = true;
            this.cbRelocationTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRelocationTable.Location = new System.Drawing.Point(8, 63);
            this.cbRelocationTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbRelocationTable.Name = "cbRelocationTable";
            this.cbRelocationTable.Size = new System.Drawing.Size(155, 17);
            this.cbRelocationTable.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbRelocationTable.TabIndex = 7;
            this.cbRelocationTable.Text = "Emit Static Relocations";
            this.cbRelocationTable.UseCustomBackColor = true;
            this.cbRelocationTable.UseSelectable = true;
            // 
            // cbEmitSymbolTable
            // 
            this.cbEmitSymbolTable.AutoSize = true;
            this.cbEmitSymbolTable.Checked = true;
            this.cbEmitSymbolTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEmitSymbolTable.Location = new System.Drawing.Point(8, 34);
            this.cbEmitSymbolTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEmitSymbolTable.Name = "cbEmitSymbolTable";
            this.cbEmitSymbolTable.Size = new System.Drawing.Size(120, 17);
            this.cbEmitSymbolTable.Style = MetroFramework.MetroColorStyle.Blue;
            this.cbEmitSymbolTable.TabIndex = 6;
            this.cbEmitSymbolTable.Text = "Emit All Symbols";
            this.cbEmitSymbolTable.UseCustomBackColor = true;
            this.cbEmitSymbolTable.UseSelectable = true;
            // 
            // tabFiles
            // 
            this.tabFiles.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabFiles.Controls.Add(this.panelAdditionalFiles);
            this.tabFiles.Location = new System.Drawing.Point(4, 22);
            this.tabFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabFiles.Name = "tabFiles";
            this.tabFiles.Size = new System.Drawing.Size(879, 523);
            this.tabFiles.TabIndex = 5;
            this.tabFiles.Text = "Included Files";
            // 
            // panelAdditionalFiles
            // 
            this.panelAdditionalFiles.Controls.Add(this.dataGridView1);
            this.panelAdditionalFiles.Controls.Add(this.btnRemoveFiles);
            this.panelAdditionalFiles.Controls.Add(this.btnAddFiles);
            this.panelAdditionalFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdditionalFiles.Location = new System.Drawing.Point(0, 0);
            this.panelAdditionalFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelAdditionalFiles.Name = "panelAdditionalFiles";
            this.panelAdditionalFiles.Size = new System.Drawing.Size(879, 523);
            this.panelAdditionalFiles.TabIndex = 23;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 4);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(821, 494);
            this.dataGridView1.TabIndex = 32;
            // 
            // btnRemoveFiles
            // 
            this.btnRemoveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFiles.Location = new System.Drawing.Point(842, 58);
            this.btnRemoveFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveFiles.Name = "btnRemoveFiles";
            this.btnRemoveFiles.Size = new System.Drawing.Size(33, 31);
            this.btnRemoveFiles.TabIndex = 30;
            this.btnRemoveFiles.Text = "-";
            this.btnRemoveFiles.UseSelectable = true;
            this.btnRemoveFiles.Click += new System.EventHandler(this.BtnRemoveFiles_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFiles.Location = new System.Drawing.Point(842, 20);
            this.btnAddFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(33, 31);
            this.btnAddFiles.TabIndex = 29;
            this.btnAddFiles.Text = "+";
            this.btnAddFiles.UseSelectable = true;
            this.btnAddFiles.Click += new System.EventHandler(this.BtnAddFiles_Click);
            // 
            // tabApplicationLocations
            // 
            this.tabApplicationLocations.Controls.Add(this.groupBox11);
            this.tabApplicationLocations.Controls.Add(this.groupBox10);
            this.tabApplicationLocations.Controls.Add(this.groupBox9);
            this.tabApplicationLocations.Controls.Add(this.groupBox8);
            this.tabApplicationLocations.Controls.Add(this.groupBox7);
            this.tabApplicationLocations.HorizontalScrollbarBarColor = true;
            this.tabApplicationLocations.HorizontalScrollbarHighlightOnWheel = false;
            this.tabApplicationLocations.HorizontalScrollbarSize = 12;
            this.tabApplicationLocations.Location = new System.Drawing.Point(4, 22);
            this.tabApplicationLocations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabApplicationLocations.Name = "tabApplicationLocations";
            this.tabApplicationLocations.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabApplicationLocations.Size = new System.Drawing.Size(879, 523);
            this.tabApplicationLocations.TabIndex = 3;
            this.tabApplicationLocations.Text = "Application Locations";
            this.tabApplicationLocations.UseVisualStyleBackColor = true;
            this.tabApplicationLocations.VerticalScrollbarBarColor = true;
            this.tabApplicationLocations.VerticalScrollbarHighlightOnWheel = false;
            this.tabApplicationLocations.VerticalScrollbarSize = 13;
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox11.Controls.Add(this.button7);
            this.groupBox11.Controls.Add(this.lbmkisofsExecutable);
            this.groupBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox11.Location = new System.Drawing.Point(11, 358);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox11.Size = new System.Drawing.Size(752, 62);
            this.groupBox11.TabIndex = 25;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "mkisofs:";
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(8, 23);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(141, 28);
            this.button7.TabIndex = 20;
            this.button7.Text = "Executable:";
            this.button7.UseSelectable = true;
            // 
            // lbmkisofsExecutable
            // 
            this.lbmkisofsExecutable.AutoSize = true;
            this.lbmkisofsExecutable.Location = new System.Drawing.Point(157, 30);
            this.lbmkisofsExecutable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbmkisofsExecutable.Name = "lbmkisofsExecutable";
            this.lbmkisofsExecutable.Size = new System.Drawing.Size(159, 20);
            this.lbmkisofsExecutable.TabIndex = 19;
            this.lbmkisofsExecutable.Text = "{mkisofs.exe Executable}";
            this.lbmkisofsExecutable.UseCustomBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox10.Controls.Add(this.button4);
            this.groupBox10.Controls.Add(this.lbVMwarePlayerExecutable);
            this.groupBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox10.Location = new System.Drawing.Point(11, 289);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox10.Size = new System.Drawing.Size(752, 62);
            this.groupBox10.TabIndex = 24;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "VMware Player:";
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(8, 23);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(141, 28);
            this.button4.TabIndex = 20;
            this.button4.Text = "Executable:";
            this.button4.UseSelectable = true;
            // 
            // lbVMwarePlayerExecutable
            // 
            this.lbVMwarePlayerExecutable.AutoSize = true;
            this.lbVMwarePlayerExecutable.Location = new System.Drawing.Point(157, 30);
            this.lbVMwarePlayerExecutable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbVMwarePlayerExecutable.Name = "lbVMwarePlayerExecutable";
            this.lbVMwarePlayerExecutable.Size = new System.Drawing.Size(181, 20);
            this.lbVMwarePlayerExecutable.TabIndex = 19;
            this.lbVMwarePlayerExecutable.Text = "{VMware Player Executable}";
            this.lbVMwarePlayerExecutable.UseCustomBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox9.Controls.Add(this.button6);
            this.groupBox9.Controls.Add(this.lbBOCHSExecutable);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox9.Location = new System.Drawing.Point(11, 220);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox9.Size = new System.Drawing.Size(752, 62);
            this.groupBox9.TabIndex = 23;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "BOCHS:";
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(8, 23);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(141, 28);
            this.button6.TabIndex = 20;
            this.button6.Text = "Executable:";
            this.button6.UseSelectable = true;
            // 
            // lbBOCHSExecutable
            // 
            this.lbBOCHSExecutable.AutoSize = true;
            this.lbBOCHSExecutable.Location = new System.Drawing.Point(157, 30);
            this.lbBOCHSExecutable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBOCHSExecutable.Name = "lbBOCHSExecutable";
            this.lbBOCHSExecutable.Size = new System.Drawing.Size(133, 20);
            this.lbBOCHSExecutable.TabIndex = 19;
            this.lbBOCHSExecutable.Text = "{BOCHS Executable}";
            this.lbBOCHSExecutable.UseCustomBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox8.Controls.Add(this.button5);
            this.groupBox8.Controls.Add(this.lbNDISASMExecutable);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox8.Location = new System.Drawing.Point(11, 21);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox8.Size = new System.Drawing.Size(752, 62);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "NDISASM:";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(8, 23);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(141, 28);
            this.button5.TabIndex = 20;
            this.button5.Text = "Executable:";
            this.button5.UseSelectable = true;
            // 
            // lbNDISASMExecutable
            // 
            this.lbNDISASMExecutable.AutoSize = true;
            this.lbNDISASMExecutable.Location = new System.Drawing.Point(157, 30);
            this.lbNDISASMExecutable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNDISASMExecutable.Name = "lbNDISASMExecutable";
            this.lbNDISASMExecutable.Size = new System.Drawing.Size(147, 20);
            this.lbNDISASMExecutable.TabIndex = 19;
            this.lbNDISASMExecutable.Text = "{NDISASM Executable}";
            this.lbNDISASMExecutable.UseCustomBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox7.Controls.Add(this.lbQEMUImgApplication);
            this.groupBox7.Controls.Add(this.button8);
            this.groupBox7.Controls.Add(this.lbQEMUBIOSDirectory);
            this.groupBox7.Controls.Add(this.button3);
            this.groupBox7.Controls.Add(this.button2);
            this.groupBox7.Controls.Add(this.lbQEMUExecutable);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox7.Location = new System.Drawing.Point(11, 90);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox7.Size = new System.Drawing.Size(752, 123);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "QEMU:";
            // 
            // lbQEMUImgApplication
            // 
            this.lbQEMUImgApplication.AutoSize = true;
            this.lbQEMUImgApplication.Location = new System.Drawing.Point(157, 91);
            this.lbQEMUImgApplication.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbQEMUImgApplication.Name = "lbQEMUImgApplication";
            this.lbQEMUImgApplication.Size = new System.Drawing.Size(171, 20);
            this.lbQEMUImgApplication.TabIndex = 24;
            this.lbQEMUImgApplication.Text = "{QEMU Image Executable}";
            this.lbQEMUImgApplication.UseCustomBackColor = true;
            // 
            // button8
            // 
            this.button8.Enabled = false;
            this.button8.Location = new System.Drawing.Point(8, 85);
            this.button8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(141, 28);
            this.button8.TabIndex = 23;
            this.button8.Text = "QEMU Image:";
            this.button8.UseSelectable = true;
            // 
            // lbQEMUBIOSDirectory
            // 
            this.lbQEMUBIOSDirectory.AutoSize = true;
            this.lbQEMUBIOSDirectory.Location = new System.Drawing.Point(157, 60);
            this.lbQEMUBIOSDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbQEMUBIOSDirectory.Name = "lbQEMUBIOSDirectory";
            this.lbQEMUBIOSDirectory.Size = new System.Drawing.Size(153, 20);
            this.lbQEMUBIOSDirectory.TabIndex = 22;
            this.lbQEMUBIOSDirectory.Text = "{QEMU BIOS Directory}";
            this.lbQEMUBIOSDirectory.UseCustomBackColor = true;
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(8, 54);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(141, 28);
            this.button3.TabIndex = 21;
            this.button3.Text = "BIOS Directory:";
            this.button3.UseSelectable = true;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(8, 23);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 28);
            this.button2.TabIndex = 20;
            this.button2.Text = "Executable:";
            this.button2.UseSelectable = true;
            // 
            // lbQEMUExecutable
            // 
            this.lbQEMUExecutable.AutoSize = true;
            this.lbQEMUExecutable.Location = new System.Drawing.Point(157, 30);
            this.lbQEMUExecutable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbQEMUExecutable.Name = "lbQEMUExecutable";
            this.lbQEMUExecutable.Size = new System.Drawing.Size(129, 20);
            this.lbQEMUExecutable.TabIndex = 19;
            this.lbQEMUExecutable.Text = "{QEMU Executable}";
            this.lbQEMUExecutable.UseCustomBackColor = true;
            // 
            // tabCounters
            // 
            this.tabCounters.Controls.Add(this.rtbCounters);
            this.tabCounters.HorizontalScrollbarBarColor = true;
            this.tabCounters.HorizontalScrollbarHighlightOnWheel = false;
            this.tabCounters.HorizontalScrollbarSize = 12;
            this.tabCounters.Location = new System.Drawing.Point(4, 22);
            this.tabCounters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabCounters.Name = "tabCounters";
            this.tabCounters.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabCounters.Size = new System.Drawing.Size(879, 523);
            this.tabCounters.TabIndex = 2;
            this.tabCounters.Text = "Counters";
            this.tabCounters.UseVisualStyleBackColor = true;
            this.tabCounters.VerticalScrollbarBarColor = true;
            this.tabCounters.VerticalScrollbarHighlightOnWheel = false;
            this.tabCounters.VerticalScrollbarSize = 13;
            // 
            // rtbCounters
            // 
            this.rtbCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbCounters.Font = new System.Drawing.Font("Consolas", 8F);
            this.rtbCounters.Location = new System.Drawing.Point(0, 0);
            this.rtbCounters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbCounters.Name = "rtbCounters";
            this.rtbCounters.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbCounters.Size = new System.Drawing.Size(869, 501);
            this.rtbCounters.TabIndex = 1;
            this.rtbCounters.Text = "";
            this.rtbCounters.WordWrap = false;
            // 
            // tabOutput
            // 
            this.tabOutput.Controls.Add(this.rtbOutput);
            this.tabOutput.HorizontalScrollbarBarColor = true;
            this.tabOutput.HorizontalScrollbarHighlightOnWheel = false;
            this.tabOutput.HorizontalScrollbarSize = 12;
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabOutput.Size = new System.Drawing.Size(879, 523);
            this.tabOutput.TabIndex = 1;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            this.tabOutput.VerticalScrollbarBarColor = true;
            this.tabOutput.VerticalScrollbarHighlightOnWheel = false;
            this.tabOutput.VerticalScrollbarSize = 13;
            // 
            // rtbOutput
            // 
            this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbOutput.Font = new System.Drawing.Font("Consolas", 8F);
            this.rtbOutput.Location = new System.Drawing.Point(4, 0);
            this.rtbOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbOutput.Size = new System.Drawing.Size(865, 501);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            this.rtbOutput.WordWrap = false;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.DefaultExt = "*.exe";
            this.openFileDialog2.Filter = "Executable|*.exe";
            this.openFileDialog2.Title = "Select Assembly";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tbOsName
            // 
            // 
            // 
            // 
            this.tbOsName.CustomButton.Image = null;
            this.tbOsName.CustomButton.Location = new System.Drawing.Point(130, 1);
            this.tbOsName.CustomButton.Margin = new System.Windows.Forms.Padding(4);
            this.tbOsName.CustomButton.Name = "";
            this.tbOsName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.tbOsName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbOsName.CustomButton.TabIndex = 1;
            this.tbOsName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbOsName.CustomButton.UseSelectable = true;
            this.tbOsName.CustomButton.Visible = false;
            this.tbOsName.Enabled = false;
            this.tbOsName.Lines = new string[] {
        "{Name}"};
            this.tbOsName.Location = new System.Drawing.Point(113, 48);
            this.tbOsName.Margin = new System.Windows.Forms.Padding(4);
            this.tbOsName.MaxLength = 32767;
            this.tbOsName.Name = "tbOsName";
            this.tbOsName.PasswordChar = '\0';
            this.tbOsName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbOsName.SelectedText = "";
            this.tbOsName.SelectionLength = 0;
            this.tbOsName.SelectionStart = 0;
            this.tbOsName.ShortcutsEnabled = true;
            this.tbOsName.Size = new System.Drawing.Size(154, 25);
            this.tbOsName.TabIndex = 39;
            this.tbOsName.Text = "{Name}";
            this.tbOsName.UseSelectable = true;
            this.tbOsName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbOsName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(8, 48);
            this.metroLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(75, 20);
            this.metroLabel1.TabIndex = 43;
            this.metroLabel1.Text = "OS Name:";
            this.metroLabel1.UseCustomBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 655);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tbApplicationLocations);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(27, 74, 27, 25);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MOSA Launcher";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tbApplicationLocations.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMemory)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.tabFiles.ResumeLayout(false);
            this.panelAdditionalFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabApplicationLocations.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabCounters.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private MetroFramework.Controls.MetroProgressBar progressBar1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private MetroFramework.Controls.MetroTabControl tbApplicationLocations;
		private MetroFramework.Controls.MetroTabPage tabOutput;
		private System.Windows.Forms.GroupBox groupBox6;
		private MetroFramework.Controls.MetroComboBox cbEmulator;
		private System.Windows.Forms.GroupBox groupBox4;
		private MetroFramework.Controls.MetroLabel lbSourceDirectory;
		private MetroFramework.Controls.MetroLabel label7;
		private MetroFramework.Controls.MetroButton btnSource;
		private MetroFramework.Controls.MetroLabel lbSource;
		private System.Windows.Forms.GroupBox groupBox3;
		private MetroFramework.Controls.MetroLabel label2;
		private MetroFramework.Controls.MetroComboBox cbBootFormat;
		private MetroFramework.Controls.MetroLabel label5;
		private MetroFramework.Controls.MetroComboBox cbPlatform;
		private MetroFramework.Controls.MetroButton btnDestination;
		private MetroFramework.Controls.MetroComboBox cbImageFormat;
		private MetroFramework.Controls.MetroLabel label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private MetroFramework.Controls.MetroCheckBox cbEnableIROptimizations;
		private MetroFramework.Controls.MetroCheckBox cbEnableSSA;
		private MetroFramework.Controls.MetroTabPage tabOptions;
		private MetroFramework.Controls.MetroLabel label4;
		private MetroFramework.Controls.MetroComboBox cbBootFileSystem;
		private System.Windows.Forms.RichTextBox rtbOutput;
		private MetroFramework.Controls.MetroTabPage tabCounters;
		private System.Windows.Forms.RichTextBox rtbCounters;
		private MetroFramework.Controls.MetroTabPage tabApplicationLocations;
		private System.Windows.Forms.GroupBox groupBox7;
		private MetroFramework.Controls.MetroLabel lbQEMUBIOSDirectory;
		private MetroFramework.Controls.MetroButton button3;
		private MetroFramework.Controls.MetroButton button2;
		private MetroFramework.Controls.MetroLabel lbQEMUExecutable;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.GroupBox groupBox8;
		private MetroFramework.Controls.MetroButton button5;
		private MetroFramework.Controls.MetroLabel lbNDISASMExecutable;
		private MetroFramework.Controls.MetroCheckBox cbExitOnLaunch;
		private System.Windows.Forms.GroupBox groupBox9;
		private MetroFramework.Controls.MetroButton button6;
		private MetroFramework.Controls.MetroLabel lbBOCHSExecutable;
		private MetroFramework.Controls.MetroLabel label6;
		private System.Windows.Forms.NumericUpDown nmMemory;
		private System.Windows.Forms.GroupBox groupBox10;
		private MetroFramework.Controls.MetroButton button4;
		private MetroFramework.Controls.MetroLabel lbVMwarePlayerExecutable;
		private System.Windows.Forms.GroupBox groupBox11;
		private MetroFramework.Controls.MetroButton button7;
		private MetroFramework.Controls.MetroLabel lbmkisofsExecutable;
		private MetroFramework.Controls.MetroLabel lbQEMUImgApplication;
		private MetroFramework.Controls.MetroButton button8;
		private System.Windows.Forms.GroupBox groupBox2;
		private MetroFramework.Controls.MetroCheckBox cbGenerateMapFile;
		private MetroFramework.Controls.MetroCheckBox cbEnableSparseConditionalConstantPropagation;
		private MetroFramework.Controls.MetroButton button1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
		private System.Windows.Forms.GroupBox groupBox12;
		private MetroFramework.Controls.MetroCheckBox cbCompilerUsesMultipleThreads;
		private MetroFramework.Controls.MetroCheckBox cbInlinedMethods;
		private MetroFramework.Controls.MetroLabel label1;
		private MetroFramework.Controls.MetroComboBox cbBootLoader;
		private MetroFramework.Controls.MetroCheckBox cbVBEVideo;
		private MetroFramework.Controls.MetroTextBox tbMode;
		private MetroFramework.Controls.MetroTabPage tabAdvanced;
		private System.Windows.Forms.GroupBox groupBox13;
		private MetroFramework.Controls.MetroCheckBox cbRelocationTable;
		private MetroFramework.Controls.MetroCheckBox cbEmitSymbolTable;
		private MetroFramework.Controls.MetroTextBox tbBaseAddress;
		private MetroFramework.Controls.MetroLabel label9;
		private System.Windows.Forms.GroupBox groupBox14;
		private MetroFramework.Controls.MetroLabel lbDestinationDirectory;
		private MetroFramework.Controls.MetroCheckBox cbGenerateASMFile;
		private System.Windows.Forms.GroupBox groupBox15;
		private MetroFramework.Controls.MetroCheckBox cbEnableQemuGDB;
		private MetroFramework.Controls.MetroCheckBox cbLaunchGDB;
		private MetroFramework.Controls.MetroCheckBox cbGenerateDebugInfoFile;
		private System.Windows.Forms.GroupBox groupBox5;
		private MetroFramework.Controls.MetroComboBox cbDebugConnectionOption;
		private MetroFramework.Controls.MetroLabel label8;
		private MetroFramework.Controls.MetroCheckBox cbMosaDebugger;
		private MetroFramework.Controls.MetroCheckBox cbLaunchMosaDebugger;
		private System.Windows.Forms.TabPage tabFiles;
		private System.Windows.Forms.Panel panelAdditionalFiles;
		private MetroFramework.Controls.MetroButton btnRemoveFiles;
		private MetroFramework.Controls.MetroButton btnAddFiles;
		private MetroFramework.Controls.MetroCheckBox cbTwoPassOptimizations;
		private MetroFramework.Controls.MetroCheckBox cbIRLongExpansion;
		private System.Windows.Forms.DataGridView dataGridView1;
		private MetroFramework.Controls.MetroCheckBox cbValueNumbering;
		private MetroFramework.Controls.MetroCheckBox cbPlugKorlib;
		private MetroFramework.Controls.MetroCheckBox cbEnableMethodScanner;
		private System.Windows.Forms.Timer timer1;
		private MetroFramework.Controls.MetroCheckBox cbGenerateCompilerTime;
		private System.Windows.Forms.GroupBox groupBox16;
		private MetroFramework.Controls.MetroCheckBox cbGenerateNASMFile;
		private MetroFramework.Controls.MetroCheckBox cbBitTracker;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroTextBox tbOsName;
	}
}
