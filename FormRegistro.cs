using System;
using System.Drawing;
using System.Windows.Forms;

namespace sistema1
{
    public partial class FormRegistro : Form
    {
        private TextBox txtUsuario = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmar = null!;
        private RadioButton rbOficial = null!;
        private RadioButton rbCivil = null!;

        // Para que el login pueda precargar el usuario recién creado
        public string UsuarioCreado { get; private set; } = string.Empty;

        public FormRegistro()
        {
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            this.Text = "Registro de Cuenta";
            this.Size = new Size(440, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.PeachPuff;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(15)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 4; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));

            txtUsuario = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F) };
            txtPassword = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), UseSystemPasswordChar = true };
            txtConfirmar = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), UseSystemPasswordChar = true };

            rbOficial = new RadioButton { Text = "Oficial", AutoSize = true, Anchor = AnchorStyles.Left };
            rbCivil = new RadioButton { Text = "Civil", AutoSize = true, Anchor = AnchorStyles.Left, Checked = true };
            FlowLayoutPanel panelRol = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            panelRol.Controls.Add(rbOficial);
            panelRol.Controls.Add(rbCivil);

            FlowLayoutPanel panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 0, 0)
            };

            Button btnCrear = new Button
            {
                Text = "Crear cuenta",
                BackColor = Color.FromArgb(106, 27, 41),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(120, 35),
                Cursor = Cursors.Hand
            };
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.Click += BtnCrear_Click;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(100, 35),
                Margin = new Padding(0, 0, 10, 0),
                DialogResult = DialogResult.Cancel,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            panelBotones.Controls.Add(btnCrear);
            panelBotones.Controls.Add(btnCancelar);

            layout.Controls.Add(CrearLabel("Usuario:"), 0, 0);
            layout.Controls.Add(txtUsuario, 1, 0);
            layout.Controls.Add(CrearLabel("Contraseña:"), 0, 1);
            layout.Controls.Add(txtPassword, 1, 1);
            layout.Controls.Add(CrearLabel("Confirmar:"), 0, 2);
            layout.Controls.Add(txtConfirmar, 1, 2);
            layout.Controls.Add(CrearLabel("Tipo de cuenta:"), 0, 3);
            layout.Controls.Add(panelRol, 1, 3);
            layout.Controls.Add(panelBotones, 1, 4);

            this.Controls.Add(layout);
            this.AcceptButton = btnCrear;
            this.CancelButton = btnCancelar;
        }

        private Label CrearLabel(string texto)
        {
            return new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            };
        }

        private void BtnCrear_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmar.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmar.Clear();
                txtConfirmar.Focus();
                return;
            }

            string rol = rbOficial.Checked ? "Oficial" : "Civil";

            if (ServicioCuentas.Registrar(txtUsuario.Text, txtPassword.Text, rol, out string mensaje))
            {
                UsuarioCreado = txtUsuario.Text.Trim();
                MessageBox.Show(mensaje, "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(mensaje, "No se pudo registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}