using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class UpdateApplicationUserDataDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string CarType { get; set; }
       // public string? PhotoId { get; set; }//photo
        //  public string IdPicture { get; set; }
      
    }
}
