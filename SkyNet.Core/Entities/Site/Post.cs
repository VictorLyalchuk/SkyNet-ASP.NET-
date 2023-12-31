﻿using SkyNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Entities.Site
{
    public class Post : IEntity
    {
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public string Description { get; set; }
        [Required, MaxLength(100)]
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        [Required]
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public Category _Category { get; set; }
    }
}
