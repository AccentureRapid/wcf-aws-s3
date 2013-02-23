using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace S3Upload.Service.Contracts
{
    [DataContract]
    public class PhotoInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PhotoUrl { get; set; }
    }
}