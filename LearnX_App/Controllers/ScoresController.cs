using System.Security.Claims;
using LearnX_ApiIntegration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Authorize]
    public class ScoresController : Controller
    {

        private readonly IScoreApiClient _scoreService;

        public ScoresController(IScoreApiClient scoreService)
        {
  
            _scoreService = scoreService;
        }
        // GET: ScoresController
        public async Task<ActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userId, out var userIds);
            var scores = await _scoreService.GetScoreAsync(userIds);
            return View(scores);
        }

    }
}
