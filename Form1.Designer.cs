namespace sistema1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            btnReportes = new Button();
            btnTramites = new Button();
            btnInfracciones = new Button();
            btnPadron = new Button();
            btnDashboard = new Button();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            label1 = new Label();
            lblUsuario = new Label();
            panel3 = new Panel();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(106, 27, 41);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(btnReportes);
            panel1.Controls.Add(btnTramites);
            panel1.Controls.Add(btnInfracciones);
            panel1.Controls.Add(btnPadron);
            panel1.Controls.Add(btnDashboard);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(220, 720);
            panel1.TabIndex = 0;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(106, 27, 41);
            btnReportes.Dock = DockStyle.Top;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(0, 270);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(220, 45);
            btnReportes.TabIndex = 0;
            btnReportes.Text = "  REPORTES";
            btnReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += btnReportes_Click;
            // 
            // btnTramites
            // 
            btnTramites.BackColor = Color.FromArgb(106, 27, 41);
            btnTramites.Dock = DockStyle.Top;
            btnTramites.FlatAppearance.BorderSize = 0;
            btnTramites.FlatStyle = FlatStyle.Flat;
            btnTramites.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnTramites.ForeColor = Color.White;
            btnTramites.Location = new Point(0, 225);
            btnTramites.Name = "btnTramites";
            btnTramites.Size = new Size(220, 45);
            btnTramites.TabIndex = 1;
            btnTramites.Text = "  TRAMITES";
            btnTramites.TextAlign = ContentAlignment.MiddleLeft;
            btnTramites.UseVisualStyleBackColor = false;
            btnTramites.Click += btnTramites_Click;
            // 
            // btnInfracciones
            // 
            btnInfracciones.BackColor = Color.FromArgb(106, 27, 41);
            btnInfracciones.Dock = DockStyle.Top;
            btnInfracciones.FlatAppearance.BorderSize = 0;
            btnInfracciones.FlatStyle = FlatStyle.Flat;
            btnInfracciones.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnInfracciones.ForeColor = Color.White;
            btnInfracciones.Location = new Point(0, 180);
            btnInfracciones.Name = "btnInfracciones";
            btnInfracciones.Size = new Size(220, 45);
            btnInfracciones.TabIndex = 2;
            btnInfracciones.Text = "  INFRACCIONES";
            btnInfracciones.TextAlign = ContentAlignment.MiddleLeft;
            btnInfracciones.UseVisualStyleBackColor = false;
            btnInfracciones.Click += btnInfracciones_Click;
            // 
            // btnPadron
            // 
            btnPadron.BackColor = Color.FromArgb(106, 27, 41);
            btnPadron.Dock = DockStyle.Top;
            btnPadron.FlatAppearance.BorderSize = 0;
            btnPadron.FlatStyle = FlatStyle.Flat;
            btnPadron.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnPadron.ForeColor = Color.White;
            btnPadron.Location = new Point(0, 135);
            btnPadron.Name = "btnPadron";
            btnPadron.Size = new Size(220, 45);
            btnPadron.TabIndex = 3;
            btnPadron.Text = "  PADRON VEHICULAR";
            btnPadron.TextAlign = ContentAlignment.MiddleLeft;
            btnPadron.UseVisualStyleBackColor = false;
            btnPadron.Click += btnPadron_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(106, 27, 41);
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(0, 90);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(220, 45);
            btnDashboard.TabIndex = 4;
            btnDashboard.Text = "  DASHBOARD GENERAL";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 90);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.PeachPuff;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(lblUsuario);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(220, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1060, 60);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Variable Text", 14F, FontStyle.Bold);
            label1.Location = new Point(20, 12);
            label1.Name = "label1";
            label1.Size = new Size(180, 37);
            label1.TabIndex = 0;
            label1.Text = "Bienvenido, ";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI Variable Text", 14F, FontStyle.Bold);
            lblUsuario.Location = new Point(160, 12);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(117, 37);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(244, 246, 249);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(220, 60);
            panel3.Name = "panel3";
            panel3.Size = new Size(1060, 660);
            panel3.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.InitialImage = null;
            pictureBox2.Location = new Point(47, 9);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(118, 81);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            ClientSize = new Size(1280, 720);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Size(1000, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Control Vehicular";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnPadron;
        private System.Windows.Forms.Button btnInfracciones;
        private System.Windows.Forms.Button btnTramites;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label label1;
        private PictureBox pictureBox2;
    }
}