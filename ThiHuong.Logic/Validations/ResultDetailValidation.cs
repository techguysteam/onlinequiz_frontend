using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Logic.Validations
{
    public class ResultDetailValidation : BaseValidation
    {
        public ResultDetailValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }



        public override bool IsActive(object Id)
        {
            throw new NotImplementedException();
        }

        public override bool IsExist(object Id)
        {
            throw new NotImplementedException();
        }
    }
}
