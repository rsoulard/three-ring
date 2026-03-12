namespace DocumentComposition.Application.Binders;

public sealed class BinderDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Status { get; init; } = null!;
    public Uri? OutputUri { get; init; }
}
