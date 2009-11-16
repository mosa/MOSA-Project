namespace Mosa.Tools.StageVisualizer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbMethods = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Source = new System.Windows.Forms.TabPage();
            this.tbSource = new System.Windows.Forms.RichTextBox();
            this.Result = new System.Windows.Forms.TabPage();
            this.tbResult = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Source.SuspendLayout();
            this.Result.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox3);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.comboBox4);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.comboBox3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbMethods);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 439);
            this.panel1.TabIndex = 1;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(11, 118);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(57, 17);
            this.checkBox3.TabIndex = 11;
            this.checkBox3.Text = "Stage:";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(11, 170);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(56, 17);
            this.checkBox2.TabIndex = 10;
            this.checkBox2.Text = "Block:";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(11, 190);
            this.comboBox4.MaxDropDownItems = 20;
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(202, 21);
            this.comboBox4.TabIndex = 9;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 223);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(55, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Label:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(11, 243);
            this.comboBox3.MaxDropDownItems = 20;
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(202, 21);
            this.comboBox3.TabIndex = 6;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(17, 308);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "Update";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Method:";
            // 
            // cbMethods
            // 
            this.cbMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMethods.FormattingEnabled = true;
            this.cbMethods.Location = new System.Drawing.Point(11, 84);
            this.cbMethods.MaxDropDownItems = 20;
            this.cbMethods.Name = "cbMethods";
            this.cbMethods.Size = new System.Drawing.Size(202, 21);
            this.cbMethods.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(17, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(190, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load Output";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 138);
            this.comboBox1.MaxDropDownItems = 20;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(202, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.Source);
            this.tabControl1.Controls.Add(this.Result);
            this.tabControl1.Location = new System.Drawing.Point(228, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(554, 439);
            this.tabControl1.TabIndex = 2;
            // 
            // Source
            // 
            this.Source.Controls.Add(this.tbSource);
            this.Source.Location = new System.Drawing.Point(4, 22);
            this.Source.Name = "Source";
            this.Source.Padding = new System.Windows.Forms.Padding(3);
            this.Source.Size = new System.Drawing.Size(546, 413);
            this.Source.TabIndex = 1;
            this.Source.Text = "Source";
            this.Source.UseVisualStyleBackColor = true;
            // 
            // tbSource
            // 
            this.tbSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSource.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSource.Location = new System.Drawing.Point(0, 0);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(543, 413);
            this.tbSource.TabIndex = 1;
            this.tbSource.Text = "";
            this.tbSource.WordWrap = false;
            // 
            // Result
            // 
            this.Result.Controls.Add(this.tbResult);
            this.Result.Location = new System.Drawing.Point(4, 22);
            this.Result.Name = "Result";
            this.Result.Padding = new System.Windows.Forms.Padding(3);
            this.Result.Size = new System.Drawing.Size(546, 413);
            this.Result.TabIndex = 0;
            this.Result.Text = "Result";
            this.Result.UseVisualStyleBackColor = true;
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Location = new System.Drawing.Point(2, 0);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(543, 413);
            this.tbResult.TabIndex = 2;
            this.tbResult.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 444);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "MOSA Compiler Visualizer";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Source.ResumeLayout(false);
            this.Result.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbMethods;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Result;
        private System.Windows.Forms.TabPage Source;
        private System.Windows.Forms.RichTextBox tbSource;
        private System.Windows.Forms.RichTextBox tbResult;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

