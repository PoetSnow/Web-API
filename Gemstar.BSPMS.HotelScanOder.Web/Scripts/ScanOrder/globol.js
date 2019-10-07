$(function () {
    //全局消息弹窗
    layer.msg = function (msg, time) {
        if (time == undefined) {
            time = 1;
        }
        layer.open({
            content: msg
            , skin: 'msg'
            , time: time
            , shade: 'background-color: rgba(0,0,0,.3)'
        });
    };



    //设置操作员按钮事件
    // $(".mui-bar.mui-bar-nav").append('<span class="py_caozuoyuan mui-icon mui-icon-contact" ></span>');

    var webRootHelper = {
        isChild: true,//是否为子网站
        rootPath: function () {
            var that = this;
            var local = window.location;
            var domain = local.protocol + '//' + local.host + '/';
            if (!that.isChild) {
                return domain;
            } else {
                var pathName = local.pathname.substring(1);
                var webName = pathName == '' ? '' : (pathName.substring(0, pathName.indexOf('/')));
                return domain + webName + "/";
            }
        }
    };

    $(".py_caozuoyuan").click(function () {
        $.ajax(
            {
                url: webRootHelper.rootPath() + "product/Getoperator",
                data: {},
                type: "post",
                datatype: "json",
                success: function (result) {
                    if (result.Success) {
                        //自定义标题风格
                        layer.open({
                            title: [
                                '操作员信息',
                                'background-color: #FF4351; color:#fff; margin:0;'
                            ]
                            , content: '<p>操作员：' + result.Data.UserName + '</p>' + '<p>消费区：' + result.Data.refeno + '</p>' +
                                '<p>SN：' + result.Data.CmpNo + '</p>'
                            , style: ''
                        });
                    }
                    else {
                        layer.msg(result.Data);
                    }
                },
                error: function () {
                    layer.msg("请求错误");
                },
                beforeSend: function () {
                    //loading带文字
                    layerindex = layer.open({
                        type: 2
                        , content: '获取中',
                        shadeClose: true
                    });
                },
                complete: function () {
                    layer.close(layerindex);
                }
            });
    });
});


const ajaxPromise = param => {
    return new Promise((resovle, reject) => {
        var ajax = new XMLHttpRequest();
        //xhr.open(param.type || "get", param.url, true);
        // get 跟post  需要分别写不同的代码
        if (param.type === 'get') {
            // 设置 方法 以及 url
            ajax.open('get', param.url);
            // send即可
            ajax.send();
        } else {
            // post请求
            // post请求 url 是不需要改变
            ajax.open("post", param.url);
            // 需要设置请求报文
            ajax.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            // 判断data send发送数据
            if (param.data) {
                // 如果有值 从send发送
                ajax.send(JSON.stringify(param.data));
            } else {
                // 木有值 直接发送即可
                ajax.send();
            }
        }

        xhr.onreadystatechange = () => {
            var DONE = 4; // readyState 4 代表已向服务器发送请求
            var OK = 200; // status 200 代表服务器返回成功
            if (xhr.readyState === DONE) {
                if (xhr.status === OK) {
                    resovle(JSON.parse(xhr.responseText));
                } else {
                    reject(xhr);
                }
            }
        };

    });
};


const JQajaxPromise = param => {
    return new Promise((resovle, reject) => {
        $.ajax({
            url: param.url,
            type: param.type,
            data: param.data,
            datatype: "json",
            success: function (data) {
                resovle(data);
            }, error: function (error) {
                reject(error.responseText);
            }
        });
    });
};

const JQajaxPromiseForLayer = param => {
    return new Promise((resovle, reject) => {
        //loading带文字
        var requestindex = layerindex = layer.open({
            type: 2
            , content: '请求中'
            , className: 'requestlayer',
            shadeClose: false,
            shade: 'background-color: rgba(0,0,0,0)'
        });
        $.ajax({
            url: param.url,
            type: param.type,
            data: param.data,
            datatype: "json",
            success: function (data) {
                resovle(data);
            }, error: function (error) {
                reject(error.responseText);
            },
            complete: function () {
                layer.close(requestindex);
            }
        });
    });
};


