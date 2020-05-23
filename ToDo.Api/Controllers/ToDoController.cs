using Microsoft.AspNetCore.Mvc;

namespace ToDo.Api.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        public ToDoController()
        {

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new { say = "hello" });
        }
    }
}