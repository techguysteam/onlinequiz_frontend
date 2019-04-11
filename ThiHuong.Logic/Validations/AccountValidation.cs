using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Constants;

namespace ThiHuong.Logic.Validations
{
    public class AccountValidation : BaseValidation
    {
        public AccountValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override bool IsActive(object Id)
        {
            var account = this.unitOfWork.AccountRepository.FindAsync(Id).Result;
            return account != null && (!account.Deleted ?? true);
        }

        public override bool IsExist(object Id)
        {
            var account = this.unitOfWork.AccountRepository.FindAsync(Id).Result;
            return account != null;
        }

        public bool IsExist(string username)
        {
            var account = this.unitOfWork.AccountRepository.Get(a => a.Username == username).FirstOrDefault();
            return account != null;
        }

        public bool IsPasswordValid(string password)
        {
            if (!string.IsNullOrEmpty(password) && password.Count() >= Constant.ValidPasswordLength)
            {
                return true;
            }
            return false;
        }

    }
}
