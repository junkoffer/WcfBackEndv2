using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBackEndv2
{
    [DataContract]
    public class ServiceCase
    {
        public int Id { get; set; } // sätts automatiskt av webbservicen vid sparning 

        [DataMember]
        public int CaseNr { get; internal set; } // sätts automatiskt av webbservicen vid sparning 

        [DataMember]
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