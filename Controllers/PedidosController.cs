using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static RestfulPrueba.Clases.Models;
using static RestfulPrueba.Negocio.RNPedidos;

[ApiController]
[Route("api/[controller]")]
[Authorize] // This protects all methods in the controller
public class PedidoController : ControllerBase
{
    private readonly CalculadoraDeCosto _calculadora;

    public PedidoController(CalculadoraDeCosto calculadora)
    {
        _calculadora = calculadora;
    }

    /// <summary>
    /// Procesa un pedido y calcula su costo total.
    /// </summary>
    /// <param name="request">El payload JSON con los detalles del pedido.</param>
    /// <returns>Una respuesta JSON con el desglose de costos.</returns>
    [HttpPost("calcular")]
    // The [Authorize] attribute is redundant here and has been removed
    public ActionResult<PedidoResponse> CalcularPedido([FromBody] PedidoRequest request)
    {
        if (request == null || request.Productos == null || !request.Productos.Any())
        {
            return BadRequest("El pedido no puede estar vacío.");
        }

        PedidoResponse response = _calculadora.Calcular(request);

        return Ok(response);
    }
}