namespace DocumentComposition.Shared.Results;
public record ValidationError(string PropertyName, string Message) : Error(Message);
