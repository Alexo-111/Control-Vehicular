using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema1
{
    public static class DatosFicticios
    {
        public static List<ReporteCiudadano> ReportesCiudadanos { get; set; } = new List<ReporteCiudadano>();
        public static List<Tramite> Tramites { get; set; } = new List<Tramite>();
        public static List<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
        public static List<Infraccion> Infracciones { get; set; } = new List<Infraccion>();
        public static List<Licencia> Licencias { get; set; } = new List<Licencia>();
        public static List<RegistroVehicular> RegistrosVehiculares { get; set; } = new List<RegistroVehicular>();

        public static int TotalTramitesEnProceso => Tramites.Count(t => t.Estado != null && t.Estado.Equals("En Proceso", StringComparison.OrdinalIgnoreCase));
        public static int TotalRobosUrgentes => ReportesCiudadanos.Count(r => r.TipoInfraccion != null && r.TipoInfraccion.IndexOf("Robo", StringComparison.OrdinalIgnoreCase) >= 0);

        static DatosFicticios()
        {
            CargarDatosIniciales();
        }

        public static void CargarDatosIniciales()
        {
            if (Vehiculos.Count == 0)
            {
                // Vehículos base requeridos
                Vehiculos.Add(new Vehiculo
                {
                    Placa = "YZA-123-A",
                    Propietario = "Carlos Gómez",
                    MarcaModelo = "Toyota Corolla",
                    Anio = 2021,
                    Serie = "1NXBR12E3M4567890",
                    Estado = "Activo",
                    FechaRegistro = DateTime.Now.AddMonths(-5),
                    NumMotor = "MTR-88231",
                    TipoCombustible = "Gasolina",
                    Origen = "Nacional",
                    Localidad = "Mérida",
                    Municipio = "Mérida",
                    Expedicion = DateTime.Now,
                    Vigencia = DateTime.Now.AddYears(1),
                    NoFolio = "FOL-00123"
                });

                Vehiculos.Add(new Vehiculo
                {
                    Placa = "ZCB-456-B",
                    Propietario = "María Fernández",
                    MarcaModelo = "Nissan Versa",
                    Anio = 2022,
                    Serie = "3N1AB7AP4KL123456",
                    Estado = "Activo",
                    FechaRegistro = DateTime.Now.AddMonths(-2),
                    NumMotor = "MTR-88231",
                    TipoCombustible = "Gasolina",
                    Origen = "Nacional",
                    Localidad = "Mérida",
                    Municipio = "Mérida",
                    Expedicion = DateTime.Now,
                    Vigencia = DateTime.Now.AddYears(1),
                    NoFolio = "FOL-00123"
                });

                Random random = new Random();
                string[] nombres = { "Carlos", "María", "José", "Ana", "Luis", "Fernanda", "Jorge", "Sofia", "Rodrigo", "Valeria", "Roberto", "Camila", "Daniela", "Alejandro" };
                string[] apellidos = { "Gómez", "Fernández", "García", "López", "Pérez", "Martínez", "Torres", "Hernández", "Díaz", "Vázquez", "Cruz", "Ramírez" };
                string[] marcasModelos = { "Toyota Corolla", "Nissan Versa", "Honda Civic", "Volkswagen Jetta", "Chevrolet Aveo", "Mazda 3", "Kia Rio", "Ford Fiesta", "Hyundai Accent", "Nissan Sentra" };
                string[] estados = { "Activo", "Activo", "Activo", "Activo", "Inactivo", "Con Reporte" };

                for (int i = 3; i <= 500; i++)
                {
                    string letrasPlaca = $"{(char)random.Next('A', 'Z' + 1)}{(char)random.Next('A', 'Z' + 1)}{(char)random.Next('A', 'Z' + 1)}";
                    int numPlaca = random.Next(100, 999);
                    char letraFin = (char)random.Next('A', 'Z' + 1);
                    string placaStr = $"{letrasPlaca}-{numPlaca}-{letraFin}";

                    string propietario = $"{nombres[random.Next(nombres.Length)]} {apellidos[random.Next(apellidos.Length)]}";
                    string modelo = marcasModelos[random.Next(marcasModelos.Length)];
                    int anio = random.Next(2012, 2026);
                    string estado = estados[random.Next(estados.Length)];
                    DateTime fecha = DateTime.Now.AddDays(-random.Next(1, 365 * 3));

                    Vehiculos.Add(new Vehiculo
                    {
                        Placa = placaStr,
                        Propietario = propietario,
                        MarcaModelo = modelo,
                        Anio = anio,
                        Serie = $"3N1AB7AP{random.Next(10, 99)}KL{random.Next(100000, 999999)}",
                        NumMotor = "MTR-88231",
                        TipoCombustible = "Gasolina",
                        Origen = "Nacional",
                        Localidad = "Mérida",
                        Municipio = "Mérida",
                        Expedicion = DateTime.Now,
                        Vigencia = DateTime.Now.AddYears(1),
                        NoFolio = "FOL-00123",
                        Estado = estado,
                        FechaRegistro = fecha
                    });
                }
            }

            if (ReportesCiudadanos.Count == 0)
            {
                ReportesCiudadanos.Add(new ReporteCiudadano
                {
                    Folio = "REP-2024-001",
                    TipoInfraccion = "Vehículo Mal Estacionado",
                    Ubicacion = "Av. Principal #123",
                    Ciudadano = "Carlos Gómez",
                    Estado = "Atendido",
                    FechaReporte = DateTime.Now.AddDays(-2)
                });
            }

            if (Tramites.Count == 0)
            {
                Tramites.Add(new Tramite
                {
                    Folio = "TRM-2024-001",
                    TipoTramite = "Renovación de Licencia de Conducir",
                    Solicitante = "Carlos Gómez",
                    Costo = 650.00m,
                    Estado = "En Proceso",
                    EstadoPago = "Pagado",
                    FechaSolicitud = DateTime.Now.AddDays(-5)
                });
            }
        }

        public static void ReiniciarDatos()
        {
            ReportesCiudadanos.Clear();
            Tramites.Clear();
            Vehiculos.Clear();
            Infracciones.Clear();
            Licencias.Clear();
            RegistrosVehiculares.Clear();

            CargarDatosIniciales();
        }
    }

    public class Tramite
    {
        public string Folio { get; set; } = "";
        public string TipoTramite { get; set; } = "";
        public string Solicitante { get; set; } = "";
        public decimal Costo { get; set; }
        public string Estado { get; set; } = "";
        public string EstadoPago { get; set; } = "";
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    }

    public class Infraccion
    {
        public string Folio { get; set; } = "";
        public string Conductor { get; set; } = "";
        public string Placa { get; set; } = "";
        public string Motivo { get; set; } = "";
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "";
        public string Agente { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.Now;
    }

    public class RegistroVehicular
    {
        public string Folio { get; set; } = "";
        public string Placa { get; set; } = "";
        public string Propietario { get; set; } = "";
        public string MarcaModelo { get; set; } = "";
        public int Anio { get; set; } = 2026;
        public string Serie { get; set; } = "";
        public string TipoTramite { get; set; } = "";
        public string Estado { get; set; } = "";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}