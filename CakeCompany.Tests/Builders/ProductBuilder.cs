namespace CakeCompany.Tests.Builders;

internal class ProductBuilder
{
    private Guid _id;
    private int _OrderId;
    private Cake _cake;
    private double _quantity;

    public ProductBuilder WithOrderId(int Id)
    {
        _OrderId = Id;
        return this;
    }

    public ProductBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithCake(Cake cake)
    {
        _cake = cake;
        return this;
    }

    public ProductBuilder WithQuantity(double quantity)
    {
        _quantity = quantity;
        return this;
    }
    public Product Build()
    {
        return new Product
        {
            Cake = _cake,
            OrderId = _OrderId,
            Quantity = _quantity,
            Id = _id
        };
    }
}
