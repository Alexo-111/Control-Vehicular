using System;
using System.Collections.Generic;
using System.Text;

namespace sistema1
{
    public static class Sesion
    {
        public static string Usuario { get; set; } = string.Empty;
        public static string Rol { get; set; } = string.Empty;

        public static string NombreUsuario
        {
            get => Usuario;
            set => Usuario = value;
        }
    }
}
