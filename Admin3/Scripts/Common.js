function HeartBeat() {
//    $.get("/home/HeartBeat?_rand" + Math.random(), {}, function () {

//    })
//    window.setInterval("HeartBeat()", 5 * 60 * 1000);
}

window.onload = function () {
    var texts = $("fieldset input[type='text']");
    if (texts.length > 0) texts[0].focus();
}