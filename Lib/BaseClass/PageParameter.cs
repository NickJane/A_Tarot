using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.BaseClass
{
    public class PageParameter
    {
        public int PCurrentPageIndex { get; set; }
        public int PPageSize { get; set; }
        public int PRecordCount { get; set; }
        public string POrderField { get; set; }
        public bool PIsAsc { get; set; }
        /// <summary>
        /// key:value|key:value
        /// </summary>
        public string OtherCondition { get; set; }
        //public int PPageCount { get {
        //    return PRecordCount % PPageSize > 0 ? (PRecordCount / PPageSize + 1) : PRecordCount / PPageSize;
        //} }
    }
}
