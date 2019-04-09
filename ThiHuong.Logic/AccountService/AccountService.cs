using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;
using ThiHuong.Framework;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Logic.BaseService;
using ThiHuong.Framework.Constants;
using ThiHuong.Logic;

namespace ThiHuong.Service
{
    public interface IAccountService : IBaseService<Account>
    {
        AccessTokenResponse Authen(UserAuthentication user);
        bool Register(UserRegisterdViewModel user);
    }

    public class AccountService : BaseService<Account>, IAccountService
    {
        private JwtSecurityTokenProvider jwtSecurityTokenProvider;

        public AccountService(UnitOfWork unitOfWork, JwtSecurityTokenProvider jwtSecurityTokenProvider) 
            : base(unitOfWork.AccountRepository, unitOfWork)
        {
            this.jwtSecurityTokenProvider = jwtSecurityTokenProvider;
        }

        public AccessTokenResponse Authen(UserAuthentication user)
        {
            var account = repository.Get(acc => acc.Username == user.Username).FirstOrDefault();
            AccessTokenResponse token = null;
            if(account != null)
            {
                var result = 
                    PasswordManipulation.VerifyPasswordHash(user.Password, account.PasswordHash, account.PasswordSalt);
                if (result)
                {
                    token = new AccessTokenResponse()
                    {
                        AccessToken = jwtSecurityTokenProvider.CreateAccesstoken(account),
                        Username = user.Username
                    };
                }
            }
            
            if(token == null)
            {
                throw new ThiHuongException("Account is not verify");
            }
            return token;
        }

        public bool Register(UserRegisterdViewModel user)
        {
            var account = user.ToEntity<Account>();
            try
            {
                byte[] hash, salt;
                PasswordManipulation.CreatePasswordHash(user.Password, out hash, out salt);
                account.PasswordHash = hash;
                account.PasswordSalt = salt;
                account.RoleId = RoleConstant.USER;
                this.repository.AddAsync(account);
                this.unitOfWork.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new ThiHuongException(ex.Message, ex);
            }
            return true;
        }
    }
}
