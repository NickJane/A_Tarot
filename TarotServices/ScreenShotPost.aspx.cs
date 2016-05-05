using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Framework.DataProvider;
using Lib.ClassExt;
using Data.MongoFramework;
using Lib.Utils;
using Lib.Data;
using MongoDB.Driver.Builders;
namespace TarotServices
{
    public partial class ScreenShotPost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TarotServices.Common.ValidIp(Request.UserHostAddress))
            {
                var id = Request.QueryString["id"];//帖子ID

                var post = MongoDBHelper.GetOne<Data.MongoFramework.ModelMate.M_Post>(CollectionName.Posts7Days, Query.EQ("_id",MongoDB.Bson.ObjectId.Parse( id)));

                Repeater1.DataSource = post.CardFormationInfo.CardInfos;
                Repeater1.DataBind();

                div_formation.Style.Add("width", post.CardFormationInfo.W + "px");
                div_formation.Style.Add("height", post.CardFormationInfo.H + "px");

                spanFormationName.InnerHtml = post.CardFormationInfo.CardFormationName;
            }
            else
            {

            }
        }
    }
}