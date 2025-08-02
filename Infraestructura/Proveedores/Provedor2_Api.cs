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
            var xml = new XElement("XML",
                new XElement("From", solicitud.MonedaOrigen),
                new XElement("To", solicitud.MonedaDestino),
                new XElement("Amount", solicitud.Monto)
            );

            var contenido = new StringContent(xml.ToString(), System.Text.Encoding.UTF8, "application/xml");
            var respuesta = await _httpClient.PostAsync("https://api.provedor2.com", contenido);
            respuesta.EnsureSuccessStatusCode();

            var contenidoTexto = await respuesta.Content.ReadAsStringAsync();
            var xmlRespuesta = XElement.Parse(contenidoTexto);
            // CAMBIO: Usar .Value para obtener el contenido del elemento
            var total = decimal.Parse(xmlRespuesta.Element("Result")!.Value);

            return new RespuestaCambio("API2", total);
        }
        catch
        {
            return null; // Manejo de errores simple, se puede mejorar
        }
    }
}