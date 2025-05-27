using System;
using System.Collections.Generic;

namespace TWeb48.Business.Helpers
{
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
    
    public class IndexViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}