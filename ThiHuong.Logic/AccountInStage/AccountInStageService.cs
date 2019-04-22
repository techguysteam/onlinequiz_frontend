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
using ThiHuong.Framework.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ThiHuong.Logic.AccountInStageService
{
    public interface IAccountInStageService : IBaseService<AccountInStage>
    {
        Task CreateAccountEnrollmentAsync(int accountId, int examId);
        BasePagination GetHallOfFamebyStageId(int stageId, int size = 10, int page = 0);
        List<ResultDetailForReview> GetResultDetailForReviews(int examId, int accountId);
    }

    public class AccountInStageService : BaseService<AccountInStage>, IAccountInStageService
    {
        private AccountInStageValidation accountInStageValidation;
        private AccountValidation accountValidation;

        public AccountInStageService(UnitOfWork unitOfWork)
            : base(unitOfWork.AccountInStageRepository, unitOfWork)
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

        public BasePagination GetHallOfFamebyStageId(int stageId, int size = 10, int page = 0)
        {
            var stageValidation = new StageValidation(unitOfWork);

            //check stageId in the system and already open
            var isValidStageId = stageValidation.IsPublicStage(stageId);
            if (!isValidStageId) throw new ThiHuongException(ErrorMessage.STAGE_NOT_PUBLIC);

            var topAccountInStage = this.unitOfWork.AccountInStageRepository.Get(accountInStage => accountInStage.Exam.StageId == stageId
                                                                                    , a => a.OrderByDescending(acc => acc.Point)
                                                                                    , includeProperties: "Account")
                                                   .Skip(size * page)
                                                   .Take(size)
                                                   .ToList();

            var totalAccountInStage = this.unitOfWork.AccountInStageRepository.Get().Count();

            var topAccounts = topAccountInStage.Select(accountInStage => accountInStage.Account);

            var topHallOfFame = topAccountInStage.Select(accountInStage =>
            {
                var hallOfFame = accountInStage.Account.ToViewModel<HallOfFameViewModel>();
                hallOfFame.Point = accountInStage.Point;
                hallOfFame.ExamName = accountInStage.Exam.Name;
                hallOfFame.StartTime = accountInStage.StartTime;
                hallOfFame.FinishTime = accountInStage.FinishTime;
                return hallOfFame;
            });

            var result = new BasePagination()
            {
                Content = topHallOfFame,
                Page = page,
                Size = size,
                Total = totalAccountInStage
            };

            return result;
        }

        public List<ResultDetailForReview> GetResultDetailForReviews(int examId, int accountId)
        {
            var examValidation = new ExamValidation(this.unitOfWork);

            //examId must be in the system
            if (! examValidation.IsExist(examId)) throw new ThiHuongException(ErrorMessage.EXAM_NOT_FOUND);

            // the account must take the exam
            var takenExam = this.repository.Get(a => a.AccountId == accountId && a.ExamId == examId).FirstOrDefault();
            if (takenExam == null) throw new ThiHuongException(ErrorMessage.EXAM_NOT_TAKEN);

            var resultDetails = takenExam.ResultDetail.ToList();

            var allResultDetailForReview = resultDetails.Select(rd =>
            {
                var question = this.unitOfWork.QuestionRepository.FindAsync(rd.QuestionId).Result;
                var resultDetailForReview = question.ToViewModel<ResultDetailForReview>();

                resultDetailForReview.Choice = rd.Choice;
                resultDetailForReview.IsCorrect = rd.IsCorrect;

                return resultDetailForReview;
            });

            return allResultDetailForReview.ToList();
        }
    }
}
