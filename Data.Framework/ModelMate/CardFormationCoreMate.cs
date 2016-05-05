using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Data.Framework.ModelMate
{
    //在ef类上加上MetadataType特性即可
    public partial class CardFormationCoreMate
    {

        [Required(ErrorMessage = "ID不能为空")]
        public int ID { get; set; }

        [Required(ErrorMessage = "FormationID不能为空")]
        public int FormationID { get; set; }

        [Required(ErrorMessage = "IsHandstand不能为空")]
        public bool IsHandstand { get; set; }

        [Required(ErrorMessage = "IsPointCard不能为空")]
        public bool IsPointCard { get; set; }

        [Required(ErrorMessage = "X不能为空")]
        public int X { get; set; }

        [Required(ErrorMessage = "Y不能为空")]
        public int Y { get; set; }

        [Required(ErrorMessage = "SortIndex不能为空")]
        public int SortIndex { get; set; }

        [Required(ErrorMessage = "Describe不能为空")]
        [StringLength(50)]
        public string Describe { get; set; }

        [Required(ErrorMessage = "W不能为空")]
        public int W { get; set; }

        public int H { get; set; }
    }
}