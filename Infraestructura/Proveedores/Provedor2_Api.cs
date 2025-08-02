using Dominio.Interfaces;
using Dominio.Modelos;
using System.Xml.Linq;

namespace Infraestructura.Proveedores;

public class Provedor2_Api : IProvedorTipoCambio
{
    private readonly HttpClient _httpClient;

    public Provedor2_Api(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RespuestaCambio?> ObtenerTipoCambioAsync(SolicitudCambio solicitud)
    {
        try
        {
            Console.WriteLine("Provedor2_Api: Simulando respuesta...");
            await Task.Delay(150);

            decimal simulatedRate = 0.86m;
            decimal montoConvertido = solicitud.Monto * simulatedRate;

            return new RespuestaCambio("API2", montoConvertido);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Provedor2_Api: Error al obtener tipo de cambio: {ex.Message}");
            return null;
        }
    }
}