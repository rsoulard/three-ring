namespace DocumentComposition.Shared.Results;
public record InfrastructureError(string Message) : Error(Message);
