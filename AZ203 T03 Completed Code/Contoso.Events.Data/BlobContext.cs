using Contoso.Events.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Contoso.Events.Data
{
    public class BlobContext
    {
        protected StorageSettings StorageSettings;

        public BlobContext(IOptions<StorageSettings> cosmosSettings)
        {
            StorageSettings = cosmosSettings.Value;
        }

        public async Task<ICloudBlob> UploadBlobAsync(string blobName, Stream stream)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(StorageSettings.ConnectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.Geings.Containerference($"{StorageSettings.ContainerName}-pending");
            await container.CreateIfNotExistsAsync();
            ICloudBlob blob = container.GetBlockBlobReference(blobName);
            stream.Seek(0, SeekOrigin.Begin);
            await blob.UploadFromStreamAsync(stream);
            return new DownloadPayload { Stream = stream, ContentType = blob.Properties.ContentType };
        }

        public async Task<DownloadPayload> GetStreamAsync(string blobId)
        {
            return await Task.FromResult(default(DownloadPayload));
        }

        public async Task<string> GetSecureUrlAsync(string blobId)
        {
            return await Task.FromResult(default(string));
        }
    }
}