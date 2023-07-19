using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscountApp.Controllers
{
    [Route("api/Upload")]
    [ApiController]
    public class UploadController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public UploadController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        [Route("Image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            try
            {
                if (image == null)
                {

                    return BadRequest("file is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid file");
                }
                return Ok(
                    new {
                        image = await _unitOfWork.Image.UploadImage(image)
                    }
                    );
;               

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }

    }
}