using Asp.Versioning;
using BusesControl.Entities.Requests.v1;
using BusesControl.Services.v1.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Utils;

namespace BusesControl.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v1/contacts")]
[ApiController]
public class ContactController(
    IContactService _contactService,
    IValidator<ContactCreateRequest> _contactCreateRequestValidator
) : ControllerBase
{
    /// <summary>
    /// Cria um novo contato
    /// </summary>
    /// <response code="204">Retorna sucesso da requisição</response>
    /// <response code="500">Retorna erro interno do servidor</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactCreateRequest request)
    {
        var validation = await ValidateModel.CheckIsValid(request, Request.Path, ModelState, _contactCreateRequestValidator);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        await _contactService.CreateAsync(request);
        return NoContent();
    }
}
