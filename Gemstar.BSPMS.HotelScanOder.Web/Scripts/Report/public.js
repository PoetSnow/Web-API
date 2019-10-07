//获取视窗宽度
var htmlWidth = document.documentElement.clientWidth || document.body.clientWidth;

var htmlDom = document.getElementsByTagName('html')[0];
htmlDom.style.fontSize = htmlWidth/7.5  + 'px';
//监听屏幕尺寸变化动态修改html的字体大小
window.addEventListener('resize',function () {
    var htmlWidth = document.documentElement.clientWidth || document.body.clientWidth;

    var htmlDom = document.getElementsByTagName('html')[0];
    htmlDom.style.fontSize = htmlWidth/7.5  + 'px';
})

