namespace Acme.Application.Interfaces;

public interface IBlobStorageService
{
    string GetImage(string fileName);
    string UploadImage(string fileName, Stream stream);
}