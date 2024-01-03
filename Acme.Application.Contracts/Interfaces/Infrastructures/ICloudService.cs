namespace Acme.Application.Contracts.Interfaces.Infrastructures;

public interface ICloudService
{
    string GetImage(string fileName);
    string UploadImage(string fileName, Stream stream);
}