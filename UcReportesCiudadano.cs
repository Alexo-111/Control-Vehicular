using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcReportesCiudadano : UserControl
    {
        private ComboBox cmbTipoIncidente = null!;
        private TextBox txtUbicacion = null!;
        private TextBox txtPlacas = null!;
        private TextBox txtDescripcion = null!;
        private Button btnEnviar = null!;
        private DataGridView dgvMisReportes = null!;

        public UcReportesCiudadano()
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
                Text = "📢 Portal de Reportes y Denuncias Ciudadanas",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainLayout.SetColumnSpan(lblTitulo, 2);

            GroupBox gbxFormulario = new GroupBox
            {
                Text = " Registrar Nuevo Reporte ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            TableLayoutPanel layoutForm = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(5)
            };

            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

            Label lblTipo = new Label { Text = "Tipo Reporte:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Black };
            cmbTipoIncidente = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Dock = DockStyle.Fill
            };
            cmbTipoIncidente.Items.AddRange(new object[] {
                "Vehículo Mal Estacionado / Obstrucción",
                "Reporte de Vehículo Robado",
                "Vehículo Abandonado en Vía Pública",
                "Conducción Temeraria / Infracción",
                "Daño a Infraestructura Vial / Bache"
            });
            cmbTipoIncidente.SelectedIndex = 0;

            Label lblUbicacion = new Label { Text = "Ubicación / Calle:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Black };
            txtUbicacion = new TextBox { Font = new Font("Segoe UI", 9.5F), Dock = DockStyle.Fill };

            Label lblPlacas = new Label { Text = "Placas / Modelo:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Black };
            txtPlacas = new TextBox { Font = new Font("Segoe UI", 9.5F), Dock = DockStyle.Fill };

            Label lblDesc = new Label { Text = "Detalles / Notas:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Anchor = AnchorStyles.Left, AutoSize = true, ForeColor = Color.Black };
            txtDescripcion = new TextBox { Multiline = true, Font = new Font("Segoe UI", 9.5F), Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical };

            btnEnviar = new Button
            {
                Text = "Enviar Reporte",
                BackColor = Color.FromArgb(106, 27, 41),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand
            };
            btnEnviar.FlatAppearance.BorderSize = 0;
            btnEnviar.Click += BtnEnviar_Click;

            layoutForm.Controls.Add(lblTipo, 0, 0);
            layoutForm.Controls.Add(cmbTipoIncidente, 1, 0);
            layoutForm.Controls.Add(lblUbicacion, 0, 1);
            layoutForm.Controls.Add(txtUbicacion, 1, 1);
            layoutForm.Controls.Add(lblPlacas, 0, 2);
            layoutForm.Controls.Add(txtPlacas, 1, 2);
            layoutForm.Controls.Add(lblDesc, 0, 3);
            layoutForm.Controls.Add(txtDescripcion, 1, 3);
            layoutForm.Controls.Add(btnEnviar, 1, 4);

            gbxFormulario.Controls.Add(layoutForm);

            GroupBox gbxHistorial = new GroupBox
            {
                Text = " Mis Reportes Enviados ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            dgvMisReportes = new DataGridView
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

            dgvMisReportes.EnableHeadersVisualStyles = false;
            dgvMisReportes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvMisReportes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMisReportes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            gbxHistorial.Controls.Add(dgvMisReportes);

            mainLayout.Controls.Add(lblTitulo, 0, 0);
            mainLayout.Controls.Add(gbxFormulario, 0, 1);
            mainLayout.Controls.Add(gbxHistorial, 1, 1);

            this.Controls.Add(mainLayout);

            CargarHistorialReportes();
        }

        private void BtnEnviar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUbicacion.Text))
            {
                MessageBox.Show("Por favor, ingrese la ubicación o calle del incidente.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipo = cmbTipoIncidente.SelectedItem?.ToString() ?? "Reporte General";
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;
            int consecutivo = DatosFicticios.ReportesCiudadanos.Count + DatosFicticios.Tramites.Count + 101;
            string nuevoFolio = $"REP-2024-{consecutivo:D3}";

            // Guardar en ReportesCiudadanos
            ReporteCiudadano nuevoReporte = new ReporteCiudadano
            {
                Folio = nuevoFolio,
                TipoInfraccion = tipo,
                Ubicacion = txtUbicacion.Text.Trim(),
                Ciudadano = usuarioActual,
                Estado = "Pendiente Atender",
                FechaReporte = DateTime.Now
            };
            DatosFicticios.ReportesCiudadanos.Add(nuevoReporte);

            // Guardar en Tramites
            Tramite nuevoTramite = new Tramite
            {
                Folio = nuevoFolio,
                TipoTramite = tipo,
                Solicitante = usuarioActual,
                Costo = 0.00m,
                Estado = "Pendiente Atender",
                EstadoPago = "N/A",
                FechaSolicitud = DateTime.Now
            };
            DatosFicticios.Tramites.Add(nuevoTramite);

            MessageBox.Show($"¡Reporte registrado exitosamente!\n\nFolio asignado: {nuevoFolio}\nLas autoridades revisarán el incidente a la brevedad.", "Reporte Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtUbicacion.Clear();
            txtPlacas.Clear();
            txtDescripcion.Clear();

            CargarHistorialReportes();
        }

        private void CargarHistorialReportes()
        {
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;

            var misReportes1 = DatosFicticios.ReportesCiudadanos
                .Where(r => string.IsNullOrEmpty(Sesion.Usuario) || r.Ciudadano.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    Folio = r.Folio,
                    Tipo = r.TipoInfraccion,
                    Ubicacion = r.Ubicacion,
                    Estado = r.Estado,
                    Fecha = r.FechaReporte.ToString("dd/MM/yyyy HH:mm")
                }).ToList();

            var misReportes2 = DatosFicticios.Tramites
                .Where(t => t.Folio.StartsWith("REP-") && (string.IsNullOrEmpty(Sesion.Usuario) || t.Solicitante.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase)))
                .Select(t => new
                {
                    Folio = t.Folio,
                    Tipo = t.TipoTramite,
                    Ubicacion = "Registrado en Vía Pública",
                    Estado = t.Estado,
                    Fecha = t.FechaSolicitud.ToString("dd/MM/yyyy HH:mm")
                }).ToList();

            var listaFinal = misReportes1.Concat(misReportes2)
                .GroupBy(x => x.Folio)
                .Select(g => g.First())
                .OrderByDescending(x => x.Fecha)
                .ToList();

            if (dgvMisReportes != null)
            {
                dgvMisReportes.DataSource = null;
                dgvMisReportes.DataSource = listaFinal;
            }
        }
    }
}