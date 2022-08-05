namespace CakeCompany.Tests.Builders;

internal class OrderBuilder
{
    private int _id;
    private string _name;
    private DateTime _estimatedDateTime;
    private Cake _cake;
    private double _quantity;
    public OrderBuilder WithId(int Id)
    {
        _id = Id;
        return this;
    }

    public OrderBuilder WithClientName(string name)
    {
        _name = name;
        return this;
    }

    public OrderBuilder WithEstimatedDeliveryDate(DateTime estimatedDateTime)
    {
        _estimatedDateTime = estimatedDateTime;
        return this;
    }

    public OrderBuilder WithCake(Cake cake)
    {
        _cake = cake;
        return this;
    }

    public OrderBuilder WithQuantity(double quantity)
    {
        _quantity = quantity;
        return this;
    }

    public Order Build()
    {
        return new Order(_name, _estimatedDateTime, _id, _cake, _quantity);
    }
}
