using S3Upload.Service.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace S3Upload.Service
{
    [ServiceContract]
    public interface IPhotoService
    {
        [OperationContract]
        PhotoInfo GetPhoto(int id);

        [OperationContract]
        void UploadPhoto(PhotoUpload photoUpload);
    }
}
