using Dominio.Modelos;

namespace Dominio.Interfaces;


public interface IProvedorTipoCambio
{
  Task<RespuestaCambio?> ObtenerTipoCambioAsync(SolicitudCambio solicitud);
}