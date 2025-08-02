using App.CasosdeUso;
using Dominio.Modelos;
using Infraestructura.Proveedores;
using Dominio.Interfaces;

var http = new HttpClient();
var solicitud = new SolicitudCambio("USD", "EUR", 3000);

var proveedores = new List<IProvedorTipoCambio>
{
    new Provedor1_Api(http),
    new Provedor2_Api(http),
    new Provedor3_Api(http)
};

var casoUso = new ObtenerMejorOferta();
var mejorOferta = await casoUso.EjecutarAsync(solicitud, proveedores);

if (mejorOferta != null)
{
    Console.WriteLine($"Mejor oferta: {mejorOferta.Proveedor} - Monto convertido: {mejorOferta.MontoConvertido}");
}
else
{
    Console.WriteLine("No se encontraron ofertas.");
}