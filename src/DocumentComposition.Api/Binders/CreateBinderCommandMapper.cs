using DocumentComposition.Application.Binders;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Binders;

public class CreateBinderCommandMapper
{
    public Result<CreateBinderCommand> Map(CreateBinderRequest request)
    {
        return Result.From(new CreateBinderCommand
        {
            Name = request.Name
        });
    }
}
