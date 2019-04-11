using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Logic.Validations
{
    public abstract class BaseValidation
    {
        protected UnitOfWork unitOfWork;

        protected BaseValidation(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public abstract bool IsExist(object Id);

        public abstract bool IsActive(object Id);

    }
}
