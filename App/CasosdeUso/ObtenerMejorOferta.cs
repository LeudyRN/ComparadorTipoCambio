using Dominio.Interfaces;
using Dominio.Modelos;

namespace App.CasosdeUso;

public class ObtenerMejorTipoCambio
{
    public async Task<RespuestaCambio> EjecutarAsync(
        SolicitudCambio solicitud,
        IEnumerable<IProvedorTipoCambio> proveedores)
    {
        var tareas = proveedores.Select(p => p.ObtenerTipoCambioAsync(solicitud));
        var resultados = await Task.WhenAll(tareas);

        var mejoresResultados = resultados
            .Where(r => r != null)
            .Cast<RespuestaCambio>()
            .OrderBy(r => r.MontoConvertido)
            .ToList();

        if (!mejoresResultados.Any())
        {
            throw new Exception("Ningún proveedor respondió con una tasa válida.");
        }

        return mejoresResultados.First();
    }
}