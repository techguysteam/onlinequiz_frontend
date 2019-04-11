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
using ThiHuong.Logic.Validations;

namespace ThiHuong.Service
{
    public interface IAccountService : IBaseService<Account>
    {
        AccessTokenResponse Authen(UserAuthentication user);
        Task<bool> Register(UserRegisterdViewModel user);
    }

    public class AccountService : BaseService<Account>, IAccountService
    {
        private JwtSecurityTokenProvider jwtSecurityTokenProvider;
        private AccountValidation accountValidation;

        public AccountService(UnitOfWork unitOfWork, JwtSecurityTokenProvider jwtSecurityTokenProvider)
            : base(unitOfWork.AccountRepository, unitOfWork)
        {
            this.jwtSecurityTokenProvider = jwtSecurityTokenProvider;
            this.accountValidation = new AccountValidation(this.unitOfWork);
        }

        public AccessTokenResponse Authen(UserAuthentication user)
        {
            var account = repository.Get(acc => acc.Username == user.Username, null, "Role").FirstOrDefault();
            AccessTokenResponse token = null;

            if (accountValidation.IsActive(account.Id))
            {
                var result =
                    PasswordManipulation.VerifyPasswordHash(user.Password, account.PasswordHash, account.PasswordSalt);
                if (result)
                {
                    token = new AccessTokenResponse()
                    {
                        AccessToken = jwtSecurityTokenProvider.CreateAccesstoken(account),
                        Username = user.Username,
                        Role = account.Role.Name
                    };
                }
            }

            if (token == null)
            {
                throw new ThiHuongException("Account is not verify");
            }
            return token;
        }

        public async Task<bool> Register(UserRegisterdViewModel user)
        {
            if (accountValidation.IsExist(user.Username))
                throw new ThiHuongException(ErrorMessage.ACCOUNT_ALREADY_EXIST);
            if (accountValidation.IsPasswordValid(user.Password))
                throw new ThiHuongException(ErrorMessage.PASSWORD_NOT_VALID);

            var account = user.ToEntity<Account>();
            try
            {
                byte[] hash, salt;
                PasswordManipulation.CreatePasswordHash(user.Password, out hash, out salt);
                account.PasswordHash = hash;
                account.PasswordSalt = salt;
                account.RoleId = RoleConstant.USER;
                account.Deleted = false;

                if (user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
                {
                    account.RoleId = RoleConstant.ADMIN;
                }

                await this.repository.AddAsync(account);
                await this.unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
