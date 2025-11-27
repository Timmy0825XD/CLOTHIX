using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GUI.Services
{
    public class VirtualTryOnApp
    {
        public class TryOnStats
        {
            public string Date { get; set; }
            public StatsData Stats { get; set; }
        }

        public class StatsData
        {
            public int Total { get; set; }
            public int Successful { get; set; }
            public int Failed { get; set; }
            public int Good_Quality { get; set; }
            public Dictionary<string, int> By_Type { get; set; }
        }

        public enum GarmentType
        {
            Upper,    // Camisas, blusas, tops (Fashn: "tops")
            Lower,    // Pantalones, faldas (Fashn: "bottoms")
            Dress     // Vestidos, monos (Fashn: "one-pieces")
        }

        public class GarmentTypeInfo
        {
            public string Name { get; set; }
            public List<string> Examples { get; set; }
            public string Recommendation { get; set; }
            public string Tip { get; set; }
            public string FashnCategory { get; set; }
        }

        public class VirtualTryOnService
        {
            private readonly HttpClient _httpClient;
            private readonly string _apiBaseUrl;

            public VirtualTryOnService(HttpClient httpClient)
            {
                _httpClient = httpClient;
                _apiBaseUrl = "http://localhost:8000";
                _httpClient.Timeout = TimeSpan.FromMinutes(5);
            }

            public async Task<bool> IsApiHealthyAsync()
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{_apiBaseUrl}/health");
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }

            public async Task<TryOnStats> GetStatsAsync()
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{_apiBaseUrl}/stats");
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadFromJsonAsync<TryOnStats>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error obteniendo estadísticas: {ex.Message}");
                    return null;
                }
            }

            private string GarmentTypeToString(GarmentType type)
            {
                return type switch
                {
                    GarmentType.Upper => "upper",
                    GarmentType.Lower => "lower",
                    GarmentType.Dress => "dress",
                    _ => "upper"
                };
            }

            /// <summary>
            /// Procesa el virtual try-on con archivos subidos (modo original)
            /// Compatible con Fashn API v1.6
            /// </summary>
            /// <param name="personImageStream">Stream de la imagen de la persona</param>
            /// <param name="personFileName">Nombre del archivo de la persona</param>
            /// <param name="garmentImageStream">Stream de la imagen de la prenda</param>
            /// <param name="garmentFileName">Nombre del archivo de la prenda</param>
            /// <param name="garmentType">Tipo de prenda (upper, lower, dress)</param>
            /// <param name="denoiseSteps">Pasos de denoise (20-40, default: 30) - NO usado en Fashn v1.6</param>
            /// <param name="seed">Seed para reproducibilidad (default: 42)</param>
            /// <param name="autoCrop">Auto-crop (default: false) - NO usado en Fashn v1.6</param>
            /// <param name="autoMask">Auto-mask (default: true) - NO usado en Fashn v1.6</param>
            /// <returns>Byte array de la imagen resultante</returns>
            public async Task<byte[]> ProcessTryOnAsync(
                Stream personImageStream,
                string personFileName,
                Stream garmentImageStream,
                string garmentFileName,
                GarmentType garmentType = GarmentType.Upper,
                int denoiseSteps = 30,
                int seed = 42,
                bool autoCrop = false,
                bool autoMask = true)
            {
                try
                {
                    using var content = new MultipartFormDataContent();

                    var personContent = new StreamContent(personImageStream);
                    personContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(personContent, "person_image", personFileName);

                    var garmentContent = new StreamContent(garmentImageStream);
                    garmentContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(garmentContent, "garment_image", garmentFileName);

                    content.Add(new StringContent(GarmentTypeToString(garmentType)), "garment_type");
                    content.Add(new StringContent(seed.ToString()), "seed");

                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}/tryon", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error del servidor: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en ProcessTryOnAsync: {ex.Message}");
                    throw;
                }
            }

            /// <summary>
            /// Procesa el virtual try-on usando URLs de imágenes (para usar con catálogo)
            /// Compatible con Fashn API v1.6
            /// </summary>
            /// <param name="personImageUrl">URL de la imagen de la persona</param>
            /// <param name="garmentImageUrl">URL de la imagen de la prenda (del catálogo)</param>
            /// <param name="garmentType">Tipo de prenda</param>
            /// <param name="denoiseSteps">NO usado en Fashn v1.6</param>
            /// <param name="seed">Seed para reproducibilidad</param>
            /// <param name="autoCrop">NO usado en Fashn v1.6</param>
            /// <param name="autoMask">NO usado en Fashn v1.6</param>
            /// <returns>Byte array de la imagen resultante</returns>
            public async Task<byte[]> ProcessTryOnFromUrlsAsync(
                string personImageUrl,
                string garmentImageUrl,
                GarmentType garmentType = GarmentType.Upper,
                int denoiseSteps = 30,
                int seed = 42,
                bool autoCrop = false,
                bool autoMask = true)
            {
                try
                {
                    using var content = new MultipartFormDataContent();

                    content.Add(new StringContent(personImageUrl), "person_image_url");
                    content.Add(new StringContent(garmentImageUrl), "garment_image_url");
                    content.Add(new StringContent(GarmentTypeToString(garmentType)), "garment_type");
                    content.Add(new StringContent(seed.ToString()), "seed");

                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}/tryon", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error del servidor: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en ProcessTryOnFromUrlsAsync: {ex.Message}");
                    throw;
                }
            }

            /// <summary>
            /// Procesa el virtual try-on en modo mixto: archivo para persona, URL para prenda del catálogo
            /// Este es el método principal para usar con el catálogo
            /// Compatible con Fashn API v1.6
            /// </summary>
            /// <param name="personImageStream">Stream de la imagen de la persona (archivo subido)</param>
            /// <param name="personFileName">Nombre del archivo de la persona</param>
            /// <param name="garmentImageUrl">URL de la imagen de la prenda del catálogo</param>
            /// <param name="garmentType">Tipo de prenda</param>
            /// <param name="denoiseSteps">NO usado en Fashn v1.6</param>
            /// <param name="seed">Seed para reproducibilidad</param>
            /// <param name="autoCrop">NO usado en Fashn v1.6</param>
            /// <param name="autoMask">NO usado en Fashn v1.6</param>
            /// <returns>Byte array de la imagen resultante</returns>
            public async Task<byte[]> ProcessTryOnMixedAsync(
                Stream personImageStream,
                string personFileName,
                string garmentImageUrl,
                GarmentType garmentType = GarmentType.Upper,
                int denoiseSteps = 30,
                int seed = 42,
                bool autoCrop = false,
                bool autoMask = true)
            {
                try
                {
                    using var content = new MultipartFormDataContent();

                    // Agregar imagen de persona (archivo)
                    var personContent = new StreamContent(personImageStream);
                    personContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(personContent, "person_image", personFileName);

                    // Agregar URL de prenda del catálogo
                    content.Add(new StringContent(garmentImageUrl), "garment_image_url");

                    // Agregar parámetros
                    content.Add(new StringContent(GarmentTypeToString(garmentType)), "garment_type");
                    content.Add(new StringContent(seed.ToString()), "seed");

                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}/tryon", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error del servidor: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en ProcessTryOnMixedAsync: {ex.Message}");
                    throw;
                }
            }

            /// <summary>
            /// Obtiene información detallada sobre los tipos de prendas soportadas
            /// Actualizado para Fashn API v1.6
            /// </summary>
            public Dictionary<GarmentType, GarmentTypeInfo> GetGarmentTypesInfo()
            {
                return new Dictionary<GarmentType, GarmentTypeInfo>
                {
                    {
                        GarmentType.Upper,
                        new GarmentTypeInfo
                        {
                            Name = "Parte Superior",
                            Examples = new List<string> { "Camisas", "Blusas", "T-shirts", "Suéteres", "Chaquetas", "Tops" },
                            Recommendation = "Foto de medio cuerpo o torso visible",
                            Tip = "Funciona mejor con fotos de frente, brazos ligeramente separados del cuerpo",
                            FashnCategory = "tops"
                        }
                    },
                    {
                        GarmentType.Lower,
                        new GarmentTypeInfo
                        {
                            Name = "Parte Inferior",
                            Examples = new List<string> { "Pantalones", "Jeans", "Faldas", "Shorts", "Leggins" },
                            Recommendation = "⚠️ Foto de CUERPO COMPLETO requerida",
                            Tip = "IMPORTANTE: Requiere foto de cuerpo completo con piernas visibles y rectas. La persona debe estar de frente.",
                            FashnCategory = "bottoms"
                        }
                    },
                    {
                        GarmentType.Dress,
                        new GarmentTypeInfo
                        {
                            Name = "Vestido / Conjunto Completo",
                            Examples = new List<string> { "Vestidos", "Monos", "Jumpsuits", "Overoles", "Prendas de cuerpo entero" },
                            Recommendation = "⚠️ Foto de CUERPO COMPLETO requerida",
                            Tip = "Requiere foto de cuerpo completo, preferiblemente de frente con brazos ligeramente separados",
                            FashnCategory = "one-pieces"
                        }
                    }
                };
            }

            /// <summary>
            /// Resetea las estadísticas del servidor
            /// </summary>
            public async Task<bool> ResetStatsAsync()
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/stats");
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reseteando estadísticas: {ex.Message}");
                    return false;
                }
            }
        }
    }
}