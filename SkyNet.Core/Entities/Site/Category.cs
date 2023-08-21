using SkyNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Entities.Site
{
    public class Category : IEntity
    {
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public List<Post> _Posts { get; set; }
    }
}
