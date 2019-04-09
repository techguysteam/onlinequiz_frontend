using AutoMapper;
using ThiHuong.Framework.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;

namespace ThiHuong.Framework.ViewModels
{
    public partial class BaseViewModel
    {
        

        protected IMapper mapper {
            get {
                return AutoMapperConfiguration.GetInstance();
            }
        }

        public TDestination ToEntity<TDestination>() where TDestination : BaseEntity
        {
            return mapper.Map<TDestination>(this);
        }
    }

}
