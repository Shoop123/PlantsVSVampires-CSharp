namespace ElliotView
{
    partial class ElliotGameView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElliotGameView));
            this.lblMoons = new System.Windows.Forms.Label();
            this.lblHealth = new System.Windows.Forms.Label();
            this.lblWave = new System.Windows.Forms.Label();
            this.lblLost = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMoons
            // 
            this.lblMoons.AutoSize = true;
            this.lblMoons.BackColor = System.Drawing.Color.Transparent;
            this.lblMoons.Font = new System.Drawing.Font("Champagne & Limousines", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoons.Location = new System.Drawing.Point(216, 30);
            this.lblMoons.Name = "lblMoons";
            this.lblMoons.Size = new System.Drawing.Size(200, 50);
            this.lblMoons.TabIndex = 0;
            this.lblMoons.Text = "Moons: 0";
            // 
            // lblHealth
            // 
            this.lblHealth.AutoSize = true;
            this.lblHealth.BackColor = System.Drawing.Color.Transparent;
            this.lblHealth.Font = new System.Drawing.Font("Champagne & Limousines", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHealth.Location = new System.Drawing.Point(955, 24);
            this.lblHealth.Name = "lblHealth";
            this.lblHealth.Size = new System.Drawing.Size(259, 67);
            this.lblHealth.TabIndex = 1;
            this.lblHealth.Text = "Health: 0";
            // 
            // lblWave
            // 
            this.lblWave.AutoSize = true;
            this.lblWave.BackColor = System.Drawing.Color.Transparent;
            this.lblWave.Font = new System.Drawing.Font("Champagne & Limousines", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWave.Location = new System.Drawing.Point(1642, 30);
            this.lblWave.Name = "lblWave";
            this.lblWave.Size = new System.Drawing.Size(185, 50);
            this.lblWave.TabIndex = 2;
            this.lblWave.Text = "Wave: 0";
            // 
            // lblLost
            // 
            this.lblLost.AutoSize = true;
            this.lblLost.BackColor = System.Drawing.Color.Transparent;
            this.lblLost.Font = new System.Drawing.Font("Champagne & Limousines", 120F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLost.ForeColor = System.Drawing.Color.Transparent;
            this.lblLost.Location = new System.Drawing.Point(638, 414);
            this.lblLost.Name = "lblLost";
            this.lblLost.Size = new System.Drawing.Size(707, 169);
            this.lblLost.TabIndex = 3;
            this.lblLost.Text = "You Lose!";
            this.lblLost.Visible = false;
            // 
            // ElliotGameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.lblLost);
            this.Controls.Add(this.lblWave);
            this.Controls.Add(this.lblHealth);
            this.Controls.Add(this.lblMoons);
            this.DoubleBuffered = true;
            this.Name = "ElliotGameView";
            this.Text = "Plants vs. Vampires!";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ElliotGameView_FormClosing);
            this.Shown += new System.EventHandler(this.ElliotGameView_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ElliotGameView_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ElliotGameView_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ElliotGameView_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMoons;
        private System.Windows.Forms.Label lblHealth;
        private System.Windows.Forms.Label lblWave;
        private System.Windows.Forms.Label lblLost;
    }
}

