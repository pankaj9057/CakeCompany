using CakeCompany.Core.Enums;

namespace CakeCompany.Core.Dtos;

public record Order(string ClientName, DateTime EstimatedDeliveryTime, int Id, Cake Name, double Quantity);