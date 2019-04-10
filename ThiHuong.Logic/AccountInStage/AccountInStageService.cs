using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Logic.BaseService;
using ThiHuong.Framework.Models;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Framework;

namespace ThiHuong.Logic.AccountInStageService
{
    public interface IAccountInStageService : IBaseService<AccountInStage>
    {
        void CreateAccountEnrollment(int accountId, int examId, int stageId);
    }


    public class AccountInStageService : BaseService<AccountInStage>, IAccountInStageService
    {
        public AccountInStageService(IBaseRepository<AccountInStage> repository, UnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public async void CreateAccountEnrollment(int accountId, int examId, int stageId)
        {
            //check account is already enroll
            var accountAlreadyEnroll = this.repository.Get(a => a.AccountId == accountId).FirstOrDefault();

            if (accountAlreadyEnroll != null 
                && accountAlreadyEnroll.FinishTime != null) throw new ThiHuongException("This account has already done exam");

            //if not => create account
            var account = await this.unitOfWork.AccountRepository.FindAsync(accountId);
            if (account != null)
            {
                var accountInStage = new AccountInStage()
                {
                    AccountId = accountId,
                    ExamId = examId,
                    StageId = stageId,
                    StartTime = DateTime.UtcNow,
                };
                this.repository.AddAsync(accountInStage);
            }
        }
    }
}
