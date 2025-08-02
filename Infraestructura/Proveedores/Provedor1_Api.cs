using Dominio.Interfaces;
using Dominio.Modelos;
using System.Net.http.Json;

namespace Infraestructura.Proveedores;

public class Provedor1_Api : IProvedorTipoCambio
{
  private readonly HttpClient _httpClient;

  public Proverdor1_Api(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<RespuestaCambio?> ObtenerTipoCambioAsync(SolicitudCambio solicitud)
  {
    try
    {
      var cuerpo = new { from = solicitud.MonedaOrigen, to = solicitud.MonedaDestino, value = solicitud.Monto };
      var respuesta = await _http.PostAsJsonAsync("https://api.provedor1.com", cuerpo);
      response.EnsureSuccessStatusCode();

      var json = await respuesta.Content.ReadFromJsonAsync<RespuestaApi1>();
      return new RespuestaCambio("API1", json!.rate * solicitud.Monto);
    }
    catch
    {
      return null; // Manejo de errores simple, se puede mejorar
    }
  }

  public class RespuestaApi1
  {
    public decimal rate { get; set; }
  }
}