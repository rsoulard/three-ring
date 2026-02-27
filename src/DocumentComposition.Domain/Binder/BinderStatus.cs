namespace DocumentComposition.Domain.Binder;

public enum BinderStatus
{
    Created,
    DocumentsAdded,
    ReadyForComposition,
    Composing,
    Completed,
    Expired
}
