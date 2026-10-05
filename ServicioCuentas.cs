using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;

namespace sistema1
{
    public class Cuenta
    {
        public string Usuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;   // "Oficial" o "Civil"
        public string Salt { get; set; } = string.Empty;  // Base64
        public string Hash { get; set; } = string.Empty;  // Base64
    }

    public enum ResultadoLogin
    {
        Correcto,
        UsuarioNoExiste,
        PasswordIncorrecta
    }

    public static class ServicioCuentas
    {
        private const int Iteraciones = 100_000;

        // Se guarda en %AppData%\sistema1\cuentas.json para que las cuentas
        // sobrevivan al cerrar la aplicación (DatosFicticios solo vive en memoria).
        private static readonly string RutaArchivo = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "sistema1", "cuentas.json");

        private static List<Cuenta> _cuentas = new List<Cuenta>();

        static ServicioCuentas()
        {
            Cargar();
        }

        public static bool Registrar(string usuario, string password, string rol, out string mensaje)
        {
            usuario = usuario.Trim();

            if (usuario.Length < 3)
            {
                mensaje = "El usuario debe tener al menos 3 caracteres.";
                return false;
            }

            if (password.Length < 6)
            {
                mensaje = "La contraseña debe tener al menos 6 caracteres.";
                return false;
            }

            if (_cuentas.Any(c => c.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase)))
            {
                mensaje = "Ese nombre de usuario ya está registrado.";
                return false;
            }

            byte[] salt = RandomNumberGenerator.GetBytes(16);

            _cuentas.Add(new Cuenta
            {
                Usuario = usuario,
                Rol = rol,
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(CalcularHash(password, salt))
            });

            Guardar();
            mensaje = "Cuenta creada correctamente.";
            return true;
        }

        public static ResultadoLogin Validar(string usuario, string password, out Cuenta? cuenta)
        {
            cuenta = _cuentas.FirstOrDefault(c => c.Usuario.Equals(usuario.Trim(), StringComparison.OrdinalIgnoreCase));

            if (cuenta == null)
                return ResultadoLogin.UsuarioNoExiste;

            byte[] salt = Convert.FromBase64String(cuenta.Salt);
            byte[] esperado = Convert.FromBase64String(cuenta.Hash);
            byte[] calculado = CalcularHash(password, salt);

            if (!CryptographicOperations.FixedTimeEquals(esperado, calculado))
            {
                cuenta = null;
                return ResultadoLogin.PasswordIncorrecta;
            }

            return ResultadoLogin.Correcto;
        }

        // ---------- Gestión de cuenta ----------

        public static string? ObtenerRol(string usuario)
        {
            return _cuentas.FirstOrDefault(c => c.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase))?.Rol;
        }

        public static bool CambiarUsuario(string usuarioActual, string password, string nuevoUsuario, out string mensaje)
        {
            nuevoUsuario = nuevoUsuario.Trim();

            if (Validar(usuarioActual, password, out Cuenta? cuenta) != ResultadoLogin.Correcto || cuenta == null)
            {
                mensaje = "La contraseña actual es incorrecta.";
                return false;
            }

            if (nuevoUsuario.Length < 3)
            {
                mensaje = "El nuevo usuario debe tener al menos 3 caracteres.";
                return false;
            }

            if (nuevoUsuario == cuenta.Usuario)
            {
                mensaje = "El nuevo usuario es igual al actual.";
                return false;
            }

            if (_cuentas.Any(c => c != cuenta && c.Usuario.Equals(nuevoUsuario, StringComparison.OrdinalIgnoreCase)))
            {
                mensaje = "Ese nombre de usuario ya está en uso.";
                return false;
            }

            string anterior = cuenta.Usuario;
            cuenta.Usuario = nuevoUsuario;
            Guardar();
            ActualizarReferencias(anterior, nuevoUsuario);

            mensaje = "Nombre de usuario actualizado correctamente.";
            return true;
        }

        public static bool CambiarPassword(string usuario, string passwordActual, string nuevaPassword, out string mensaje)
        {
            if (Validar(usuario, passwordActual, out Cuenta? cuenta) != ResultadoLogin.Correcto || cuenta == null)
            {
                mensaje = "La contraseña actual es incorrecta.";
                return false;
            }

            if (nuevaPassword.Length < 6)
            {
                mensaje = "La nueva contraseña debe tener al menos 6 caracteres.";
                return false;
            }

            if (nuevaPassword == passwordActual)
            {
                mensaje = "La nueva contraseña debe ser distinta a la actual.";
                return false;
            }

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            cuenta.Salt = Convert.ToBase64String(salt);
            cuenta.Hash = Convert.ToBase64String(CalcularHash(nuevaPassword, salt));
            Guardar();

            mensaje = "Contraseña actualizada correctamente.";
            return true;
        }

        public static bool Eliminar(string usuario, string password, out string mensaje)
        {
            if (Validar(usuario, password, out Cuenta? cuenta) != ResultadoLogin.Correcto || cuenta == null)
            {
                mensaje = "La contraseña es incorrecta.";
                return false;
            }

            _cuentas.Remove(cuenta);
            Guardar();

            mensaje = "Cuenta eliminada correctamente.";
            return true;
        }

        // Los datos de ejemplo se enlazan por nombre; al renombrar la cuenta,
        // se actualizan para que el usuario no pierda sus registros.
        private static void ActualizarReferencias(string anterior, string nuevo)
        {
            foreach (var r in DatosFicticios.ReportesCiudadanos)
                if (r.Ciudadano.Equals(anterior, StringComparison.OrdinalIgnoreCase)) r.Ciudadano = nuevo;

            foreach (var t in DatosFicticios.Tramites)
                if (t.Solicitante.Equals(anterior, StringComparison.OrdinalIgnoreCase)) t.Solicitante = nuevo;

            foreach (var v in DatosFicticios.Vehiculos)
                if (v.Propietario.Equals(anterior, StringComparison.OrdinalIgnoreCase)) v.Propietario = nuevo;

            foreach (var i in DatosFicticios.Infracciones)
                if (i.Conductor.Equals(anterior, StringComparison.OrdinalIgnoreCase)) i.Conductor = nuevo;

            foreach (var l in DatosFicticios.Licencias)
                if (l.Conductor.Equals(anterior, StringComparison.OrdinalIgnoreCase)) l.Conductor = nuevo;
        }

        private static byte[] CalcularHash(string password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteraciones, HashAlgorithmName.SHA256, 32);
        }

        private static void Cargar()
        {
            try
            {
                if (File.Exists(RutaArchivo))
                {
                    string json = File.ReadAllText(RutaArchivo);
                    _cuentas = JsonSerializer.Deserialize<List<Cuenta>>(json) ?? new List<Cuenta>();
                }
            }
            catch
            {
                _cuentas = new List<Cuenta>(); // Archivo dañado: se empieza vacío
            }
        }

        private static void Guardar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(RutaArchivo)!);
            string json = JsonSerializer.Serialize(_cuentas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RutaArchivo, json);
        }
    }
}