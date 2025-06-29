﻿using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic; 
namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IBLCategory _categoryServiec;

        public CategoryController(IBL BL)
        {
            _categoryServiec = BL.Category;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BLCategory>> GetAll() 
        {
            try
            {
                var categories = _categoryServiec.GetAll();
                return Ok(categories); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("{id}")]
        public ActionResult<BLCategory> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID cannot be zero or negative.");
            }
            var category = _categoryServiec.Read(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(category);
        }

        [HttpPost]
        public ActionResult<BLCategory> Create(BLCategory category)
        {
            if (category == null)
            {
                return BadRequest("Category cannot be null.");
            }
            try
            {
                BLCategory bLCategory = _categoryServiec.Create(category);
                return CreatedAtAction(nameof(GetById), new { id = bLCategory.CategoryId }, bLCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<BLCategory> Update(BLCategory category)
        {
            if (category == null)
            {
                return BadRequest("Category cannot be null.");
            }
            try
            {
                _categoryServiec.Update(category);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID cannot be zero or negative.");
            }
            try
            {
                _categoryServiec.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}