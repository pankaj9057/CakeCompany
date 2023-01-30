using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using MediatR;

public class BakeCakeHandler : IRequestHandler<BakeCakeRequest, Product>
{
    private readonly ICakeProvider _cakeProvider;

    public BakeCakeHandler(ICakeProvider cakeProvider)
    {
        _cakeProvider = cakeProvider;
    }
    public async Task<Product> Handle(BakeCakeRequest request, CancellationToken cancellationToken)
    {
        return await _cakeProvider.Bake(request.order);
    }
}