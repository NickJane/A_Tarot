

using System.Text.RegularExpressions;
namespace Dottext.Framework.EntryHandling
{
    /// <summary> 
    /// Responsible for valiating and formatting the entry before it is save to the database.
    /// by Lei Han 
    /// This Factory should happen Synchronously 
    /// </summary> 
    public class HtmlFilterHandler 
    {
        public HtmlFilterHandler()
        {
            // 
            // TODO: Add constructor logic here 
            // 
        }

        #region IEntryFactoryHandler Members

        /// <summary> 
        /// Responsible for valiating and formatting the Comment before it is save to the database.
        /// This Factory should happen Synchronously 
        /// </summary> 
        /// <param name="entry"></param> 
        //public void Process(Dottext.Framework.Components.Entry entry)
        //{
        //    entry.Author = FilterAll(entry.Author);
        //    entry.Title = FilterAll(entry.Title);
        //    //……爱过滤什么过滤什么 
        //}

        public void Configure() { }

        #endregion

        #region 过滤功能代码
        public static string FilterAll(string content)
        {

            return FilterScript(FilterHtml(FilterObject(content)));

        }

        public static string FilterScript(string content)
        {

            string regexstr = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";

            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);

        }


        public static string FilterHtml(string content)
        {

            string newstr = FilterScript(content);

            string regexstr = @"<[^>]*>";

            return Regex.Replace(newstr, regexstr, string.Empty, RegexOptions.IgnoreCase);

        }



        public static string FilterObject(string content)
        {

            string regexstr = @"(?i)<Object([^>])*>(\w|\W)*</Object([^>])*>";

            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);

        }

        #endregion

    }
}
