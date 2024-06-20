using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class RepaireOrderOutDto
    {
        public string OrderId { get; set; }
        public string cartype { get; set; }
        public string location { get; set; }
        public string ProblemDescription { get; set; }

        public string ClientId { get; set; }

        public string ServiceProviderId { get; set; }


        public DateTime? Date { get; set; }

        public Status Status { get; set; }

        //Navigation Properity 
        public SellerDto Client { get; set; }
    }
}
