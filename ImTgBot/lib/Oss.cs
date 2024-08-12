 
//using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class Oss
    {
        public static void testOss()
        {
            var oss = new Oss();
            oss.UploadObject("bk1", "C:\\Users\\Administrator\\OneDrive\\圖片\\22fec20c765f43d6bcfab168e734f392~noop.png", "objnm1");
            oss.DownloadObject("bk1", "objnm1", "mmm1038.png");
        }
        public void UploadObject(string bucketName, string localFilePath, string objectName)
        {
         //   var storageClient = StorageClient.Create();
            using var fileStream = File.OpenRead(localFilePath);
            ObjectInfo objectInfo = UploadObjectToStorageClient(bucketName, objectName, null, fileStream);
            Console.WriteLine($"Uploaded {objectInfo.Name} to bucket {bucketName}.");
        }
      

        public void DownloadObject(string bucketName, string objectName, string localFilePath)
        {
           
            using var fileStream = File.OpenWrite(localFilePath);
             DownloadObjectFrmStorageClient(bucketName, objectName, fileStream);
            Console.WriteLine($"Downloaded {objectName} from bucket {bucketName} to {localFilePath}.");
        }

        public void DownloadObjectFrmStorageClient(string bucketName, string objectName, FileStream fileStream)
        {
            string filePath = $"bkss/{bucketName}/{objectName}";
            CopyFileToStream(filePath, fileStream);

        }
        private static void CopyFileToStream(string filePath, FileStream destinationStream)
        {
            // 使用 FileStream 读取源文件
            using var sourceStream = File.OpenRead(filePath);

            // 缓冲区用于读取和写入数据
            byte[] buffer = new byte[8192]; // 8KB 缓冲区大小
            int bytesRead;

            // 从源文件读取数据并写入到目标 FileStream
            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }
        }

        public ObjectInfo UploadObjectToStorageClient(string bucketName, string objectName, object value, FileStream fileStream)
        {
            string destinationFilePath = $"bkss/{bucketName}/{objectName}";
            using FileStream destinationStream = Wrt(fileStream, destinationFilePath);
            return new ObjectInfo(bucketName, objectName);
        }

        private static FileStream Wrt(FileStream fileStream, string destinationFilePath)
        {
            Mkdir4File(destinationFilePath);
            // 使用 FileStream 创建目标文件
            var destinationStream = File.OpenWrite(destinationFilePath);

            // 缓冲区用于读取和写入数据
            byte[] buffer = new byte[8192]; // 8KB 缓冲区大小
            int bytesRead;

            // 从源文件读取数据并写入到目标文件
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }

            return destinationStream;
        }
    }
}
