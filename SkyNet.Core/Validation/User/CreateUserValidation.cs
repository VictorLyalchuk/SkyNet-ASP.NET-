using FluentValidation;
using SkyNet.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Validation.User
{
    public class CreateUserValidation : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidation()
        {
            RuleFor(r => r.FirstName).NotEmpty().WithMessage("Field must not be empty");
            RuleFor(r => r.LastName).NotEmpty().WithMessage("Field must not be empty");
            RuleFor(r => r.Role).NotEmpty().WithMessage("Field must not be empty");
            RuleFor(r => r.Email).NotEmpty().WithMessage("Field must not be empty").EmailAddress().WithMessage("Invalid email address");
            RuleFor(r => r.Password).NotEmpty().WithMessage("Field must not be empty").MinimumLength(7).WithMessage("Password must be at least 7 characters").MaximumLength(128);
            RuleFor(r => r.ConfirmPassword).NotEmpty().WithMessage("Field must not be empty").MinimumLength(7).WithMessage("Password must be at least 7 characters").MaximumLength(128).Equal(p => p.Password);
        }
    }
}
