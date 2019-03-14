using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfBackEndv2
{
    [DataContract]
    public class ServiceCasePost
    {
        // Enbart för databasen. Syns inte i API:et
        public int Id { get; set; }

        // OBS: internal set förhindrar inte att Date anges manuellt, men
        // är en fingevisning om att denna property kommer att skrivas över
        [DataMember]
        public DateTime Date { get; internal set; } // sätts automatiskt av webbservicen vid sparning 

        [DataMember]
        public string UserDisplayName { get; set; } // obligatoriskt
        [DataMember]
        public string UserEmail { get; set; } // obligatoriskt
        [DataMember]
        public bool Private { get; set; } // obligatoriskt
        [DataMember]
        public string Message { get; set; } // obligatoriskt

        [DataMember]
        [NotMapped]
        // Enbart för API:et. Syns inte i databasen
        public List<string> ApiErrors { get; set; } // obligatoriskt

        public ServiceCasePost()
        {
            ApiError = new List<string>();
        }
    }
}