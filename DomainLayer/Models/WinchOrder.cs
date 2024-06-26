﻿using System;
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
        [Key]
        public string OrderId { get; set; }
        public string cartype { get; set; }
        public string location { get; set; }
        public string ProblemDescription { get; set; }

        public string DriverId { get; set; }
        public string ClientId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Date { get; set; }
        public Status Status { get; set; }

        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        public string? ClientPhoto { get; set; }



        [ForeignKey("ClientId")]
        public ApplicationUser Client { get; set; }
       
        public ICollection<ApplicationUser_WinchOrder> ApplicationUserWinchOrders { get; set; }

    }
}
