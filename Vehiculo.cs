using System;

namespace sistema1
{
    public class Vehiculo
    {
        public string Placa { get; set; } = string.Empty;
        public string Propietario { get; set; } = string.Empty;
        public string MarcaModelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Serie { get; set; } = string.Empty;
        public string NumMotor { get; set; } = string.Empty;
        public string TipoCombustible { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public DateTime? Expedicion { get; set; }
        public DateTime? Vigencia { get; set; }
        public string NoFolio { get; set; } = string.Empty;
        public string Estado { get; set; } = "Vigente";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Constructor explícito sin parámetros para permitir la sintaxis de inicialización con llaves {}
        public Vehiculo() { }
    }
}