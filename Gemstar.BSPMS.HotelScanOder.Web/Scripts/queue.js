!(function (win, doc) {
    function setFontSize() {
        // 获取window 宽度 // zepto实现 $(window).width()就是这么干的
        var winWidth = window.innerWidth;
        if (winWidth > 1024) {
            winWidth = 1024;
        }
        else if (winWidth > 640) {
            //winWidth = 640;
        }
        else if (winWidth < 320) {
            winWidth = 320;
        }
        doc.documentElement.style.fontSize = (winWidth / 640) * 100 + 'px';
    }
    var evt = 'onorientationchange' in win ? 'orientationchange' : 'resize';
    var timer = null;
    win.addEventListener(evt, function () {
        clearTimeout(timer);
        timer = setTimeout(setFontSize, 300);
    }, false);

    win.addEventListener("pageshow", function (e) {
        if (e.persisted) {
            clearTimeout(timer);
            stimer = setTimeout(setFontSize, 300);
        }
    }, false);    // 初始化 
    setFontSize();
}(window, document))