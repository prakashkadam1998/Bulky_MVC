using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(10)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Range(1, 100,ErrorMessage ="Value should be Between 1-100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
