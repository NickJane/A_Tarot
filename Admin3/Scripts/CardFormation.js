var CardF = CardFormationJS = {
    AddCardPlaceToDiv: function (targetDiv, targetDivDesc, parms) {
        ///	<summary>
        ///    向目标div里面装入塔罗牌占位符. targetDiv塔罗牌容器,targetDivDesc说明容器 \n
        /// {x:left, y:top, index:牌阵的第几张牌, w:宽, h:高, desc:描述}   id规则为divTarotCard+parms.index和divTarotCardDesc+parms.index
        ///	</summary>
        ///	<returns type="void" />
        var p = [];
        var parm = {};

        p.push("<div id='divTarotCard", parms.index, "' index=",parms.index," style='z-index:", parms.index + 10, ";width:", parms.w, "px;height:", parms.h, "px;top:", parms.y, "px;left:",
                                parms.x, "px;position:absolute;border: 0px solid red;'> ");
        p.push("<div style='position:relative;'>");
        p.push("<div style='width:15px; height:15px; background-color:white; border:0px solid green;z-index:", parms.index + 1, "; position:absolute;'>", parms.index, "</div>");
        p.push("<img src='../../content/images/tarotcard/bg.jpg' style='width:100%;' /></div>");
        p.push("</div>");
        $(targetDiv).append(p.join(''));
        var des = [];
        des.push("<div id='divTarotCardDesc", parms.index, "' index=", parms.index, " class='divTarotCardDes'>", parms.desc, "</div>");
        $(targetDivDesc).append(des.join(''));
    },
    RemoveCardPlaceFromDiv: function (index) {
        ///	<summary>
        ///    删除塔罗牌占位符
        ///	</summary>
        ///	<returns type="void" />
        $('#divTarotCard' + index).remove();
        $('#divTarotCardDesc' + index).remove();

    }
}