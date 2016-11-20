using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoScout.Models
{
    public class ImageRenderViewModel
    {
        public string Base64String { get; set; }


        public ImageRenderViewModel()
        {

        }

        public ImageRenderViewModel(string base64String)
        {
            Base64String = base64String;
        }
    }
}