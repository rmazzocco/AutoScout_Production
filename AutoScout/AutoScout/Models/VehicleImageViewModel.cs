using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScout.Models
{
    class VehicleImageViewModel
    {
        public int Id { get; set; }
        public string Base64String { get; set; }
        
        public VehicleImageViewModel()
        {

        }

        public VehicleImageViewModel(int id, string base64String)
        {
            Id = id;
            Base64String = base64String;
        }
    }
}
