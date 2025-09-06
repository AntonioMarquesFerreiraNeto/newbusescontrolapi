using BusesControl.Entities.Responses.v1;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Api.Controllers.v1
{
    [Route("api/v1/reports")]
    [ApiController]
    public class ReportController(
        IReportService _reportService
    ) : ControllerBase
    {
        /// <summary>
        /// Retorna financeiros recentes
        /// </summary>
        /// <response code="200">Retorna sucesso da requisição</response>
        /// <response code="401">Retorna erro de não autorizado</response>
        /// <response code="500">Retorna erro interno do servidor</response>
        [ProducesResponseType(typeof(IEnumerable<FinancialResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("financials/recents")]
        public async Task<IActionResult> GetRecentsFinancials([FromQuery] int quantities)
        {
            var response = await _reportService.GetFinancialsRecents(quantities);
            return Ok(response);
        }

        /// <summary>
        /// Retorna o comparativo financeiro baseado em mês e por tipo
        /// </summary>
        /// <response code="200">Retorna sucesso da requisição</response>
        /// <response code="401">Retorna erro de não autorizado</response>
        /// <response code="500">Retorna erro interno do servidor</response>
        [ProducesResponseType(typeof(IEnumerable<FinancialComparativeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("financials/comparative")]
        public async Task<IActionResult> GetYearlyComparative()
        {
            var response = await _reportService.GetYearlyComparativeAsync();
            return Ok(response);
        }
    }
}
