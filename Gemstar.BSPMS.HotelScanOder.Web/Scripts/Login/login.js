$(function () {
    $("#login").click(function (e) {
     
        e.preventDefault(this);
        doLogin();
    });
})

function doLogin() {
    //showPassword('showPassword', 0);
    //数据有效性检测
    if ($("#IsCS").val()=="false") {
        //线上餐饮
        var hid = $.trim($("#Hid").val());
        if (hid.length == 0) {
            layer.msg("请输入酒店代码");
            return;
        }
    }
   
    var uid = $.trim($("#Username").val());
    if (uid.length == 0) {
        layer.msg("请输入登录名");
        return;
    }
    var pwd = $.trim($("#Password").val());
    if (pwd.length == 0) {
        layer.msg("请输入密码");
        return;
    }
   
    var f = $("#login")[0].form;
   
    var validator = $(f).validate();
    if (validator.form()) {        
        $.post(
            $(f).attr("action"),
            $(f).serialize() ,
            function (data) {
                if (data.Success) {
                    //lOpen("登录成功")
                    layer.open({
                        content: '登录成功'
                        , skin: 'msg'
                        , time: 2 //2秒后自动关闭
                        , end: function () {
                            window.location.href = "/Tab/Index";
                           // window.location.href = "/Report/Index";
                        }
                    });
                } else {
                    layer.msg(data.Data); 
                }
            },
            "json");
        
    }
}