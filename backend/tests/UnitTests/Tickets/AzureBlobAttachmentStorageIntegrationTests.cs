using Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Contracts.Storage;

namespace UnitTests.Tickets;

public sealed class AzureBlobAttachmentStorageIntegrationTests
{
    [AzuriteFact]
    public async Task Upload_read_and_delete_work_with_local_azurite()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:BlobStorage"] = "UseDevelopmentStorage=true"
                })
            .Build();
        var storage = new AzureBlobAttachmentStorage(
            configuration,
            NullLogger<AzureBlobAttachmentStorage>.Instance);
        var storagePath = $"integration/{Guid.NewGuid():N}.txt";
        var expectedContent = "Azurite streaming test";
        var uploaded = false;

        try
        {
            await using var content = new MemoryStream(
                System.Text.Encoding.UTF8.GetBytes(expectedContent));
            await storage.UploadAsync(
                storagePath,
                content,
                "text/plain",
                CancellationToken.None);
            uploaded = true;

            await using var downloaded = await storage.OpenReadAsync(
                storagePath,
                CancellationToken.None);
            using var result = new MemoryStream();
            await downloaded.CopyToAsync(result);

            Assert.Equal(
                expectedContent,
                System.Text.Encoding.UTF8.GetString(result.ToArray()));

            await storage.DeleteAsync(storagePath, CancellationToken.None);
            uploaded = false;
        }
        finally
        {
            if (uploaded)
            {
                await storage.DeleteAsync(storagePath, CancellationToken.None);
            }
        }
    }
}

public sealed class AzuriteFactAttribute : FactAttribute
{
    public AzuriteFactAttribute()
    {
        if (Environment.GetEnvironmentVariable(
                "RUN_AZURITE_INTEGRATION_TESTS") != "1")
        {
            Skip = "Set RUN_AZURITE_INTEGRATION_TESTS=1 to run the Azurite integration test.";
        }
    }
}
