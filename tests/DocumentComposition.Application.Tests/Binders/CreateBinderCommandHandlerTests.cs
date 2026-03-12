using NSubstitute;
using DocumentComposition.Application.Binders;
using DocumentComposition.Application.Data;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Application.Tests.Binders;

public class CreateBinderCommandHandlerTests
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IBinderRepository binderRepository;
    private readonly CreateBinderCommandHandler commandHandler;

    public CreateBinderCommandHandlerTests()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.CommitWorkAsync().Returns(Result.Successful());

        binderRepository = Substitute.For<IBinderRepository>();
        binderRepository.AddAsync(Arg.Any<Binder>()).Returns(callInfo => Task.FromResult(Result.From(callInfo.Arg<Binder>())));

        commandHandler = new(unitOfWork, binderRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldAddAndCommit_WhenBinderIsValid()
    {
        var command = new CreateBinderCommand
        {
            Name = "Test Binder"
        };

        var result = await commandHandler.HandleAsync(command);

        await binderRepository.Received(1).AddAsync(Arg.Is<Binder>(binder =>
            binder.Name == command.Name
        ));

        await unitOfWork.Received(1).CommitWorkAsync();

        Assert.True(result.IsSuccessful);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenBinderIsNotValid()
    {
        var command = new CreateBinderCommand
        {
            Name = ""
        };

        var result = await commandHandler.HandleAsync(command);

        await binderRepository.DidNotReceiveWithAnyArgs().AddAsync(Arg.Any<Binder>());

        await unitOfWork.DidNotReceive().CommitWorkAsync();

        Assert.False(result.IsSuccessful);
        Assert.IsType<ValidationError>(result.Error);
    }
}
