using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcLicencias : UserControl
    {
        private TabControl tabControlLicencias = null!;
        private DataGridView dgvLicencias = null!;

        // Campos Expedición
        private TextBox txtNombreExpedicion = null!, txtCURPExpedicion = null!, txtRFCExpedicion = null!;
        private ComboBox cmbTipoLicenciaExpedicion = null!, cmbVigenciaExpedicion = null!;

        // Campos Renovación
        private TextBox txtNumLicenciaRenovacion = null!, txtNuevoNombreRenovacion = null!;
        private ComboBox cmbVigenciaRenovacion = null!;

        // Campos Penalización
        private TextBox txtNumLicenciaPenalizacion = null!, txtPuntosDescuento = null!, txtMotivoPenalizacion = null!;

        public UcLicencias()
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
                Padding = new Padding(12)
            };

            // 260px le da espacio suficiente al TabControl para mostrar todos los campos y botones sin recortar
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Configurar TabControl
            tabControlLicencias = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            TabPage tabExpedicion = new TabPage("Expedición Nueva") { BackColor = Color.White };
            TabPage tabRenovacion = new TabPage("Renovación de Licencia") { BackColor = Color.White };
            TabPage tabPenalizacion = new TabPage("Sistema de Penalizaciones (Puntos)") { BackColor = Color.White };

            ConstruirTabExpedicion(tabExpedicion);
            ConstruirTabRenovacion(tabRenovacion);
            ConstruirTabPenalizacion(tabPenalizacion);

            tabControlLicencias.TabPages.Add(tabExpedicion);
            tabControlLicencias.TabPages.Add(tabRenovacion);
            tabControlLicencias.TabPages.Add(tabPenalizacion);

            // Tabla de Registro de Licencias
            dgvLicencias = new DataGridView
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

            dgvLicencias.EnableHeadersVisualStyles = false;
            dgvLicencias.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvLicencias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLicencias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            mainLayout.Controls.Add(tabControlLicencias, 0, 0);
            mainLayout.Controls.Add(dgvLicencias, 0, 1);

            this.Controls.Add(mainLayout);
            CargarTablaLicencias();
        }

        private Label CrearLabel(string texto)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
        }

        #region Construcción de Pestañas

        private void ConstruirTabExpedicion(TabPage tab)
        {
            TableLayoutPanel layout = CrearLayoutBase(4, 4);

            txtNombreExpedicion = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5F) };
            txtCURPExpedicion = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5F) };
            txtRFCExpedicion = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5F) };

            cmbTipoLicenciaExpedicion = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cmbTipoLicenciaExpedicion.Items.AddRange(new string[] { "Automovilista (Tipo A)", "Chofer (Tipo B)", "Motociclista (Tipo C)" });
            cmbTipoLicenciaExpedicion.SelectedIndex = 0;

            cmbVigenciaExpedicion = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cmbVigenciaExpedicion.Items.AddRange(new string[] { "2 Años", "3 Años", "5 Años" });
            cmbVigenciaExpedicion.SelectedIndex = 1;

            Button btnExpedir = CrearBoton("Expedir Licencia", Color.FromArgb(41, 128, 185));
            btnExpedir.Click += BtnExpedir_Click;

            layout.Controls.Add(CrearLabel("Nombre Conductor:"), 0, 0);
            layout.Controls.Add(txtNombreExpedicion, 1, 0);
            layout.Controls.Add(CrearLabel("CURP:"), 2, 0);
            layout.Controls.Add(txtCURPExpedicion, 3, 0);

            layout.Controls.Add(CrearLabel("Tipo Licencia:"), 0, 1);
            layout.Controls.Add(cmbTipoLicenciaExpedicion, 1, 1);
            layout.Controls.Add(CrearLabel("Vigencia:"), 2, 1);
            layout.Controls.Add(cmbVigenciaExpedicion, 3, 1);

            layout.Controls.Add(CrearLabel("RFC / Identificación:"), 0, 2);
            layout.Controls.Add(txtRFCExpedicion, 1, 2);

            layout.Controls.Add(btnExpedir, 3, 3);

            tab.Controls.Add(layout);
        }

        private void ConstruirTabRenovacion(TabPage tab)
        {
            TableLayoutPanel layout = CrearLayoutBase(4, 3);

            txtNumLicenciaRenovacion = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5F) };
            txtNuevoNombreRenovacion = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Segoe UI", 9.5F) };

            txtNumLicenciaRenovacion.Leave += (s, e) =>
            {
                var lic = DatosFicticios.Licencias.FirstOrDefault(l => l.NumeroLicencia.Equals(txtNumLicenciaRenovacion.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                txtNuevoNombreRenovacion.Text = lic != null ? lic.Conductor : "Licencia no encontrada";
            };

            cmbVigenciaRenovacion = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cmbVigenciaRenovacion.Items.AddRange(new string[] { "2 Años", "3 Años", "5 Años" });
            cmbVigenciaRenovacion.SelectedIndex = 1;

            Button btnRenovar = CrearBoton("Renovar Licencia", Color.FromArgb(230, 126, 34));
            btnRenovar.Click += BtnRenovar_Click;

            layout.Controls.Add(CrearLabel("N° de Licencia:"), 0, 0);
            layout.Controls.Add(txtNumLicenciaRenovacion, 1, 0);
            layout.Controls.Add(CrearLabel("Nueva Vigencia:"), 2, 0);
            layout.Controls.Add(cmbVigenciaRenovacion, 3, 0);

            layout.Controls.Add(CrearLabel("Nombre Conductor:"), 0, 1);
            layout.Controls.Add(txtNuevoNombreRenovacion, 1, 1);

            layout.Controls.Add(btnRenovar, 3, 2);

            tab.Controls.Add(layout);
        }

        private void ConstruirTabPenalizacion(TabPage tab)
        {
            TableLayoutPanel layout = CrearLayoutBase(4, 3);

            txtNumLicenciaPenalizacion = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5F) };
            txtPuntosDescuento = new TextBox { Dock = DockStyle.Fill, Text = "3", Font = new Font("Segoe UI", 9.5F) };
            txtMotivoPenalizacion = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5F) };

            Button btnAplicarPenalizacion = CrearBoton("Aplicar Penalización", Color.FromArgb(192, 57, 43));
            btnAplicarPenalizacion.Click += BtnAplicarPenalizacion_Click;

            layout.Controls.Add(CrearLabel("N° de Licencia:"), 0, 0);
            layout.Controls.Add(txtNumLicenciaPenalizacion, 1, 0);
            layout.Controls.Add(CrearLabel("Puntos a Restar:"), 2, 0);
            layout.Controls.Add(txtPuntosDescuento, 3, 0);

            layout.Controls.Add(CrearLabel("Motivo de Penalización:"), 0, 1);
            layout.Controls.Add(txtMotivoPenalizacion, 1, 1);

            layout.Controls.Add(btnAplicarPenalizacion, 3, 2);

            tab.Controls.Add(layout);
        }

        private TableLayoutPanel CrearLayoutBase(int cols, int rows)
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = cols,
                RowCount = rows,
                Padding = new Padding(10)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Fijar alturas específicas para cada fila asegura que no haya desbordamientos
            for (int i = 0; i < rows; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }

            return layout;
        }

        private Button CrearBoton(string texto, Color bg)
        {
            Button btn = new Button
            {
                Text = texto,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Right, // Cambiado a DockStyle.Right para ocupar altura disponible sin salirse
                Width = 160,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 3, 0, 3)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        #endregion

        #region Lógica de Negocio y Eventos

        private void CargarTablaLicencias()
        {
            dgvLicencias.DataSource = null;
            dgvLicencias.DataSource = DatosFicticios.Licencias.ToList();
        }

        private void BtnExpedir_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreExpedicion.Text) || string.IsNullOrWhiteSpace(txtCURPExpedicion.Text))
            {
                MessageBox.Show("Por favor ingresa el Nombre y la CURP.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevaLicencia = new Licencia
            {
                NumeroLicencia = $"LIC-{Random.Shared.Next(10000, 99999)}",
                Conductor = txtNombreExpedicion.Text.Trim(),
                TipoLicencia = cmbTipoLicenciaExpedicion.SelectedItem?.ToString() ?? "Automovilista (Tipo A)",
                Vigencia = cmbVigenciaExpedicion.SelectedItem?.ToString() ?? "3 Años",
                Puntos = 12,
                Estado = "Activa",
                FechaExpedicion = DateTime.Now
            };

            DatosFicticios.Licencias.Add(nuevaLicencia);
            MessageBox.Show($"Licencia expedida exitosamente.\nNúmero de Licencia: {nuevaLicencia.NumeroLicencia}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtNombreExpedicion.Clear();
            txtCURPExpedicion.Clear();
            txtRFCExpedicion.Clear();
            CargarTablaLicencias();
        }

        private void BtnRenovar_Click(object? sender, EventArgs e)
        {
            string num = txtNumLicenciaRenovacion.Text.Trim();
            var licencia = DatosFicticios.Licencias.FirstOrDefault(l => l.NumeroLicencia.Equals(num, StringComparison.OrdinalIgnoreCase));

            if (licencia == null)
            {
                MessageBox.Show("No se encontró ninguna licencia con el número especificado.", "No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            licencia.Vigencia = cmbVigenciaRenovacion.SelectedItem?.ToString() ?? "3 Años";
            licencia.Puntos = 12; // Restablecer puntos al renovar
            licencia.Estado = "Activa";
            licencia.FechaExpedicion = DateTime.Now;

            MessageBox.Show($"Licencia {licencia.NumeroLicencia} renovada exitosamente.\nPuntos restablecidos a 12.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtNumLicenciaRenovacion.Clear();
            txtNuevoNombreRenovacion.Clear();
            CargarTablaLicencias();
        }

        private void BtnAplicarPenalizacion_Click(object? sender, EventArgs e)
        {
            string num = txtNumLicenciaPenalizacion.Text.Trim();
            var licencia = DatosFicticios.Licencias.FirstOrDefault(l => l.NumeroLicencia.Equals(num, StringComparison.OrdinalIgnoreCase));

            if (licencia == null)
            {
                MessageBox.Show("No se encontró la licencia especificada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPuntosDescuento.Text, out int puntosARestar) || puntosARestar <= 0)
            {
                MessageBox.Show("Ingresa una cantidad válida de puntos a descontar.", "Valor Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            licencia.Puntos -= puntosARestar;

            if (licencia.Puntos <= 0)
            {
                licencia.Puntos = 0;
                licencia.Estado = "Suspendida";
                MessageBox.Show($"¡Atención! Se han descontado {puntosARestar} puntos.\nLa licencia ha llegado a 0 puntos y ahora está **SUSPENDIDA**.", "Licencia Suspendida", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                MessageBox.Show($"Se restaron {puntosARestar} puntos por: {txtMotivoPenalizacion.Text.Trim()}.\nPuntos restantes: {licencia.Puntos}", "Penalización Aplicada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtNumLicenciaPenalizacion.Clear();
            txtMotivoPenalizacion.Clear();
            CargarTablaLicencias();
        }

        #endregion
    }
}