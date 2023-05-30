
using System.ComponentModel.DataAnnotations;

namespace BizLandTemplate.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
