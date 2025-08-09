using Microsoft.AspNetCore.Mvc;
using System;
namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }


        [HttpPost]
        public ActionResult ReceivePlatform()
        {
            Console.WriteLine("--> Received Platform via POST");
            return Ok("platforms");
        }
    }
}