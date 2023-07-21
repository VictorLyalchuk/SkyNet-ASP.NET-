using FluentValidation;
using SkyNet.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Validation.User
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserValidation()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(r => r.FirstName).NotEmpty().WithMessage("Field must not be empty");
            RuleFor(r => r.LastName).NotEmpty().WithMessage("Field must not be empty");
        }
    }
}
