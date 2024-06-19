using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        //public string PictureURL { get; set; }
        [RegularExpression(@"\d+(\.\d{1,2})?")] // make number like 15.00 
        public double ? Price { get; set; }
        public string? Description { get; set; }
        public string CategoryId { get; set; }
     
    }
}
