namespace RestfulPrueba.Clases
{
    public class Models
    {

        // Objeto para un item del pedido
        public class ItemPedido
        {
            public string Producto { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
        }

        // Objeto para la solicitud del pedido (payload JSON)
        public class PedidoRequest
        {
            public int Estrato { get; set; } // Valor del estrato en Colombia
            public List<ItemPedido> Productos { get; set; }
        }

        // Objeto para la respuesta del API
        public class PedidoResponse
        {
            public decimal Subtotal { get; set; }
            public decimal CostoEnvio { get; set; }
            public decimal DescuentoAplicado { get; set; }
            public decimal TotalFinal { get; set; }
        }
    }
}
