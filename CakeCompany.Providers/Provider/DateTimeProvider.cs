using CakeCompany.Core.Interfaces;

namespace CakeCompany.Providers.Provider;

/// <summary>
/// This provider will help to do operation related to DateTime.Now 
/// and mock date time 
/// So we can write unit test eaisly
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}
