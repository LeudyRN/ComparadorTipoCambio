using Dominio.Interfaces;
using Dominio.Modelos;
using System.Net.Http.Json;

namespace Infraestructura.Proveedores;

public class Provedor3_Api : IProvedorTipoCambio
{
    private readonly HttpClient _httpClient;

    public Provedor3_Api(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RespuestaCambio?> ObtenerTipoCambioAsync(SolicitudCambio solicitud)
    {
        try
        {
            var cuerpo = new
            {
                sourceCurrency = solicitud.MonedaOrigen,
                targetCurrency = solicitud.MonedaDestino,
                quantity = solicitud.Monto
            };

            var respuesta = await _httpClient.PostAsJsonAsync("https://api3.com", cuerpo);
            respuesta.EnsureSuccessStatusCode();

            var json = await respuesta.Content.ReadFromJsonAsync<RespuestaApi3>();

            return new RespuestaCambio("API3", json!.data.total * solicitud.Monto);
        }
        catch
        {
            return null; // Manejo de errores simple, se puede mejorar
        }
    }

    public class RespuestaApi3
    {
        public int statusCode { get; set; }
        public string message { get; set; } = "";
        public Datos data { get; set; } = new();
        // Asumiendo que el exchangeRate se obtiene de data.total
        public decimal exchangeRate => data.total;
    }

    // CAMBIO: Hacer la clase Datos pública
    public class Datos
    {
        public decimal total { get; set; }
    }
}