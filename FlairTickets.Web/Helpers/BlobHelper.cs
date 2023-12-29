using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FlairTickets.Web.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FlairTickets.Web.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        readonly BlobServiceClient _blobClient;

        public BlobHelper(IConfiguration configuration)
        {
            _blobClient = new BlobServiceClient(configuration["ConnectionStrings:Blob"]);
        }


        public async Task DeleteBlobAsync(string blobName, string containerName)
        {
            var container = await GetContainerAsync(containerName);

            await container.DeleteBlobAsync(blobName);
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(byte[] image, string containerName)
        {
            var stream = new MemoryStream(image);
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadStreamAsync(stream, containerName);
        }


        async Task<BlobContainerClient> GetContainerAsync(string containerName)
        {
            var container = _blobClient.GetBlobContainerClient(containerName);

            await container.CreateIfNotExistsAsync();

            await container.SetAccessPolicyAsync(PublicAccessType.Blob);

            return container;
        }

        async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            var guid = Guid.NewGuid();

            var container = await GetContainerAsync(containerName);

            await container.UploadBlobAsync(guid.ToString(), stream);

            stream.Close();

            return guid;
        }
    }
}
