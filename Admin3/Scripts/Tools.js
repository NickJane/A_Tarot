/*
已有函数
Request
ConvertJsTimeToShortDate
ConvertShortDateToJsTime
ConvertToNStyle
ConvertToFloatStyle
CreateHidden
*/
var Tools = {};
Tools.Request = function(strName) {
    var strHref = window.document.location.href; ;
    var intPos = strHref.indexOf("?");
    var strRight = strHref.substr(intPos + 1);
    var arrTmp = strRight.split("&");
    for (var i = 0; i < arrTmp.length; i++) {
        var arrTemp = arrTmp[i].split("=");
        if (arrTemp[0].toUpperCase() == strName.toUpperCase()) return arrTemp[1];
    }
    return "";
}
Tools.ConvertJsTimeToShortDate=function(date) {//将js时间转化为yyyy-mm-dd格式
    var month = date.getMonth();
    var day = date.getDate();
    if (month.toString().length == 1)
        month = "0" + (parseInt(month, 10) + 1);
    else
        month = (parseInt(month, 10) + 1);
    if (day.toString().length == 1)
        day = "0" + day;
    return date.getFullYear() + "-" + month + "-" + day;
}
Tools.ConvertJsTimeToLongDate = function (date) {//将js时间转化为yyyy-mm-dd格式
    var month = date.getMonth();
    var day = date.getDate();
    if (month.toString().length == 1)
        month = "0" + (parseInt(month, 10) + 1);
    else
        month = (parseInt(month, 10) + 1);
    if (day.toString().length == 1)
        day = "0" + day;
    return date.getFullYear() + "-" + month + "-" + day + " " +
        (date.getHours().toString().length == 1 ? ("0" + date.getHours()) : date.getHours()) + ":" +
        (date.getMinutes().toString().length == 1 ? ("0" + date.getMinutes()) : date.getMinutes()) + ":" +
        (date.getSeconds().toString().length == 1 ? ("0" + date.getSeconds()) : date.getSeconds());
}
Tools.ConvertShortDateToJsTime = function (str) {//将yyyy-mm-dd格式转化为js时间
    var temp = str.split('-');
    var date = new Date(temp[0], parseInt(temp[1], 10) - 1, temp[2]);
    return date;
}

//转化为千分位数字
Tools.ConvertToNStyle = function (inputValue, type) {

    if (inputValue.toString().split('.')[0] == undefined || inputValue.toString().split('.')[0] == null)
        return;
    target = inputValue.toString().split('.')[0];
    var temp = parseInt(target.toString().length);
    var beishu = temp / 3;

    for (var i = 1; i <= beishu; i++) {

        var left = target.toString().substring(0, target.toString().length - (i * 3 + i - 1));
        if (left.toString().length > 0) {
            var right = target.toString().substring(target.toString().length - (i * 3 + i - 1));
            target = left + "," + right;
        }
    }
    //return "￥" + target;
    //return "￥" + target + ".00";
    if (type) {
        if (type == "qianfenshu")
            return target;
    }
    return target + "." + (inputValue.toString().split('.')[1] ? inputValue.toString().split('.')[1] : "00");
}
//千分位转换为普通数字
Tools.ConvertToFloatStyle = function (inputValue) {

    var temp1 = inputValue;
    if (inputValue.toString().indexOf("￥") != -1)
        temp1 = inputValue.toString().substr(1, inputValue.toString().length);

    return temp1.split(",").join("");
}
//合并某个表的colIndex列. 将所有文本一样的列合并
//ps, 如果一个表格需要合并很多列, 那么先合并colIndex大的.
Tools.MergeTableCols = function (table, colIndex) {
    var tbody = table.tBodies[0];
    var currentText = "";
    var merges = 1;
    var firstTd;
    for (var i = 0; i < tbody.rows.length; i++) {
        if (!currentText) { currentText = $(tbody.rows[i].cells[colIndex]).text(); firstTd = $(tbody.rows[i].cells[colIndex]); continue; }
        if ($(tbody.rows[i].cells[colIndex]).text() == currentText) {
            $(tbody.rows[i].cells[colIndex]).remove();
            merges++;
            firstTd[0].rowSpan = merges;
        } else {
            merges = 1;
            currentText = $(tbody.rows[i].cells[colIndex]).text();
            firstTd = $(tbody.rows[i].cells[colIndex]);
        }

    }
}

Tools.CreateHidden = function (parms) {
    ///	<summary>
    ///    新建隐藏域 {name:"", value:""}
    ///	</summary>
    ///	<returns type="array" />
    if ($("#" + parms.name).length == 0) {
        var hid = "<input type='hidden' id='" + parms.name + "' name='" + parms.name + "' value=" + parms.value + " />";
        $("form").append(hid);
    }
    else {
        $("#" + parms.name).val(parms.value);
    }
}
Tools.JsonToString = function (obj) {
    var str = [];
    str.push('{')
    for (var i in obj) {
        str.push("'", i, "':'", obj[i], "',");
    }
    return str.join('').substring(0, str.join('').length - 1) + "}";
}


//去掉两边空格
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
//去掉数组重复项
Array.prototype.uniq = function () {
    ///	<summary>
    ///    去掉数组重复项
    ///	</summary>
    ///	<returns type="array" />
    var temp = {}, len = this.length;

    for (var i = 0; i < len; i++) {
        if (typeof temp[this[i]] == "undefined") {
            temp[this[i]] = 1;
        }
    }
    this.length = 0;
    len = 0;
    for (var i in temp) {
        this[len++] = i;
    }
    return this;
}
