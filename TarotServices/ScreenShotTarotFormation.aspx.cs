using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Framework.DataProvider;
using Lib.ClassExt;
namespace TarotServices
{
    public partial class ScreenShotTarotFormation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TarotServices.Common.ValidIp(Request.UserHostAddress))
            {
                var id = Request.QueryString["id"].ToInt(0);//牌阵ID 或者 帖子ID

                Repeater1.DataSource = CardFormationVisitor.GetBaseDbContext().CardFormationCore.Where(x => x.FormationID == id).ToList();
                Repeater1.DataBind();
                var model= CardFormationVisitor.GetInstance().GetModelByFunc(x => x.CardFormationID ==  id);
                div_formation.Style.Add("width", model.PlaceWidth+"px");
                div_formation.Style.Add("height", model.PlaceHeight + "px");

            }
            else
            { 
                
            }
        }
    }
}