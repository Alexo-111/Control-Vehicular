using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcTramitesCivil : UserControl
    {
        private ComboBox cmbTipoTramite = null!;
        private TextBox txtDetalles = null!;
        private Button btnSolicitar = null!;
        private DataGridView dgvMisTramites = null!;

        public UcTramitesCivil()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(240, 243, 246);

            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            this.Controls.Clear();

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(15)
            };

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblTitulo = new Label
            {
                Text = "📄 Solicitud y Gestión de Trámites",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainLayout.SetColumnSpan(lblTitulo, 2);

            // Panel Izquierdo: Formulario
            GroupBox gbxFormulario = new GroupBox
            {
                Text = " Solicitar Nuevo Trámite ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            TableLayoutPanel layoutForm = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(5)
            };

            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

            Label lblTipo = new Label { Text = "Tipo de Trámite:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Black };
            cmbTipoTramite = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Dock = DockStyle.Fill
            };
            cmbTipoTramite.Items.AddRange(new object[] {
                "Renovación de Licencia de Conducir",
                "Expedición de Licencia Nueva",
                "Permiso Temporal de Conducción",
                "Constancia de No Infracción",
                "Aclaración / Disputa de Infracción"
            });
            cmbTipoTramite.SelectedIndex = 0;

            Label lblDetalles = new Label { Text = "Notas / Folio Ref:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Black };
            txtDetalles = new TextBox { Multiline = true, Font = new Font("Segoe UI", 9.5F), Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical };

            btnSolicitar = new Button
            {
                Text = "Iniciar Trámite",
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand
            };
            btnSolicitar.FlatAppearance.BorderSize = 0;
            btnSolicitar.Click += BtnSolicitar_Click;

            layoutForm.Controls.Add(lblTipo, 0, 0);
            layoutForm.Controls.Add(cmbTipoTramite, 1, 0);
            layoutForm.Controls.Add(lblDetalles, 0, 1);
            layoutForm.Controls.Add(txtDetalles, 1, 1);
            layoutForm.Controls.Add(btnSolicitar, 1, 3);

            gbxFormulario.Controls.Add(layoutForm);

            // Panel Derecho: Tabla
            GroupBox gbxHistorial = new GroupBox
            {
                Text = " Mis Trámites Registrados ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            dgvMisTramites = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 32,
                RowTemplate = { Height = 28 }
            };

            dgvMisTramites.EnableHeadersVisualStyles = false;
            dgvMisTramites.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvMisTramites.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMisTramites.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            gbxHistorial.Controls.Add(dgvMisTramites);

            mainLayout.Controls.Add(lblTitulo, 0, 0);
            mainLayout.Controls.Add(gbxFormulario, 0, 1);
            mainLayout.Controls.Add(gbxHistorial, 1, 1);

            this.Controls.Add(mainLayout);

            CargarHistorialTramites();
        }

        private void BtnSolicitar_Click(object? sender, EventArgs e)
        {
            string tipo = cmbTipoTramite.SelectedItem?.ToString() ?? "Trámite General";
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;

            // Generar folio de trámite (ej. TRM-2024-105)
            int consecutivo = DatosFicticios.Tramites.Count + 101;
            string nuevoFolio = $"TRM-2024-{consecutivo:D3}";

            decimal costo = 0.00m;
            if (tipo.Contains("Renovación")) costo = 650.00m;
            else if (tipo.Contains("Nueva")) costo = 950.00m;
            else if (tipo.Contains("Permiso")) costo = 350.00m;

            // Guardar en la lista global de trámites
            Tramite nuevoTramite = new Tramite
            {
                Folio = nuevoFolio,
                TipoTramite = tipo,
                Solicitante = usuarioActual,
                Costo = costo,
                Estado = "En Proceso",
                EstadoPago = costo > 0 ? "Pendiente" : "N/A",
                FechaSolicitud = DateTime.Now
            };
            DatosFicticios.Tramites.Add(nuevoTramite);

            MessageBox.Show($"¡Trámite registrado exitosamente!\n\nFolio asignado: {nuevoFolio}\nEstado: En Proceso", "Trámite Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtDetalles.Clear();

            // Refrescar la tabla local inmediatamente
            CargarHistorialTramites();
        }

        private void CargarHistorialTramites()
        {
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;

            // Obtener todos los trámites del usuario (excluyendo o incluyendo según se requiera)
            var misTramites = DatosFicticios.Tramites
                .Where(t => string.IsNullOrEmpty(Sesion.Usuario) || t.Solicitante.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(t => t.FechaSolicitud)
                .Select(t => new
                {
                    Folio = t.Folio,
                    Trámite = t.TipoTramite,
                    Costo = t.Costo > 0 ? $"${t.Costo:F2}" : "Gratuito",
                    Estado = t.Estado,
                    Pago = t.EstadoPago,
                    Fecha = t.FechaSolicitud.ToString("dd/MM/yyyy HH:mm")
                }).ToList();

            if (dgvMisTramites != null)
            {
                dgvMisTramites.DataSource = null;
                dgvMisTramites.DataSource = misTramites;
            }
        }
    }
}