using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CatalogService.Core.Models
{
    public class Item
    {
        public Item()
        {
        }

        public int ItemId { get; set; }

        [Required]
        [StringLength(40)]
        public string ItemName { get; set; }

        public int? CategoryId { get; set; }
    }
}
