using Azure.Storage.Blobs;


namespace sheargenius_backend.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobService(IConfiguration configuration)
        {
		        //We will need 3 things
		        //1. Our Connection String to our Blob Storage
		        //2. Fill in our Container Name
		        //3. Fill in our blob Service Client
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
				
				//We will create an Async to upload our files We Will pass through a FileStream and the File name
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            
            var blobClient = containerClient.GetBlobClient(fileName);
						
						//If the File name Exists we will have to overwrite it 
            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString(); // Return file URL
        }

    }

}