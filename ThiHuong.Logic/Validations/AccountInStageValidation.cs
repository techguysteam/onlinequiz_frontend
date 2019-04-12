using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;

namespace ThiHuong.Logic.Validations
{
    public class AccountInStageValidation : BaseValidation
    {
        public AccountInStageValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsValidToSubmitAnswer(AccountInStage accountInStage)
        {
            return accountInStage != null && accountInStage.FinishTime == null && accountInStage.Point == null;
        }
        public override bool IsActive(object Id)
        {
            throw new NotImplementedException();
        }

        public bool IsAccountInStage(int accountId, int examId)
        {
            var accountInStage = this.unitOfWork.AccountInStageRepository.Get(a => a.AccountId == accountId && a.ExamId == examId).FirstOrDefault();
            return accountInStage != null;
        }

        public bool IsValidAccountInStageToEnroll(AccountInStage accountInStage)
        {
            return accountInStage.FinishTime == null && accountInStage.Point == null;
        }

        public override bool IsExist(object Id)
        {
            var accountInStage = this.unitOfWork.AccountInStageRepository.FindAsync(Id).Result;
            return IsExist(accountInStage);
        }

        public bool IsExist(AccountInStage accountInStage)
        {
            return accountInStage != null;
        }

    }
}
