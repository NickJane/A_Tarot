using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Data.Framework.ModelMate
{
    public partial class TarotCardMate
    {

        public int CardID { get; set; }

        [Required(ErrorMessage = "CardName不能为空")]
        [StringLength(10)]
        public string CardName { get; set; }

        /// <summary>
        /// 图片文件夹路径. 工具正逆位来指定文件夹.
        /// 如果IsBackgroundCard=true则是牌的背面, 图片是塔罗牌正位文件夹的bg.jpg. 
        /// </summary>
        public string CardUrl { get; set; }
        /// <summary>
        /// 塔罗牌正false,逆true, 位
        /// </summary>
        public bool IsHandstand { get; set; }

        /// <summary>
        /// true, 则是牌的背面, 图片是塔罗牌正位文件夹的bg.jpg. 
        /// </summary>
        public bool IsBackgroundCard { get; set; }
    }
    public partial class CardFormationMate
    {

        public int CardFormationID { get; set; }

        [Required(ErrorMessage = "FormationName不能为空")]
        [StringLength(50)]
        public string FormationName { get; set; }

        [Required(ErrorMessage = "Describe不能为空")]
        [StringLength(500)]
        public string Describe { get; set; }

        [Required(ErrorMessage = "Popularity不能为空")]
        public int Popularity { get; set; }

        [Required(ErrorMessage = "State不能为空")]
        public int State { get; set; }

        /// <summary>
        /// 1 78,2 22,3 56
        /// </summary>
        [Required(ErrorMessage = "NeedAllCards不能为空")]
        public int NeedAllCards { get; set; }

        public int PlaceHeight { get; set; }
        public int PlaceWidth { get; set; }
        /// <summary>
        /// 是否绑定了主题
        /// </summary>
        public bool BindTag { get; set; }

        /// <summary>
        /// 此牌阵共几张牌.NeedAllCards是参与牌数.
        /// </summary>
        public int CardsCount { get; set; }

        /// <summary>
        /// 此牌阵从属于那些分类
        /// </summary>
        public IList<int> TagIDs { get; set; }
    }


}