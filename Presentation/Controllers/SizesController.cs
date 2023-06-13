using Application;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static Application.Services.Methods;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        // GET: api/Sizes
        [HttpGet]
        public ActionResult GetSizeNames()
        {
            List<string> sizeNames = SizeNamesToStringArray();

            return Ok(sizeNames);
        }

        // POST: api/Sizes
        [HttpPost]
        public ActionResult PostSizeNames([FromBody] List<SizeModel> sizes)
        {

            foreach (SizeModel size in sizes)
            {
                GlobalConfig.Connection.AddNewSize(size);
            }

            return Ok(sizes);
        }


    }
}
