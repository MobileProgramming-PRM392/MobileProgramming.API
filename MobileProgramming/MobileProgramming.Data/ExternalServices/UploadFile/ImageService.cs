using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.ExternalServices.UploadFile;

public class ImageService : IImageService
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductImageRepository _productImageRepository;

    public ImageService(IConfiguration config, IUnitOfWork unitOfWork, IProductImageRepository productImageRepository)
    {
        _config = config;
        _unitOfWork = unitOfWork;
        _productImageRepository = productImageRepository;
    }

    public async Task<bool> DeleteBlob(string blobName)
    {
        try
        {
            BlobContainerClient blobContainerClient = GetBlobContainerClient();
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public BlobContainerClient GetBlobContainerClient()
    {
        string? containerName = _config["AzureStorageSettings:ContainerName"];
        string? connectionString = _config["AzureStorageSettings:ConnectionString"];
        BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
        return blobContainerClient;
    }

    public async Task<string?> UploadImage(string base64, string imageId)
    {
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = "image/png" // file type
        };
        BlobContainerClient blobContainerClient = GetBlobContainerClient();
        BlobClient blobClient = blobContainerClient.GetBlobClient(imageId);

        // Decode base64 string to byte array
        byte[] imageBytes = Convert.FromBase64String(base64);

        using (var memoryStream = new MemoryStream(imageBytes))
        {
            await blobClient.UploadAsync(memoryStream, httpHeaders);
        }

        string absPath = blobClient.Uri.AbsoluteUri;
        return absPath;
    }
}
