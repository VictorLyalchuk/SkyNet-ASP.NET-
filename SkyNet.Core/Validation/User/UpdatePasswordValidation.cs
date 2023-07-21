using FluentValidation;
using SkyNet.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Validation.User
{
    public class UpdatePasswordValidation : AbstractValidator<UpdatePasswordDTO>
    {
        public UpdatePasswordValidation()
        {
            RuleFor(r => r.OldPassword).MinimumLength(7).WithMessage("Field must be 7 symbols").NotEmpty();
            RuleFor(r => r.NewPassword).MinimumLength(7).WithMessage("Field must be 7 symbols").NotEmpty();
            RuleFor(r => r.ConfirmPassword).MinimumLength(7).WithMessage("Field must be 7 symbols").Equal(r => r.NewPassword).NotEmpty();
        }
    }
}
