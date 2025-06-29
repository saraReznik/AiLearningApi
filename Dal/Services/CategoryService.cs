using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class CategoryService : ICategory
    {
        private readonly DatabaseManager _context;

        public CategoryService(DatabaseManager db)
        {
            _context = db;
        }

        public Category? Create(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Category entity cannot be null.");

            _context.Categories.Add(entity);
            _context.SaveChanges();

            // החזרת האובייקט עצמו — EF כבר יעדכן את ה־ID שלו
            return entity;
        }

        public void Delete(int id)
        {
            var category = Read(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Category with ID {id} was not found.");
            }
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                .Include(c => c.SubCategories) // אם את רוצה לכלול תתי קטגוריות כבר כאן
                .ToList();
        }

        public Category? Read(int id)
        {
            return _context.Categories
                .Include(c => c.SubCategories) // לכלול גם תתי קטגוריות
                .FirstOrDefault(c => c.CategoryId == id);
        }

        public void Update(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!_context.Categories.Any(c => c.CategoryId == entity.CategoryId))
                throw new KeyNotFoundException($"Category with ID {entity.CategoryId} not found.");

            _context.Categories.Update(entity);
            _context.SaveChanges();
        }
    }
}
