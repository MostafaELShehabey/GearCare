using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class Response
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public object? Model { get; set; }
        public bool IsDone { get; set; }
        public List<string> ?Errors { get; set; }
        public List<string> ?Items { get; set; }
    }
}
