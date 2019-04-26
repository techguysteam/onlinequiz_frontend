using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;

namespace ThiHuong.Logic.Validations.FluentValidation
{
    public class AccountValidation : AbstractValidator<Account>
    {
        private UnitOfWork unitOfWork;
        public AccountValidation(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            

        }



    }
}
