using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.Models;
using DAL;
using System.Linq;
using DiscountApp.ViewModels;

namespace DiscountApp.Controllers
{

    [Route("api/product")]
    [Authorize]
    public class ProductController: Controller
    {
        
        private IUnitOfWork _unitOfWork;


        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private int GetUserId()
        {
            var userid = ((System.Security.Claims.ClaimsIdentity)this.User.Identity).FindFirst("sub").Value;
            int Id = -1;
            var LstUser = _unitOfWork.User.GetAll().ToList();
            if (LstUser.Count > 0)
            {

                Id = LstUser.Where(p => p.AspNetUsersId == userid).FirstOrDefault().User_Id;
            }

            return Id;
        }


        [HttpGet, Route("search")]
        public IActionResult Search(string keyword)
        {
            if (keyword == null) {
                return NotFound("Keyword cannot be empty");
            }

            try
            {

                 var products = _unitOfWork.Products.Search(keyword);
                 return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet, Route("all")]
        public IActionResult GetAllProducts(int page = 0, int limit = 10)
        {
            ProductResultApi productResult = new ProductResultApi();


            try
            {
                var products = _unitOfWork.Products.GetAllProducts(page, limit, GetUserId());
                productResult.DataList = products.ToList();
                productResult.Result = "Success";
                return Ok(productResult);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
          
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _unitOfWork.Products.GetProductById(id, GetUserId());

                if (product == null)
                {
                   
                    return NotFound();
                }
                else
                {
                    
                    return Ok(product);
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
               
                    return BadRequest("Product object is null");
                }

                if (!ModelState.IsValid)
                {
                   
                    return BadRequest("Invalid Product object");
                }

                _unitOfWork.Products.CreateProduct(product);

                return Ok(_unitOfWork.Products.Get(product.Id));
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("AddFavorite")]
        [Authorize]
        public IActionResult FavoriteProduct(int Id)


        {
         
            try
            {
                if (Id == null)
                {

                    return BadRequest("Product ID cannot be null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid Product Id" +
                        "");
                }
             


                ProductUser productUser = new ProductUser();
                productUser.ProductId = Id;
                productUser.UserId = GetUserId();
                _unitOfWork.Products.FollowProduct(productUser);

                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("RemoveFavorite")]
        [Authorize]
        public IActionResult RemoveFavorite(int Id)


        {
          
            try
            {
              
                _unitOfWork.Products.RemoveProduct(Id, GetUserId());

                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet, Route("Favorites")]
        public IActionResult GetAllFavoriteProducts(int page = 0, int limit = 10)
        {
           
            ProductResultApi productResult = new ProductResultApi();


            try
            {
                var products = _unitOfWork.Products.GetFavoriteProducts(page, limit, GetUserId());
                productResult.DataList = products.ToList();
                productResult.Result = "Success";
                return Ok(productResult);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut]
        public IActionResult UpdateProduct([FromBody]Product product)

        {
            try
            {
                if (product == null)
                {

                    return BadRequest("Product object is null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid Product object");
                }

                _unitOfWork.Products.UpdateProduct(product);

                return Ok(_unitOfWork.Products.Get(product.Id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _unitOfWork.Products.GetProductById(id,GetUserId());
                if (product == null)
                {
                    
                    return NotFound();
                }

               // _unitOfWork.Products.DeleteProduct(product);

                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("New")]
        public IActionResult GetNewProduct(int page = 0, int limit = 10)
        {

            ProductResultApi productResult = new ProductResultApi();

            try
            {
                var products = _unitOfWork.Products.GetNewProducts(page, limit, GetUserId());
                productResult.DataList = products.ToList();
                productResult.Result = "Success";
                return Ok(productResult);
                  return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("OnSale")]
        public IActionResult GetSaleProduct(int page = 0, int limit = 10)
        {

            ProductResultApi productResult = new ProductResultApi();

            try
            {
                var products = _unitOfWork.Products.GetSaleProducts(page, limit, GetUserId());
                productResult.DataList = products.ToList();
                productResult.Result = "Success";
                return Ok(productResult);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
         
        }

        [HttpGet("Trending")]
        public IActionResult GetTrendingProduct(int page = 0, int limit = 10)
        {

            ProductResultApi productResult = new ProductResultApi();

            try
            {
                var products = _unitOfWork.Products.GetTrendingProducts(page, limit, GetUserId());
                productResult.DataList = products.ToList();
                productResult.Result = "Success";
                return Ok(productResult);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
         
        }
        [HttpGet("ItemList")]
        public IActionResult GetSaleTrendingNewProducts(int page = 0, int limit = 10,int Id = 2)
        {
            ProductViewModel viewModel = new ProductViewModel();
            try
            {
                var result = _unitOfWork.Products.GetSaleTrendingNewProducts(page, limit,GetUserId(), Id);
                viewModel.Sale = result.Item2;
                viewModel.Trending = result.Item3;
                viewModel.New = result.Item1;

                return Ok(viewModel);
                //  return Ok(new {viewModel.Sale,viewModel.Trending,viewModel.New });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Home")]
        public IActionResult GetHomePage(DateTime date, int page = 0, int limit = 10)
        {
            // date = DateTime.Now.Date;
            HomeViewModel viewModel = new HomeViewModel();
            try
            {
                var result = _unitOfWork.Products.GetHomePage(date.Date, page, limit,GetUserId());
                viewModel.Sale = result.Item1;
                viewModel.Trending = result.Item2;
                viewModel.Deal = result.Item3;
                return Ok(viewModel);
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
