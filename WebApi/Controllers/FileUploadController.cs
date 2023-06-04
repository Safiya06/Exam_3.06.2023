using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class FileUploadController:Controller
{
    private readonly IWebHostEnvironment _webhostenvironment;

    public FileUploadController(IWebHostEnvironment webhostenvironment)
    {
        _webhostenvironment = webhostenvironment;
    }
    [HttpPost("UploadFile")]
    public string UploadFile(IFormFile file) 
    {
        var currentFolder = _webhostenvironment.WebRootPath;
        var fullpath = Path.Combine(currentFolder,"images",file.FileName);
        using (var stream = new FileStream(fullpath,FileMode.OpenOrCreate))
        {
            file.CopyTo(stream);
        }
        return fullpath;
    }
}