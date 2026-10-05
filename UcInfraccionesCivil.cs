using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcInfraccionesCivil : UserControl
    {
        private DataGridView dgvInfracciones = null!;
        private TextBox txtBuscar = null!;

        public UcInfraccionesCivil()
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
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Título
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Barra de Búsqueda y Botón de Pago
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Tabla de infracciones

            // 1. Título
            Label lblTitulo = new Label
            {
                Text = " Mis Infracciones Registradas",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(106, 27, 41),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 2. Barra de Búsqueda y Botón de Pago Directo
            Panel panelBarra = new Panel { Dock = DockStyle.Fill };

            Label lblBuscar = new Label
            {
                Text = "Buscar por Folio:",
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
            txtBuscar.TextChanged += (s, e) => FiltrarInfracciones();

            Button btnPagar = new Button
            {
                Text = "💳 Pagar Infracción Seleccionada",
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Size = new Size(240, 32),
                Location = new Point(430, 7),
                Cursor = Cursors.Hand
            };
            btnPagar.FlatAppearance.BorderSize = 0;
            btnPagar.Click += BtnPagar_Click;

            panelBarra.Controls.Add(lblBuscar);
            panelBarra.Controls.Add(txtBuscar);
            panelBarra.Controls.Add(btnPagar);

            // 3. DataGridView
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
                RowTemplate = { Height = 32 }
            };

            dgvInfracciones.EnableHeadersVisualStyles = false;
            dgvInfracciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 27, 41);
            dgvInfracciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInfracciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            Panel panelTabla = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            panelTabla.Controls.Add(dgvInfracciones);

            mainLayout.Controls.Add(lblTitulo, 0, 0);
            mainLayout.Controls.Add(panelBarra, 0, 1);
            mainLayout.Controls.Add(panelTabla, 0, 2);

            this.Controls.Add(mainLayout);

            // Cargar automáticamente las infracciones al abrir la pantalla
            CargarInfracciones();
        }

        private void CargarInfracciones()
        {
            // Nombre del usuario en sesión actual (por defecto "Carlos Gómez" para la prueba)
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;

            // Filtra únicamente las infracciones precargadas asociadas al conductor o muestra todas si es admin/oficial
            var lista = DatosFicticios.Infracciones
                .Where(i => string.IsNullOrEmpty(Sesion.Usuario) ||
                            Sesion.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                            Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase) ||
                            i.Conductor.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                .Select(i => new
                {
                    Folio = i.Folio,
                    Placa = i.Placa,
                    Conductor = i.Conductor,
                    Motivo = i.Motivo,
                    Monto = i.Monto.ToString("C2"),
                    Estado = i.Estado,
                    Fecha = i.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    Agente = i.Agente
                }).ToList();

            dgvInfracciones.DataSource = null;
            dgvInfracciones.DataSource = lista;
        }

        private void FiltrarInfracciones()
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;

            var listaFiltrada = DatosFicticios.Infracciones
                .Where(i => (string.IsNullOrEmpty(Sesion.Usuario) ||
                             Sesion.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                             Sesion.Rol.Equals("oficial", StringComparison.OrdinalIgnoreCase) ||
                             i.Conductor.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase)) &&
                            (i.Folio.ToLower().Contains(filtro) ||
                             i.Placa.ToLower().Contains(filtro) ||
                             i.Motivo.ToLower().Contains(filtro)))
                .Select(i => new
                {
                    Folio = i.Folio,
                    Placa = i.Placa,
                    Conductor = i.Conductor,
                    Motivo = i.Motivo,
                    Monto = i.Monto.ToString("C2"),
                    Estado = i.Estado,
                    Fecha = i.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    Agente = i.Agente
                }).ToList();

            dgvInfracciones.DataSource = null;
            dgvInfracciones.DataSource = listaFiltrada;
        }

        private void BtnPagar_Click(object? sender, EventArgs e)
        {
            if (dgvInfracciones.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, selecciona una infracción de la lista para proceder con el pago.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string folio = dgvInfracciones.SelectedRows[0].Cells["Folio"].Value?.ToString() ?? "";
            var infraccion = DatosFicticios.Infracciones.FirstOrDefault(i => i.Folio.Equals(folio, StringComparison.OrdinalIgnoreCase));

            if (infraccion == null) return;

            if (infraccion.Estado.Equals("Pagada", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Esta infracción ya se encuentra pagada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (FormPagarInfraccion dlg = new FormPagarInfraccion(infraccion))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Refresca la lista actualizando el estado a "Pagada"
                    CargarInfracciones();
                }
            }
        }
    }

    public partial class FormPagarInfraccion : Form
    {
        private Infraccion _infraccion;

        public FormPagarInfraccion(Infraccion infraccion)
        {
            _infraccion = infraccion;
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            this.Text = "Procesar Pago de Infracción";
            this.Size = new Size(440, 310);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(15)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F)); // Folio
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F)); // Motivo
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F)); // Monto
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F)); // Forma de Pago
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Botones

            Label lblFolioT = new Label { Text = "Folio Infracción:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            Label lblFolioV = new Label { Text = _infraccion.Folio, Font = new Font("Segoe UI", 9.5F), Anchor = AnchorStyles.Left };

            Label lblMotivoT = new Label { Text = "Motivo:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            Label lblMotivoV = new Label { Text = _infraccion.Motivo, Font = new Font("Segoe UI", 9.5F), Anchor = AnchorStyles.Left };

            Label lblMontoT = new Label { Text = "Total a Pagar:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            Label lblMontoV = new Label { Text = _infraccion.Monto.ToString("C2"), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.DarkGreen, Anchor = AnchorStyles.Left };

            Label lblMetodoT = new Label { Text = "Forma de Pago:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Anchor = AnchorStyles.Left };
            ComboBox cmbMetodo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Dock = DockStyle.Fill
            };
            cmbMetodo.Items.AddRange(new object[] { "Tarjeta de Débito / Crédito", "Transferencia SPEI", "Ventanilla Bancaria" });
            cmbMetodo.SelectedIndex = 0;

            FlowLayoutPanel panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            Button btnConfirmar = new Button
            {
                Text = "Confirmar Pago",
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(130, 35),
                Cursor = Cursors.Hand
            };
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.Click += (s, e) =>
            {
                _infraccion.Estado = "Pagada";
                MessageBox.Show($"¡Pago realizado con éxito mediante {cmbMetodo.SelectedItem}!\n\nEl estado de la infracción '{_infraccion.Folio}' ha cambiado a 'Pagada'.", "Pago Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            Button btnCancelar = new Button
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

            panelBotones.Controls.Add(btnConfirmar);
            panelBotones.Controls.Add(btnCancelar);

            layout.Controls.Add(lblFolioT, 0, 0);
            layout.Controls.Add(lblFolioV, 1, 0);

            layout.Controls.Add(lblMotivoT, 0, 1);
            layout.Controls.Add(lblMotivoV, 1, 1);

            layout.Controls.Add(lblMontoT, 0, 2);
            layout.Controls.Add(lblMontoV, 1, 2);

            layout.Controls.Add(lblMetodoT, 0, 3);
            layout.Controls.Add(cmbMetodo, 1, 3);

            layout.Controls.Add(panelBotones, 1, 4);

            this.Controls.Add(layout);
        }
    }
}