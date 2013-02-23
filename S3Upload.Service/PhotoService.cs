using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using S3Upload.Service.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace S3Upload.Service
{
    /// <summary>
    /// Service to upload photo s3.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PhotoService : IPhotoService
    {
        /// <summary>
        /// test memory store for uploaded photo info.
        /// </summary>
        List<PhotoInfo> photos = new List<PhotoInfo>();

        /// <summary>
        /// Get photo info for given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PhotoInfo GetPhoto(int id)
        {
            return photos.Find(p => p.Id == id);
        }

        /// <summary>
        /// Upload a photo to s3.
        /// </summary>
        /// <param name="photoUpload"></param>
        public void UploadPhoto(PhotoUpload photoUpload)
        {
            var sourceStream = photoUpload.FileByteStream;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    //and save to memory
                    const int bufferLen = 4096;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        memoryStream.Write(buffer, 0, count);
                    }

                    if (memoryStream.Length > 0)
                    {
                        //insert any file manipulation here, ex. create thumbnails of image

                        var photoUrl = UploadFileToS3(photoUpload.FileName, memoryStream);
                        photos.Add(new PhotoInfo
                        {
                            Id = photos.Count + 1,
                            PhotoUrl = photoUrl
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //handle any errors, ex. convert to FaultException
                throw ex;
            }
            finally
            {
                sourceStream.Close();
            }
        }

        /// <summary>
        /// Upload memory stream to S3.
        /// NOTE: bucket name should be all lowercase.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string UploadFileToS3(string fileName, MemoryStream stream)
        {
            //aws s3 configs
            string accessKey = "my_access_key";
            string secretKey = "my_secret_key";
            string bucketName = "my_bucket";
            string key = "S3Upload.Service/" + fileName;

            using (var client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey))
            {
                var ms = new MemoryStream();
                var putRequest = new PutObjectRequest();
                putRequest.WithBucketName(bucketName)
                          .WithCannedACL(S3CannedACL.PublicRead)
                          .WithKey(key)
                          .InputStream = stream;

                S3Response response = client.PutObject(putRequest);
                return string.Format("http://{0}.s3.amazonaws.com/{1}", bucketName, key);
            }
        }
    }
}