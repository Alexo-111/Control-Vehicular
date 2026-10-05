using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcInfracciones : UserControl
    {
        // Controles del formulario
        private TextBox txtPlaca = null!;
        private TextBox txtPropietario = null!;
        private TextBox txtVehiculo = null!;
        private CheckBox chkForaneo = null!;
        private ComboBox cmbInfraccion = null!;
        private TextBox txtUMAs = null!;
        private TextBox txtMontoTotal = null!;
        private TextBox txtLugar = null!;
        private TextBox txtObservaciones = null!;
        private DataGridView dgvInfracciones = null!;

        // Valor de la UMA (2026 de referencia)
        private const decimal VALOR_UMA = 108.57m;

        // Catálogo de Infracciones del Reglamento Vial
        private readonly (string Motivo, int Umas)[] catalogoInfracciones = new[]
        {
            ("Exceso de velocidad", 15),
            ("Estacionar en lugar prohibido / Discapacitados", 20),
            ("Uso de teléfono celular al conducir", 10),
            ("Falta de cinturón de seguridad", 5),
            ("Conducir sin licencia vigente", 12),
            ("Pasarse luz roja de semáforo", 25),
            ("Conducir bajo la influencia del alcohol", 50)
        };

        public UcInfracciones()
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
                RowCount = 2,
                Padding = new Padding(15)
            };
            // Se incrementa la altura a 320px para asegurar espacio al botón
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Panel Superior: Formulario de Infracción
            GroupBox grpInfraccion = new GroupBox
            {
                Text = "HU-02: Registro de Infracción Vial",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            TableLayoutPanel layoutForm = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5,
                Padding = new Padding(5)
            };

            // Anchos de columna optimizados para evitar recorte de textos
            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            layoutForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Alturas de fila proporcionales
            for (int i = 0; i < 4; i++)
            {
                layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            }
            layoutForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F)); // Fila del Botón

            // Instanciar Controles
            txtPlaca = new TextBox { Dock = DockStyle.Fill };
            txtPlaca.Leave += TxtPlaca_Leave;
            txtPlaca.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) TxtPlaca_Leave(s, e); };

            chkForaneo = new CheckBox
            {
                Text = "Vehículo Foráneo",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };
            chkForaneo.CheckedChanged += ChkForaneo_CheckedChanged;

            txtPropietario = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };
            txtVehiculo = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };

            cmbInfraccion = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };
            foreach (var item in catalogoInfracciones)
            {
                cmbInfraccion.Items.Add($"{item.Motivo} ({item.Umas} UMAs)");
            }
            cmbInfraccion.SelectedIndexChanged += CmbInfraccion_SelectedIndexChanged;

            txtUMAs = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };
            txtMontoTotal = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            txtLugar = new TextBox { Dock = DockStyle.Fill };
            txtObservaciones = new TextBox { Dock = DockStyle.Fill };

            // Botón Principal
            Button btnRegistrar = new Button
            {
                Text = " Registrar Infracción",
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Height = 38,
                Cursor = Cursors.Hand
            };
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Click += BtnRegistrar_Click;

            // Fila 0: Placa y Checkbox Foráneo
            layoutForm.Controls.Add(CrearLabel("Placa Vehículo:"), 0, 0);
            layoutForm.Controls.Add(txtPlaca, 1, 0);
            layoutForm.Controls.Add(chkForaneo, 2, 0);

            // Fila 1: Propietario y Vehículo
            layoutForm.Controls.Add(CrearLabel("Propietario:"), 0, 1);
            layoutForm.Controls.Add(txtPropietario, 1, 1);
            layoutForm.Controls.Add(CrearLabel("Marca/Modelo:"), 2, 1);
            layoutForm.Controls.Add(txtVehiculo, 3, 1);

            // Fila 2: Motivo y Cálculo en Pesos
            layoutForm.Controls.Add(CrearLabel("Infracción:"), 0, 2);
            layoutForm.Controls.Add(cmbInfraccion, 1, 2);
            layoutForm.Controls.Add(CrearLabel("Monto (UMAs / MXN):"), 2, 2);

            TableLayoutPanel layoutCalculo = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            layoutCalculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            layoutCalculo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            layoutCalculo.Controls.Add(txtUMAs, 0, 0);
            layoutCalculo.Controls.Add(txtMontoTotal, 1, 0);
            layoutForm.Controls.Add(layoutCalculo, 3, 2);

            // Fila 3: Lugar y Observaciones
            layoutForm.Controls.Add(CrearLabel("Lugar de la falta:"), 0, 3);
            layoutForm.Controls.Add(txtLugar, 1, 3);
            layoutForm.Controls.Add(CrearLabel("Observaciones:"), 2, 3);
            layoutForm.Controls.Add(txtObservaciones, 3, 3);

            // Fila 4: Botón abarcará las columnas 2 y 3 en la parte inferior derecha
            layoutForm.Controls.Add(btnRegistrar, 2, 4);
            layoutForm.SetColumnSpan(btnRegistrar, 2);

            grpInfraccion.Controls.Add(layoutForm);

            // Tabla Inferior: Historial de Registros
            dgvInfracciones = new DataGridView
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

            dgvInfracciones.EnableHeadersVisualStyles = false;
            dgvInfracciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvInfracciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInfracciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            mainLayout.Controls.Add(grpInfraccion, 0, 0);
            mainLayout.Controls.Add(dgvInfracciones, 0, 1);

            this.Controls.Add(mainLayout);

            if (cmbInfraccion.Items.Count > 0) cmbInfraccion.SelectedIndex = 0;
            CargarTabla();
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

        private void TxtPlaca_Leave(object? sender, EventArgs e)
        {
            string placa = txtPlaca.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(placa) || chkForaneo.Checked) return;

            var vehiculo = DatosFicticios.Vehiculos.FirstOrDefault(v => v.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase));

            if (vehiculo != null)
            {
                txtPropietario.Text = vehiculo.Propietario;
                txtVehiculo.Text = vehiculo.MarcaModelo;
            }
            else
            {
                txtPropietario.Clear();
                txtVehiculo.Clear();
                MessageBox.Show("Placa no registrada en el padrón local. Marque 'Vehículo Foráneo' si proviene de otro estado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ChkForaneo_CheckedChanged(object? sender, EventArgs e)
        {
            bool esForaneo = chkForaneo.Checked;
            txtPropietario.ReadOnly = !esForaneo;
            txtVehiculo.ReadOnly = !esForaneo;

            if (esForaneo)
            {
                txtPropietario.Text = "CONDUCTOR FORÁNEO";
                txtVehiculo.Text = "VEHÍCULO FORÁNEO";
            }
            else
            {
                txtPropietario.Clear();
                txtVehiculo.Clear();
                TxtPlaca_Leave(null, EventArgs.Empty);
            }
        }

        private void CmbInfraccion_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbInfraccion.SelectedIndex >= 0 && cmbInfraccion.SelectedIndex < catalogoInfracciones.Length)
            {
                int umas = catalogoInfracciones[cmbInfraccion.SelectedIndex].Umas;
                decimal montoMXN = umas * VALOR_UMA;

                txtUMAs.Text = $"{umas} UMA";
                txtMontoTotal.Text = $"$ {montoMXN:N2} MXN";
            }
        }

        private void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlaca.Text))
            {
                MessageBox.Show("Por favor, ingrese el número de placa del vehículo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string placa = txtPlaca.Text.Trim().ToUpper();
            int umas = catalogoInfracciones[cmbInfraccion.SelectedIndex].Umas;
            decimal monto = umas * VALOR_UMA;

            var vehiculo = DatosFicticios.Vehiculos.FirstOrDefault(v => v.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase));
            if (vehiculo != null)
            {
                vehiculo.Estado = "Con Infracciones";
            }

            MessageBox.Show($"Infracción registrada con éxito.\n\nPlaca: {placa}\nInfracción: {catalogoInfracciones[cmbInfraccion.SelectedIndex].Motivo}\nMonto: ${monto:N2} MXN ({umas} UMAs)", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarFormulario();
            CargarTabla();
        }

        private void LimpiarFormulario()
        {
            txtPlaca.Clear();
            txtPropietario.Clear();
            txtVehiculo.Clear();
            txtLugar.Clear();
            txtObservaciones.Clear();
            chkForaneo.Checked = false;
            if (cmbInfraccion.Items.Count > 0) cmbInfraccion.SelectedIndex = 0;
        }

        private void CargarTabla()
        {
            dgvInfracciones.DataSource = null;
            dgvInfracciones.DataSource = DatosFicticios.Vehiculos
                .Select(v => new
                {
                    v.Placa,
                    v.Propietario,
                    v.MarcaModelo,
                    Estado = v.Estado,
                    Fecha = v.FechaRegistro.ToString("dd/MM/yyyy")
                }).ToList();
        }
    }
}