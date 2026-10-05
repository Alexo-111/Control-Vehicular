using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcPadronVehicularCivil : UserControl
    {
        private DataGridView dgvVehiculos = null!;
        private TextBox txtBuscar = null!;

        public UcPadronVehicularCivil()
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
                Padding = new Padding(20)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Encabezado
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Barra de Búsqueda y Acciones
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Tabla de Vehículos

            // 1. Encabezado
            Label lblTitulo = new Label
            {
                Text = " Mis Vehículos Registrados",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 2. Barra Superior (Filtro + Botón Registrar)
            Panel panelBarra = new Panel { Dock = DockStyle.Fill };

            Label lblBuscar = new Label
            {
                Text = "Buscar por Placa:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(0, 12),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Location = new Point(190, 9),
                Width = 220,
                Font = new Font("Segoe UI", 10F)
            };
            txtBuscar.TextChanged += (s, e) => FiltrarVehiculos();

            Button btnRegistrar = new Button
            {
                Text = "+ Registrar Nuevo Vehículo",
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Size = new Size(220, 32),
                Location = new Point(430, 7),
                Cursor = Cursors.Hand
            };
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Click += BtnRegistrar_Click;

            panelBarra.Controls.Add(lblBuscar);
            panelBarra.Controls.Add(txtBuscar);
            panelBarra.Controls.Add(btnRegistrar);

            // 3. Tabla DataGridView
            dgvVehiculos = new DataGridView
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
                RowTemplate = { Height = 30 }
            };

            dgvVehiculos.EnableHeadersVisualStyles = false;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvVehiculos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            Panel panelTabla = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            panelTabla.Controls.Add(dgvVehiculos);

            mainLayout.Controls.Add(lblTitulo, 0, 0);
            mainLayout.Controls.Add(panelBarra, 0, 1);
            mainLayout.Controls.Add(panelTabla, 0, 2);

            this.Controls.Add(mainLayout);

            CargarVehiculos();
        }

        private void CargarVehiculos()
        {
            var lista = DatosFicticios.Vehiculos
                .Where(v => string.IsNullOrEmpty(Sesion.Usuario) ||
                            Sesion.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                            Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase) ||
                            v.Propietario.Equals(Sesion.Usuario, StringComparison.OrdinalIgnoreCase))
                .Select(v => new
                {
                    Placa = v.Placa,
                    Propietario = v.Propietario,
                    MarcaModelo = v.MarcaModelo,
                    Año = v.Anio,
                    Estado = v.Estado,
                    FechaRegistro = v.FechaRegistro.ToString("dd/MM/yyyy")
                }).ToList();

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = lista;
        }

        private void FiltrarVehiculos()
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            var listaFiltrada = DatosFicticios.Vehiculos
                .Where(v => (string.IsNullOrEmpty(Sesion.Usuario) ||
                             Sesion.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                             Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase) ||
                             v.Propietario.Equals(Sesion.Usuario, StringComparison.OrdinalIgnoreCase)) &&
                            (v.Placa.ToLower().Contains(filtro) ||
                             v.MarcaModelo.ToLower().Contains(filtro) ||
                             v.Propietario.ToLower().Contains(filtro)))
                .Select(v => new
                {
                    Placa = v.Placa,
                    Propietario = v.Propietario,
                    MarcaModelo = v.MarcaModelo,
                    Año = v.Anio,
                    Estado = v.Estado,
                    FechaRegistro = v.FechaRegistro.ToString("dd/MM/yyyy")
                }).ToList();

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = listaFiltrada;
        }

        private void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            FormAltaVehiculo dlg = new FormAltaVehiculo();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CargarVehiculos();
            }
        }
    }

    public partial class FormAltaVehiculo : Form
    {
        public TextBox txtPlaca = null!;
        public TextBox txtMarcaModelo = null!;
        public TextBox txtAnio = null!;
        public Button btnGuardar = null!;
        public Button btnCancelar = null!;

        public FormAltaVehiculo()
        {
            // Se eliminó InitializeComponent() para prevenir CS0103 al usar código programático
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            this.Text = "Registro de Nuevo Vehículo";
            this.Size = new Size(440, 260);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(15)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Placa
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Marca/Modelo
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Año
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Botones

            // Campos e Inputs
            Label lblPlaca = new Label { Text = "Placa:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            txtPlaca = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill };

            Label lblMarca = new Label { Text = "Marca:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            txtMarcaModelo = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill };

            Label lblAnio = new Label { Text = "Año:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            txtAnio = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill };

            // Panel de Botones
            FlowLayoutPanel panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            btnGuardar = new Button
            {
                Text = "Guardar",
                BackColor = Color.FromArgb(106, 27, 41),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(100, 35),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = new Button
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

            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Controls.Add(btnCancelar);

            // Agregar al Layout
            layout.Controls.Add(lblPlaca, 0, 0);
            layout.Controls.Add(txtPlaca, 1, 0);

            layout.Controls.Add(lblMarca, 0, 1);
            layout.Controls.Add(txtMarcaModelo, 1, 1);

            layout.Controls.Add(lblAnio, 0, 2);
            layout.Controls.Add(txtAnio, 1, 2);

            layout.Controls.Add(panelBotones, 1, 3);

            this.Controls.Add(layout);
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlaca.Text) || string.IsNullOrWhiteSpace(txtMarcaModelo.Text))
            {
                MessageBox.Show("Por favor, ingresa la Placa y la Marca/Modelo del vehículo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int.TryParse(txtAnio.Text, out int anioVal);

            // Cambiado de 'new RegistroVehicular()' a 'new Vehiculo()'
            DatosFicticios.Vehiculos.Add(new Vehiculo()
            {
                Placa = txtPlaca.Text.Trim().ToUpper(),
                Propietario = string.IsNullOrEmpty(Sesion.Usuario) ? "Ciudadano" : Sesion.Usuario,
                MarcaModelo = txtMarcaModelo.Text.Trim(),
                Anio = anioVal > 0 ? anioVal : DateTime.Now.Year,
                Estado = "Vigente",
                FechaRegistro = DateTime.Now
            });

            MessageBox.Show("¡Vehículo registrado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}