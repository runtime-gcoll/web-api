using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SwirlTheoryApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class NoauthController : ControllerBase
    {
        [Route("/noauth")]
        public IActionResult NoAuth() {
            return StatusCode(403);
        }
    }
}