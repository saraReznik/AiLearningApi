using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using System; // Required for ArgumentNullException
using System.Collections.Generic;
using System.Linq;

namespace BL.Services
{
    public class CategoryManagement : IBLCategory
    {
        private readonly ICategory _category;

        public CategoryManagement(IDal dal)
        {
            _category = dal.Category;
        }

        // =================================================================
        //                         התיקון הקריטי כאן
        // =================================================================
        // שברנו את המעגליות על ידי כך שהפונקציה מחזירה רק את המידע
        // הדרוש עבור רשימת הקטגוריות, בלי לנסות לטעון את תתי-הקטגוריות
        // שגרמו ללולאה האינסופית בזמן ההמרה ל-JSON.
        public IEnumerable<BLCategory> GetAll()
        {
            return _category.GetAll().Select(c => new BLCategory
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
                // אנחנו לא כוללים את SubCategories בכוונה כדי למנוע את המעגליות
            });
        }
        // =================================================================

        public BLCategory Create(BLCategory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Category cannot be null.");
            }
            Category category = new Category
            {
                Name = entity.Name,
                Description = entity.Description
            };
            var createdCategory = _category.Create(category);
            entity.CategoryId = createdCategory.CategoryId;
            return entity;
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }
            _category.Delete(id);
        }

        public BLCategory Read(int id)
        {
            var c = _category.Read(id);
            if (c == null)
            {
                return null;
            }
            return new BLCategory
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                SubCategories = c.SubCategories?.Select(sc => new BLSubCategory
                {
                    SubCategoryId = sc.SubCategoryId,
                    Name = sc.Name,
                    CategoryId = sc.CategoryId
                }).ToList() ?? new List<BLSubCategory>()
            };
        }

        public void Update(BLCategory entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var category = new Category
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Description = entity.Description
            };
            _category.Update(category);
        }
    }
}