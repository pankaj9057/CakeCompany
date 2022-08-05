using CakeCompany.Core.Enums;

namespace CakeCompany.Core.Dtos;

public class Product
{
    public Guid Id { get; set; }
    public Cake Cake { get; set; }
    public double Quantity { get; set; }
    public int OrderId { get; set; }
}
