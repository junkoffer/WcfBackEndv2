using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WcfBackEndv2
{
    public class ServiceCasePost
    {
        public int Id { get; set; }

        public DateTime Date { get; internal set; } // sätts automatiskt av webbservicen vid sparning 

        public string Message { get; set; } // obligatoriskt
    }
}