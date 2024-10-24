using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Interfaces;

public interface IImageService
{
    public Task<string?> UploadImage(string base64, string imageId);

    //public Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId);
    public Task<bool> DeleteBlob(string blobName);
    public BlobContainerClient GetBlobContainerClient();
}
