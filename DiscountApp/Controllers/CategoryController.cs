using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.Models;
using DAL;

namespace DiscountApp.Controllers
{
    [Route("api/category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet, Route("all")]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = _unitOfWork.ProductCategory.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var category = _unitOfWork.ProductCategory.GetCategoryById(id);

                if (category == null)
                {

                    return NotFound();
                }
                else
                {

                    return Ok(category);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public IActionResult CreateCategory([FromBody]Category category)
        {
            try
            {
                if (category == null)
                {

                    return BadRequest("category object is null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid category object");
                }

                _unitOfWork.ProductCategory.CreateCategory(category);

                return Ok(_unitOfWork.ProductCategory.GetCategoryById(category.Id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public IActionResult UpdateCategory([FromBody]Category category)

        {
            try
            {
                if (category == null)
                {

                    return BadRequest("category object is null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid category object");
                }

                _unitOfWork.ProductCategory.UpdateCategory(category);

                return Ok(_unitOfWork.ProductCategory.GetCategoryById(category.Id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var category = _unitOfWork.ProductCategory.GetCategoryById(id);
                if (category == null)
                {

                    return NotFound();
                }

                _unitOfWork.ProductCategory.DeleteCategory(category);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }
    
}
}