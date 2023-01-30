using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class OrderHandler : IRequestHandler<OrderRequest, Order[]?>
    {
        private readonly IOrderProvider _orderProvider;
        public OrderHandler(IOrderProvider orderProvider)
        {
            _orderProvider = orderProvider;
        }
        public Task<Order[]?> Handle(OrderRequest request, CancellationToken cancellationToken)
        {
           return _orderProvider.GetLatestOrders();
        }
    }
}
