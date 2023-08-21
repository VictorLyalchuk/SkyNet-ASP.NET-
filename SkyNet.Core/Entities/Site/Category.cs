using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Infrastructure.Entities
{
    public class Category
    {
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public List<Post> _Posts { get; set; }
    }
}
