using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class CompleteSelerData
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string? PhotoId { get; set; }
        public string? IdPicture { get; set; }
        public UserType UserType { get; set; }

    }
}
