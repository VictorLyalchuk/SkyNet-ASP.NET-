using FluentValidation;
using SkyNet.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Validation.User
{
    public class LoginUserValidation : AbstractValidator<UserLoginDTO>
    {
        public LoginUserValidation()
        {
            RuleFor(r => r.Email).NotEmpty().WithMessage("Field must not be empty").EmailAddress().WithMessage("Invalid email address");
            RuleFor(r => r.Password).NotEmpty().WithMessage("Field must not be empty").MinimumLength(7).WithMessage("Password must be at least 7 characters").MaximumLength(128);
        }
    }
}
