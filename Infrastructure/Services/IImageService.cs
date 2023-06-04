using Microsoft.AspNetCore.Http;
namespace Infrastructure.Services;

public interface IImageService
{
    string CreateFile (string path,IFormFile file);
    bool DeleteFile (string folder,string filename);
}


