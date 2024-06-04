//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace DomainLayer.Models
//{
//    public class Service_Provider:IdentityUser 
//    {
//        [Key]
//        public string Id { get; set; }
//        [MaxLength(50)]
//        public string Name { get; set; }
//        public string Location { get; set; }
//        public string Photo { get; set; }
//        public int Rate { get; set; } = 0;
//        public string Spezilization { get; set; }
//        public string IdPhoto { get; set; }
//        public string CarTypeToRepaire { get; set; }
//        public bool available { get; set; }=true;
//        public string Password { get; set; }
//        public bool Isbanned { get; set; } = false;


//        // Navigation Properties
//        public ServiceProviderType ServiceProviderType { get; set; }

//        // Navigation Properties
//        public ICollection<RepareOrder> RepairOrder { get; set; }

//    }

    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum ServiceProviderType
    //{
    //    Mechanic,
    //    Electrrical,
    //    WinchDriver,
    //    Seller
    //}


