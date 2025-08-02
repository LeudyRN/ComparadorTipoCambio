using Dominio.interfaces;
using Dominio.Modelos;
using System.Xml.Ling;

namespace Infraestructura.Provedores;

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
        new XElement("Amoun", solicitud.Monto)
      );

      var contenido = new StringContent(xml.ToString(), System.Text.Encoding.UTF8, "application/xml");
      var respuesta = await _httpClient.PostAsync("https://api.provedor2.com", contenido);
      respuesta.EnsureSuccessStatusCode();

      var contenidoTexto = await respuesta.Content.ReadAsStringAsync();
      var xmlRespuesta = XElement.Parse(contenidoTexto);
      var total = decimal.Parse(xmlRespuesta.Element("Result")!.value);

      return new RespuestaCambio("API2", total);
    }
    catch
    {
      return null; // Manejo de errores simple, se puede mejorar
    }
  }
}