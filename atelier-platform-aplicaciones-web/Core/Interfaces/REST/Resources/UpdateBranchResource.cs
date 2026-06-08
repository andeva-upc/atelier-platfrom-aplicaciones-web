namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record UpdateBranchResource(
    string Code,
    string Name,
    string Address,
    string Phone
);
