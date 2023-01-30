using CakeCompany.Core.Dtos;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class TransportRequest : IRequest<Type>
    {
        public List<Product> products { get; set; }

    }
}
