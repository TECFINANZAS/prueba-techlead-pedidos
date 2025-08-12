using static RestfulPrueba.Clases.Models;

namespace RestfulPrueba.Negocio
{
    public class RNPedidos
    {
        /// <summary>
        /// Interfaz para la estrategia de cálculo de envío.
        /// Permite implementar diferentes reglas de costo de envío.
        /// </summary>
        public interface IEstrategiaDeEnvio
        {
            decimal CalcularCostoEnvio(int estrato);
        }

        /// <summary>
        /// Implementación de la estrategia de envío basada en estrato socioeconómico (Colombia).
        /// </summary>
        public class EstrategiaEnvioPorEstrato : IEstrategiaDeEnvio
        {
            // Costos de envío por estrato
            private readonly Dictionary<int, decimal> costosPorEstrato = new Dictionary<int, decimal>
            {
                { 1, 3000M },
                { 2, 4000M },
                { 3, 5000M },
                { 4, 6000M },
                { 5, 8000M },
                { 6, 10000M }
            };

            public decimal CalcularCostoEnvio(int estrato)
            {
                if (costosPorEstrato.TryGetValue(estrato, out decimal costo))
                {
                    return costo;
                }

                // Puedes cambiar este comportamiento:
                // return 0M; // Si estrato inválido => envío gratis
                throw new ArgumentException($"Estrato {estrato} no es válido para el cálculo de envío.");
            }
        }

        /// <summary>
        /// Servicio que realiza el cálculo total del pedido, incluyendo subtotal, envío y descuentos.
        /// </summary>
        public class CalculadoraDeCosto
        {
            // Constantes para evitar "números mágicos"
            private const decimal MONTO_MINIMO_DESCUENTO = 100000M;
            private const decimal PORCENTAJE_DESCUENTO = 0.10M;

            private readonly IEstrategiaDeEnvio _estrategiaEnvio;

            public CalculadoraDeCosto(IEstrategiaDeEnvio estrategiaEnvio)
            {
                _estrategiaEnvio = estrategiaEnvio;
            }

            /// <summary>
            /// Calcula el costo total de un pedido, incluyendo el envío y los descuentos.
            /// </summary>
            /// <param name="request">El objeto de solicitud del pedido.</param>
            /// <returns>Objeto con el desglose de costos.</returns>
            public PedidoResponse Calcular(PedidoRequest request)
            {
                // Validar que el request sea válido
                if (request == null || request.Productos == null || !request.Productos.Any())
                {
                    throw new ArgumentException("La solicitud de pedido no contiene productos.");
                }

                // 1. Calcular subtotal de los productos
                decimal subtotal = request.Productos.Sum(p => p.Precio * p.Cantidad);

                // 2. Calcular costo de envío según el estrato
                decimal costoEnvio = _estrategiaEnvio.CalcularCostoEnvio(request.Estrato);

                // 3. Aplicar descuento si cumple el monto mínimo
                decimal descuentoAplicado = subtotal >= MONTO_MINIMO_DESCUENTO
                    ? subtotal * PORCENTAJE_DESCUENTO
                    : 0M;

                // 4. Calcular el total final
                decimal totalFinal = subtotal + costoEnvio - descuentoAplicado;

                return new PedidoResponse
                {
                    Subtotal = subtotal,
                    CostoEnvio = costoEnvio,
                    DescuentoAplicado = descuentoAplicado,
                    TotalFinal = totalFinal
                };
            }
        }
    }
}
