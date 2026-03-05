using HackTech_Service.DTOs;
using HackTech_Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class NavigationController : ControllerBase
{
    private readonly INavigationService _navigationService;

    public NavigationController(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [HttpPost("route")]
    public async Task<IActionResult> GetRoute([FromBody] RouteRequest request)
    {
        // Folosim serviciul injectat
        var path = await _navigationService.FindShortestPath(request.StartNodeId, request.EndNodeId);

        if (path == null || path.Count == 0)
            return NotFound("Nu s-a putut găsi un drum între aceste puncte.");

        return Ok(path);
    }
}