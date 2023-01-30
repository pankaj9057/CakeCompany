using CakeCompany.Core.Dtos;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class OrderRequest:IRequest<Order[]>
    {
    }
}
