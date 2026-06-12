using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;

public record DeleteProductCommand(Guid ProductId);
