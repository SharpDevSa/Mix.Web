using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace DotOrg.Core.Helpers
{
    public static class AutoMapperExtensions
    {
        public static TOut MapTo<TOut>(this object from)
        {
            return Mapper.Map<TOut>(from);
        }

        public static IEnumerable<TOut> Project<TIn, TOut>(this IEnumerable<TIn> list)
        {
            return list.AsQueryable().ProjectTo<TOut>();
        }
    }
}
