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
            Console.WriteLine("Provedor3_Api: Simulando respuesta...");
            await Task.Delay(200);

            decimal simulatedRate = 0.84m;
            decimal montoConvertido = solicitud.Monto * simulatedRate;

            return new RespuestaCambio("API3", montoConvertido);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Provedor3_Api: Error al obtener tipo de cambio: {ex.Message}");
            return null;
        }
    }

    public class RespuestaApi3
    {
        public int statusCode { get; set; }
        public string message { get; set; } = "";
        public Datos data { get; set; } = new();
        public decimal exchangeRate => data.total;
    }

    public class Datos
    {
        public decimal total { get; set; }
    }
}