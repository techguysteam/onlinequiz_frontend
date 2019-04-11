using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Logic.BaseService;
using ThiHuong.Framework.Models;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Framework;
using ThiHuong.Logic.Validations;
using ThiHuong.Framework.Constants;

namespace ThiHuong.Logic.AccountInStageService
{
    public interface IAccountInStageService : IBaseService<AccountInStage>
    {
        Task CreateAccountEnrollmentAsync(int accountId, int examId);
    }


    public class AccountInStageService : BaseService<AccountInStage>, IAccountInStageService
    {
        private AccountInStageValidation accountInStageValidation;
        private AccountValidation accountValidation;

        public AccountInStageService(IBaseRepository<AccountInStage> repository, UnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            this.accountInStageValidation = new AccountInStageValidation(this.unitOfWork);
            this.accountValidation = new AccountValidation(this.unitOfWork);
        }

        public async Task CreateAccountEnrollmentAsync(int accountId, int examId)
        {
            if (accountInStageValidation.IsAccountInStage(accountId, examId)) throw new ThiHuongException(ErrorMessage.ACCOUNT_IN_STAGE_ALREADY_EXIST);
            if (!accountValidation.IsExist(accountId)) throw new ThiHuongException(ErrorMessage.ACCOUNT_IS_NOT_EXIST);

            //if not => create account
            var account = await this.unitOfWork.AccountRepository.FindAsync(accountId);

            var accountInStage = new AccountInStage()
            {
                AccountId = accountId,
                ExamId = examId,
                StartTime = DateTime.UtcNow,
            };
            await this.repository.AddAsync(accountInStage);
            await this.unitOfWork.SaveChangesAsync();

        }
    }
}
