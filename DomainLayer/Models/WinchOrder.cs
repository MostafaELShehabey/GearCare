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
    public class WinchOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
       
       // [Range(0,5)]
       // public int Rate { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
     
        public string DriverId { get; set; }
        
        public string ClientId  { get; set; }

        public string cartype { get; set; }
        public string location { get; set; }
        public string ProblemDescription { get; set; }
        [JsonIgnore]
        public ApplicationUser Client { get; set; }
        [JsonIgnore]
        public ApplicationUser Driver { get; set; }

        
    }
}
