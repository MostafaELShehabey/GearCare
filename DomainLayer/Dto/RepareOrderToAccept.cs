using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class RepareOrderToAccept
    {
        public string OrderId { get; set; }

        public string ClientId { get; set; }
        public double Price { get; set; }

        public DateTime? Date { get; set; }

        public string ProblemDescription { get; set; }

        public Status Status { get; set; }
    }
}
