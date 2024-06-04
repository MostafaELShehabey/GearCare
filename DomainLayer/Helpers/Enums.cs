using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DomainLayer.Helpers
{
    public class Enums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ServiceProviderType
        {
            Mechanic,
            Electrician,
            WinchDriver
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserType
        {  
            Mechanic,
            Electrician,
            WinchDriver,
            Client,
            Seller
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Status
        {
            Cancelled,          //0     cancelled 
            PendingApproval,    //1     waiting the aproval 
            inProgress,         //2     approved put didnt copmpleted 
            Comlpeted           //3     completed successfuly 
        }

       

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrderBy
        {
            time ,
            status 
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrderStatus
        {
            time,
            status
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrderAction
        {
            Accept,  // Accept the order
            Refuse,  // Refuse the order
            Cancel,  // Cancel the order
            Completed
        }
    }
}
