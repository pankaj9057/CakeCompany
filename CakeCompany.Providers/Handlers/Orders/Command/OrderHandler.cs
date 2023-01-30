using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class OrderCommandHandler : IRequestHandler<OrderCommandRequest,Unit>
    {
        private readonly IOrderProvider _orderProvider;
        public OrderCommandHandler(IOrderProvider orderProvider)
        {
            _orderProvider = orderProvider;
        } 

        public async Task<Unit> Handle(OrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderProvider.UpdateOrders(request.orders);
           return await Task.FromResult(Unit.Value);
        }
    }
}
