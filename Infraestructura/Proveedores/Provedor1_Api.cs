using Dominio.Interfaces;
using Dominio.Modelos;
using System.Net.Http.Json;

namespace Infraestructura.Proveedores;

public class Proveedor1_Api : IProvedorTipoCambio
{
    private readonly HttpClient _httpClient;

    public Proveedor1_Api(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RespuestaCambio?> ObtenerTipoCambioAsync(SolicitudCambio solicitud)
    {
        try
        {
            Console.WriteLine("Proveedor1_Api: Simulando respuesta...");
            await Task.Delay(100);

            decimal simulatedRate = 0.80m;
            decimal montoConvertido = solicitud.Monto * simulatedRate;

            return new RespuestaCambio("API1", montoConvertido);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Proveedor1_Api: Error al obtener tipo de cambio: {ex.Message}");
            return null;
        }
    }

    public class RespuestaApi1
    {
        public decimal rate { get; set; }
    }
}