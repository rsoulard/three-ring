using DocumentComposition.Api.Results;
using DocumentComposition.Application.Binders;
using Microsoft.AspNetCore.Mvc;

namespace DocumentComposition.Api.Binders;


[Route("api/[controller]")]
[ApiController]
public class BindersController : ControllerBase
{

    /// <summary>
    /// Create a new binder.
    /// </summary>
    /// <param name="handler">The command handler for this operation.</param>
    /// <param name="mapper">The mapper that turns the request into a command.</param>
    /// <param name="request">The binder creation parameters.</param>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromServices] CreateBinderCommandHandler handler, [FromServices] CreateBinderCommandMapper mapper, [FromBody] CreateBinderRequest request)
    {
        return await mapper.Map(request)
            .BindAsync(handler.HandleAsync)
            .ToActionResult();
    }

    /// <summary>
    /// Retrieve a binder by its id.
    /// </summary>
    /// <param name="handler">The command handler for this operation.</param>
    /// <param name="mapper">The mapper that turns the request into a command.</param>
    /// <param name="id">The id for the binder to retrieve.</param>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BinderIdQueryHandler handler, [FromServices] BinderIdQueryMapper mapper, Guid id)
    {
        return await mapper.Map(id)
            .BindAsync(handler.HandleAsync)
            .ToActionResult();
    }
}
