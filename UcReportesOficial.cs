using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcReportesOficial : UserControl
    {
        private DataGridView dgvReportes = null!;
        private ComboBox cmbFiltroEstado = null!;
        private TextBox txtFolioSeleccionado = null!;
        private ComboBox cmbNuevoEstado = null!;
        private TextBox txtDetalleReporte = null!;

        public UcReportesOficial()
        {
            InitializeComponent();
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
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(15)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Barra de búsqueda/Filtro
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));   // Tabla de Reportes
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));   // Panel de Gestión / Atención

            // 1. Barra Superior de Filtros
            FlowLayoutPanel panelFiltro = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White,
                Padding = new Padding(8)
            };

            Label lblFiltro = new Label
            {
                Text = "Filtrar por Estado: ",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };

            cmbFiltroEstado = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Width = 180
            };
            cmbFiltroEstado.Items.AddRange(new string[] { "Todos", "Pendiente", "En Proceso", "Atendido", "Rechazado" });
            cmbFiltroEstado.SelectedIndex = 0;
            cmbFiltroEstado.SelectedIndexChanged += (s, e) => CargarTablaReportes();

            panelFiltro.Controls.Add(lblFiltro);
            panelFiltro.Controls.Add(cmbFiltroEstado);

            // 2. Tabla de Reportes
            dgvReportes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 28 }
            };

            dgvReportes.EnableHeadersVisualStyles = false;
            dgvReportes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvReportes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReportes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvReportes.SelectionChanged += DgvReportes_SelectionChanged;

            // 3. Panel Inferior de Atención de Reportes
            GroupBox grpGestion = new GroupBox
            {
                Text = "Atención y Seguimiento de Denuncia Ciudadana",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            TableLayoutPanel layoutGestion = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2,
                Padding = new Padding(5)
            };

            layoutGestion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layoutGestion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layoutGestion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layoutGestion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            txtFolioSeleccionado = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };
            cmbNuevoEstado = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbNuevoEstado.Items.AddRange(new string[] { "Pendiente", "En Proceso", "Atendido", "Rechazado" });

            txtDetalleReporte = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, Height = 50 };

            Button btnActualizar = new Button
            {
                Text = " Actualizar Estado",
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Height = 35,
                Cursor = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += BtnActualizar_Click;

            layoutGestion.Controls.Add(CrearLabel("Folio Reporte:"), 0, 0);
            layoutGestion.Controls.Add(txtFolioSeleccionado, 1, 0);
            layoutGestion.Controls.Add(CrearLabel("Cambiar Estado:"), 2, 0);
            layoutGestion.Controls.Add(cmbNuevoEstado, 3, 0);

            layoutGestion.Controls.Add(CrearLabel("Descripción Denuncia:"), 0, 1);
            layoutGestion.Controls.Add(txtDetalleReporte, 1, 1);
            layoutGestion.Controls.Add(btnActualizar, 3, 1);

            grpGestion.Controls.Add(layoutGestion);

            // Agregar al Layout Principal
            mainLayout.Controls.Add(panelFiltro, 0, 0);
            mainLayout.Controls.Add(dgvReportes, 0, 1);
            mainLayout.Controls.Add(grpGestion, 0, 2);

            this.Controls.Add(mainLayout);
            CargarTablaReportes();
        }

        private Label CrearLabel(string texto)
        {
            return new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            };
        }

        private void CargarTablaReportes()
        {
            string filtro = cmbFiltroEstado.SelectedItem?.ToString() ?? "Todos";

            var lista = DatosFicticios.ReportesCiudadanos
                .Where(r => filtro == "Todos" || r.Estado.Equals(filtro, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    r.Folio,
                    r.Ciudadano,
                    Placa = r.PlacaReportada,
                    Infraccion = r.TipoInfraccion,
                    r.Ubicacion,
                    r.Estado,
                    Fecha = r.FechaReporte.ToString("dd/MM/yyyy HH:mm")
                }).ToList();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = lista;
        }

        private void DgvReportes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvReportes.SelectedRows.Count > 0)
            {
                string folio = dgvReportes.SelectedRows[0].Cells["Folio"].Value?.ToString() ?? "";
                var reporte = DatosFicticios.ReportesCiudadanos.FirstOrDefault(r => r.Folio == folio);

                if (reporte != null)
                {
                    txtFolioSeleccionado.Text = reporte.Folio;
                    cmbNuevoEstado.SelectedItem = reporte.Estado;
                    txtDetalleReporte.Text = $"{reporte.Detalle} (Ubicación: {reporte.Ubicacion})";
                }
            }
        }

        private void BtnActualizar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolioSeleccionado.Text))
            {
                MessageBox.Show("Seleccione un reporte de la tabla para gestionar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string folio = txtFolioSeleccionado.Text;
            var reporte = DatosFicticios.ReportesCiudadanos.FirstOrDefault(r => r.Folio == folio);

            if (reporte != null)
            {
                reporte.Estado = cmbNuevoEstado.SelectedItem?.ToString() ?? "Pendiente";
                MessageBox.Show($"El reporte {folio} se ha actualizado a estado '{reporte.Estado}'.", "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarTablaReportes();
            }
        }
    }
}