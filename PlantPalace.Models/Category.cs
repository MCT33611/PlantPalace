using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlantPalace.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30,ErrorMessage ="Maximun charecters are 30")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Tax(GST)")]
        [Range(1,100, ErrorMessage = "Only allow 1 to 100")]
        public int Tax { get; set; }
    }
}
