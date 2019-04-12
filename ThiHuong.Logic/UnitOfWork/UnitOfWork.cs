using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework;
using ThiHuong.Framework.Models;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Logic.ExamService;

namespace ThiHuong.Logic
{
    public class UnitOfWork : IDisposable
    {

        private ThiHuongDbContext dbContext;
        private IBaseRepository<Exam> examRepository;
        private IBaseRepository<Question> questionRepository;
        private IBaseRepository<Account> accountRepository;
        private IBaseRepository<ExamDetail> examDetailRepository;
        private IBaseRepository<AccountInStage> accountInStageRepository;
        private IBaseRepository<Stage> stageRepository;

        private IBaseRepository<ResultDetail> resultDetailRepository;





        public UnitOfWork(ThiHuongDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IBaseRepository<ResultDetail> ResultDetailRepository
        {
            get
            {
                if (resultDetailRepository == null)
                {
                    resultDetailRepository = new BaseRepository<ResultDetail>(dbContext);
                }
                return resultDetailRepository;
            }
        }
        public IBaseRepository<Stage> StageRepository
        {
            get
            {
                if (stageRepository == null)
                {
                    stageRepository = new BaseRepository<Stage>(dbContext);
                }
                return stageRepository;
            }
        }
        public IBaseRepository<Exam> ExamRepository
        {
            get
            {
                if (examRepository == null)
                {
                    examRepository = new BaseRepository<Exam>(dbContext);
                }
                return examRepository;
            }
        }
        public IBaseRepository<Question> QuestionRepository
        {
            get
            {
                if (questionRepository == null)
                {
                    questionRepository = new BaseRepository<Question>(dbContext);
                }
                return questionRepository;

            }
        }
        public IBaseRepository<Account> AccountRepository
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = new BaseRepository<Account>(dbContext);
                }
                return accountRepository;

            }
        }
        public IBaseRepository<ExamDetail> ExamDetailRepository
        {
            get
            {
                if (examDetailRepository == null)
                {
                    examDetailRepository = new BaseRepository<ExamDetail>(dbContext);
                }
                return examDetailRepository;

            }
        }
        public IBaseRepository<AccountInStage> AccountInStageRepository
        {
            get
            {
                if (accountInStageRepository == null)
                {
                    accountInStageRepository = new BaseRepository<AccountInStage>(dbContext);
                }
                return accountInStageRepository;

            }
        }


        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
