using System;
using System.Drawing;
using System.Windows.Forms;

namespace sistema1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            ConfigurarPantallaCentrada();
        }

        // NUEVO: deja el login limpio cuando se vuelve desde "Cerrar sesión"
        public void ReiniciarLogin()
        {
            txtUsuario.Clear();
            txtPassword.Clear();
            rbOficial.Checked = false;
            rbCivil.Checked = false;
            txtUsuario.Focus();
        }

        // NUEVO: enlace "Regístrate" que se coloca debajo de la tarjeta beige
        private LinkLabel CrearEnlaceRegistro()
        {
            LinkLabel lnkRegistro = new LinkLabel
            {
                Text = "¿No tienes cuenta? Regístrate",
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                LinkColor = Color.PeachPuff,
                ActiveLinkColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 12, 0, 0)
            };
            lnkRegistro.LinkClicked += (s, e) =>
            {
                using (FormRegistro dlg = new FormRegistro())
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        // Precarga el usuario recién creado y pide la contraseña
                        txtUsuario.Text = dlg.UsuarioCreado;
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            };
            return lnkRegistro;
        }

        private void ConfigurarPantallaCentrada()
        {
            // 1. Todo el formulario principal en color guinda
            this.BackColor = Color.FromArgb(106, 27, 41);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 500);

            // 2. Crear panel contenedor transparente para centrar de forma fluida
            TableLayoutPanel layoutCentrado = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                BackColor = Color.Transparent
            };

            layoutCentrado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layoutCentrado.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layoutCentrado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            layoutCentrado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layoutCentrado.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutCentrado.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // 3. Contenedor interno para agrupar los títulos y la tarjeta beige
            FlowLayoutPanel contenedorLogin = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None
            };

            if (label1 != null) // "Sistema de Control Vehicular"
            {
                label1.AutoSize = true;
                label1.ForeColor = Color.PeachPuff;
                label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
                label1.Margin = new Padding(0, 0, 0, 5);
                this.Controls.Remove(label1);
                contenedorLogin.Controls.Add(label1);
            }

            if (label2 != null) // "Seleccione su perfil"
            {
                label2.AutoSize = true;
                label2.ForeColor = Color.PeachPuff;
                label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                label2.Margin = new Padding(0, 0, 0, 15);
                this.Controls.Remove(label2);
                contenedorLogin.Controls.Add(label2);
            }

            if (panel1 != null)
            {
                // NUEVO: panel1 medía 1270x490 y desbordaba la ventana, ocultando
                // la parte inferior de la tarjeta (incluido el enlace de registro).
                // Lo ajustamos al tamaño de la tarjeta beige.
                panel2.Location = new Point(0, 0);
                panel1.Dock = DockStyle.None;
                panel1.Size = panel2.Size;

                this.Controls.Remove(panel1);
                panel1.Margin = new Padding(0);
                contenedorLogin.Controls.Add(panel1);
                contenedorLogin.Controls.Add(CrearEnlaceRegistro());
            }

            foreach (Control ctrl in contenedorLogin.Controls)
            {
                ctrl.Anchor = AnchorStyles.None;
            }

            layoutCentrado.Controls.Add(contenedorLogin, 1, 1);

            this.Controls.Clear();
            this.Controls.Add(layoutCentrado);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1. Validar nombre de usuario
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor, ingrese su nombre de usuario.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            // 2. Validar contraseña
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, ingrese su contraseña.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // 3. Validar selección de perfil
            if (!rbOficial.Checked && !rbCivil.Checked)
            {
                MessageBox.Show("Por favor, seleccione un perfil (Oficial o Civil).", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. NUEVO: verificar credenciales contra las cuentas registradas
            ResultadoLogin resultado = ServicioCuentas.Validar(txtUsuario.Text, txtPassword.Text, out Cuenta? cuenta);

            if (resultado == ResultadoLogin.UsuarioNoExiste)
            {
                MessageBox.Show("El usuario no existe. Regístrese para crear una cuenta.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
                return;
            }

            if (resultado == ResultadoLogin.PasswordIncorrecta || cuenta == null)
            {
                MessageBox.Show("La contraseña es incorrecta.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // 5. NUEVO: el perfil elegido debe coincidir con el tipo de cuenta
            string perfilElegido = rbOficial.Checked ? "Oficial" : "Civil";
            if (!cuenta.Rol.Equals(perfilElegido, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"Esta cuenta es de tipo {cuenta.Rol}. Seleccione el perfil correcto.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 6. Guardar datos en la sesión (usando los datos de la cuenta)
            Sesion.Usuario = cuenta.Usuario;
            Sesion.Rol = cuenta.Rol;

            // 7. Abrir ventana principal
            Form1 principal = new Form1();
            principal.Show();
            this.Hide();
        }
    }
}
