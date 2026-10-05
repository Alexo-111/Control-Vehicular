using System;

namespace sistema1
{
    public class ReporteCiudadano
    {
        public string Folio { get; set; } = string.Empty;
        public string Ciudadano { get; set; } = string.Empty;
        public string PlacaReportada { get; set; } = string.Empty;
        public string TipoInfraccion { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Proceso, Atendido, Rechazado
        public DateTime FechaReporte { get; set; } = DateTime.Now;
    }
}