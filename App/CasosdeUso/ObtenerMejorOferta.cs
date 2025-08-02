using Dominio.Interfaces;
using Dominio.Modelos;

namespace App.CasosdeUso;

public class ObtenerMejorOferta
{
  public async Task<RespuestaCambio?> EjecutarAsync(
      SolicitudCambio solicitud,
      IEnumerable<IProvedorTipoCambio> proveedores)
  {
    var tareas = proveedores.Select(p => p.ObtenerTipoCambioAsync(solicitud));
    var resultados = await Task.WhenAll(tareas);

    var mejorOferta = resultados
        .Where(r => r != null)
        .OrderBy(r => r!.MontoConvertido)
        .FirstOrDefault();

    if (mejorOferta == null)
    {
      throw new Exception("Ningun Proveedor respondio");
    }

    return mejorOferta;
  }
}