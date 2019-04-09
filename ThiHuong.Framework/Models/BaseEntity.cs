using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.AutoMapper;
using ThiHuong.Framework.ViewModels;

namespace ThiHuong.Framework.Models
{
    public class BaseEntity
    {
        protected IMapper mapper {
            get {
                return AutoMapperConfiguration.GetInstance();
            }
        }

        public TDestination ToViewModel<TDestination>() where TDestination : BaseViewModel
        {
            return mapper.Map<TDestination>(this);
        }

    }
}
