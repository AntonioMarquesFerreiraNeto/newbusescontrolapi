using Asp.Versioning;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/contracts-description")]
public class ContractDescriptionController(
    IContractDescriptionService _contractDescriptionService
) : ControllerBase
{
}
