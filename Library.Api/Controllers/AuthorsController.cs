using Library.Application.Features.Authors.Commands.CreateAuthor;
using Library.Application.Features.Authors.Commands.DeleteAuthor;

namespace Library.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class AuthorsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request)
        {
            var command = new CreateAuthorCommand
            (
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Nationality,
                request.Biography
            );

            var result = await mediator.Send(command);

            var response = result.Match<IActionResult>
            (
                success => CreatedAtAction
                (
                    nameof(GetById),
                    new { id = result.Value!.Id },
                    result.Value
                ),
                error => result.ValidationErrors.Length > 0
                    ? BadRequest(new { result.Error, result.ValidationErrors })
                    : Conflict(new { result.Error })
            );

            return response;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteAuthorCommand(id);
            var result = await mediator.Send(command);

            var response = result.Match<IActionResult>
            (
                NoContent,
                error => error.Contains("no existe")
                    ? NotFound(new { result.Error })
                    : Conflict(new { result.Error })
            );

            return response;
        }
    }
}
