using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcDashboardOficial : UserControl
    {
        private TextBox txtBuscarPlaca = null!;
        private DataGridView dgvVehiculos = null!;
        private Label lblTotalVehiculos = null!;
        private Label lblTramitesProceso = null!;
        private Label lblAlertasRobo = null!;

        public UcDashboardOficial()
        {
            InitializeComponentCustom();
            ConstruirInterfazDashboard();
            CargarDatos();
        }

        private void InitializeComponentCustom()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 246, 250);
        }

        private void ConstruirInterfazDashboard()
        {
            this.Controls.Clear();

            // TableLayoutPanel principal con márgenes adecuados
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(20),
                AutoScroll = true
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));  // 1. Saludo
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // 2. Tarjetas
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));  // 3. Título + Buscador
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // 4. Tabla

            // 1. Encabezado Único de Bienvenida
            Label lblBienvenida = new Label
            {
                Text = $"Bienvenido, {Sesion.Usuario ?? "Oficial"}",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 2. Tarjetas de Resumen
            TableLayoutPanel layoutTarjetas = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 5, 0, 10)
            };
            layoutTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            layoutTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            layoutTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            Panel cardVehiculos = CrearTarjetaEstadistica("VEHÍCULOS REGISTRADOS", "0", Color.FromArgb(41, 128, 185), out lblTotalVehiculos);
            Panel cardTramites = CrearTarjetaEstadistica("TRÁMITES EN PROCESO", "0", Color.FromArgb(230, 126, 34), out lblTramitesProceso);
            Panel cardRobos = CrearTarjetaEstadistica("ALERTAS DE ROBO", "0", Color.FromArgb(192, 57, 43), out lblAlertasRobo);

            layoutTarjetas.Controls.Add(cardVehiculos, 0, 0);
            layoutTarjetas.Controls.Add(cardTramites, 1, 0);
            layoutTarjetas.Controls.Add(cardRobos, 2, 0);

            // 3. Panel con Título de la Tabla y Buscador por Placa
            Panel panelBarra = new Panel { Dock = DockStyle.Fill };

            Label lblTituloTabla = new Label
            {
                Text = "Últimos Vehículos Registrados",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Location = new Point(0, 8),
                AutoSize = true
            };

            txtBuscarPlaca = new TextBox
            {
                Width = 220,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Buscar por placa...",
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            txtBuscarPlaca.Location = new Point(panelBarra.Width - txtBuscarPlaca.Width - 10, 6);
            txtBuscarPlaca.TextChanged += TxtBuscarPlaca_TextChanged;

            // Ajustar posición dinámica del buscador si cambia el tamaño
            panelBarra.SizeChanged += (s, e) =>
            {
                txtBuscarPlaca.Location = new Point(panelBarra.Width - txtBuscarPlaca.Width - 5, 6);
            };

            panelBarra.Controls.Add(lblTituloTabla);
            panelBarra.Controls.Add(txtBuscarPlaca);

            // 4. DataGridView
            dgvVehiculos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 28 }
            };

            dgvVehiculos.EnableHeadersVisualStyles = false;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvVehiculos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // Agregar componentes al layout
            mainLayout.Controls.Add(lblBienvenida, 0, 0);
            mainLayout.Controls.Add(layoutTarjetas, 0, 1);
            mainLayout.Controls.Add(panelBarra, 0, 2);
            mainLayout.Controls.Add(dgvVehiculos, 0, 3);

            this.Controls.Add(mainLayout);
        }

        private Panel CrearTarjetaEstadistica(string titulo, string valorInicial, Color colorHeader, out Label lblValor)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 6,
                BackColor = colorHeader
            };

            Label lblTituloCard = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Location = new Point(10, 12),
                AutoSize = true
            };

            lblValor = new Label
            {
                Text = valorInicial,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = colorHeader,
                Location = new Point(10, 32),
                AutoSize = true
            };

            card.Controls.Add(lblValor);
            card.Controls.Add(lblTituloCard);
            card.Controls.Add(header);

            return card;
        }

        private void CargarDatos(string filtroPlaca = "")
        {
            var consulta = DatosFicticios.Vehiculos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filtroPlaca))
            {
                consulta = consulta.Where(v => v.Placa.Contains(filtroPlaca.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            var resultado = consulta.Select(v => new
            {
                v.Placa,
                v.Propietario,
                Vehiculo = v.MarcaModelo,
                Año = v.Anio,
                v.Estado,
                Registro = v.FechaRegistro.ToString("dd/MM/yyyy")
            }).ToList();

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = resultado;

            // Actualizar contadores
            if (lblTotalVehiculos != null) lblTotalVehiculos.Text = DatosFicticios.Vehiculos.Count.ToString();
            if (lblTramitesProceso != null) lblTramitesProceso.Text = DatosFicticios.TotalTramitesEnProceso.ToString();
            if (lblAlertasRobo != null) lblAlertasRobo.Text = DatosFicticios.TotalRobosUrgentes.ToString();
        }

        private void TxtBuscarPlaca_TextChanged(object? sender, EventArgs e)
        {
            CargarDatos(txtBuscarPlaca.Text);
        }
    }
}