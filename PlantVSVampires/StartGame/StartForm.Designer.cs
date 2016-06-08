namespace StartGame
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.btnElliot = new System.Windows.Forms.Button();
            this.btnDaniel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.picElliot = new System.Windows.Forms.PictureBox();
            this.picDaniel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picElliot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDaniel)).BeginInit();
            this.SuspendLayout();
            // 
            // btnElliot
            // 
            this.btnElliot.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElliot.Location = new System.Drawing.Point(6, 265);
            this.btnElliot.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnElliot.Name = "btnElliot";
            this.btnElliot.Size = new System.Drawing.Size(210, 130);
            this.btnElliot.TabIndex = 0;
            this.btnElliot.Text = "Elliot\'s Game";
            this.btnElliot.UseVisualStyleBackColor = true;
            this.btnElliot.Click += new System.EventHandler(this.btnElliot_Click);
            // 
            // btnDaniel
            // 
            this.btnDaniel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDaniel.Location = new System.Drawing.Point(394, 265);
            this.btnDaniel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDaniel.Name = "btnDaniel";
            this.btnDaniel.Size = new System.Drawing.Size(210, 130);
            this.btnDaniel.TabIndex = 1;
            this.btnDaniel.Text = "Daniel\'s Game";
            this.btnDaniel.UseVisualStyleBackColor = true;
            this.btnDaniel.Click += new System.EventHandler(this.btnDaniel_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(52, 5);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(499, 73);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Choose a Game";
            // 
            // picElliot
            // 
            this.picElliot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picElliot.BackgroundImage")));
            this.picElliot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picElliot.Location = new System.Drawing.Point(6, 129);
            this.picElliot.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picElliot.Name = "picElliot";
            this.picElliot.Size = new System.Drawing.Size(210, 130);
            this.picElliot.TabIndex = 3;
            this.picElliot.TabStop = false;
            // 
            // picDaniel
            // 
            this.picDaniel.Image = global::StartGame.Properties.Resources.PVV_Screentshot;
            this.picDaniel.Location = new System.Drawing.Point(394, 129);
            this.picDaniel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picDaniel.Name = "picDaniel";
            this.picDaniel.Size = new System.Drawing.Size(210, 130);
            this.picDaniel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDaniel.TabIndex = 4;
            this.picDaniel.TabStop = false;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(610, 401);
            this.Controls.Add(this.picDaniel);
            this.Controls.Add(this.picElliot);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDaniel);
            this.Controls.Add(this.btnElliot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plants VS. Vampires";
            ((System.ComponentModel.ISupportInitialize)(this.picElliot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDaniel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnElliot;
        private System.Windows.Forms.Button btnDaniel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picElliot;
        private System.Windows.Forms.PictureBox picDaniel;
    }
}

