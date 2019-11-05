namespace DXFRenderingBitmap
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
            this.groupBoxSaveControls = new System.Windows.Forms.GroupBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonSaveFile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonPerformSaving = new System.Windows.Forms.Button();
            this.userControlForPaint1 = new DXFRenderingBitmap.UserControlForPaint();
            this.groupBoxSaveControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSaveControls
            // 
            this.groupBoxSaveControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSaveControls.Controls.Add(this.buttonPerformSaving);
            this.groupBoxSaveControls.Controls.Add(this.textBox2);
            this.groupBoxSaveControls.Controls.Add(this.textBox1);
            this.groupBoxSaveControls.Controls.Add(this.buttonSaveFile);
            this.groupBoxSaveControls.Controls.Add(this.buttonOpenFile);
            this.groupBoxSaveControls.Location = new System.Drawing.Point(0, 474);
            this.groupBoxSaveControls.Name = "groupBoxSaveControls";
            this.groupBoxSaveControls.Size = new System.Drawing.Size(797, 70);
            this.groupBoxSaveControls.TabIndex = 4;
            this.groupBoxSaveControls.TabStop = false;
            this.groupBoxSaveControls.Text = "SaveAndLoad";
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(7, 15);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(97, 23);
            this.buttonOpenFile.TabIndex = 0;
            this.buttonOpenFile.Text = "Open file dialog";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonSaveFile
            // 
            this.buttonSaveFile.Location = new System.Drawing.Point(7, 42);
            this.buttonSaveFile.Name = "buttonSaveFile";
            this.buttonSaveFile.Size = new System.Drawing.Size(97, 23);
            this.buttonSaveFile.TabIndex = 1;
            this.buttonSaveFile.Text = "Save File Dialog";
            this.buttonSaveFile.UseVisualStyleBackColor = true;
            this.buttonSaveFile.Click += new System.EventHandler(this.buttonSaveFile_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(111, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(263, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(111, 44);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(263, 20);
            this.textBox2.TabIndex = 3;
            // 
            // buttonPerformSaving
            // 
            this.buttonPerformSaving.Location = new System.Drawing.Point(410, 40);
            this.buttonPerformSaving.Name = "buttonPerformSaving";
            this.buttonPerformSaving.Size = new System.Drawing.Size(109, 23);
            this.buttonPerformSaving.TabIndex = 4;
            this.buttonPerformSaving.Text = "Perform Saving";
            this.buttonPerformSaving.UseVisualStyleBackColor = true;
            this.buttonPerformSaving.Click += new System.EventHandler(this.buttonPerformSaving_Click);
            // 
            // userControlForPaint1
            // 
            this.userControlForPaint1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlForPaint1.AutoScroll = true;
            this.userControlForPaint1.BackColor = System.Drawing.Color.White;
            this.userControlForPaint1.Location = new System.Drawing.Point(0, 0);
            this.userControlForPaint1.Name = "userControlForPaint1";
            this.userControlForPaint1.Size = new System.Drawing.Size(797, 468);
            this.userControlForPaint1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 544);
            this.Controls.Add(this.groupBoxSaveControls);
            this.Controls.Add(this.userControlForPaint1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBoxSaveControls.ResumeLayout(false);
            this.groupBoxSaveControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private UserControlForPaint userControlForPaint1;
        private System.Windows.Forms.GroupBox groupBoxSaveControls;
        private System.Windows.Forms.Button buttonPerformSaving;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonSaveFile;
        private System.Windows.Forms.Button buttonOpenFile;
    }
}

