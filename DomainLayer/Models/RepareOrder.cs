using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Models
{
    public class RepareOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public string OrderId { get; set; }

        [ForeignKey("ClientId")]
        public string ClientId { get; set; }
        public double Price { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ?Date { get; set; }

        public string ProblemDescription { get; set; }

        public Status Status { get; set; }

        //Navigation Properity 
        public ApplicationUser Client { get; set; }


    }
    
}

