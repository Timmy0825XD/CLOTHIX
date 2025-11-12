using ENTITY.Usuarios;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GUI.Services
{
    public class SesionService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private LoginResponseDTO? _usuarioActual;
        private const string STORAGE_KEY = "usuario_sesion";

        public SesionService(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        /// <summary>
        /// Inicia sesión y guarda los datos del usuario
        /// </summary>
        public async Task IniciarSesion(LoginResponseDTO usuario)
        {
            _usuarioActual = usuario;

            // Guardar en LocalStorage para persistencia
            await _localStorage.SetAsync(STORAGE_KEY, usuario);

            Console.WriteLine($"✅ Sesión iniciada: {usuario.NombreCompleto} (ID: {usuario.IdUsuario})");
        }

        /// <summary>
        /// Cierra la sesión
        /// </summary>
        public async Task CerrarSesion()
        {
            _usuarioActual = null;

            // Eliminar de LocalStorage
            await _localStorage.DeleteAsync(STORAGE_KEY);

            Console.WriteLine("🚪 Sesión cerrada");
        }

        /// <summary>
        /// Obtiene el usuario actual (con carga desde LocalStorage si es necesario)
        /// </summary>
        public async Task<LoginResponseDTO?> ObtenerUsuarioActual()
        {
            // Si ya está en memoria, devolverlo
            if (_usuarioActual != null)
            {
                return _usuarioActual;
            }

            // Intentar cargar desde LocalStorage
            try
            {
                var result = await _localStorage.GetAsync<LoginResponseDTO>(STORAGE_KEY);
                if (result.Success && result.Value != null)
                {
                    _usuarioActual = result.Value;
                    Console.WriteLine($"✅ Sesión recuperada desde LocalStorage: {_usuarioActual.NombreCompleto} (ID: {_usuarioActual.IdUsuario})");
                    return _usuarioActual;
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
            return usuario != null;
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