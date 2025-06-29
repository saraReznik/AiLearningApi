using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IBLSubCategory _subcategoryService;

        public SubCategoryController(IBL bl)
        {
            _subcategoryService = bl.SubCategory;
        }

        [HttpGet("byCategory/{id}")]
        public ActionResult<List<BLSubCategory>> GetAllByCategory(int id)
        {

            var fakeList = new List<BLSubCategory>
            {
                new BLSubCategory { SubCategoryId = 99, Name = "Debug SubCat 1", CategoryId = id },
                new BLSubCategory { SubCategoryId = 100, Name = "Debug SubCat 2", CategoryId = id }
            };

            return Ok(fakeList);
        }

        [HttpGet]
        public ActionResult<List<BLSubCategory>> GetAll()
        {
            try
            {
                var subcategories = _subcategoryService.GetAll();
                return Ok(subcategories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving subcategories: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<BLSubCategory> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");
            var sub = _subcategoryService.Read(id);
            if (sub == null)
                return NotFound($"Subcategory ID {id} not found.");
            return Ok(sub);
        }

        [HttpPost]
        public ActionResult<BLSubCategory> Create([FromBody] BLSubCategory subcategory)
        {
            if (subcategory == null)
                return BadRequest("Subcategory is null.");
            try
            {
                var created = _subcategoryService.Create(subcategory);
                return CreatedAtAction(nameof(GetById), new { id = created.SubCategoryId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating subcategory: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<BLSubCategory> Update([FromBody] BLSubCategory subcategory)
        {
            if (subcategory == null)
                return BadRequest("Subcategory is null.");
            try
            {
                _subcategoryService.Update(subcategory);
                return Ok(subcategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating subcategory: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");
            try
            {
                _subcategoryService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Subcategory ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting subcategory: {ex.Message}");
            }
        }
    }
}