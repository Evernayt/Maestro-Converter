namespace Maestro_Converter
{
    partial class fmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
            pbxInfo = new PictureBox();
            lblInfo = new Label();
            lblExtensions = new Label();
            btnCreateToggle = new Button();
            ((System.ComponentModel.ISupportInitialize)pbxInfo).BeginInit();
            SuspendLayout();
            // 
            // pbxInfo
            // 
            pbxInfo.Image = Properties.Resources.success;
            pbxInfo.Location = new Point(178, 45);
            pbxInfo.Name = "pbxInfo";
            pbxInfo.Size = new Size(100, 100);
            pbxInfo.TabIndex = 0;
            pbxInfo.TabStop = false;
            // 
            // lblInfo
            // 
            lblInfo.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblInfo.Location = new Point(12, 158);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(433, 25);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Пункт меню «Конвертировать» добавлен";
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblExtensions
            // 
            lblExtensions.ForeColor = Color.DimGray;
            lblExtensions.Location = new Point(12, 189);
            lblExtensions.Name = "lblExtensions";
            lblExtensions.Size = new Size(433, 20);
            lblExtensions.TabIndex = 2;
            lblExtensions.Text = "JPG, JPEG, PNG, WEBP, HEIC, BMP, TIFF";
            lblExtensions.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnCreateToggle
            // 
            btnCreateToggle.BackColor = Color.Firebrick;
            btnCreateToggle.FlatAppearance.BorderSize = 0;
            btnCreateToggle.FlatStyle = FlatStyle.Flat;
            btnCreateToggle.ForeColor = Color.White;
            btnCreateToggle.Location = new Point(135, 252);
            btnCreateToggle.Name = "btnCreateToggle";
            btnCreateToggle.Size = new Size(187, 33);
            btnCreateToggle.TabIndex = 3;
            btnCreateToggle.Text = "Убрать пункт меню";
            btnCreateToggle.UseVisualStyleBackColor = false;
            btnCreateToggle.Click += btnCreateToggle_Click;
            // 
            // fmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(457, 316);
            Controls.Add(btnCreateToggle);
            Controls.Add(lblExtensions);
            Controls.Add(lblInfo);
            Controls.Add(pbxInfo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "fmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Maestro Converter";
            ((System.ComponentModel.ISupportInitialize)pbxInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbxInfo;
        private Label lblInfo;
        private Label lblExtensions;
        private Button btnCreateToggle;
    }
}