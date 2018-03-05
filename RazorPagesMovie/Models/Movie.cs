using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesMovie.Models
{
    /// <summary>
    /// 软件开发的一个关键原则称为DRY(即"不要自我重复")
    /// Razor页面和Entity Framework提供的验证支持是DRY原则的极佳示例
    /// 验证规则再模型类中的某处以声明方式指定，且在应用的所有位置强制执行
    /// </summary>
    public class Movie
    {
        public int ID { get; set; }
        [StringLength(60,MinimumLength =3)]
        [Required]
        public string Title { get; set; }
        [Display(Name="Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }
        [Range(1,100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(5)]
        [Required]
        public string Rating { get; set; }
    }
}
