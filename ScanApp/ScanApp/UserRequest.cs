
using System.ComponentModel.DataAnnotations;

namespace ScanApp
{
  public class UserRequest
  {
    [Required]
    public string Email { get; set; }

    [Required]
    public string AppId { get; set; }
  }
}
