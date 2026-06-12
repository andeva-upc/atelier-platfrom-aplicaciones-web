using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;

public record AddBatchToProductCommand(Guid ProductId, int Quantity, string Description);
