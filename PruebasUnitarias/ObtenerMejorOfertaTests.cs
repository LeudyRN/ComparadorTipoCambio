using App.CasosdeUso;
using Dominio.Interfaces;
using Dominio.Modelos;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class ObtenerMejorTipoCambioTests
{
    private class ProveedorMock : IProvedorTipoCambio
    {
        private readonly decimal _monto;
        private readonly bool _falla;
        private readonly string _nombreProveedor;

        public ProveedorMock(decimal monto, string nombreProveedor = "Mock", bool falla = false)
        {
            _monto = monto;
            _falla = falla;
            _nombreProveedor = nombreProveedor;
        }

        public Task<RespuestaCambio?> ObtenerTipoCambioAsync(SolicitudCambio solicitud)
        {
            if (_falla) return Task.FromResult<RespuestaCambio?>(null);
            return Task.FromResult<RespuestaCambio?>(new(_nombreProveedor, _monto));
        }
    }

    [Theory]
    [InlineData(5800, "ProveedorB", new object[] { 5700.0, 5800.0, 5500.0 })]
    [InlineData(6000, "ProveedorA", new object[] { 6000.0, 5900.0, 5800.0 })]
    [InlineData(5000, "ProveedorA", new object[] { 5000.0 })]
    public async Task RetornaLaMejorOferta(decimal expectedMonto, string expectedProveedor, object[] montosProveedoresObj)
    {
        var solicitud = new SolicitudCambio("USD", "DOP", 100);
        var caso = new ObtenerMejorTipoCambio();

        var montosProveedores = montosProveedoresObj.Cast<double>().Select(m => (decimal)m).ToArray();

        var proveedores = new List<IProvedorTipoCambio>();
        for (int i = 0; i < montosProveedores.Length; i++)
        {
            proveedores.Add(new ProveedorMock(montosProveedores[i], $"Proveedor{(char)('A' + i)}"));
        }

        if (montosProveedores.Length == 1)
        {
             expectedProveedor = $"Proveedor{(char)('A')}";
        }

        var mejor = await caso.EjecutarAsync(solicitud, proveedores);

        Assert.Equal(expectedMonto, mejor.MontoConvertido);
        Assert.Equal(expectedProveedor, mejor.Proveedor);
    }

    [Fact]
    public async Task IgnoraProveedoresQueFallen()
    {
        var solicitud = new SolicitudCambio("USD", "DOP", 100);
        var caso = new ObtenerMejorTipoCambio();

        var proveedores = new List<IProvedorTipoCambio>
        {
            new ProveedorMock(0, "ProveedorFallido", true),
            new ProveedorMock(5900, "ProveedorValido")
        };

        var mejor = await caso.EjecutarAsync(solicitud, proveedores);

        Assert.Equal(5900, mejor.MontoConvertido);
        Assert.Equal("ProveedorValido", mejor.Proveedor);
    }

    [Fact]
    public async Task LanzaErrorSiTodosFallan()
    {
        var solicitud = new SolicitudCambio("USD", "DOP", 100);
        var caso = new ObtenerMejorTipoCambio();

        var proveedores = new List<IProvedorTipoCambio>
        {
            new ProveedorMock(0, "ProveedorFallido1", true),
            new ProveedorMock(0, "ProveedorFallido2", true)
        };

        await Assert.ThrowsAsync<Exception>(() => caso.EjecutarAsync(solicitud, proveedores));
    }
}
