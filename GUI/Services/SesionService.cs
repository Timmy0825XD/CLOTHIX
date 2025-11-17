using ENTITY.Usuarios;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GUI.Services
{
    public class SesionService
    {
        // ⚠️ CAMBIO IMPORTANTE: Usar ProtectedSessionStorage en vez de ProtectedLocalStorage
        private readonly ProtectedSessionStorage _sessionStorage;
        private LoginResponseDTO? _usuarioActual;
        private const string STORAGE_KEY = "usuario_sesion";

        // ⚠️ CAMBIO: Inyectar ProtectedSessionStorage
        public SesionService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        /// <summary>
        /// Inicia sesión y guarda los datos del usuario
        /// </summary>
        public async Task IniciarSesion(LoginResponseDTO usuario)
        {
            _usuarioActual = usuario;
            // Guardar en SessionStorage (se borra al cerrar navegador/app)
            await _sessionStorage.SetAsync(STORAGE_KEY, usuario);
            Console.WriteLine($"✅ Sesión iniciada: {usuario.NombreCompleto} (ID: {usuario.IdUsuario})");
        }

        /// <summary>
        /// Cierra la sesión COMPLETAMENTE - limpia memoria y storage
        /// </summary>
        public async Task CerrarSesion()
        {
            try
            {
                // 1. Limpiar variable en memoria
                _usuarioActual = null;

                // 2. Eliminar de SessionStorage
                await _sessionStorage.DeleteAsync(STORAGE_KEY);

                Console.WriteLine("🚪 Sesión cerrada completamente - Storage limpiado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cerrar sesión: {ex.Message}");
                // Aún así limpiar la memoria
                _usuarioActual = null;
            }
        }

        /// <summary>
        /// Obtiene el usuario actual (con carga desde SessionStorage si es necesario)
        /// </summary>
        public async Task<LoginResponseDTO?> ObtenerUsuarioActual()
        {
            // Si ya está en memoria, devolverlo
            if (_usuarioActual != null)
            {
                return _usuarioActual;
            }

            // Intentar cargar desde SessionStorage
            try
            {
                var result = await _sessionStorage.GetAsync<LoginResponseDTO>(STORAGE_KEY);
                if (result.Success && result.Value != null)
                {
                    _usuarioActual = result.Value;
                    Console.WriteLine($"✅ Sesión recuperada desde SessionStorage: {_usuarioActual.NombreCompleto} (ID: {_usuarioActual.IdUsuario})");
                    return _usuarioActual;
                }
                else
                {
                    Console.WriteLine("ℹ️ No hay sesión guardada - Usuario debe iniciar sesión");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al recuperar sesión: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Obtiene el ID del usuario actual
        /// </summary>
        public async Task<int?> ObtenerIdUsuario()
        {
            var usuario = await ObtenerUsuarioActual();
            return usuario?.IdUsuario;
        }

        /// <summary>
        /// Verifica si hay un usuario logueado
        /// </summary>
        public async Task<bool> EstaLogueado()
        {
            var usuario = await ObtenerUsuarioActual();
            bool logueado = usuario != null;
            Console.WriteLine($"🔍 Verificación de sesión: {(logueado ? "Logueado" : "No logueado")}");
            return logueado;
        }

        /// <summary>
        /// Verifica si el usuario es admin
        /// </summary>
        public async Task<bool> EsAdmin()
        {
            var usuario = await ObtenerUsuarioActual();
            return usuario?.IdRol == 1;
        }
    }
}