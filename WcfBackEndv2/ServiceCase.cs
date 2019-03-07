using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBackEndv2
{
    [DataContract]
    public class ServiceCase
    {
        public ServiceCase()
        {
            Posts = new List<ServiceCasePost>();
        }

        public int Id { get; set; } // sätts automatiskt av webbservicen vid sparning 

        [DataMember]
        // OBS: internal set förhindrar inte att CaseNr anges manuellt, men
        // är en fingevisning om att denna property kommer att skrivas över
        public int CaseNr { get; internal set; } // sätts automatiskt av webbservicen vid sparning 

        [DataMember]
        // OBS: internal set förhindrar inte att Date anges manuellt, men
        // är en fingevisning om att denna property kommer att skrivas över
        public DateTime Date { get; internal set; } // sätts automatiskt av webbservicen vid sparning 

        [DataMember]
        public int FlatNr { get; set; } // required 

        [DataMember]
        public string Name { get; set; } // required 

        [DataMember]
        public string ContactEmail { get; set; } // optional 

        [DataMember]
        public List<ServiceCasePost> Posts { get; set; }
    }
}