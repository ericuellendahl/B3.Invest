using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace B3.Invest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvestmentController(ICalculateInvestmentUseCase calculateInvestmentUseCase) : ControllerBase
{
    private readonly ICalculateInvestmentUseCase _calculateInvestmentUseCase = calculateInvestmentUseCase;

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(InvestmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PostCalculate([FromBody] InvestmentRequest request)
    {
        var result = _calculateInvestmentUseCase.Execute(request);
        return Ok(result);
    }
}
