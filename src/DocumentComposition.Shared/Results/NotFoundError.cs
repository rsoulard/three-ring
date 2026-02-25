namespace DocumentComposition.Shared.Results;

public record NotFoundError(string Message) : Error(Message);
