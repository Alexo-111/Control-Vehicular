using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MinimumSize = new Size(1000, 600);
            CrearBotonesCuenta();
        }

        // Indica que Form1 se cierra por "Cerrar sesión" (y no por la X de la ventana)
        private bool _cerrandoSesion = false;

        // NUEVO: botones "MI CUENTA" y "CERRAR SESIÓN" al fondo del menú lateral.
        // El último control añadido se acopla más abajo, por eso "Cerrar sesión" va al final.
        private void CrearBotonesCuenta()
        {
            Button btnMiCuenta = CrearBotonMenuInferior("  MI CUENTA", Color.FromArgb(106, 27, 41));
            btnMiCuenta.Click += BtnMiCuenta_Click;
            panel1.Controls.Add(btnMiCuenta);

            Button btnCerrarSesion = CrearBotonMenuInferior("  CERRAR SESIÓN", Color.FromArgb(80, 18, 30));
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            panel1.Controls.Add(btnCerrarSesion);
        }

        private Button CrearBotonMenuInferior(string texto, Color fondo)
        {
            Button btn = new Button
            {
                Text = texto,
                Dock = DockStyle.Bottom,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = fondo,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void BtnMiCuenta_Click(object? sender, EventArgs e)
        {
            using (FormCuenta dlg = new FormCuenta())
            {
                dlg.ShowDialog(this);

                if (dlg.Accion == AccionCuenta.CerrarSesion || dlg.Accion == AccionCuenta.CuentaEliminada)
                {
                    CerrarSesion();
                    return;
                }

                if (dlg.UsuarioCambiado)
                {
                    // Refresca el saludo y el dashboard con el nuevo nombre
                    AlinearSaludo();
                    btnDashboard_Click(this, EventArgs.Empty);
                }
            }
        }

        private void BtnCerrarSesion_Click(object? sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Desea cerrar la sesión actual?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                CerrarSesion();
            }
        }

        // Cierra la sesión sin tocar las cuentas guardadas: solo limpia quién está conectado
        private void CerrarSesion()
        {
            _cerrandoSesion = true;

            Sesion.Usuario = string.Empty;
            Sesion.Rol = string.Empty;

            Form2? login = Application.OpenForms.OfType<Form2>().FirstOrDefault();
            if (login != null)
            {
                login.ReiniciarLogin();
                login.Show();
                login.Activate();
            }

            this.Close();
        }

        // Si se cierra Form1 con la X, se termina la aplicación completa
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (!_cerrandoSesion)
            {
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AlinearSaludo();

            // Carga automáticamente el Dashboard al abrir Form1
            btnDashboard_Click(this, EventArgs.Empty);
        }

        private void AlinearSaludo()
        {
            // Asigna el texto del usuario
            lblUsuario.Text = string.IsNullOrEmpty(Sesion.Usuario) ? "admin" : Sesion.Usuario;

            // Posiciona lblUsuario inmediatamente después de label1 ("Bienvenido, ")
            label1.Left = 20;
            label1.Top = 15;

            lblUsuario.Left = label1.Right + 5; // Evita que se encima o recorte
            lblUsuario.Top = 15;
            lblUsuario.AutoSize = true;
        }

        // Método auxiliar para cambiar controles en el panel3
        private void AbrirUserControl(UserControl uc)
        {
            panel3.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panel3.Controls.Add(uc);
            uc.BringToFront();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase))
            {
                var ucOficial = new UcDashboardOficial();
                // ucOficial.ActualizarEstadisticas();
                AbrirUserControl(ucOficial);
            }
            else
            {
                var ucCivil = new UcDashboardCivil();

                // Suscribir los eventos del Dashboard para redireccionar a sus respectivas pantallas
                ucCivil.SolicitadoPagarInfraccion += (s, ev) =>
                {
                    btnInfracciones_Click(this, EventArgs.Empty);
                };

                ucCivil.SolicitadoRenovarLicencia += (s, ev) =>
                {
                    btnTramites_Click(this, EventArgs.Empty);
                };

                ucCivil.SolicitadoLevantarReporte += (s, ev) =>
                {
                    btnReportes_Click(this, EventArgs.Empty);
                };

                ucCivil.ActualizarEstadisticas(); // Forzamos la actualización explícita
                AbrirUserControl(ucCivil);
            }
        }

        private void btnPadron_Click(object sender, EventArgs e)
        {
            if (Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase))
            {
                AbrirUserControl(new UcPadronVehicular());
            }
            else
            {
                AbrirUserControl(new UcPadronVehicularCivil());
            }
        }

        private void btnInfracciones_Click(object sender, EventArgs e)
        {
            if (Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase))
            {
                AbrirUserControl(new UcInfracciones());
            }
            else
            {
                AbrirUserControl(new UcInfraccionesCivil());
            }
        }

        private void btnTramites_Click(object sender, EventArgs e)
        {
            if (Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase))
            {
                AbrirUserControl(new UcLicencias());
            }
            else
            {
                AbrirUserControl(new UcTramitesCivil());
            }
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            if (Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase))
            {
                AbrirUserControl(new UcReportesOficial());
            }
            else
            {
                AbrirUserControl(new UcReportesCiudadano());
            }
        }
    }
}