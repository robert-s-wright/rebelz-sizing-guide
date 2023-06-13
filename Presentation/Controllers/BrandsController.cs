using Application;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        // GET: api/Brands
        [HttpGet]
        public ActionResult Get()
        {
            List<BrandModel> brands = GlobalConfig.Connection.GetBrands_All();

            return Ok(brands);


        }

        // POST :api/Brands
        [HttpPost]

        public ActionResult<BrandModel> Post([FromBody] BrandModel brand)
        {

            GlobalConfig.Connection.AddNewBrand(brand);

            return Ok(brand);
        }

    }
}
