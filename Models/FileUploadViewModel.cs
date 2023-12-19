using System.ComponentModel.DataAnnotations;

namespace MyMOVEItTask.Models;

public class FileUploadViewModel
{
    [Display(Name = "AccessToken", Prompt = "Bearer Access Token...")]
    public string AccessToken { get; set; }
    public IFormFile File { get; set; }
}
