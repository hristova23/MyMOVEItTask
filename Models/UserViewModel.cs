using System.ComponentModel.DataAnnotations;

namespace MyMOVEItTask.Models;

public class UserViewModel
{
    [Display(Name = "Username", Prompt = "username")]
    public string Username { get; set; }

    [Display(Name = "Password", Prompt = "password")]
    public string Password { get; set; }
}
