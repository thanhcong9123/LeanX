using LearnX_Application.SystemService;
using LearnX_Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppRoleController : ControllerBase
    {
       private readonly IRoleService _service;

        public AppRoleController(IRoleService context)
        {
            _service = context;
        }

        // GET: api/AppRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppRole>>> GetAppRoles()
        {
            var list = await _service.GetAll();

            return Ok(list);
        }
        // POST: api/AppRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppRole>> PostAppRole(AppRole appRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var add =  await _service.CreateRole(appRole);
            if (!add.IsSuccessed)
            {
                return BadRequest(add);
            }
            return Ok(add);
        }
    }
}
