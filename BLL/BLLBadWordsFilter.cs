using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 敏感词过滤
    /// </summary>
    public class BLLBadWordsFilter
    {
        public static Lib.Utils.BadWordsFilter badWordsFilter;

        public static void init(string words) {
            badWordsFilter = new Lib.Utils.BadWordsFilter();
            foreach (var item in words.Split(';'))
            {
                if(!string.IsNullOrEmpty(item))
                badWordsFilter.AddKey(item);
            }
        }


        public static string Filter(string words, char maskChar = '*')
        {
            if (badWordsFilter == null) init(BLL.BLLSystemConfig.BadWords);

            return badWordsFilter.Replace(words, maskChar);
        }
        public static bool HasBadWord(string words) {
            if (badWordsFilter == null) init(BLL.BLLSystemConfig.BadWords);
            return badWordsFilter.HasBadWord(words);
        }
    }
}
