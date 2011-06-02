namespace Mosa.Tools.TypeExplorer
{
    partial class CodeForm
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
			this.tbText = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbText
			// 
			this.tbText.Location = new System.Drawing.Point(12, 13);
			this.tbText.Name = "tbText";
			this.tbText.Size = new System.Drawing.Size(521, 368);
			this.tbText.TabIndex = 0;
			this.tbText.Text = "";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(418, 389);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(115, 25);
			this.button1.TabIndex = 1;
			this.button1.Text = "Compile";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// CodeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(545, 427);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tbText);
			this.Name = "CodeForm";
			this.Text = "Code Snippet";
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tbText;
        private System.Windows.Forms.Button button1;
    }
}

