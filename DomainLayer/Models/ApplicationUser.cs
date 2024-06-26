using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser
    {
        //Basic data 
        public string Name { get; set; }
        public string Location { get; set; }
        [ForeignKey("userphotoId")]
        public string? PhotoId { get; set; } = null;
        public string? IdPicture { get; set; } = null;
        public string Password { get; set; }
        public bool Isbanned { get; set; }=false;
        public UserType UserType { get; set; }

        //user data 
        public string ?CarType { get; set; } = null;


        //Servise Provider Data 
        public int Rate { get; set; } = 0;
        public int NumberOfRates { get; set; } = 0;
       // public string? IdPhoto { get; set; } = null;
        public bool available { get; set; } = true;
        public List<string>? Spezilization { get; set; }= new List<string>();
        public List<string>? CarTypeToRepaire { get; set; } = null;

       
        //navigation properity 
       // [ForeignKey("userphotoId")]
        //public Photo photo { get; set; }
        public WinchDriver WinchDriver { get; set; }
        [JsonIgnore]
        public ICollection<RepareOrder_ApplicationUser> RepairOrder { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public ICollection<Product> Products { get; set; }
       // public ICollection<WinchOrder> ClientOrders { get; set; }
        //public ICollection<WinchOrder> DriverOrders { get; set; }
        public ICollection<ApplicationUser_WinchOrder> ApplicationUserWinchOrders { get; set; }


    }
}




