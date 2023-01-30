using CakeCompany.Core.Interfaces;
using MediatR;

public class CheckCakeHandler : IRequestHandler<CheckCakeRequest, DateTime>
{
    private readonly ICakeProvider _cakeProvider;

    public CheckCakeHandler(ICakeProvider cakeProvider)
    {
        _cakeProvider = cakeProvider;
    }
    public async Task<DateTime> Handle(CheckCakeRequest request, CancellationToken cancellationToken)
    {
        return await _cakeProvider.Check(request.order);
    }
}