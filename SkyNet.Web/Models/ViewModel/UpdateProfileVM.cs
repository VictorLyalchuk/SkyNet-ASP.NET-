using SkyNet.Core.DTOs.User;

namespace SkyNet.Web.Models.ViewModel
{
    public class UpdateProfileVM
    {
        public UpdateUserDTO UserInfo { get; set; }
        public UpdatePasswordDTO UserPassword { get; set; }
    }
}
