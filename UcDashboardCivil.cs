using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace sistema1
{
    public partial class UcDashboardCivil : UserControl
    {
        // Eventos públicos marcados como opcionales (?) para solucionar CS8618
        public event EventHandler? SolicitadoPagarInfraccion;
        public event EventHandler? SolicitadoRenovarLicencia;
        public event EventHandler? SolicitadoLevantarReporte;

        private Label lblReportes;
        private Label lblTramites;
        private Label lblTotalActividad;
        private DataGridView dgvActividadReciente;

        public UcDashboardCivil()
        {
            InitializeComponentCustom();
            ConstruirInterfaz();
            CargarDatosDashboard();
        }

        private void InitializeComponentCustom()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 246, 250);
        }

        private void ConstruirInterfaz()
        {
            this.Controls.Clear();

            // Panel Principal
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20),
                AutoScroll = true
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // Tarjetas resumidas
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F)); // Acciones Frecuentes
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Historial / DataGridView

            // 1. Tarjetas de Resumen
            TableLayoutPanel layoutTarjetas = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            layoutTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            layoutTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            layoutTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            Panel cardReportes = CrearTarjetaEstadistica("Denuncias Activas", "0", Color.FromArgb(192, 57, 43), out lblReportes);
            Panel cardTramites = CrearTarjetaEstadistica("Trámites en Curso", "0", Color.FromArgb(41, 128, 185), out lblTramites);
            Panel cardTotal = CrearTarjetaEstadistica("Total Actividades", "0", Color.FromArgb(39, 174, 96), out lblTotalActividad);

            layoutTarjetas.Controls.Add(cardReportes, 0, 0);
            layoutTarjetas.Controls.Add(cardTramites, 1, 0);
            layoutTarjetas.Controls.Add(cardTotal, 2, 0);

            // 2. Panel de Acciones Frecuentes
            GroupBox gbAcciones = new GroupBox
            {
                Text = "Acciones Frecuentes",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 10)
            };

            TableLayoutPanel layoutBotones = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(10)
            };
            layoutBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            layoutBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            layoutBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            // Crear botones de acceso directo
            Button btnPagarInfraccion = CrearBotonAcceso("Pagar Infracción / Multa", Color.FromArgb(192, 57, 43));
            Button btnRenovarLicencia = CrearBotonAcceso("Tramitar / Renovar Licencia", Color.FromArgb(41, 128, 185));
            Button btnLevantarReporte = CrearBotonAcceso("Levantar Denuncia Ciudadana", Color.FromArgb(106, 27, 41));

            // Conectar eventos Click a los delegados
            btnPagarInfraccion.Click += (s, e) => SolicitadoPagarInfraccion?.Invoke(this, EventArgs.Empty);
            btnRenovarLicencia.Click += (s, e) => SolicitadoRenovarLicencia?.Invoke(this, EventArgs.Empty);
            btnLevantarReporte.Click += (s, e) => SolicitadoLevantarReporte?.Invoke(this, EventArgs.Empty);

            layoutBotones.Controls.Add(btnPagarInfraccion, 0, 0);
            layoutBotones.Controls.Add(btnRenovarLicencia, 1, 0);
            layoutBotones.Controls.Add(btnLevantarReporte, 2, 0);

            gbAcciones.Controls.Add(layoutBotones);

            // 3. Tabla de Historial Reciente
            GroupBox gbHistorial = new GroupBox
            {
                Text = "Mis Últimos Reportes y Trámites",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill
            };

            dgvActividadReciente = new DataGridView
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
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
            };

            gbHistorial.Controls.Add(dgvActividadReciente);

            // Agregar secciones al Layout Principal
            mainLayout.Controls.Add(layoutTarjetas, 0, 0);
            mainLayout.Controls.Add(gbAcciones, 0, 1);
            mainLayout.Controls.Add(gbHistorial, 0, 2);

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
                Height = 8,
                BackColor = colorHeader
            };

            Label lblTituloCard = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Location = new Point(10, 15),
                AutoSize = true
            };

            lblValor = new Label
            {
                Text = valorInicial,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 38),
                AutoSize = true
            };

            card.Controls.Add(lblValor);
            card.Controls.Add(lblTituloCard);
            card.Controls.Add(header);

            return card;
        }

        private Button CrearBotonAcceso(string texto, Color colorFondo)
        {
            Button btn = new Button
            {
                Text = texto,
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = colorFondo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        public void CargarDatosDashboard()
        {
            ActualizarEstadisticas();
            CargarHistorialReciente();
        }

        // Cambiado de private a public para solucionar CS0122
        public void ActualizarEstadisticas()
        {
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;
            bool esAdmin = !string.IsNullOrEmpty(Sesion.Rol) && Sesion.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase);

            int reportesActivos = DatosFicticios.ReportesCiudadanos
                .Count(r => (esAdmin || r.Ciudadano.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase)) &&
                            !r.Estado.Equals("Resuelto", StringComparison.OrdinalIgnoreCase) &&
                            !r.Estado.Equals("Cancelado", StringComparison.OrdinalIgnoreCase));

            int tramitesEnCurso = DatosFicticios.Tramites
                .Count(t => (esAdmin || t.Solicitante.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase)) &&
                            !t.Estado.Equals("Finalizado", StringComparison.OrdinalIgnoreCase) &&
                            !t.Estado.Equals("Aprobado", StringComparison.OrdinalIgnoreCase) &&
                            !t.Estado.Equals("Rechazado", StringComparison.OrdinalIgnoreCase));

            int totalActividades = DatosFicticios.ReportesCiudadanos
                .Where(r => esAdmin || r.Ciudadano.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.Folio)
                .Concat(DatosFicticios.Tramites
                    .Where(t => esAdmin || t.Solicitante.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                    .Select(t => t.Folio))
                .Distinct()
                .Count();

            if (lblReportes != null) lblReportes.Text = reportesActivos.ToString();
            if (lblTramites != null) lblTramites.Text = tramitesEnCurso.ToString();
            if (lblTotalActividad != null) lblTotalActividad.Text = totalActividades.ToString();
        }

        private void CargarHistorialReciente()
        {
            string usuarioActual = string.IsNullOrEmpty(Sesion.Usuario) ? "Carlos Gómez" : Sesion.Usuario;
            bool esAdmin = !string.IsNullOrEmpty(Sesion.Rol) && Sesion.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase);

            // 1. Cargar Denuncias / Reportes
            var listaReportes = DatosFicticios.ReportesCiudadanos
                .Where(r => esAdmin || string.IsNullOrEmpty(Sesion.Usuario) || r.Ciudadano.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    Folio = r.Folio,
                    Tipo = "Denuncia Ciudadana",
                    Detalle = r.TipoInfraccion,
                    Estado = r.Estado,
                    Fecha = r.FechaReporte.ToString("dd/MM/yyyy HH:mm"),
                    FechaRaw = r.FechaReporte
                }).ToList();

            // 2. Cargar Trámites
            var listaTramites = DatosFicticios.Tramites
                .Where(t => esAdmin || string.IsNullOrEmpty(Sesion.Usuario) || t.Solicitante.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
                .Select(t => new
                {
                    Folio = t.Folio,
                    Tipo = t.Folio.StartsWith("REP-") ? "Denuncia Ciudadana" : "Trámite",
                    Detalle = t.TipoTramite,
                    Estado = t.Estado,
                    Fecha = t.FechaSolicitud.ToString("dd/MM/yyyy HH:mm"),
                    FechaRaw = t.FechaSolicitud
                }).ToList();

            // 3. Unir y ordenar por fecha más reciente
            var listaCombinada = listaReportes
                .Concat(listaTramites)
                .GroupBy(x => x.Folio)
                .Select(g => g.First())
                .OrderByDescending(x => x.FechaRaw)
                .Select(x => new
                {
                    Folio = x.Folio,
                    Tipo = x.Tipo,
                    Detalle = x.Detalle,
                    Estado = x.Estado,
                    Fecha = x.Fecha
                })
                .Take(10)
                .ToList();

            if (dgvActividadReciente != null)
            {
                dgvActividadReciente.DataSource = null;
                dgvActividadReciente.DataSource = listaCombinada;
            }
        }
    }
}