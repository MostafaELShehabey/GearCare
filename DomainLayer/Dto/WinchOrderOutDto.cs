﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class WinchOrderOutDto
    {
        public string OrderId { get; set; }
        public string cartype { get; set; }
        public string location { get; set; }
        public string ProblemDescription { get; set; }

        public string ClientId { get; set; }

        public string DriverId { get; set; }


        public DateTime? Date { get; set; }

        public Status Status { get; set; }

        //Navigation Properity 
        public SellerDto Client
        {
            get; set;
        }
        public SellerDto Driver
        {
            get; set;
        }
    }
}
