using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Framework.ModelMate;
using Data.Framework.Model;

using Lib.Utils;
using Lib.ClassExt;
using Lib.BaseClass;
using BLL.Enums;
namespace BLL
{
    public class BLLTarotCard
    {
        private static bool[] updown = new bool[2] { true, false };
        /// <summary>
        /// 获得随即的供用户选择的背牌
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TarotCardMate> GetCardList(Func<TarotCard,bool> func)
        {
            var list = StaticData.TarotCard.AsEnumerable();

            list=list.Where(func);

            var cards = list.OrderBy(x => Guid.NewGuid()).Select(x => new TarotCardMate { 
                CardID=x.CardID,
                CardName=x.CardName,
                //CardUrl="../../content/images/tarotcard/bg.jpg",
                IsBackgroundCard=true,
                IsHandstand=updown.OrderBy(y=>Guid.NewGuid()).First()
            });//乱序

            return cards;
        }
    }
}
