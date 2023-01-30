using CakeCompany.Core.Dtos;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class OrderCommandRequest:IRequest<Unit>
    {
        public Order[] orders { get; set; }
    }
}
