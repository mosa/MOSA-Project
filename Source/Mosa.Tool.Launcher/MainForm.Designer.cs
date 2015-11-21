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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tbOptions = new System.Windows.Forms.TabPage();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.cbCompilerUsesMultipleThreads = new System.Windows.Forms.CheckBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.cbDebugConnectionOption = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.cbMOSADebugger = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbGenerateASMFile = new System.Windows.Forms.CheckBox();
			this.cbGenerateMapFile = new System.Windows.Forms.CheckBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.nmMemory = new System.Windows.Forms.NumericUpDown();
			this.cbExitOnLaunch = new System.Windows.Forms.CheckBox();
			this.cbEmulator = new System.Windows.Forms.ComboBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lbSourceDirectory = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnSource = new System.Windows.Forms.Button();
			this.lbSource = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbBootLoader = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbBootFileSystem = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbBootFormat = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cbPlatform = new System.Windows.Forms.ComboBox();
			this.btnDestination = new System.Windows.Forms.Button();
			this.cbImageFormat = new System.Windows.Forms.ComboBox();
			this.lbDestinationDirectory = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbInlinedMethods = new System.Windows.Forms.CheckBox();
			this.cbEnableSparseConditionalConstantPropagation = new System.Windows.Forms.CheckBox();
			this.cbEnableIROptimizations = new System.Windows.Forms.CheckBox();
			this.cbEnableSSA = new System.Windows.Forms.CheckBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.button7 = new System.Windows.Forms.Button();
			this.lbmkisofsExecutable = new System.Windows.Forms.Label();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.button4 = new System.Windows.Forms.Button();
			this.lbVMwarePlayerExecutable = new System.Windows.Forms.Label();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.button6 = new System.Windows.Forms.Button();
			this.lbBOCHSExecutable = new System.Windows.Forms.Label();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.button5 = new System.Windows.Forms.Button();
			this.lbNDISASMExecutable = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.lbQEMUImgApplication = new System.Windows.Forms.Label();
			this.button8 = new System.Windows.Forms.Button();
			this.lbQEMUBIOSDirectory = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.lbQEMUExecutable = new System.Windows.Forms.Label();
			this.tpOutput = new System.Windows.Forms.TabPage();
			this.rtbOutput = new System.Windows.Forms.RichTextBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.rtbCounters = new System.Windows.Forms.RichTextBox();
			this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.tabControl1.SuspendLayout();
			this.tbOptions.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nmMemory)).BeginInit();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tpOutput.SuspendLayout();
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
			this.progressBar1.Size = new System.Drawing.Size(634, 20);
			this.progressBar1.TabIndex = 3;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControl1.Controls.Add(this.tbOptions);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tpOutput);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.ItemSize = new System.Drawing.Size(48, 18);
			this.tabControl1.Location = new System.Drawing.Point(0, 32);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(0, 0);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(634, 410);
			this.tabControl1.TabIndex = 22;
			// 
			// tbOptions
			// 
			this.tbOptions.BackColor = System.Drawing.SystemColors.Control;
			this.tbOptions.Controls.Add(this.groupBox12);
			this.tbOptions.Controls.Add(this.statusStrip1);
			this.tbOptions.Controls.Add(this.groupBox5);
			this.tbOptions.Controls.Add(this.groupBox2);
			this.tbOptions.Controls.Add(this.groupBox6);
			this.tbOptions.Controls.Add(this.groupBox4);
			this.tbOptions.Controls.Add(this.groupBox3);
			this.tbOptions.Controls.Add(this.groupBox1);
			this.tbOptions.Location = new System.Drawing.Point(4, 22);
			this.tbOptions.Margin = new System.Windows.Forms.Padding(0);
			this.tbOptions.Name = "tbOptions";
			this.tbOptions.Size = new System.Drawing.Size(626, 384);
			this.tbOptions.TabIndex = 0;
			this.tbOptions.Text = "MOSA Options";
			// 
			// groupBox12
			// 
			this.groupBox12.Controls.Add(this.cbCompilerUsesMultipleThreads);
			this.groupBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox12.Location = new System.Drawing.Point(286, 317);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(136, 38);
			this.groupBox12.TabIndex = 30;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "Misc Options:";
			// 
			// cbCompilerUsesMultipleThreads
			// 
			this.cbCompilerUsesMultipleThreads.AutoSize = true;
			this.cbCompilerUsesMultipleThreads.Checked = true;
			this.cbCompilerUsesMultipleThreads.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbCompilerUsesMultipleThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbCompilerUsesMultipleThreads.Location = new System.Drawing.Point(6, 19);
			this.cbCompilerUsesMultipleThreads.Name = "cbCompilerUsesMultipleThreads";
			this.cbCompilerUsesMultipleThreads.Size = new System.Drawing.Size(164, 21);
			this.cbCompilerUsesMultipleThreads.TabIndex = 13;
			this.cbCompilerUsesMultipleThreads.Text = "Use Multiple Threads";
			this.cbCompilerUsesMultipleThreads.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 362);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(626, 22);
			this.statusStrip1.TabIndex = 32;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tsStatusLabel
			// 
			this.tsStatusLabel.Name = "tsStatusLabel";
			this.tsStatusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.cbDebugConnectionOption);
			this.groupBox5.Controls.Add(this.label8);
			this.groupBox5.Controls.Add(this.checkBox1);
			this.groupBox5.Controls.Add(this.cbMOSADebugger);
			this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox5.Location = new System.Drawing.Point(283, 196);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(139, 115);
			this.groupBox5.TabIndex = 31;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Debugger:";
			// 
			// cbDebugConnectionOption
			// 
			this.cbDebugConnectionOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDebugConnectionOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDebugConnectionOption.FormattingEnabled = true;
			this.cbDebugConnectionOption.Items.AddRange(new object[] {
            "None",
            "Pipe",
            "TCP Server",
            "TCP Client"});
			this.cbDebugConnectionOption.Location = new System.Drawing.Point(9, 87);
			this.cbDebugConnectionOption.Name = "cbDebugConnectionOption";
			this.cbDebugConnectionOption.Size = new System.Drawing.Size(120, 25);
			this.cbDebugConnectionOption.TabIndex = 36;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(6, 71);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(146, 17);
			this.label8.TabIndex = 35;
			this.label8.Text = "Debug Connection:";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBox1.Location = new System.Drawing.Point(6, 41);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(137, 21);
			this.checkBox1.TabIndex = 14;
			this.checkBox1.Text = "MOSA Debugger";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// cbMOSADebugger
			// 
			this.cbMOSADebugger.AutoSize = true;
			this.cbMOSADebugger.Enabled = false;
			this.cbMOSADebugger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbMOSADebugger.Location = new System.Drawing.Point(6, 19);
			this.cbMOSADebugger.Name = "cbMOSADebugger";
			this.cbMOSADebugger.Size = new System.Drawing.Size(160, 21);
			this.cbMOSADebugger.TabIndex = 13;
			this.cbMOSADebugger.Text = "MOSA TinySimulator";
			this.cbMOSADebugger.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cbGenerateASMFile);
			this.groupBox2.Controls.Add(this.cbGenerateMapFile);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(8, 317);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(272, 38);
			this.groupBox2.TabIndex = 29;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Diagnostic Output:";
			// 
			// cbGenerateASMFile
			// 
			this.cbGenerateASMFile.AutoSize = true;
			this.cbGenerateASMFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbGenerateASMFile.Location = new System.Drawing.Point(137, 19);
			this.cbGenerateASMFile.Name = "cbGenerateASMFile";
			this.cbGenerateASMFile.Size = new System.Drawing.Size(149, 21);
			this.cbGenerateASMFile.TabIndex = 14;
			this.cbGenerateASMFile.Text = "Generate ASM File";
			this.cbGenerateASMFile.UseVisualStyleBackColor = true;
			// 
			// cbGenerateMapFile
			// 
			this.cbGenerateMapFile.AutoSize = true;
			this.cbGenerateMapFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbGenerateMapFile.Location = new System.Drawing.Point(6, 19);
			this.cbGenerateMapFile.Name = "cbGenerateMapFile";
			this.cbGenerateMapFile.Size = new System.Drawing.Size(149, 21);
			this.cbGenerateMapFile.TabIndex = 13;
			this.cbGenerateMapFile.Text = "Generate MAP File";
			this.cbGenerateMapFile.UseVisualStyleBackColor = true;
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox6.Controls.Add(this.button1);
			this.groupBox6.Controls.Add(this.label6);
			this.groupBox6.Controls.Add(this.nmMemory);
			this.groupBox6.Controls.Add(this.cbExitOnLaunch);
			this.groupBox6.Controls.Add(this.cbEmulator);
			this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox6.Location = new System.Drawing.Point(428, 197);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(188, 158);
			this.groupBox6.TabIndex = 28;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Virtual Machine Emulator:";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(14, 129);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(168, 23);
			this.button1.TabIndex = 28;
			this.button1.Text = "Compile and Run (F5)";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(7, 52);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 17);
			this.label6.TabIndex = 24;
			this.label6.Text = "Memory:";
			// 
			// nmMemory
			// 
			this.nmMemory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nmMemory.Location = new System.Drawing.Point(67, 50);
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
			this.nmMemory.Size = new System.Drawing.Size(56, 23);
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
			this.cbExitOnLaunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbExitOnLaunch.Location = new System.Drawing.Point(14, 106);
			this.cbExitOnLaunch.Name = "cbExitOnLaunch";
			this.cbExitOnLaunch.Size = new System.Drawing.Size(132, 21);
			this.cbExitOnLaunch.TabIndex = 22;
			this.cbExitOnLaunch.Text = "Exit on Emulator";
			this.cbExitOnLaunch.UseVisualStyleBackColor = true;
			// 
			// cbEmulator
			// 
			this.cbEmulator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEmulator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEmulator.FormattingEnabled = true;
			this.cbEmulator.Items.AddRange(new object[] {
            "QEMU",
            "Bochs",
            "VMware"});
			this.cbEmulator.Location = new System.Drawing.Point(10, 19);
			this.cbEmulator.Name = "cbEmulator";
			this.cbEmulator.Size = new System.Drawing.Size(171, 25);
			this.cbEmulator.TabIndex = 21;
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
			this.groupBox4.Size = new System.Drawing.Size(611, 70);
			this.groupBox4.TabIndex = 25;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Source:";
			// 
			// lbSourceDirectory
			// 
			this.lbSourceDirectory.AutoSize = true;
			this.lbSourceDirectory.Location = new System.Drawing.Point(119, 48);
			this.lbSourceDirectory.Name = "lbSourceDirectory";
			this.lbSourceDirectory.Size = new System.Drawing.Size(105, 15);
			this.lbSourceDirectory.TabIndex = 20;
			this.lbSourceDirectory.Text = "{Source Directory}";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(7, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(135, 17);
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
			this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
			// 
			// lbSource
			// 
			this.lbSource.AutoSize = true;
			this.lbSource.Location = new System.Drawing.Point(119, 24);
			this.lbSource.Name = "lbSource";
			this.lbSource.Size = new System.Drawing.Size(54, 15);
			this.lbSource.TabIndex = 17;
			this.lbSource.Text = "{Source}";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
			this.groupBox3.Location = new System.Drawing.Point(5, 83);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(611, 108);
			this.groupBox3.TabIndex = 24;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Output:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(182, 82);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 17);
			this.label1.TabIndex = 36;
			this.label1.Text = "Boot Loader:";
			// 
			// cbBootLoader
			// 
			this.cbBootLoader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBootLoader.FormattingEnabled = true;
			this.cbBootLoader.Items.AddRange(new object[] {
            "Syslinux 3.72",
            "Syslinux 6.03",
            "Grub 0.97"});
			this.cbBootLoader.Location = new System.Drawing.Point(267, 78);
			this.cbBootLoader.Name = "cbBootLoader";
			this.cbBootLoader.Size = new System.Drawing.Size(127, 21);
			this.cbBootLoader.TabIndex = 35;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(7, 81);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 17);
			this.label4.TabIndex = 34;
			this.label4.Text = "File System:";
			// 
			// cbBootFileSystem
			// 
			this.cbBootFileSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBootFileSystem.FormattingEnabled = true;
			this.cbBootFileSystem.Items.AddRange(new object[] {
            "FAT12",
            "FAT16"});
			this.cbBootFileSystem.Location = new System.Drawing.Point(88, 78);
			this.cbBootFileSystem.Name = "cbBootFileSystem";
			this.cbBootFileSystem.Size = new System.Drawing.Size(88, 21);
			this.cbBootFileSystem.TabIndex = 33;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(183, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 17);
			this.label2.TabIndex = 32;
			this.label2.Text = "Boot Format:";
			// 
			// cbBootFormat
			// 
			this.cbBootFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBootFormat.FormattingEnabled = true;
			this.cbBootFormat.Items.AddRange(new object[] {
            "Multiboot v0.7",
            "Multiboot v0.7 Video"});
			this.cbBootFormat.Location = new System.Drawing.Point(267, 47);
			this.cbBootFormat.Name = "cbBootFormat";
			this.cbBootFormat.Size = new System.Drawing.Size(127, 21);
			this.cbBootFormat.TabIndex = 31;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(25, 51);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(73, 17);
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
			this.cbPlatform.Location = new System.Drawing.Point(88, 47);
			this.cbPlatform.Name = "cbPlatform";
			this.cbPlatform.Size = new System.Drawing.Size(88, 21);
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
			this.btnDestination.Click += new System.EventHandler(this.btnDestination_Click);
			// 
			// cbImageFormat
			// 
			this.cbImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbImageFormat.FormattingEnabled = true;
			this.cbImageFormat.Items.AddRange(new object[] {
            "IMG (.img)",
            "ISO Image (.iso)",
            "Microsoft (.vhd)",
            "Virtual Box (.vdi)",
            "VMware (.vmdk)"});
			this.cbImageFormat.Location = new System.Drawing.Point(493, 47);
			this.cbImageFormat.Name = "cbImageFormat";
			this.cbImageFormat.Size = new System.Drawing.Size(100, 21);
			this.cbImageFormat.TabIndex = 16;
			// 
			// lbDestinationDirectory
			// 
			this.lbDestinationDirectory.AutoSize = true;
			this.lbDestinationDirectory.Location = new System.Drawing.Point(119, 23);
			this.lbDestinationDirectory.Name = "lbDestinationDirectory";
			this.lbDestinationDirectory.Size = new System.Drawing.Size(128, 15);
			this.lbDestinationDirectory.TabIndex = 14;
			this.lbDestinationDirectory.Text = "{Destination Directory}";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(400, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(111, 17);
			this.label3.TabIndex = 12;
			this.label3.Text = "Image Format:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbInlinedMethods);
			this.groupBox1.Controls.Add(this.cbEnableSparseConditionalConstantPropagation);
			this.groupBox1.Controls.Add(this.cbEnableIROptimizations);
			this.groupBox1.Controls.Add(this.cbEnableSSA);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(5, 196);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 115);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Optimizations:";
			// 
			// cbInlinedMethods
			// 
			this.cbInlinedMethods.AutoSize = true;
			this.cbInlinedMethods.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbInlinedMethods.Location = new System.Drawing.Point(6, 89);
			this.cbInlinedMethods.Name = "cbInlinedMethods";
			this.cbInlinedMethods.Size = new System.Drawing.Size(177, 21);
			this.cbInlinedMethods.TabIndex = 9;
			this.cbInlinedMethods.Text = "Enable Inlined Methods";
			this.cbInlinedMethods.UseVisualStyleBackColor = true;
			// 
			// cbEnableSparseConditionalConstantPropagation
			// 
			this.cbEnableSparseConditionalConstantPropagation.AutoSize = true;
			this.cbEnableSparseConditionalConstantPropagation.Checked = true;
			this.cbEnableSparseConditionalConstantPropagation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableSparseConditionalConstantPropagation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEnableSparseConditionalConstantPropagation.Location = new System.Drawing.Point(6, 65);
			this.cbEnableSparseConditionalConstantPropagation.Name = "cbEnableSparseConditionalConstantPropagation";
			this.cbEnableSparseConditionalConstantPropagation.Size = new System.Drawing.Size(338, 21);
			this.cbEnableSparseConditionalConstantPropagation.TabIndex = 8;
			this.cbEnableSparseConditionalConstantPropagation.Text = "Enable Sparse Conditional Constant Propagation";
			this.cbEnableSparseConditionalConstantPropagation.UseVisualStyleBackColor = true;
			// 
			// cbEnableIROptimizations
			// 
			this.cbEnableIROptimizations.AutoSize = true;
			this.cbEnableIROptimizations.Checked = true;
			this.cbEnableIROptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableIROptimizations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEnableIROptimizations.Location = new System.Drawing.Point(6, 42);
			this.cbEnableIROptimizations.Name = "cbEnableIROptimizations";
			this.cbEnableIROptimizations.Size = new System.Drawing.Size(180, 21);
			this.cbEnableIROptimizations.TabIndex = 7;
			this.cbEnableIROptimizations.Text = "Enable IR Optimizations";
			this.cbEnableIROptimizations.UseVisualStyleBackColor = true;
			// 
			// cbEnableSSA
			// 
			this.cbEnableSSA.AutoSize = true;
			this.cbEnableSSA.Checked = true;
			this.cbEnableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableSSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEnableSSA.Location = new System.Drawing.Point(6, 18);
			this.cbEnableSSA.Name = "cbEnableSSA";
			this.cbEnableSSA.Size = new System.Drawing.Size(274, 21);
			this.cbEnableSSA.TabIndex = 6;
			this.cbEnableSSA.Text = "Enable Static Single Assignment (SSA)";
			this.cbEnableSSA.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.groupBox11);
			this.tabPage4.Controls.Add(this.groupBox10);
			this.tabPage4.Controls.Add(this.groupBox9);
			this.tabPage4.Controls.Add(this.groupBox8);
			this.tabPage4.Controls.Add(this.groupBox7);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(626, 384);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Application Settings";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// groupBox11
			// 
			this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox11.Controls.Add(this.button7);
			this.groupBox11.Controls.Add(this.lbmkisofsExecutable);
			this.groupBox11.Location = new System.Drawing.Point(8, 291);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(611, 50);
			this.groupBox11.TabIndex = 25;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "mkisofs:";
			// 
			// button7
			// 
			this.button7.Enabled = false;
			this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button7.Location = new System.Drawing.Point(6, 19);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(106, 23);
			this.button7.TabIndex = 20;
			this.button7.Text = "Executable:";
			this.button7.UseVisualStyleBackColor = true;
			// 
			// lbmkisofsExecutable
			// 
			this.lbmkisofsExecutable.AutoSize = true;
			this.lbmkisofsExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbmkisofsExecutable.Name = "lbmkisofsExecutable";
			this.lbmkisofsExecutable.Size = new System.Drawing.Size(144, 15);
			this.lbmkisofsExecutable.TabIndex = 19;
			this.lbmkisofsExecutable.Text = "{mkisofs.exe Executable}";
			// 
			// groupBox10
			// 
			this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox10.Controls.Add(this.button4);
			this.groupBox10.Controls.Add(this.lbVMwarePlayerExecutable);
			this.groupBox10.Location = new System.Drawing.Point(8, 235);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(611, 50);
			this.groupBox10.TabIndex = 24;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "VMware Player:";
			// 
			// button4
			// 
			this.button4.Enabled = false;
			this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button4.Location = new System.Drawing.Point(6, 19);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(106, 23);
			this.button4.TabIndex = 20;
			this.button4.Text = "Executable:";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// lbVMwarePlayerExecutable
			// 
			this.lbVMwarePlayerExecutable.AutoSize = true;
			this.lbVMwarePlayerExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbVMwarePlayerExecutable.Name = "lbVMwarePlayerExecutable";
			this.lbVMwarePlayerExecutable.Size = new System.Drawing.Size(161, 15);
			this.lbVMwarePlayerExecutable.TabIndex = 19;
			this.lbVMwarePlayerExecutable.Text = "{VMware Player Executable}";
			// 
			// groupBox9
			// 
			this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox9.Controls.Add(this.button6);
			this.groupBox9.Controls.Add(this.lbBOCHSExecutable);
			this.groupBox9.Location = new System.Drawing.Point(8, 179);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(611, 50);
			this.groupBox9.TabIndex = 23;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "BOCHS:";
			// 
			// button6
			// 
			this.button6.Enabled = false;
			this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button6.Location = new System.Drawing.Point(6, 19);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(106, 23);
			this.button6.TabIndex = 20;
			this.button6.Text = "Executable:";
			this.button6.UseVisualStyleBackColor = true;
			// 
			// lbBOCHSExecutable
			// 
			this.lbBOCHSExecutable.AutoSize = true;
			this.lbBOCHSExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbBOCHSExecutable.Name = "lbBOCHSExecutable";
			this.lbBOCHSExecutable.Size = new System.Drawing.Size(121, 15);
			this.lbBOCHSExecutable.TabIndex = 19;
			this.lbBOCHSExecutable.Text = "{BOCHS Executable}";
			// 
			// groupBox8
			// 
			this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox8.Controls.Add(this.button5);
			this.groupBox8.Controls.Add(this.lbNDISASMExecutable);
			this.groupBox8.Location = new System.Drawing.Point(8, 17);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(611, 50);
			this.groupBox8.TabIndex = 1;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "NDISASM:";
			// 
			// button5
			// 
			this.button5.Enabled = false;
			this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button5.Location = new System.Drawing.Point(6, 19);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(106, 23);
			this.button5.TabIndex = 20;
			this.button5.Text = "Executable:";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// lbNDISASMExecutable
			// 
			this.lbNDISASMExecutable.AutoSize = true;
			this.lbNDISASMExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbNDISASMExecutable.Name = "lbNDISASMExecutable";
			this.lbNDISASMExecutable.Size = new System.Drawing.Size(134, 15);
			this.lbNDISASMExecutable.TabIndex = 19;
			this.lbNDISASMExecutable.Text = "{NDISASM Executable}";
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox7.Controls.Add(this.lbQEMUImgApplication);
			this.groupBox7.Controls.Add(this.button8);
			this.groupBox7.Controls.Add(this.lbQEMUBIOSDirectory);
			this.groupBox7.Controls.Add(this.button3);
			this.groupBox7.Controls.Add(this.button2);
			this.groupBox7.Controls.Add(this.lbQEMUExecutable);
			this.groupBox7.Location = new System.Drawing.Point(8, 73);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(611, 100);
			this.groupBox7.TabIndex = 0;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "QEMU:";
			// 
			// lbQEMUImgApplication
			// 
			this.lbQEMUImgApplication.AutoSize = true;
			this.lbQEMUImgApplication.Location = new System.Drawing.Point(118, 74);
			this.lbQEMUImgApplication.Name = "lbQEMUImgApplication";
			this.lbQEMUImgApplication.Size = new System.Drawing.Size(154, 15);
			this.lbQEMUImgApplication.TabIndex = 24;
			this.lbQEMUImgApplication.Text = "{QEMU Image Executable}";
			// 
			// button8
			// 
			this.button8.Enabled = false;
			this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button8.Location = new System.Drawing.Point(6, 69);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(106, 23);
			this.button8.TabIndex = 23;
			this.button8.Text = "QEMU Image:";
			this.button8.UseVisualStyleBackColor = true;
			// 
			// lbQEMUBIOSDirectory
			// 
			this.lbQEMUBIOSDirectory.AutoSize = true;
			this.lbQEMUBIOSDirectory.Location = new System.Drawing.Point(118, 49);
			this.lbQEMUBIOSDirectory.Name = "lbQEMUBIOSDirectory";
			this.lbQEMUBIOSDirectory.Size = new System.Drawing.Size(134, 15);
			this.lbQEMUBIOSDirectory.TabIndex = 22;
			this.lbQEMUBIOSDirectory.Text = "{QEMU BIOS Directory}";
			// 
			// button3
			// 
			this.button3.Enabled = false;
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button3.Location = new System.Drawing.Point(6, 44);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(106, 23);
			this.button3.TabIndex = 21;
			this.button3.Text = "BIOS Directory:";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Location = new System.Drawing.Point(6, 19);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(106, 23);
			this.button2.TabIndex = 20;
			this.button2.Text = "Executable:";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// lbQEMUExecutable
			// 
			this.lbQEMUExecutable.AutoSize = true;
			this.lbQEMUExecutable.Location = new System.Drawing.Point(118, 24);
			this.lbQEMUExecutable.Name = "lbQEMUExecutable";
			this.lbQEMUExecutable.Size = new System.Drawing.Size(116, 15);
			this.lbQEMUExecutable.TabIndex = 19;
			this.lbQEMUExecutable.Text = "{QEMU Executable}";
			// 
			// tpOutput
			// 
			this.tpOutput.Controls.Add(this.rtbOutput);
			this.tpOutput.Location = new System.Drawing.Point(4, 22);
			this.tpOutput.Name = "tpOutput";
			this.tpOutput.Padding = new System.Windows.Forms.Padding(3);
			this.tpOutput.Size = new System.Drawing.Size(626, 384);
			this.tpOutput.TabIndex = 1;
			this.tpOutput.Text = "Output";
			this.tpOutput.UseVisualStyleBackColor = true;
			// 
			// rtbOutput
			// 
			this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbOutput.Font = new System.Drawing.Font("Consolas", 8F);
			this.rtbOutput.Location = new System.Drawing.Point(0, 0);
			this.rtbOutput.Name = "rtbOutput";
			this.rtbOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.rtbOutput.Size = new System.Drawing.Size(629, 402);
			this.rtbOutput.TabIndex = 0;
			this.rtbOutput.Text = "";
			this.rtbOutput.WordWrap = false;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.rtbCounters);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(626, 384);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Counters";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// rtbCounters
			// 
			this.rtbCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbCounters.Font = new System.Drawing.Font("Consolas", 8F);
			this.rtbCounters.Location = new System.Drawing.Point(0, 1);
			this.rtbCounters.Name = "rtbCounters";
			this.rtbCounters.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.rtbCounters.Size = new System.Drawing.Size(629, 404);
			this.rtbCounters.TabIndex = 1;
			this.rtbCounters.Text = "";
			this.rtbCounters.WordWrap = false;
			// 
			// openFileDialog2
			// 
			this.openFileDialog2.DefaultExt = "*.exe";
			this.openFileDialog2.Filter = "Executable|*.exe";
			this.openFileDialog2.Title = "Select Assembly";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 441);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "MOSA Launcher v1.6";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.tabControl1.ResumeLayout(false);
			this.tbOptions.ResumeLayout(false);
			this.tbOptions.PerformLayout();
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
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
			this.tabPage4.ResumeLayout(false);
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
			this.tpOutput.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpOutput;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ComboBox cbEmulator;
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
		private System.Windows.Forms.Label lbDestinationDirectory;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbEnableIROptimizations;
		private System.Windows.Forms.CheckBox cbEnableSSA;
		private System.Windows.Forms.TabPage tbOptions;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbBootFileSystem;
		private System.Windows.Forms.RichTextBox rtbOutput;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.RichTextBox rtbCounters;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label lbQEMUBIOSDirectory;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label lbQEMUExecutable;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Label lbNDISASMExecutable;
		private System.Windows.Forms.CheckBox cbExitOnLaunch;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label lbBOCHSExecutable;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nmMemory;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label lbVMwarePlayerExecutable;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Label lbmkisofsExecutable;
		private System.Windows.Forms.Label lbQEMUImgApplication;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox cbMOSADebugger;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbGenerateASMFile;
		private System.Windows.Forms.CheckBox cbGenerateMapFile;
		private System.Windows.Forms.CheckBox cbEnableSparseConditionalConstantPropagation;
		private System.Windows.Forms.ComboBox cbDebugConnectionOption;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.CheckBox cbCompilerUsesMultipleThreads;
		private System.Windows.Forms.CheckBox cbInlinedMethods;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbBootLoader;
	}
}
