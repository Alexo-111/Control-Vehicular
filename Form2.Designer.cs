namespace sistema1
{
    partial class Form2
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
            panel1 = new Panel();
            panel2 = new Panel();
            btnIngresar = new Button();
            txtPassword = new TextBox();
            rbOficial = new RadioButton();
            rbCivil = new RadioButton();
            txtUsuario = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(106, 27, 41);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1270, 490);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.PeachPuff;
            panel2.Controls.Add(btnIngresar);
            panel2.Controls.Add(txtPassword);
            panel2.Controls.Add(rbOficial);
            panel2.Controls.Add(rbCivil);
            panel2.Controls.Add(txtUsuario);
            panel2.Location = new Point(462, 143);
            panel2.Name = "panel2";
            panel2.Size = new Size(390, 314);
            panel2.TabIndex = 5;
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.FromArgb(106, 27, 41);
            btnIngresar.ForeColor = Color.MistyRose;
            btnIngresar.Location = new Point(140, 240);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(112, 34);
            btnIngresar.TabIndex = 6;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.Silver;
            txtPassword.Location = new Point(67, 100);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Contraseña";
            txtPassword.Size = new Size(254, 31);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // rbOficial
            // 
            rbOficial.AutoSize = true;
            rbOficial.BackColor = Color.Transparent;
            rbOficial.ForeColor = SystemColors.ActiveCaptionText;
            rbOficial.Location = new Point(67, 188);
            rbOficial.Name = "rbOficial";
            rbOficial.Size = new Size(86, 29);
            rbOficial.TabIndex = 2;
            rbOficial.TabStop = true;
            rbOficial.Text = "Oficial";
            rbOficial.UseVisualStyleBackColor = false;
            // 
            // rbCivil
            // 
            rbCivil.AutoSize = true;
            rbCivil.ForeColor = SystemColors.ActiveCaptionText;
            rbCivil.Location = new Point(252, 188);
            rbCivil.Name = "rbCivil";
            rbCivil.Size = new Size(69, 29);
            rbCivil.TabIndex = 3;
            rbCivil.TabStop = true;
            rbCivil.Text = "Civil";
            rbCivil.UseVisualStyleBackColor = true;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.Silver;
            txtUsuario.Location = new Point(67, 41);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "Usuario";
            txtUsuario.Size = new Size(254, 31);
            txtUsuario.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoEllipsis = true;
            label2.AutoSize = true;
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Palatino Linotype", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.NavajoWhite;
            label2.Location = new Point(462, 85);
            label2.Name = "label2";
            label2.Size = new Size(376, 55);
            label2.TabIndex = 1;
            label2.Text = "Seleccione su perfil";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // label1
            // 
            label1.AutoEllipsis = true;
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Palatino Linotype", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.NavajoWhite;
            label1.Location = new Point(337, 25);
            label1.Name = "label1";
            label1.Size = new Size(617, 60);
            label1.TabIndex = 0;
            label1.Text = "Sistema de Control Vehicular";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1270, 490);
            Controls.Add(panel1);
            Name = "Form2";
            Text = "LoginForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Label label2;
        private RadioButton rbCivil;
        private RadioButton rbOficial;
        private TextBox txtUsuario;
        private Panel panel2;
        private Button btnIngresar;
        private TextBox txtPassword;
    }
}