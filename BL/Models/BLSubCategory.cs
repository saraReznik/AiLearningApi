using Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class BLSubCategory
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // מפתח רץ
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")] 
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }



    }
}

