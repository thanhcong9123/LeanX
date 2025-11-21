using LearnX_Application.SystemService;
using LearnX_Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;
        public RolesController(IRoleService service)
        {
            _service = service;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(AppRole appRole)
        {
            var result = await _service.CreateRole(appRole);
            return Ok(result);
        }
    }
}
