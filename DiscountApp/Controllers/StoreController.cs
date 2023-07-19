using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using DiscountApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DiscountApp.Controllers
{
    [Produces("application/json")]
    [Route("api/store")]
    [Authorize]
    public class StoreController : ControllerBase
    {
       
        private IUnitOfWork _unitOfWork;
        public StoreController(IUnitOfWork unitOfWork)
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

        [HttpGet, Route("all")]
        public IActionResult GetAllStores(int page = 0, int limit = 10)
        {
           
            try
            {
                var stores = _unitOfWork.Store.GetAllStores(page, limit,GetUserId());
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet, Route("Featured")]
        public IActionResult GetFeaturedStores(int page = 0, int limit = 10)
        {

            try
            {
                var stores = _unitOfWork.Store.GetFeaturedStores(page, limit,  GetUserId());
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet, Route("Recommended")]
        public IActionResult GetRecommendedStores(int page = 0, int limit = 10)
        {
         
            try
            {
                var stores = _unitOfWork.Store.GetRecommendedStores(page, limit, GetUserId());
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet, Route("search")]
        public IActionResult Search(string keyword)
        {
            if (keyword == null)
            {
                return NotFound("Keyword cannot be empty");
            }

            try
            {

                var products = _unitOfWork.Store.Search(keyword);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetStoreById(int id)
        {
            try
            {
                var store = _unitOfWork.Store.GetStoreById(id, GetUserId());

                if (store == null)
                {

                    return NotFound();
                }
                else
                {

                    return Ok(store);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateStore([FromBody]Store store)
        {
            try
            {
                if (store == null)
                {

                    return BadRequest("store object is null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid store object");
                }

                _unitOfWork.Store.CreateStore(store);

                return Ok(_unitOfWork.Store.Get(store.Id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public IActionResult UpdateStore([FromBody]Store store)

        {
            try
            {
                if (store == null)
                {

                    return BadRequest("store object is null");
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest("Invalid store object");
                }

                _unitOfWork.Store.UpdateStore(store);

                return Ok(_unitOfWork.Store.Get(store.Id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(int id)
        {
            try
            {
                var store = _unitOfWork.Store.GetStoreById(id,GetUserId());
                if (store == null)
                {

                    return NotFound();
                }

                _unitOfWork.Store.DeleteStore(store);

                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("AddFavorite")]
        [Authorize]
        public IActionResult FavoriteStore(int Id)


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
          
             


                StoreUser storeUser = new StoreUser();
                storeUser.StoreId = Id;
                storeUser.UserId = GetUserId();
                _unitOfWork.Store.FollowStore(storeUser);

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

                _unitOfWork.Store.RemoveProduct(Id, GetUserId());

                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet, Route("Favorites")]
        public IActionResult GetAllFavoriteStores(int page = 0, int limit = 10)
        {

            try
            {
                var stores = _unitOfWork.Store.GetFavoriteStores(page, limit, GetUserId());
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet, Route("GetByCategoryId")]
        public IActionResult GetStoresByCategoryId(int Id ,int page = 0, int limit = 10)
        {

            try
            {
                var stores = _unitOfWork.Store.GetStoresByCategory(page, limit,Id, GetUserId());
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Home")]
        public IActionResult GetHomePage(int page = 0, int limit = 10)
        {
            // date = DateTime.Now.Date;
            StoreHomeViewModel viewModel = new StoreHomeViewModel();
            try
            {
                var result = _unitOfWork.Store.GetHomePage(page,limit,GetUserId());
                viewModel.Deals = result.Item1;
                viewModel.Recommended = result.Item2;
                viewModel.Featured = result.Item3;
                return Ok(viewModel);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}