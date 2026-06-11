using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;

public record GetQuotesByBranchIdQuery(Guid BranchId);
