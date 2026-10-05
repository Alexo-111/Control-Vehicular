using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcPadronVehicular : UserControl
    {
        private TabControl tabControlPadron = null!;
        private DataGridView dgvVehiculos = null!;

        // Campos Alta
        private TextBox txtPlacaAlta = null!, txtPropietarioAlta = null!, txtMarcaModeloAlta = null!, txtAnioAlta = null!, txtSerieAlta = null!;
        private TextBox txtNumMotorAlta = null!, txtLocalidadAlta = null!, txtMunicipioAlta = null!, txtFolioAlta = null!;
        private ComboBox cmbCombustibleAlta = null!, cmbOrigenAlta = null!;
        private DateTimePicker dtpExpedicionAlta = null!, dtpVigenciaAlta = null!;

        // Campos Cambio Propietario
        private TextBox txtPlacaCambio = null!, txtNuevoPropietario = null!;

        // Campos Baja
        private TextBox txtPlacaBaja = null!, txtMotivoBaja = null!;

        public UcPadronVehicular()
        {
            InitializeComponentCustom();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(240, 243, 246);
            ConstruirInterfaz();
        }

        private void InitializeComponentCustom()
        {
            // Método auxiliar para evitar llamadas a componentes inexistentes de diseñador
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
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 290F)); // Área de Formularios / Pestañas
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Tabla General de Vehículos

            // Configurar TabControl de Operaciones
            tabControlPadron = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            TabPage tabAlta = new TabPage("Alta de Vehículo") { BackColor = Color.White };
            TabPage tabCambio = new TabPage("Cambio de Propietario") { BackColor = Color.White };
            TabPage tabBaja = new TabPage("Baja Vehicular") { BackColor = Color.White };

            ConstruirTabAlta(tabAlta);
            ConstruirTabCambio(tabCambio);
            ConstruirTabBaja(tabBaja);

            tabControlPadron.TabPages.Add(tabAlta);
            tabControlPadron.TabPages.Add(tabCambio);
            tabControlPadron.TabPages.Add(tabBaja);

            // Tabla de Vehículos Registrados
            dgvVehiculos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 28 }
            };

            dgvVehiculos.EnableHeadersVisualStyles = false;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvVehiculos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            mainLayout.Controls.Add(tabControlPadron, 0, 0);
            mainLayout.Controls.Add(dgvVehiculos, 0, 1);

            this.Controls.Add(mainLayout);
            ActualizarTabla();
        }

        private void ConstruirTabAlta(TabPage tab)
        {
            // 3 pares etiqueta/campo por fila (6 columnas)
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 5,
                Padding = new Padding(10)
            };

            for (int i = 0; i < 3; i++)
            {
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            }
            for (int r = 0; r < 5; r++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

            txtPlacaAlta = new TextBox { Dock = DockStyle.Fill };
            txtPropietarioAlta = new TextBox { Dock = DockStyle.Fill };
            txtMarcaModeloAlta = new TextBox { Dock = DockStyle.Fill };
            txtAnioAlta = new TextBox { Dock = DockStyle.Fill, MaxLength = 4 };
            txtSerieAlta = new TextBox { Dock = DockStyle.Fill, MaxLength = 17, CharacterCasing = CharacterCasing.Upper };
            txtNumMotorAlta = new TextBox { Dock = DockStyle.Fill, CharacterCasing = CharacterCasing.Upper };
            txtLocalidadAlta = new TextBox { Dock = DockStyle.Fill };
            txtMunicipioAlta = new TextBox { Dock = DockStyle.Fill };
            txtFolioAlta = new TextBox { Dock = DockStyle.Fill, CharacterCasing = CharacterCasing.Upper };

            cmbCombustibleAlta = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCombustibleAlta.Items.AddRange(new object[] { "Gasolina", "Diésel", "Gas LP", "Gas Natural", "Híbrido", "Eléctrico" });
            cmbCombustibleAlta.SelectedIndex = 0;

            cmbOrigenAlta = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbOrigenAlta.Items.AddRange(new object[] { "Nacional", "Importado" });
            cmbOrigenAlta.SelectedIndex = 0;

            dtpExpedicionAlta = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Today };
            dtpVigenciaAlta = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddYears(1) };

            Button btnRegistrarAlta = new Button
            {
                Text = " Registrar Vehículo",
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Height = 35,
                Cursor = Cursors.Hand
            };
            btnRegistrarAlta.FlatAppearance.BorderSize = 0;
            btnRegistrarAlta.Click += BtnRegistrarAlta_Click;

            void Fila(int fila, int col, string texto, Control campo)
            {
                layout.Controls.Add(new Label { Text = texto, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 3, 8, 3) }, col, fila);
                campo.Margin = new Padding(3, 6, 12, 6);
                layout.Controls.Add(campo, col + 1, fila);
            }

            // Fila 0
            Fila(0, 0, "Placa:", txtPlacaAlta);
            Fila(0, 2, "Propietario:", txtPropietarioAlta);
            Fila(0, 4, "Marca/Modelo:", txtMarcaModeloAlta);

            // Fila 1
            Fila(1, 0, "Año:", txtAnioAlta);
            Fila(1, 2, "Número de serie (VIN):", txtSerieAlta);
            Fila(1, 4, "Número de motor:", txtNumMotorAlta);

            // Fila 2
            Fila(2, 0, "Tipo de combustible:", cmbCombustibleAlta);
            Fila(2, 2, "Origen:", cmbOrigenAlta);
            Fila(2, 4, "Localidad:", txtLocalidadAlta);

            // Fila 3
            Fila(3, 0, "Municipio:", txtMunicipioAlta);
            Fila(3, 2, "Expedición:", dtpExpedicionAlta);
            Fila(3, 4, "Vigencia:", dtpVigenciaAlta);

            // Fila 4
            Fila(4, 0, "No. de folio:", txtFolioAlta);
            btnRegistrarAlta.Margin = new Padding(3, 3, 12, 3);
            layout.Controls.Add(btnRegistrarAlta, 4, 4);
            layout.SetColumnSpan(btnRegistrarAlta, 2);

            tab.Controls.Add(layout);
        }

        private void ConstruirTabCambio(TabPage tab)
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(15)
            };

            txtPlacaCambio = new TextBox { Width = 180 };
            txtNuevoPropietario = new TextBox { Width = 250 };

            Button btnGuardarCambio = new Button
            {
                Text = " Actualizar Tarjeta de Circulación",
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Height = 35,
                Cursor = Cursors.Hand
            };
            btnGuardarCambio.FlatAppearance.BorderSize = 0;
            btnGuardarCambio.Click += BtnGuardarCambio_Click;

            layout.Controls.Add(new Label { Text = "Placa del Vehículo:", AutoSize = true }, 0, 0);
            layout.Controls.Add(txtPlacaCambio, 1, 0);

            layout.Controls.Add(new Label { Text = "Nuevo Propietario:", AutoSize = true }, 0, 1);
            layout.Controls.Add(txtNuevoPropietario, 1, 1);

            layout.Controls.Add(btnGuardarCambio, 1, 2);

            tab.Controls.Add(layout);
        }

        private void ConstruirTabBaja(TabPage tab)
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(15)
            };

            txtPlacaBaja = new TextBox { Width = 180 };
            txtMotivoBaja = new TextBox { Width = 250 };

            Button btnProcesarBaja = new Button
            {
                Text = " Procesar Baja Vehicular",
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Height = 35,
                Cursor = Cursors.Hand
            };
            btnProcesarBaja.FlatAppearance.BorderSize = 0;
            btnProcesarBaja.Click += BtnProcesarBaja_Click;

            layout.Controls.Add(new Label { Text = "Placa a Dar de Baja:", AutoSize = true }, 0, 0);
            layout.Controls.Add(txtPlacaBaja, 1, 0);

            layout.Controls.Add(new Label { Text = "Motivo de Baja:", AutoSize = true }, 0, 1);
            layout.Controls.Add(txtMotivoBaja, 1, 1);

            layout.Controls.Add(btnProcesarBaja, 1, 2);

            tab.Controls.Add(layout);
        }

        private void BtnRegistrarAlta_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlacaAlta.Text) || string.IsNullOrWhiteSpace(txtPropietarioAlta.Text))
            {
                MessageBox.Show("Por favor complete los campos obligatorios (Placa y Propietario).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string serie = txtSerieAlta.Text.Trim().ToUpper();
            if (serie.Length > 0 && !System.Text.RegularExpressions.Regex.IsMatch(serie, "^[A-HJ-NPR-Z0-9]{17}$"))
            {
                MessageBox.Show("El número de serie (VIN) debe tener 17 caracteres entre letras y números (no se usan las letras I, O ni Q).", "Número de serie inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSerieAlta.Focus();
                return;
            }

            if (dtpVigenciaAlta.Value.Date < dtpExpedicionAlta.Value.Date)
            {
                MessageBox.Show("La fecha de vigencia no puede ser anterior a la fecha de expedición.", "Fechas inválidas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpVigenciaAlta.Focus();
                return;
            }

            string folio = txtFolioAlta.Text.Trim().ToUpper();
            if (folio.Length > 0 && DatosFicticios.Vehiculos.Any(v => v.NoFolio.Equals(folio, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Ya existe un vehículo registrado con ese número de folio.", "Folio duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFolioAlta.Focus();
                return;
            }

            int.TryParse(txtAnioAlta.Text, out int anio);

            DatosFicticios.Vehiculos.Add(new Vehiculo
            {
                Placa = txtPlacaAlta.Text.ToUpper(),
                Propietario = txtPropietarioAlta.Text,
                MarcaModelo = txtMarcaModeloAlta.Text,
                Anio = anio > 0 ? anio : 2026,
                Serie = serie,
                NumMotor = txtNumMotorAlta.Text.Trim(),
                TipoCombustible = cmbCombustibleAlta.Text,
                Origen = cmbOrigenAlta.Text,
                Localidad = txtLocalidadAlta.Text.Trim(),
                Municipio = txtMunicipioAlta.Text.Trim(),
                Expedicion = dtpExpedicionAlta.Value.Date,
                Vigencia = dtpVigenciaAlta.Value.Date,
                NoFolio = folio,
                Estado = "Vigente",
                FechaRegistro = DateTime.Now
            });

            MessageBox.Show($"Vehículo {txtPlacaAlta.Text.ToUpper()} registrado exitosamente en el Padrón.", "Alta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
            ActualizarTabla();
        }

        private void BtnGuardarCambio_Click(object? sender, EventArgs e)
        {
            string placa = txtPlacaCambio.Text.Trim().ToUpper();
            var vehiculo = DatosFicticios.Vehiculos.FirstOrDefault(v => v.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase));

            if (vehiculo == null)
            {
                MessageBox.Show("No se encontró ningún vehículo con esa placa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNuevoPropietario.Text))
            {
                MessageBox.Show("Ingrese el nombre del nuevo propietario.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            vehiculo.Propietario = txtNuevoPropietario.Text;
            MessageBox.Show($"Propietario actualizado para la placa {placa}. Nueva Tarjeta de Circulación generada.", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
            ActualizarTabla();
        }

        private void BtnProcesarBaja_Click(object? sender, EventArgs e)
        {
            string placa = txtPlacaBaja.Text.Trim().ToUpper();
            var vehiculo = DatosFicticios.Vehiculos.FirstOrDefault(v => v.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase));

            if (vehiculo == null)
            {
                MessageBox.Show("No se encontró ningún vehículo con esa placa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            vehiculo.Estado = "Baja";
            MessageBox.Show($"El vehículo con placa {placa} ha sido dado de BAJA en el sistema.", "Baja Procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
            ActualizarTabla();
        }

        private void ActualizarTabla()
        {
            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = DatosFicticios.Vehiculos
                .Select(v => new
                {
                    v.Placa,
                    v.Propietario,
                    Vehiculo = v.MarcaModelo,
                    Año = v.Anio,
                    Serie = v.Serie,
                    Motor = v.NumMotor,
                    Combustible = v.TipoCombustible,
                    Origen = v.Origen,
                    Localidad = v.Localidad,
                    Municipio = v.Municipio,
                    Expedición = v.Expedicion?.ToString("dd/MM/yyyy") ?? "",
                    Vigencia = v.Vigencia?.ToString("dd/MM/yyyy") ?? "",
                    Folio = v.NoFolio,
                    v.Estado,
                    Fecha = v.FechaRegistro.ToString("dd/MM/yyyy")
                }).ToList();
        }

        private void LimpiarCampos()
        {
            txtPlacaAlta.Clear(); txtPropietarioAlta.Clear(); txtMarcaModeloAlta.Clear(); txtAnioAlta.Clear(); txtSerieAlta.Clear();
            txtNumMotorAlta.Clear(); txtLocalidadAlta.Clear(); txtMunicipioAlta.Clear(); txtFolioAlta.Clear();
            cmbCombustibleAlta.SelectedIndex = 0; cmbOrigenAlta.SelectedIndex = 0;
            dtpExpedicionAlta.Value = DateTime.Today; dtpVigenciaAlta.Value = DateTime.Today.AddYears(1);
            txtPlacaCambio.Clear(); txtNuevoPropietario.Clear();
            txtPlacaBaja.Clear(); txtMotivoBaja.Clear();
        }
    }
}