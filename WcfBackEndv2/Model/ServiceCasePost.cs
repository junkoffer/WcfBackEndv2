using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace WcfBackEndv2.Model
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
        public string Name { get; set; } // obligatoriskt
        [DataMember]
        public string ContactEmail { get; set; } // obligatoriskt
        [DataMember]
        public bool Private { get; set; } // obligatoriskt
        [DataMember]
        public string Message { get; set; } // obligatoriskt

        [DataMember]
        [NotMapped]
        // Enbart för API:et. Syns inte i databasen. 
        // Webservicen använder denna för att meddela ev. fel till den som använder servicen.
        public List<string> Errors { get; internal set; } 

        public ServiceCasePost()
        {
            Errors = new List<string>();
        }
    }
}