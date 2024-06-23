using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class RepareOrderDto
    {
        
        public string ServiceProvierId { get; set; }
        public string cartype { get; set; }
        public string location { get; set; }
        public string ProblemDescription  { get; set; }
  
    }
}
