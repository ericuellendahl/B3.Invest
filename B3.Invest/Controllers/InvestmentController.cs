using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace B3.Invest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvestmentController(
    ICalculateInvestmentUseCase calculateInvestmentUseCase,
    ILogger<InvestmentController> logger) : ControllerBase
{
    private readonly ICalculateInvestmentUseCase _calculateInvestmentUseCase = calculateInvestmentUseCase;
    private readonly ILogger<InvestmentController> _logger = logger;

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(InvestmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PostCalculate([FromBody] InvestmentRequest request)
    {
        try
        {
            var result = _calculateInvestmentUseCase.Execute(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação no cálculo de investimento: {Message}", ex.Message);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Investment", new[] { ex.Message } }
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao processar requisição de cálculo de investimento.");
            return StatusCode(500, "Erro interno no servidor");
        }
    }
}
