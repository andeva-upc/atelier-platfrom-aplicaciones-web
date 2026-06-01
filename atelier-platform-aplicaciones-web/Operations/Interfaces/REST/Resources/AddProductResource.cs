using System;
namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record AddProductResource(Guid ProductId, int Quantity, decimal UnitPrice);