namespace DocumentComposition.Shared.Results;

public record ConcurrencyError(string Message) : Error(Message);
