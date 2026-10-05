using System;

namespace sistema1
{
    public class Licencia
    {
        public string NumeroLicencia { get; set; } = string.Empty;
        public string Conductor { get; set; } = string.Empty;
        public string TipoLicencia { get; set; } = "Automovilista (Tipo A)";
        public string Vigencia { get; set; } = "3 Años";
        public int Puntos { get; set; } = 12;
        public string Estado { get; set; } = "Activa"; // Activa, Suspendida, Vencida
        public DateTime FechaExpedicion { get; set; } = DateTime.Now;
    }
}