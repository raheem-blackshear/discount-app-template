using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using DAL.Models.Questions;
using Microsoft.AspNetCore.Mvc;

namespace DiscountApp.Controllers
{
    public class QuestionController : ControllerBase
    {

   
       
        private IUnitOfWork _unitOfWork;
        public QuestionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult AddQuestion([FromBody] Question question)
        {
            try
            {
                if (question == null)
                {

                    return BadRequest("question object is null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid question object");
                }

              //  _unitOfWork.Store.CreateStore(store);

                return Ok(_unitOfWork.Store.Get(question.Id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

    }
}