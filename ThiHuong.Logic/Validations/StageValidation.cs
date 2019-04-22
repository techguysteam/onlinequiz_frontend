using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Constants;
using ThiHuong.Framework.Models;

namespace ThiHuong.Logic.Validations
{
    public class StageValidation : BaseValidation
    {
        public StageValidation(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsPublicStage(Stage stage)
        {
            return stage != null && stage.Status.Equals(StatusConstant.PUBLIC_STAGE, StringComparison.OrdinalIgnoreCase) && stage.EndDate == null;
        }

        public bool IsPublicStage(int? stageId)
        {
            if (stageId != null)
            {
                var stage = this.unitOfWork.StageRepository.FindAsync(stageId).Result;
                return IsPublicStage(stage);
            }
            return false;
        }

        public bool IsPendingStage(Stage stage)
        {
            return stage != null && stage.Status.Equals(StatusConstant.PENDING_STAGE);
        }

        public bool IsPendingStage(object stageId)
        {
            var stage = this.unitOfWork.StageRepository.FindAsync(stageId).Result;
            return IsPendingStage(stage);
        }

        public override bool IsActive(object Id)
        {
            throw new NotImplementedException();
        }

        public override bool IsExist(object Id)
        {
            var stage = this.unitOfWork.StageRepository.FindAsync(Id).Result;
            return stage != null;
        }
    }
}
