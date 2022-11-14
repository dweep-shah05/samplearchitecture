using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HazGo.BuildingBlocks.Persistence.EF.SearchRepository
{
    public class SearchOptions<T>
    {
        public string Filter { get; set; }

        public string OrderBy { get; set; }

        public OrderByDirection OrderByDirection { get; set; }

        public int Skip { get; set; } = 0;


        public int Top { get; set; } = 1000;


        public string? SkipToken { get; set; }

        public string? GetContinuationToken()
        {
            if (SkipToken == null)
            {
                return null;
            }

            byte[] bytes = Convert.FromBase64String(SkipToken);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
