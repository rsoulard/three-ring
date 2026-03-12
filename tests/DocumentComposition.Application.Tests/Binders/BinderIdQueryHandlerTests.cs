using DocumentComposition.Application.Binders;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;
using NSubstitute;

namespace DocumentComposition.Application.Tests.Binders;

public class BinderIdQueryHandlerTests
{
    private readonly IBinderReadRepository binderRepository;
    private readonly BinderIdQueryHandler queryHandler;

    public BinderIdQueryHandlerTests()
    {
        binderRepository = Substitute.For<IBinderReadRepository>();

        queryHandler = new(binderRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnBinder_WhenBinderExists()
    {
        var binderId = BinderId.Create();
        var binder = new BinderDto
        {
            Id = binderId.Value,
            Name = "Test Binder",
            Status = "Created",
            OutputUri = null
        };

        binderRepository.GetByIdAsync(Arg.Is(binderId)).Returns(Result.From(binder));

        var query = new BinderIdQuery
        {
            Id = binderId
        };

        var result = await queryHandler.HandleAsync(query);

        Assert.True(result.IsSuccessful);
        Assert.Equal(binderId.Value, result.Value.Id);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenBinderDoesNotExist()
    {
        var binderId = BinderId.Create();

        binderRepository.GetByIdAsync(Arg.Is(binderId)).Returns(Result.Failed<BinderDto>(new NotFoundError("Binder not found")));

        var query = new BinderIdQuery
        {
            Id = binderId
        };

        var result = await queryHandler.HandleAsync(query);

        Assert.False(result.IsSuccessful);
        Assert.IsType<NotFoundError>(result.Error);
    }
}
