/*
已有函数
Request
ConvertJsTimeToShortDate
ConvertShortDateToJsTime
ConvertToNStyle
ConvertToFloatStyle
*/
var Tools = {};
Tools.Request = function (strName) {
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
Tools.ConvertJsTimeToShortDate = function (date) {//将js时间转化为yyyy-mm-dd格式
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

Tools.ascii = function (str) {
    return str.replace(/[^\u0000-\u00FF]/g, function ($0) { return escape($0).replace(/(%u)(\w{4})/gi, "\\u$2") });
}

Tools.unascii = function (str) {
    return unescape(str.replace(/\\u/g, "%u"));
}



//分页
jQuery.fn.pagination = function (a, b) { b = jQuery.extend({ items_per_page: 10, num_display_entries: 10, current_page: 0, num_edge_entries: 0, link_to: "javascript:void(0)", prev_text: "Prev", next_text: "Next", ellipse_text: "...", prev_show_always: true, next_show_always: true, callback: function () { return false } }, b || {}); return this.each(function () { function f() { return Math.ceil(a / b.items_per_page) } function h() { var k = Math.ceil(b.num_display_entries / 2); var l = f(); var j = l - b.num_display_entries; var m = g > k ? Math.max(Math.min(g - k, j), 0) : 0; var i = g > k ? Math.min(g + k, l) : Math.min(b.num_display_entries, l); return [m, i] } function e(j, i) { g = j; c(); var k = b.callback(j, d); if (!k) { if (i.stopPropagation) { i.stopPropagation() } else { i.cancelBubble = true } } return k } function c() { d.empty(); var k = h(); var o = f(); var p = function (i) { return function (q) { return e(i, q) } }; var n = function (q, r) { var i = r; q = q < 0 ? 0 : (q < o ? q : o - 1); r = jQuery.extend({ text: q + 1, classes: "current" }, r || {}); if (q == g) { var s = $("<span class='current'>" + (r.text) + "</span>") } else { var s = $("<a>" + (r.text) + "</a>").bind("click", p(q)).attr("href", b.link_to.replace(/__id__/, q)) } if (i) { s.removeAttr("class"); s.addClass(r.classes) } d.append(s) }; if (b.prev_text && (g > 0 || b.prev_show_always)) { n(g - 1, { text: b.prev_text, classes: "disabled" }) } if (k[0] > 0 && b.num_edge_entries > 0) { var j = Math.min(b.num_edge_entries, k[0]); for (var l = 0; l < j; l++) { n(l) } if (b.num_edge_entries < k[0] && b.ellipse_text) { jQuery("<span>" + b.ellipse_text + "</span>").appendTo(d) } } for (var l = k[0]; l < k[1]; l++) { n(l) } if (k[1] < o && b.num_edge_entries > 0) { if (o - b.num_edge_entries > k[1] && b.ellipse_text) { jQuery("<span>" + b.ellipse_text + "</span>").appendTo(d) } var m = Math.max(o - b.num_edge_entries, k[1]); for (var l = m; l < o; l++) { n(l) } } if (b.next_text && (g < o - 1 || b.next_show_always)) { n(g + 1, { text: b.next_text, classes: "disabled" }) } } var g = b.current_page; a = (!a || a < 0) ? 1 : a; b.items_per_page = (!b.items_per_page || b.items_per_page < 0) ? 1 : b.items_per_page; var d = jQuery(this); this.selectPage = function (i) { e(i) }; this.prevPage = function () { if (g > 0) { e(g - 1); return true } else { return false } }; this.nextPage = function () { if (g < f() - 1) { e(g + 1); return true } else { return false } }; c() }) };
//ligerui拖拽
(function (a) { a.ligerDefaults = a.ligerDefaults || {}; a.ligerDefaults.Drag = { onStartDrag: false, onDrag: false, onStopDrag: false }; a.fn.ligerDrag = function (b) { b = a.extend({}, a.ligerDefaults.Drag, b || {}); return this.each(function () { if (this.useDrag) { return } var c = { start: function (d) { a("body").css("cursor", "move"); c.current = { target: c.target, left: c.target.offset().left, top: c.target.offset().top, startX: d.pageX || d.screenX, startY: d.pageY || d.clientY }; a(document).bind("mouseup.drag", c.stop); a(document).bind("mousemove.drag", c.drag); if (b.onStartDrag) { b.onStartDrag(c.current, d) } }, drag: function (g) { if (!c.current) { return } var f = g.pageX || g.screenX; var d = g.pageY || g.screenY; c.current.diffX = f - c.current.startX; c.current.diffY = d - c.current.startY; if (b.onDrag) { if (b.onDrag(c.current, g) != false) { c.applyDrag() } } else { c.applyDrag() } }, stop: function (d) { a(document).unbind("mousemove.drag"); a(document).unbind("mouseup.drag"); a("body").css("cursor", ""); if (b.onStopDrag) { b.onStopDrag(c.current, d) } c.current = null }, applyDrag: function () { if (c.current.diffX) { c.target.css("left", (c.current.left + c.current.diffX)) } if (c.current.diffY) { c.target.css("top", (c.current.top + c.current.diffY)) } } }; c.target = a(this); if (b.handler == undefined || b.handler == null) { c.handler = a(this) } else { c.handler = (typeof b.handler == "string" ? a(b.handler, this) : b.handle) } c.handler.hover(function () { a("body").css("cursor", "move") }, function () { a("body").css("cursor", "default") }).mousedown(function (d) { c.start(d); return false }); this.useDrag = true }) } })(jQuery);
//弹窗
var ligerDialogImagePath = "../../lib/ligerUI/skins/Aqua/images/dialog/"; (function (a) { a.ligerDefaults = a.ligerDefaults || {}; a.ligerDefaults.Dialog = { cls: null, id: null, buttons: null, isDrag: true, width: 280, height: null, content: "", target: null, url: null, load: false, type: "warn", left: null, top: null, modal: true, name: null, isResize: false, allowClose: true, opener: null, timeParmName: null }; a.ligerDefaults.DialogString = { titleMessage: "提示", waittingMessage: "正在等待中,请稍候..." }; a.ligerDialog = {}; a.ligerDialog.open = function (d) { d = a.extend({}, a.ligerDefaults.Dialog, a.ligerDefaults.DialogString, d || {}); var i = { applyWindowMask: function () { a(".l-window-mask").remove(); a("<div class='l-window-mask' style='display: block;'></div>").height(a(window).height() + a(window).scrollTop()).appendTo("body") }, removeWindowMask: function () { a(".l-window-mask").remove() }, applyDrag: function () { if (a.fn.ligerDrag) { i.dialog.ligerDrag({ handler: ".l-dialog-title" }) } }, applyResize: function () { if (a.fn.ligerResizable) { i.dialog.ligerResizable({ onStopResize: function (n, m) { var l = 0; var g = 0; if (!isNaN(parseInt(i.dialog.css("top")))) { l = parseInt(i.dialog.css("top")) } if (!isNaN(parseInt(i.dialog.css("left")))) { g = parseInt(i.dialog.css("left")) } if (n.diffTop != undefined) { i.dialog.css({ top: l + n.diffTop, left: g + n.diffLeft }); i.dialog.body.css({ width: n.newWidth - 26 }); a(".l-dialog-content", i.dialog.body).height(n.newHeight - 46 - a(".l-dialog-buttons", i.dialog).height()) } return false } }) } }, setImage: function () { if (d.type) { if (d.type == "success" || d.type == "donne" || d.type == "ok") { a(".l-dialog-image", i.dialog).addClass("l-dialog-image-donne").show(); a(".l-dialog-content", i.dialog).css({ paddingLeft: 64, paddingBottom: 30 }) } else { if (d.type == "error") { a(".l-dialog-image", i.dialog).addClass("l-dialog-image-error").show(); a(".l-dialog-content", i.dialog).css({ paddingLeft: 64, paddingBottom: 30 }) } else { if (d.type == "warn") { a(".l-dialog-image", i.dialog).addClass("l-dialog-image-warn").show(); a(".l-dialog-content", i.dialog).css({ paddingLeft: 64, paddingBottom: 30 }) } else { if (d.type == "question") { a(".l-dialog-image", i.dialog).addClass("l-dialog-image-question").show(); a(".l-dialog-content", i.dialog).css({ paddingLeft: 64, paddingBottom: 40 }) } } } } } } }; i.dialog = a('<div class="l-dialog"><table class="l-dialog-table" cellpadding="0" cellspacing="0" border="0"><tbody><tr><td class="l-dialog-tl"></td><td class="l-dialog-tc"><div class="l-dialog-tc-inner"><div class="l-dialog-icon"></div><div class="l-dialog-title"></div><div class="l-dialog-close"></div></div></td><td class="l-dialog-tr"></td></tr><tr><td class="l-dialog-cl"></td><td class="l-dialog-cc"><div class="l-dialog-body"><div class="l-dialog-image"></div> <div class="l-dialog-content"></div><div class="l-dialog-buttons"><div class="l-dialog-buttons-inner"></div></td><td class="l-dialog-cr"></td></tr><tr><td class="l-dialog-bl"></td><td class="l-dialog-bc"></td><td class="l-dialog-br"></td></tr></tbody></table></div>'); a("body").append(i.dialog); i.dialog.body = a(".l-dialog-body:first", i.dialog); i.dialog.close = function () { if (i.dialog.frame) { a(i.dialog.frame.document).ready(function () { i.removeWindowMask(); i.dialog.remove() }) } else { i.removeWindowMask(); i.dialog.remove() } }; i.dialog.doShow = function () { i.dialog.show() }; if (d.allowClose == false) { a(".l-dialog-close", i.dialog).remove() } if (d.target || d.url || d.type == "none") { d.type = null } if (d.cls) { i.dialog.addClass(d.cls) } if (d.id) { i.dialog.attr("id", d.id) } if (d.modal) { i.applyWindowMask() } if (d.isDrag) { i.applyDrag() } if (d.isResize) { i.applyResize() } if (d.type) { i.setImage() } else { a(".l-dialog-image", i.dialog).remove(); a(".l-dialog-content", i.dialog.body).addClass("l-dialog-content-noimage") } if (d.target) { a(".l-dialog-content", i.dialog.body).prepend(d.target) } else { if (d.url) { if (d.timeParmName) { d.url += d.url.indexOf("?") == -1 ? "?" : "&"; d.url += d.timeParmName + "=" + new Date().getTime() } var h = a("<iframe frameborder='0'></iframe>"); var b = d.name ? d.name : "ligerwindow" + new Date().getTime(); h.attr("name", b); a(".l-dialog-content", i.dialog.body).prepend(h); a(".l-dialog-content", i.dialog.body).addClass("l-dialog-content-nopadding"); setTimeout(function () { h.attr("src", d.url); i.dialog.frame = window.frames[h.attr("name")] }, 0) } else { if (d.content) { a(".l-dialog-content", i.dialog.body).html(d.content) } } } if (d.opener) { i.dialog.opener = d.opener } if (d.buttons) { a(d.buttons).each(function (l, m) { var g = a('<div class="l-dialog-btn"><div class="l-dialog-btn-l"></div><div class="l-dialog-btn-r"></div><div class="l-dialog-btn-inner"></div></div>'); a(".l-dialog-btn-inner", g).html(m.text); a(".l-dialog-buttons-inner", i.dialog.body).prepend(g); m.width && g.width(m.width); m.onclick && g.click(function () { m.onclick(m, i.dialog, l) }) }) } else { a(".l-dialog-buttons", i.dialog).remove() } a(".l-dialog-buttons-inner", i.dialog).append("<div class='l-clear'></div>"); d.width && i.dialog.body.width(d.width - 26); if (d.height) { a(".l-dialog-content", i.dialog.body).height(d.height - 46 - a(".l-dialog-buttons", i.dialog).height()) } d.title = d.title || d.titleMessage; d.title && a(".l-dialog-title", i.dialog).html(d.title); a(".l-dialog-title", i.dialog).bind("selectstart", function () { return false }); a(".l-dialog-btn", i.dialog.body).hover(function () { a(this).addClass("l-dialog-btn-over") }, function () { a(this).removeClass("l-dialog-btn-over") }); a(".l-dialog-tc .l-dialog-close", i.dialog).hover(function () { a(this).addClass("l-dialog-close-over") }, function () { a(this).removeClass("l-dialog-close-over") }).click(function () { i.dialog.close() }); var k = (navigator.appName == "Microsoft Internet Explorer" && parseInt(navigator.appVersion) == 4 && navigator.appVersion.indexOf("MSIE 5.5") != -1); var f = (navigator.appName == "Microsoft Internet Explorer" && parseInt(navigator.appVersion) == 4 && navigator.appVersion.indexOf("MSIE 6.0") != -1); if (a.browser.msie && (k || f)) { a(".l-dialog-tl:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "dialog-tl.png',sizingMethod='crop');" }); a(".l-dialog-tc:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "ie6/dialog-tc.png',sizingMethod='crop');" }); a(".l-dialog-tr:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "dialog-tr.png',sizingMethod='crop');" }); a(".l-dialog-cl:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "ie6/dialog-cl.png',sizingMethod='crop');" }); a(".l-dialog-cr:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "ie6/dialog-cr.png',sizingMethod='crop');" }); a(".l-dialog-bl:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "dialog-bl.png',sizingMethod='crop');" }); a(".l-dialog-bc:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "ie6/dialog-bc.png',sizingMethod='crop');" }); a(".l-dialog-br:first", i.dialog).css({ background: "none", filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + ligerDialogImagePath + "dialog-br.png',sizingMethod='crop');" }) } var e = 0; var j = 0; var c = d.width || i.dialog.width(); if (d.left != null) { e = d.left } else { e = 0.5 * (a(window).width() - c) } if (d.top != null) { j = d.top } else { j = 0.5 * (a(window).height() - i.dialog.height()) + a(window).scrollTop() - 10 } i.dialog.css({ left: e, top: j }); i.dialog.doShow(); return i.dialog }; a.ligerDialog.close = function () { a(".l-dialog,.l-window-mask").remove() }; a.ligerDialog.alert = function (d, e, c, f) { d = d || ""; if (typeof (e) == "function") { f = e; c = null } else { if (typeof (c) == "function") { f = c } } var b = function (i, g, h) { g.close(); if (f) { f(i, g, h) } }; p = { content: d, buttons: [{ text: "确定", onclick: b}] }; if (typeof (e) == "string" && e != "") { p.title = e } if (typeof (c) == "string" && c != "") { p.type = c } a.ligerDialog.open(p) }; a.ligerDialog.confirm = function (c, d, e) { if (typeof (d) == "function") { e = d; type = null } var b = function (g, f) { f.close(); if (e) { e(g.type == "ok") } }; p = { type: "question", content: c, buttons: [{ text: "是", onclick: b, type: "ok" }, { text: "否", onclick: b, type: "no"}] }; if (typeof (d) == "string" && d != "") { p.title = d } a.ligerDialog.open(p) }; a.ligerDialog.warning = function (c, d, e) { if (typeof (d) == "function") { e = d; type = null } var b = function (g, f) { f.close(); if (e) { e(g.type) } }; p = { type: "question", content: c, buttons: [{ text: "是", onclick: b, type: "yes" }, { text: "否", onclick: b, type: "no" }, { text: "取消", onclick: b, type: "cancel"}] }; if (typeof (d) == "string" && d != "") { p.title = d } a.ligerDialog.open(p) }; a.ligerDialog.waitting = function (b) { b = b || a.ligerDefaults.Dialog.waittingMessage; a.ligerDialog.open({ cls: "l-dialog-waittingdialog", type: "none", content: '<div style="padding:4px">' + b + "</div>", allowClose: false }) }; a.ligerDialog.closeWaitting = function () { a(".l-dialog-waittingdialog,.l-window-mask").remove() }; a.ligerDialog.success = function (c, d, b) { a.ligerDialog.alert(c, d, "success", b) }; a.ligerDialog.error = function (c, d, b) { a.ligerDialog.alert(c, d, "error", b) }; a.ligerDialog.warn = function (c, d, b) { a.ligerDialog.alert(c, d, "warn", b) }; a.ligerDialog.question = function (b, c) { a.ligerDialog.alert(b, c, "question") }; a.ligerDialog.prompt = function (f, c, e, g) { var d = a('<input type="text" class="l-dialog-inputtext"/>'); if (typeof (e) == "function") { g = e } if (typeof (c) == "function") { g = c } else { if (typeof (c) == "boolean") { e = c } } if (typeof (e) == "boolean" && e) { d = a('<textarea class="l-dialog-textarea"></textarea>') } if (typeof (c) == "string" || typeof (c) == "int") { d.val(c) } var b = function (j, h, i) { h.close(); if (g) { g(j.type == "yes", d.val()) } }; p = { title: f, target: d, width: 320, buttons: [{ text: "确定", onclick: b, type: "yes" }, { text: "取消", onclick: b, type: "cancel"}] }; a.ligerDialog.open(p) } })(jQuery);

//弹窗新的
(function (h) { var n = window, v, t, j, g = (h.browser.msie && h.browser.version < 7) ? true : false, m = g ? '<iframe hideFocus="true" frameborder="0" src="about:blank" style="position:absolute;z-index:-1;width:100%;height:100%;top:0px;left:0px;filter:progid:DXImageTransform.Microsoft.Alpha(opacity=0)"></iframe>' : "", f = function (e) { e = e || document; return e.compatMode == "CSS1Compat" ? e.documentElement : e.body }, k = function () { if (!j) { j = 1976 } return ++j }, a = function () { if ("pageXOffset" in n) { return { x: n.pageXOffset || 0, y: n.pageYOffset || 0} } else { var e = f(v); return { x: e.scrollLeft || 0, y: e.scrollTop || 0} } }, d = function () { var e = f(v); return { w: e.clientWidth || 0, h: e.clientHeight || 0} }, q = (function () { var z = document.getElementsByTagName("script"), y = "", w = 0, e = z.length, x = /lhgdialog(?:\.min)?\.js/i; for (; w < e; w++) { if (x.test(z[w].src)) { y = !!document.querySelector ? z[w].src : z[w].getAttribute("src", 4); break } } return y.split("?") })(), b = q[0].substr(0, q[0].lastIndexOf("/") + 1), o = function (w) { if (q[1]) { var z = q[1].split("&"), x = 0, e = z.length, y; for (; x < e; x++) { y = z[x].split("="); if (w === y[0]) { return y[1] } } } return null }, r = o("s") || "default", c = function () { var e = f(v); h(t).css({ width: Math.max(e.scrollWidth, e.clientWidth || 0) - 1 + "px", height: Math.max(e.scrollHeight, e.clientHeight || 0) - 1 + "px" }) }; while (n.parent && n.parent != n) { try { if (n.parent.document.domain != document.domain) { break } } catch (u) { break } n = n.parent } if (o("t") === "self" || n.document.getElementsByTagName("frameset").length > 0) { n = window } v = n.document; try { v.execCommand("BackgroundImageCache", false, true) } catch (u) { } r = r.split(","); for (var s = 0, p = r.length; s < p; s++) { h("head", v).append('<link href="../scripts/lhgdialog/' + b + "skins/" + r[s] + '.css" rel="stylesheet" type="text/css"/>') } h.fn.fixie6png = function () { var y = h("*", this), w, z; for (var x = 0, e = y.length; x < e; x++) { w = h(y[x]).css("ie6png"); z = b + "skins/" + w; if (w) { y[x].style.backgroundImage = "none"; y[x].runtimeStyle.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + z + "',sizingMethod='scale')" } } }; h.fn.dialog = function (i) { var e = false; if (this[0]) { e = new h.dialog(i); this.bind("click", e.ShowDialog) } return e }; h.dialog = function (K) { var x = this, P, F, J, i, e, y, W, D, I, l = h.extend({ title: "lhgdialog \u5F39\u51FA\u7A97\u53E3", cover: false, titleBar: true, btnBar: true, xButton: true, maxBtn: true, minBtn: false, cancelBtn: true, width: 400, height: 300, id: "lhgdgId", link: false, html: null, page: null, parent: null, dgOnLoad: null, onXclick: null, onCancel: null, onMinSize: null, fixed: false, top: "center", left: "center", drag: true, skin: r[0], resize: true, autoSize: false, rang: false, timer: null, iconTitle: true, bgcolor: "#fff", opacity: 0.5, args: null, lockScroll: false, autoPos: false, autoCloseFn: null, cancelBtnTxt: "\u53D6\u6D88", loadingText: "\u7A97\u53E3\u5185\u5BB9\u52A0\u8F7D\u4E2D\uFF0C\u8BF7\u7A0D\u7B49..." }, K || {}), U, T = {}, z = false, E, O = function (Y) { var X = Y.style, S = f(v), aa = parseInt(X.left) - S.scrollLeft, Z = parseInt(X.top) - S.scrollTop; X.removeExpression("left"); X.removeExpression("top"); X.setExpression("left", "this.ownerDocument.documentElement.scrollLeft" + aa); X.setExpression("top", "this.ownerDocument.documentElement.scrollTop + " + Z) }, N = function () { var S, X; if (l.html) { if (typeof l.html === "string") { S = '<div id="lhgdg_inbox_' + l.id + '" class="lhgdg_inbox_' + l.skin + '" style="display:none">' + l.html + "</div>" } else { S = '<div id="lhgdg_inbox_' + l.id + '" class="lhgdg_inbox_' + l.skin + '" style="display:none"></div>' } } else { if (l.page) { S = '<iframe id="lhgfrm_' + l.id + '" frameborder="0" src="' + l.page + '" scrolling="auto" style="display:none;width:100%;height:100%;"></iframe>' } } X = ['<div id="lhgdlg_', l.id, '" class="lhgdialog_', l.skin, '" style="width:', l.width, "px;height:", l.height, 'px;">', '<table border="0" cellspacing="0" cellpadding="0" width="100%">', "<tr>", '<td class="lhgdg_leftTop_', l.skin, '"></td>', '<td id="lhgdg_drag_', l.id, '" class="lhgdg_top_', l.skin, '">', l.titleBar ? ('<div class="lhgdg_title_icon_' + l.skin + '">' + (l.iconTitle ? '<div class="lhgdg_icon_' + l.skin + '"></div>' : "") + '<div class="lhgdg_title_' + l.skin + '">' + l.title + "</div>" + (l.minBtn ? ('<a class="lhgdg_minbtn_' + l.skin + '" id="lhgdg_minbtn_' + l.id + '" href="javascript:void(0);" target="_self"></a>') : "") + (l.maxBtn ? ('<a class="lhgdg_maxbtn_' + l.skin + '" id="lhgdg_maxbtn_' + l.id + '" href="javascript:void(0);" target="_self"></a>') : "") + (l.xButton ? ('<a class="lhgdg_xbtn_' + l.skin + '" id="lhgdg_xbtn_' + l.id + '" href="javascript:void(0);" target="_self"></a>') : "") + "</div>") : "", "</td>", '<td class="lhgdg_rightTop_', l.skin, '"></td>', "</tr>", "<tr>", '<td class="lhgdg_left_', l.skin, '"></td>', "<td>", '<table border="0" cellspacing="0" cellpadding="0" width="100%">', "<tr>", '<td id="lhgdg_content_', l.id, '" style="background-color:#fff">', S, '<div id="lhgdg_load_', l.id, '" class="lhgdg_load_', l.skin, '"><span>', l.loadingText, "</span></div>", "</td>", "</tr>", l.btnBar ? ('<tr><td id="lhgdg_btnBar_' + l.id + '" class="lhgdg_btnBar_' + l.skin + '"><div class="lhgdg_btn_div_' + l.skin + '"></div></td></tr>') : "", "</table>", "</td>", '<td class="lhgdg_right_', l.skin, '"></td>', "</tr>", "<tr>", '<td class="lhgdg_leftBottom_', l.skin, '"></td>', '<td class="lhgdg_bottom_', l.skin, '"></td>', '<td id="lhgdg_drop_', l.id, '" class="lhgdg_rightBottom_', l.skin, '"></td>', "</tr>", "</table>", m, "</div>"].join(""); return X }, B = function () { t = h("#lhgdgCover", v)[0]; if (!t) { var S = '<div id="lhgdgCover" style="position:absolute;top:0px;left:0px;background-color:' + l.bgcolor + ';">' + m + "</div>"; t = h(S, v).css("opacity", l.opacity).appendTo(v.body)[0] } if (l.lockScroll) { h("html", v).addClass("lhgdg_lockScroll") } h(n).bind("resize", c); c(); h(t).css({ display: "", zIndex: k() }) }, w = function (ai, ak, ad, af) { var al = d(), ac = a(), ae = parseInt(ai.style.width, 10), S = parseInt(ai.style.height, 10), aj, ah, Y, X, aa, ab, ag, Z; if (af) { Y = g ? ac.x : 0; X = g ? al.w + ac.x - ae : al.w - ae; aa = g ? (X + ac.x - 20) / 2 : (X - 20) / 2; ab = g ? ac.y : 0; ag = g ? al.h + ac.y - S : al.h - S; Z = g ? (ag + ac.y - 20) / 2 : (ag - 20) / 2 } else { Y = ac.x; aa = ac.x + (al.w - ae - 20) / 2; X = ac.x + al.w - ae; ab = ac.y; Z = ac.y + (al.h - S - 20) / 2; ag = ac.y + al.h - S } switch (ad) { case "center": aj = aa; break; case "left": aj = Y; break; case "right": aj = X; break; default: if (af && g) { ad = ad + ac.x } aj = ad; break } switch (ak) { case "center": ah = Z; break; case "top": ah = ab; break; case "bottom": ah = ag; break; default: if (af && g) { ak = ak + ac.y } ah = ak; break } if (ah < ac.y && !af) { ah = ac.y } h(ai).css({ top: ah + "px", left: aj + "px" }); if (af && g) { O(ai) } }, M = function (S) { x.curWin = window; x.curDoc = document; h(S).bind("contextmenu", function (X) { X.preventDefault() }).bind("mousedown", Q); if (l.html && l.html.nodeType) { h(F).append(l.html); l.html.style.display = "" } D = [window]; if (n != window) { D.push(n) } if (l.page) { x.dgFrm = h("#lhgfrm_" + l.id, v)[0]; if (!l.link) { x.dgWin = x.dgFrm.contentWindow; x.dgFrm.lhgDG = x } h(x.dgFrm).bind("load", function () { this.style.display = "block"; if (!l.link) { var X = h.browser.msie ? x.dgWin.document : x.dgWin; h(X).bind("mousedown", Q); D.push(x.dgWin); x.dgDoc = x.dgWin.document; if (l.autoSize) { H() } h.isFunction(l.dgOnLoad) && l.dgOnLoad.call(x) } P.style.display = "none" }) } if (l.xButton && l.titleBar) { h(J).bind("click", l.onXclick || x.cancel) } if (l.maxBtn && l.titleBar) { h(U).bind("click", x.maxSize); h(i).bind("dblclick", x.maxSize) } if (l.minBtn && l.titleBar && h.isFunction(l.onMinSize)) { h(E).bind("click", l.onMinSize) } }, C = function (aa) { var Z = i.offsetHeight, S = e.offsetHeight, X = l.btnBar ? W.offsetHeight : 0, Y = parseInt(aa.style.height, 10) - Z - S - X; if (Y < 0) { Y = 20 } P.style.lineHeight = Y + "px"; y.style.height = Y + "px" }, H = function () { var aa = i.offsetHeight, S = e.offsetHeight, Z = l.btnBar ? W.offsetHeight : 0, ab = e.offsetWidth * 2, X, ac, Y; if (l.html) { X = Math.max(F.scrollHeight, F.clientHeight || 0); ac = Math.max(F.scrollWidth, F.clientWidth || 0) } else { if (l.page && !l.link) { if (!x.dgDoc) { return } Y = f(x.dgDoc); X = Math.max(Y.scrollHeight, Y.clientHeight || 0); ac = Math.max(Y.scrollWidth, Y.clientWidth || 0) } } X = X + aa + S + Z; ac = ac + ab; x.reDialogSize(ac, X); w(x.dg, "center", "center", l.fixed) }, R = function (ab) { var ad, Y, X, Z, aa = D, af, ac; function ae(ah) { var ag = { x: ah.screenX, y: ah.screenY }; Z = { x: Z.x + (ag.x - ad.x), y: Z.y + (ag.y - ad.y) }; ad = ag; if (l.rang) { if (Z.x < ac.x) { Z.x = ac.x } if (Z.y < ac.y) { Z.y = ac.y } if (Z.x > Y) { Z.x = Y } if (Z.y > X) { Z.y = X } } x.dg.style.top = l.fixed && !g ? Z.y - ac.y + "px" : Z.y + "px"; x.dg.style.left = l.fixed && !g ? Z.x - ac.x + "px" : Z.x + "px" } function S(ai) { for (var ah = 0, ag = aa.length; ah < ag; ah++) { h(aa[ah].document).unbind("mousemove", ae); h(aa[ah].document).unbind("mouseup", S) } ad = null; ab = null; if (Z.y < ac.y) { x.dg.style.top = ac.y + "px" } if (l.fixed && g) { O(x.dg) } if (h.browser.msie) { x.dg.releaseCapture() } } h(ab).bind("mousedown", function (al) { if (al.target.id === "lhgdg_xbtn_" + l.id) { return } af = d(); ac = a(); var ag = x.dg.offsetLeft, am = x.dg.offsetTop, ai = x.dg.clientWidth, ak = x.dg.clientHeight; Z = l.fixed && !g ? { x: ag + ac.x, y: am + ac.y} : { x: ag, y: am }; ad = { x: al.screenX, y: al.screenY }; Y = af.w + ac.x - ai; X = af.h + ac.y - ak; x.dg.style.zIndex = parseInt(j, 10) + 1; for (var aj = 0, ah = aa.length; aj < ah; aj++) { h(aa[aj].document).bind("mousemove", ae); h(aa[aj].document).bind("mouseup", S) } al.preventDefault(); if (h.browser.msie) { x.dg.setCapture() } }) }, L = function (aa) { var ac, X, ad, Y, Z = D, af, ag, ab; function ae(ai) { var ah = { x: ai.screenX, y: ai.screenY }; af = { w: ah.x - ac.x, h: ah.y - ac.y }; if (af.w < 200) { af.w = 200 } if (af.h < 100) { af.h = 100 } x.dg.style.top = l.fixed ? Y.y - ab.y + "px" : Y.y + "px"; x.dg.style.left = l.fixed ? Y.x - ab.x + "px" : Y.x + "px"; x.reDialogSize(af.w, af.h) } function S(aj) { for (var ai = 0, ah = Z.length; ai < ah; ai++) { h(Z[ai].document).unbind("mousemove", ae); h(Z[ai].document).unbind("mouseup", S) } ac = null; aa = null; if (h.browser.msie) { x.dg.releaseCapture() } } h(aa).bind("mousedown", function (ak) { ad = x.dg.clientWidth; X = x.dg.clientHeight; af = { w: ad, h: X }; ag = d(); ab = a(); var ah = x.dg.offsetLeft, al = x.dg.offsetTop; Y = l.fixed ? { x: ah + ab.x, y: al + ab.y} : { x: ah, y: al }; ac = { x: ak.screenX - ad, y: ak.screenY - X }; x.dg.style.zIndex = parseInt(j, 10) + 1; for (var aj = 0, ai = Z.length; aj < ai; aj++) { h(Z[aj].document).bind("mousemove", ae); h(Z[aj].document).bind("mouseup", S) } ak.preventDefault(); if (h.browser.msie) { x.dg.setCapture() } }) }, Q = function (S) { x.dg.style.zIndex = parseInt(j, 10) + 1; j = parseInt(x.dg.style.zIndex, 10); S.stopPropagation() }, V = function () { if (l.autoPos === true) { l.autoPos = { left: "center", top: "center"} } w(x.dg, l.autoPos.top, l.autoPos.left, l.fixed) }, G = function () { if (h.isFunction(l.onCancel)) { l.onCancel.call(x) } x.cancel() }, A = function () { if (x.dgFrm) { if (!l.link) { h(x.dgFrm).unbind("load") } x.dgFrm.src = "about:blank"; x.dgFrm = null } if (l.html && l.html.nodeType) { h(x.curDoc.body).append(l.html); l.html.style.display = "none" } if (l.autoPos) { h(n).unbind("resize", V) } D = []; h(x.dg).remove(); x.dg = null; z = false; T = {}; P = F = J = i = e = W = y = U = E = null }; x.ShowDialog = function () { if (h("#lhgdlg_" + l.id, v)[0]) { return x } if (l.cover) { B() } if (l.fixed) { l.maxBtn = false; l.minBtn = false } var X = l.fixed && !g ? "fixed" : "absolute", S = N(); x.dg = h(S, v).css({ position: X, zIndex: k() }).appendTo(v.body)[0]; P = h("#lhgdg_load_" + l.id, v)[0]; F = h("#lhgdg_inbox_" + l.id, v)[0]; J = h("#lhgdg_xbtn_" + l.id, v)[0]; i = h("#lhgdg_drag_" + l.id, v)[0]; e = h("#lhgdg_drop_" + l.id, v)[0]; W = h("#lhgdg_btnBar_" + l.id, v)[0]; y = h("#lhgdg_content_" + l.id, v)[0]; U = h("#lhgdg_maxbtn_" + l.id, v)[0]; E = h("#lhgdg_minbtn_" + l.id, v)[0]; C(x.dg); w(x.dg, l.top, l.left, l.fixed); M(x.dg); if (l.drag) { R(i) } else { i.style.cursor = "default" } if (l.resize) { L(e) } else { e.style.cursor = "default" } if (g && h(e).css("ie6png")) { h(x.dg).fixie6png() } if (l.btnBar && l.cancelBtn) { x.addBtn("dgcancelBtn", l.cancelBtnTxt, G) } if (l.html) { P.style.display = "none"; F.style.display = ""; if (l.autoSize) { H() } } if (l.timer) { x.closeTime(l.timer, l.autoCloseFn) } if (l.html && h.isFunction(l.dgOnLoad)) { l.dgOnLoad.call(x) } if (l.autoPos) { h(n).bind("resize", V) } }; x.reDialogSize = function (X, S) { h(x.dg).css({ width: X + "px", height: S + "px" }); C(x.dg) }; x.maxSize = function () { var X, S; X = d(); S = a(); if (!z) { T.dgW = x.dg.offsetWidth; T.dgH = x.dg.offsetHeight; T.dgT = x.dg.style.top; T.dgL = x.dg.style.left; h(x.dg).css({ top: S.y + "px", left: S.x + "px" }); x.reDialogSize(X.w, X.h); h(U).addClass("lhgdg_rebtn_" + l.skin).removeClass("lhgdg_maxbtn_" + l.skin); if (l.drag) { h(i).unbind("mousedown").css("cursor", "default") } if (l.resize) { h(e).unbind("mousedown").css("cursor", "default") } z = true } else { x.reDialogSize(T.dgW, T.dgH); h(x.dg).css({ top: T.dgT, left: T.dgL }); h(U).addClass("lhgdg_maxbtn_" + l.skin).removeClass("lhgdg_rebtn_" + l.skin); if (l.drag) { R(i); i.style.cursor = "move" } if (l.resize) { L(e); e.style.cursor = "nw-resize" } z = false } }; x.SetMinBtn = function (S) { if (l.minBtn && l.titleBar) { if (h.isFunction(S)) { h(E).unbind("click").bind("click", S) } } }; x.addBtn = function (ab, S, Z, aa) { aa = aa || "left"; if (l.btnBar) { if (h("#lhgdg_" + l.id + "_" + ab, v)[0]) { h("#lhgdg_" + l.id + "_" + ab, v).html("<em>" + S + "</em>").unbind("click").bind("click", Z) } else { var Y = '<a id="lhgdg_' + l.id + "_" + ab + '" class="lhgdg_button_' + l.skin + '" href="javascript:void(0);" target="_self"><em>' + S + "</em></a>", X = h(Y, v).bind("click", Z)[0]; if (aa === "left") { h(".lhgdg_btn_div_" + l.skin, W).prepend(X) } else { h(".lhgdg_btn_div_" + l.skin, W).append(X) } } } }; x.removeBtn = function (S) { if (h("#lhgdg_" + l.id + "_" + S, v)[0]) { h("#lhgdg_" + l.id + "_" + S, v).remove() } }; x.SetIndex = function () { x.dg.style.zIndex = parseInt(j, 10) + 1; j = parseInt(x.dg.style.zIndex, 10) }; x.SetXbtn = function (S, X) { if (l.xButton && l.titleBar) { if (h.isFunction(S)) { h(J).unbind("click").bind("click", S) } if (X) { J.style.display = "none" } else { J.style.display = "" } } }; x.SetTitle = function (S) { if (l.titleBar && typeof S === "string") { h(".lhgdg_title_" + l.skin, x.dg).html(S) } }; x.cancel = function () { A(); if (t) { if (l.parent && l.parent.isCover) { var S = l.parent.dg.style.zIndex; t.style.zIndex = parseInt(S, 10) - 1 } else { t.style.display = "none" } if (l.lockScroll) { h("html", v).removeClass("lhgdg_lockScroll") } } }; x.cleanDialog = function () { if (x.dg) { A() } if (t) { h(t).remove(); t = null } }; x.closeTime = function (X, S, Y) { if (I) { clearTimeout(I) } if (S) { S.call(x) } if (X) { I = setTimeout(function () { if (Y) { Y.call(x) } x.cancel(); clearTimeout(I) }, 1000 * X) } }; x.SetPosition = function (X, S) { w(x.dg, S, X, l.fixed) }; x.iWin = function (S) { if (h("#lhgfrm_" + S, v)[0]) { return h("#lhgfrm_" + S, v)[0].contentWindow } return null }; x.iDoc = function (S) { if (h("#lhgfrm_" + S, v)[0]) { return h("#lhgfrm_" + S, v)[0].contentWindow.document } return null }; x.iDG = function (S) { return v.getElementById("lhgdlg_" + S) || null }; x.SetCancelBtn = function (S, X) { X = X || x.cancel; if (h("#lhgdg_" + l.id + "_dgcancelBtn", v)[0]) { h("#lhgdg_" + l.id + "_dgcancelBtn", v).html("<em>" + S + "</em>").unbind("click").bind("click", X) } }; x.setArgs = function (S) { l.args = S }; x.getArgs = function () { return l.args }; x.dialogId = l.id; x.parent = l.parent; x.isCover = l.cover ? true : false; h(window).bind("unload", x.cleanDialog) }; h(function () { var e = setTimeout(function () { new h.dialog({ id: "reLoadId", html: "lhgdialog", width: 100, title: "reLoad", height: 100, left: -9000, btnBar: false }).ShowDialog(); clearTimeout(e) }, 150) }) })(window.lhgcore || window.jQuery);
var dg1 = new $.dialog();
function OpenUrl(url, title,onCancel, height, width) {
    var id = 'dgNew1'; //+ parseInt(Math.random() * 100 + 1);
    dg1 = new $.dialog(
    {
        id: id, page: url, link: false,
        width: (width ? width : 800),
        height: (height ? height : 580), title: title, iconTitle: false, cover: true, loadingText: '窗口内容加载中，请稍候……', cancelBtnTxt: '确定',
        onCancel: function () { eval(onCancel); }
    });
    dg1.ShowDialog();
}
//自动改变textarea的大小
(function (a) { function b(c, f) { for (var d = 0, e = ""; d < f; d++) { e += c } return e } a.fn.autogrow = function (c) { this.filter("textarea").each(function () { this.timeoutId = null; var f = a(this), e = f.height(); var h = a("<div></div>").css({ position: "absolute", wordWrap: "break-word", top: 0, left: -9999, display: "none", width: f.width(), fontSize: f.css("fontSize"), fontFamily: f.css("fontFamily"), lineHeight: f.css("lineHeight") }).appendTo(document.body); var g = function () { var i = this.value.replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/&/g, "&amp;").replace(/\n$/, "<br/>&nbsp;").replace(/\n/g, "<br/>").replace(/ {2,}/g, function (j) { return b("&nbsp;", j.length - 1) + " " }); h.html(i); a(this).css("height", Math.max(h.height(), e)) }; var d = function () { clearTimeout(this.timeoutId); var i = this; this.timeoutId = setTimeout(function () { g.apply(i) }, 100) }; a(this).change(g).keyup(d).keydown(d); g.apply(this) }); return this } })(jQuery);

//slidershow
$.fn.ImgsTurn = function (opts) { var defaultParams = { divCss: { border: "solid 1px #C5E88E", overflow: "hidden", width: "580", height: "169", background: "#C5E88E", position: "relative" }, PhotoCss: { position: "absolute", top: "0px", width: "490", height: "169" }, spanCss: { padding: "5px 0px 0px 5px", width: "490", height: "30", position: "absolute", left: "0px", background: "black", opacity: 0.7, "text-overflow": "ellipsis", cursor: "pointer", color: "white", "overflow": "hidden", "white-space": "nowrap"} }; var ps = { divH: defaultParams.divCss.height, divW: defaultParams.divCss.width, divPhotoW: defaultParams.PhotoCss.width, photosSrc: null, spanText: "", spanHref: "#" }; ps = $.extend(ps, opts || {}); defaultParams.divCss.height = ps.divH; defaultParams.divCss.width = ps.divW; defaultParams.PhotoCss.width = ps.divPhotoW; defaultParams.spanCss.width = ps.divPhotoW; defaultParams.spanCss.top = parseFloat(ps.divH) - 5; return $(this.get(0)).each(function () { var me = $(this); me.css(defaultParams.divCss); $.each(ps.photosSrc, function (i, j) { var divShow = document.createElement("div"); $(divShow).css(defaultParams.PhotoCss); var photo = document.createElement("img"); $(photo).css({ "width": ps.divPhotoW, "height": ps.divH }); photo.setAttribute("src", j); var span = document.createElement("span"); $(span).css(defaultParams.spanCss); span.innerHTML = "<a style='color:white' target='_blank' href='" + ps.spanHref[i] + "'>" + ps.spanText[i] + "</a>"; divShow.appendChild(photo); divShow.appendChild(span); me.append(divShow) }); var minWidth = (ps.divW - ps.divPhotoW) / (ps.photosSrc.length - 1); var avgWidth = ps.divW / ps.photosSrc.length; $("div", me).each(function (i) { var hoverObj = this; $(this).css({ "z-index": i, left: avgWidth * i }).hover(function () { $("div", me).each(function (j) { if (j < i && j > 0) $(this).stop().animate({ left: minWidth * j }, "slow"); if (j > i) $(this).stop().animate({ left: parseInt((minWidth * (j - 1)) + parseInt(ps.divPhotoW)) }, "slow"); if (j == i) $(this).stop().animate({ left: minWidth * j }, "slow") }); var jquerySpan = $("span", hoverObj); jquerySpan.stop().animate({ top: (parseFloat(jquerySpan.css("top")) - parseFloat(jquerySpan.css("height"))) }, "slow") }, function () { $("div", me).each(function (j) { $(this).stop().animate({ left: avgWidth * j }, "slow") }); var jquerySpan = $("span", hoverObj); jquerySpan.stop().animate({ top: (parseFloat(me.css("height")) - 5) }, "slow") }) }) }) }


$(document).ready(function () {
    //        var oDiv = $("div[float]")[0];
    //        var H = 0, iE6;
    //        var Y = oDiv;
    //        while (Y) { H += Y.offsetTop; Y = Y.offsetParent };
    //        iE6 = window.ActiveXObject && !window.XMLHttpRequest;
    //        if (!iE6) {
    //            window.onscroll = function () {
    //                var s = document.body.scrollTop || document.documentElement.scrollTop;
    //                if (s > H) { oDiv.className = "div1 div2"; if (iE6) { oDiv.style.top = (s - H) + "px"; } }
    //                else { oDiv.className = "div1"; }
    //            };
    //        }
    var oDiv = $("div[float]");
    var H = 0, iE6;
    for (var i = 0; i < oDiv.length; i++) {
//        alert(oDiv.height())
        if (oDiv.attr("float") == "" && oDiv.height() > 300) continue;
        var Y = oDiv[i];
        while (Y) { H += Y.offsetTop; Y = Y.offsetParent };
    }

    iE6 = window.ActiveXObject && !window.XMLHttpRequest;
    //if (!iE6) {
    window.onscroll = function () {
        var s = document.body.scrollTop || document.documentElement.scrollTop;
        for (var i = 0; i < oDiv.length; i++) {
            if (oDiv.attr("float") == "" && oDiv.height() > 300) continue;
            if (s > H) {
                oDiv[i].className = "div1 div2";
                if (iE6) {
                    oDiv[i].style.top = (s - H) + "px";
                }
            }
            else { oDiv[i].className = "div1"; }
        }
    };
});



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


/*json 特殊用户标示*/
Tools.UserSpecial = [
        { UserSpecialID: 1, UserSpecialName: '管理员', UserSpecialIcon: 'http://www.taluolaile.com/favicon.ico' }
];
Tools.UserSpecialToSpan = function () {
    $(".span_UserSpecial").each(function () {
        var specialids = this.getAttribute('UserSpecialID');
        var imgs = "";
        for (var i = 0; i < Tools.UserSpecial.length; i++) {
            var item = Tools.UserSpecial[i];
            if (specialids.indexOf(item.UserSpecialID + ",") > -1) {
                imgs += "<img src='" + item.UserSpecialIcon + "' alt='" + item.UserSpecialName + "' title='" + item.UserSpecialName + "'>";
            }
        }
        $(this).html(imgs);
    });
}
$(document).ready(function () {
    Tools.UserSpecialToSpan();
    $("textarea").autogrow();
});