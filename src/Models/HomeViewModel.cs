using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace League.Models
{
    public class HomeViewModel
    {
        public string? Message {get; set;}

        public FileContent? Echo {get;set;}
        public FileContent? Invert {get;set;}
        public string? Flatten {get; set;}

        public long? Sum {get;set;}
        public long? Multiply {get;set;}
    }
}