using S3Upload.Client.PhotoServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Upload.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileName = @"X:\Work\wcf-aws-s3\S3Upload.Client\s3.jpg";

                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var fileInfo = new FileInfo(fileName);

                    var photoUpload = new PhotoUpload();
                    photoUpload.FileName = fileInfo.Name;
                    photoUpload.Length = fileInfo.Length;
                    photoUpload.FileByteStream = stream;

                    IPhotoService photoService = new PhotoServiceClient();
                    photoService.UploadPhoto(photoUpload);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}
