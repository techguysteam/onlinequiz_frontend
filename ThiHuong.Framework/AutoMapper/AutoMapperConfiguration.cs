using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;

namespace ThiHuong.Framework.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        private static IMapper mapper;

        static AutoMapperConfiguration()
        {
            if (mapper == null)
            {
                mapper = Mapper.Instance;
            }
        }


        public static IMapper GetInstance()
        {
            if (mapper == null)
            {
                mapper = Mapper.Instance;
            }
            return mapper;
        }

        public static TDestination ToViewModel<TDestination>(this BaseEntity entity) where TDestination : BaseViewModel
        {
            return mapper.Map<TDestination>(entity);
        }

        public static TDestination ToEntity<TDestination>(this BaseViewModel viewModel) where TDestination : BaseEntity
        {
            return mapper.Map<TDestination>(viewModel);
        }


    }
}
