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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelScaleFactor = new System.Windows.Forms.Label();
            this.FolderBtn = new System.Windows.Forms.Button();
            this.listBoxDxfFiles = new System.Windows.Forms.ListBox();
            this.textBoxFolderPath = new System.Windows.Forms.TextBox();
            this.userControlForPaint1 = new DXFRenderingBitmap.UserControlForPaint();
            this.numericUpDownAngle = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownScaleFactor = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.numericUpDownScaleFactor);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownAngle);
            this.groupBox1.Controls.Add(this.labelScaleFactor);
            this.groupBox1.Controls.Add(this.FolderBtn);
            this.groupBox1.Controls.Add(this.listBoxDxfFiles);
            this.groupBox1.Controls.Add(this.textBoxFolderPath);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 479);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "All good things";
            // 
            // labelScaleFactor
            // 
            this.labelScaleFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelScaleFactor.AutoSize = true;
            this.labelScaleFactor.Location = new System.Drawing.Point(0, 446);
            this.labelScaleFactor.Name = "labelScaleFactor";
            this.labelScaleFactor.Size = new System.Drawing.Size(37, 26);
            this.labelScaleFactor.TabIndex = 4;
            this.labelScaleFactor.Text = "Scale \r\nfactor";
            // 
            // FolderBtn
            // 
            this.FolderBtn.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderBtn.Location = new System.Drawing.Point(207, 11);
            this.FolderBtn.Name = "FolderBtn";
            this.FolderBtn.Size = new System.Drawing.Size(46, 23);
            this.FolderBtn.TabIndex = 3;
            this.FolderBtn.Text = "Folder";
            this.FolderBtn.UseVisualStyleBackColor = true;
            this.FolderBtn.Click += new System.EventHandler(this.FolderBtn_Click);
            // 
            // listBoxDxfFiles
            // 
            this.listBoxDxfFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDxfFiles.FormattingEnabled = true;
            this.listBoxDxfFiles.Location = new System.Drawing.Point(0, 40);
            this.listBoxDxfFiles.Name = "listBoxDxfFiles";
            this.listBoxDxfFiles.Size = new System.Drawing.Size(253, 394);
            this.listBoxDxfFiles.TabIndex = 1;
            this.listBoxDxfFiles.SelectedValueChanged += new System.EventHandler(this.listBoxDxfFiles_SelectedValueChanged);
            // 
            // textBoxFolderPath
            // 
            this.textBoxFolderPath.Location = new System.Drawing.Point(0, 14);
            this.textBoxFolderPath.Name = "textBoxFolderPath";
            this.textBoxFolderPath.Size = new System.Drawing.Size(201, 20);
            this.textBoxFolderPath.TabIndex = 0;
            this.textBoxFolderPath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxFolderPath_KeyUp);
            // 
            // userControlForPaint1
            // 
            this.userControlForPaint1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlForPaint1.AutoScroll = true;
            this.userControlForPaint1.BackColor = System.Drawing.Color.White;
            this.userControlForPaint1.Location = new System.Drawing.Point(266, 1);
            this.userControlForPaint1.Name = "userControlForPaint1";
            this.userControlForPaint1.Size = new System.Drawing.Size(532, 479);
            this.userControlForPaint1.TabIndex = 3;
            // 
            // numericUpDownAngle
            // 
            this.numericUpDownAngle.Location = new System.Drawing.Point(168, 450);
            this.numericUpDownAngle.Name = "numericUpDownAngle";
            this.numericUpDownAngle.Size = new System.Drawing.Size(85, 20);
            this.numericUpDownAngle.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 446);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Rotation\r\nangle";
            // 
            // numericUpDownScaleFactor
            // 
            this.numericUpDownScaleFactor.Location = new System.Drawing.Point(36, 450);
            this.numericUpDownScaleFactor.Name = "numericUpDownScaleFactor";
            this.numericUpDownScaleFactor.Size = new System.Drawing.Size(73, 20);
            this.numericUpDownScaleFactor.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 481);
            this.Controls.Add(this.userControlForPaint1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleFactor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelScaleFactor;
        private System.Windows.Forms.Button FolderBtn;
        private System.Windows.Forms.ListBox listBoxDxfFiles;
        private System.Windows.Forms.TextBox textBoxFolderPath;
        private UserControlForPaint userControlForPaint1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownAngle;
        private System.Windows.Forms.NumericUpDown numericUpDownScaleFactor;
    }
}

