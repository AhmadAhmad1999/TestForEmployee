using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyriaTrustPlanning.Domain.Common
{
    public class FilterObject
    {
        public List<Filter>? Filters { get; set; }
    }

    public class Filter
    {
        public string? Key { get; set; }
        public string? Value { get; set; } = null!;
        public DateTimeRange? DateRange { get; set; }
    }
}
