using System;
using System.Drawing;
using System.Windows.Forms;

namespace sistema1
{
    public enum AccionCuenta
    {
        Ninguna,
        CerrarSesion,
        CuentaEliminada
    }

    public partial class FormCuenta : Form
    {
        private Label lblEncabezado = null!;

        // Pestaña: cambiar usuario
        private TextBox txtNuevoUsuario = null!;
        private TextBox txtPassParaUsuario = null!;

        // Pestaña: cambiar contraseña
        private TextBox txtPassActual = null!;
        private TextBox txtPassNueva = null!;
        private TextBox txtPassConfirmar = null!;

        // Pestaña: eliminar cuenta
        private TextBox txtPassEliminar = null!;

        // Resultado que consulta Form1 al cerrarse este diálogo
        public AccionCuenta Accion { get; private set; } = AccionCuenta.Ninguna;
        public bool UsuarioCambiado { get; private set; } = false;

        public FormCuenta()
        {
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            this.Text = "Mi cuenta";
            this.Size = new Size(480, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            TableLayoutPanel main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            lblEncabezado = new Label
            {
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            ActualizarEncabezado();

            TabControl tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            TabPage tabUsuario = new TabPage("Cambiar usuario") { BackColor = Color.White };
            TabPage tabPassword = new TabPage("Cambiar contraseña") { BackColor = Color.White };
            TabPage tabEliminar = new TabPage("Eliminar cuenta") { BackColor = Color.White };

            ConstruirTabUsuario(tabUsuario);
            ConstruirTabPassword(tabPassword);
            ConstruirTabEliminar(tabEliminar);

            tabs.TabPages.Add(tabUsuario);
            tabs.TabPages.Add(tabPassword);
            tabs.TabPages.Add(tabEliminar);

            // Barra inferior con "Cerrar sesión"
            FlowLayoutPanel barra = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 0, 0)
            };

            Button btnCerrarSesion = CrearBoton("Cerrar sesión", Color.FromArgb(80, 18, 30), 140);
            btnCerrarSesion.Click += (s, e) =>
            {
                Accion = AccionCuenta.CerrarSesion;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            Button btnCerrar = CrearBoton("Cerrar ventana", Color.Gray, 130);
            btnCerrar.Margin = new Padding(0, 0, 10, 0);
            btnCerrar.Click += (s, e) => this.Close();

            barra.Controls.Add(btnCerrarSesion);
            barra.Controls.Add(btnCerrar);

            main.Controls.Add(lblEncabezado, 0, 0);
            main.Controls.Add(tabs, 0, 1);
            main.Controls.Add(barra, 0, 2);
            this.Controls.Add(main);
        }

        private void ActualizarEncabezado()
        {
            lblEncabezado.Text = $"Cuenta: {Sesion.Usuario}  ({Sesion.Rol})";
        }

        // ---------- Pestañas ----------

        private void ConstruirTabUsuario(TabPage tab)
        {
            TableLayoutPanel layout = CrearLayoutTab(3);

            txtNuevoUsuario = CrearTextBox(false);
            txtPassParaUsuario = CrearTextBox(true);

            Button btn = CrearBoton("Cambiar usuario", Color.FromArgb(41, 128, 185), 170);
            btn.Click += BtnCambiarUsuario_Click;

            layout.Controls.Add(CrearLabel("Nuevo usuario:"), 0, 0);
            layout.Controls.Add(txtNuevoUsuario, 1, 0);
            layout.Controls.Add(CrearLabel("Contraseña actual:"), 0, 1);
            layout.Controls.Add(txtPassParaUsuario, 1, 1);
            layout.Controls.Add(btn, 1, 2);

            tab.Controls.Add(layout);
        }

        private void ConstruirTabPassword(TabPage tab)
        {
            TableLayoutPanel layout = CrearLayoutTab(4);

            txtPassActual = CrearTextBox(true);
            txtPassNueva = CrearTextBox(true);
            txtPassConfirmar = CrearTextBox(true);

            Button btn = CrearBoton("Cambiar contraseña", Color.FromArgb(230, 126, 34), 170);
            btn.Click += BtnCambiarPassword_Click;

            layout.Controls.Add(CrearLabel("Contraseña actual:"), 0, 0);
            layout.Controls.Add(txtPassActual, 1, 0);
            layout.Controls.Add(CrearLabel("Nueva contraseña:"), 0, 1);
            layout.Controls.Add(txtPassNueva, 1, 1);
            layout.Controls.Add(CrearLabel("Confirmar nueva:"), 0, 2);
            layout.Controls.Add(txtPassConfirmar, 1, 2);
            layout.Controls.Add(btn, 1, 3);

            tab.Controls.Add(layout);
        }

        private void ConstruirTabEliminar(TabPage tab)
        {
            TableLayoutPanel layout = CrearLayoutTab(3);
            layout.RowStyles[0] = new RowStyle(SizeType.Absolute, 75F);

            Label lblAviso = new Label
            {
                Text = "Al eliminar tu cuenta ya no podrás iniciar sesión con ella. Esta acción no se puede deshacer.",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                Dock = DockStyle.Fill
            };
            layout.Controls.Add(lblAviso, 0, 0);
            layout.SetColumnSpan(lblAviso, 2);

            txtPassEliminar = CrearTextBox(true);

            Button btn = CrearBoton("Eliminar mi cuenta", Color.FromArgb(192, 57, 43), 170);
            btn.Click += BtnEliminar_Click;

            layout.Controls.Add(CrearLabel("Contraseña:"), 0, 1);
            layout.Controls.Add(txtPassEliminar, 1, 1);
            layout.Controls.Add(btn, 1, 2);

            tab.Controls.Add(layout);
        }

        // ---------- Eventos ----------

        private void BtnCambiarUsuario_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNuevoUsuario.Text) || string.IsNullOrEmpty(txtPassParaUsuario.Text))
            {
                MessageBox.Show("Ingrese el nuevo usuario y su contraseña actual.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ServicioCuentas.CambiarUsuario(Sesion.Usuario, txtPassParaUsuario.Text, txtNuevoUsuario.Text, out string mensaje))
            {
                Sesion.Usuario = txtNuevoUsuario.Text.Trim();
                UsuarioCambiado = true;
                ActualizarEncabezado();

                txtNuevoUsuario.Clear();
                txtPassParaUsuario.Clear();
                MessageBox.Show(mensaje, "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensaje, "No se pudo cambiar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCambiarPassword_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassActual.Text) || string.IsNullOrEmpty(txtPassNueva.Text))
            {
                MessageBox.Show("Complete todos los campos.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassNueva.Text != txtPassConfirmar.Text)
            {
                MessageBox.Show("La nueva contraseña y su confirmación no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassConfirmar.Clear();
                txtPassConfirmar.Focus();
                return;
            }

            if (ServicioCuentas.CambiarPassword(Sesion.Usuario, txtPassActual.Text, txtPassNueva.Text, out string mensaje))
            {
                txtPassActual.Clear();
                txtPassNueva.Clear();
                txtPassConfirmar.Clear();
                MessageBox.Show(mensaje, "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensaje, "No se pudo cambiar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassEliminar.Text))
            {
                MessageBox.Show("Ingrese su contraseña para confirmar.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmar = MessageBox.Show(
                $"¿Seguro que desea eliminar la cuenta '{Sesion.Usuario}'?\nEsta acción no se puede deshacer.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirmar != DialogResult.Yes) return;

            if (ServicioCuentas.Eliminar(Sesion.Usuario, txtPassEliminar.Text, out string mensaje))
            {
                Accion = AccionCuenta.CuentaEliminada;
                MessageBox.Show(mensaje, "Cuenta eliminada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(mensaje, "No se pudo eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassEliminar.Clear();
            }
        }

        // ---------- Utilidades de diseño ----------

        private TableLayoutPanel CrearLayoutTab(int filas)
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = filas,
                Padding = new Padding(10)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < filas; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            return layout;
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

        private TextBox CrearTextBox(bool esPassword)
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                UseSystemPasswordChar = esPassword
            };
        }

        private Button CrearBoton(string texto, Color fondo, int ancho)
        {
            Button btn = new Button
            {
                Text = texto,
                BackColor = fondo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(ancho, 35),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
    }
}