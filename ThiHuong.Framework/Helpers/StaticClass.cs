﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;

namespace ThiHuong.Framework.Helpers
{
    public static partial class ClaimTypes
    {
        public const string UserId = "UserId";
        public const string Username = "Username";
    }

}

namespace ThiHuong.Framework
{
    public static class EntitiesToViewModelsMapping
    {
        public static List<TDest> ToListViewModel<TSource, TDest>(this List<TSource> entities)
            where TDest : BaseViewModel
            where TSource : BaseEntity
        {
            return entities.Select(e => e.ToViewModel<TDest>()).ToList();
        }

        public static List<TDest> ToListEntity<TSource, TDest>(this List<TSource> entities)
            where TSource : BaseViewModel
            where TDest : BaseEntity
        {
            return entities.Select(e => e.ToEntity<TDest>()).ToList();
        }
    }
}
