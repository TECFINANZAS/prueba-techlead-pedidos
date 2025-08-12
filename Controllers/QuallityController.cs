using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static RestfulPrueba.Clases.Models;
using static RestfulPrueba.Negocio.RNPedidos;

public class Quallity : Controller
{
    [Fact]
    public void Calcular_PedidoSinDescuento_CalculaCorrectamente()
    {
        // Arrange: Configuración del escenario de prueba
        var mockEstrategiaEnvio = new Mock<IEstrategiaDeEnvio>();
        mockEstrategiaEnvio.Setup(x => x.CalcularCostoEnvio(1)).Returns(3000M);
        var calculadora = new CalculadoraDeCosto(mockEstrategiaEnvio.Object);

        var request = new PedidoRequest
        {
            Estrato = 1,
            Productos = new List<ItemPedido>
            {
                new ItemPedido { Precio = 20000, Cantidad = 2 }, // Subtotal: 40.000
                new ItemPedido { Precio = 5000, Cantidad = 1 }   // Subtotal: 5.000
            }
        };

        // Act: Ejecución del método a probar
        var result = calculadora.Calcular(request);

        // Assert: Verificación de los resultados
        Assert.Equal(45000M, result.Subtotal);
        Assert.Equal(3000M, result.CostoEnvio);
        Assert.Equal(0M, result.DescuentoAplicado);
        Assert.Equal(48000M, result.TotalFinal);
    }

    [Fact]
    public void Calcular_PedidoConDescuento_AplicaCorrectamente()
    {
        // Arrange
        var mockEstrategiaEnvio = new Mock<IEstrategiaDeEnvio>();
        mockEstrategiaEnvio.Setup(x => x.CalcularCostoEnvio(3)).Returns(5000M);
        var calculadora = new CalculadoraDeCosto(mockEstrategiaEnvio.Object);

        var request = new PedidoRequest
        {
            Estrato = 3,
            Productos = new List<ItemPedido>
            {
                new ItemPedido { Precio = 50000, Cantidad = 2 }, // Subtotal: 100.000
                new ItemPedido { Precio = 5000, Cantidad = 2 }   // Subtotal: 10.000
            }
        };

        // Act
        var result = calculadora.Calcular(request);

        // Assert
        Assert.Equal(110000M, result.Subtotal);
        Assert.Equal(5000M, result.CostoEnvio);
        Assert.Equal(11000M, result.DescuentoAplicado); // 10% de 110.000
        Assert.Equal(104000M, result.TotalFinal);
    }
}