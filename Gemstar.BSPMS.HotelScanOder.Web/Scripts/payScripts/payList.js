function payHtml(code) {
    switch (code) {
        case "mbrCard":
        case "mbrLargess":
        case "mbrCardAndLargess":
            mbrCard();
            break;
        case "house":
            house();
            break;
        case "corp":
            corp();
        case "WxBarcode":       //微信条码支付
            WxBarcode2();
            break;
        default:
    }
}

//会员卡
function mbrCard() {
    var str = "<div class='roomNum'><span>卡号/手机号：</span><input type='text' id='mbrCardText'  onkeydown='getmbrCardInfo(this)'></div>";
    str += "<ul class='roomMessage'>";
    str += " <li><span>客户名：</span><p></p></li>";
    str += " <li><span>卡类型：</span><p></p></li>";
    str += " <li><span>储值余额：</span><p></p></li>";
    str += " <li><span>状态：</span><p></p></li>";
    str += " <li><span>用户类型：</span><p></p></li>";
    str += " <li><span>有效期：</span><p></p></li>";
    str += " <li><span>联系人：</span><p></p></li>";
    str += " <li><span>性别：</span><p></p></li>";
    str += " <li><span>生日：</span><p></p></li>";
    str += " <li><span>可用积分：</span><p></p></li>";
    str += " <li><span>已用金额：</span><p></p></li>";
    str += " <li><span>本金余额：</span><p></p></li>";
    str += " <li><span>赠券余额：</span><p></p></li>";
    str += " <li><span>市场类型：</span><p></p></li>";
    str += " <li><span>公司名称：</span><p></p></li>";
    str += " <li><span>消费次数：</span><p></p></li>";
    str += "</ul><div class='payClose' onclick='close_choose_attr()'></div>";
    str += "<input type='hidden' id='profileId' value=''  >";
    $(".payMessage").html(str);
    document.body.style.overflow = 'hidden';
    $(".payMessage").animate({ height: '8.6rem' }, [10000]);
    $(".pay-shade").show();
    $(".pay-shade").on('touchmove', function (e) {
        e.preventDefault();  //阻止默认行为
    })
}

//房账
function house() {
    var str = "<div class='roomNum'><span>房间号：</span><input type='text' id='roomNo'  onkeydown='getRommInfo(this)'></div>";
    str += "<ul class='roomMessage'>";
    str += " <li><span>客人名：</span><p></p></li>";
    str += " <li><span>抵店日期：</span><p></p></li>";
    str += " <li><span>余额：</span><p></p></li>";
    str += " <li><span>授权金额：</span><p></p></li>";
    str += " <li><span>信用调节额：</span><p></p></li>";
    str += " <li><span>可用余额：</span><p></p></li>";
    str += " <li><span>挂账限额：</span><p></p></li>";
    str += " <li><span>付款方式：</span><p></p></li>";
    str += " <li><span>已挂账金额：</span><p></p></li>";
    str += " <li><span>房价：</span><p></p></li>";
    str += " <li><span>可用限额：</span><p></p></li>";
    str += " <li><span>备注：</span><p></p></li>";
    str += " <li><span>收银说明：</span><p></p></li>";

    str += "</ul>";
    str += "<div class='payClose' onclick='close_choose_attr()'></div>";

    $(".payMessage").html(str);
    document.body.style.overflow = 'hidden';
    $(".payMessage").animate({ height: '8.6rem' }, [10000]);
    $(".pay-shade").show();
    $(".pay-shade").on('touchmove', function (e) {
        e.preventDefault();  //阻止默认行为
    })
}


//挂账
function corp() {
    var str = "<div class='roomNum'><span>合约单位：</span><input type='text'> </div>";
    str += "<div class='roomNum'><span>签单人：</span><input type='text'> </div>"
    str += "<ul class='roomMessage'>";
    str += " <li><span>联系人：</span><p>202</p></li>";
    str += " <li><span>联系电话：</span><p>202</p></li>";
    str += " <li><span>合同号：</span><p>202</p></li>";
    str += " <li><span>业务员：</span><p>202</p></li>";
    str += " <li><span>有效期：</span><p>202</p></li>";
    str += " <li><span>余额：</span><p>202</p></li>";
    str += " <li><span>信用等级：</span><p>202</p></li>";
    str += " <li><span>信用金额：</span><p>202</p></li>";
    str += " <li><span>备注：</span><p>202</p></li>";
    str += "</ul><div class='payClose' onclick='close_choose_attr()'></div>";

    $(".payMessage").html(str);
    document.body.style.overflow = 'hidden';
    $(".payMessage").animate({ height: '8.6rem' }, [10000]);
    $(".pay-shade").show();
    $(".pay-shade").on('touchmove', function (e) {
        e.preventDefault();  //阻止默认行为
    })

}

function WxBarcode2() {
    var str = "<div class='roomNum'><span>条码：</span><input type='text' id='WxBarcode'> </div>";
    str += "<div class='payClose' onclick='close_choose_attr()'></div>";
    $(".payMessage").html(str);
    document.body.style.overflow = 'hidden';
    $(".payMessage").animate({ height: '4.6rem' }, [10000]);
    $(".pay-shade").show();
    $(".pay-shade").on('touchmove', function (e) {
        e.preventDefault();  //阻止默认行为
    })
}


//获取当前设备号
function GetPdaNo() {
    if (typeof Android2JS != "undefined") {
        var wifi = Android2JS.GetWifiInfo();
        cmpsn = $.parseJSON(wifi).deviceSN;
        return cmpsn;
    }
    return "";
}


function getRommInfo(obj, action) {
    if (event.keyCode == 13) { lAlert($(obj).val()); }
}

//获取会员信息
function getmbrCardInfo(obj) {
    if (event.keyCode == 13) {
        var mbrCardNo = $(obj).val();//会员卡 或者手机号码
        if (mbrCardNo != "") {
            var li = $(".payMode li");
            var _li;
            li.each(function () {
                if ($(this).is(".checked")) {
                    _li = $(this);
                    return false;
                }
            })
            var payUrl = $(_li).attr("data-payurl");
            var payRemark = $(_li).attr("data-payreamk");
            var parms = {
                url: "/PayMethod/GetmbrCardInfo",
                type: 'post',
                dataType: "json",
                data: { payUrl: payUrl, mbrCardNo: mbrCardNo, payRemark: payRemark }
            };
            JQajaxPromiseForLayer(parms)
                .then(function (data) {
                    if (data.Success) {
                        var result = JSON.parse(data.Data);
                        $(".roomMessage li").eq(0).find("p").html(result.GuestCName);
                        $(".roomMessage li").eq(1).find("p").html(result.MbrCardTypeName);
                        $(".roomMessage li").eq(2).find("p").html(result.Balance);
                        $(".roomMessage li").eq(3).find("p").html(result.Status);
                        $(".roomMessage li").eq(4).find("p").html(result.GuestType);
                        $(".roomMessage li").eq(5).find("p").html(result.MbrExpired);

                        $(".roomMessage li").eq(6).find("p").html(result.GuestCName);
                        $(".roomMessage li").eq(7).find("p").html(result.Gender);
                        $(".roomMessage li").eq(8).find("p").html(result.Birthday);

                        $(".roomMessage li").eq(9).find("p").html(result.ValidScore);
                        $(".roomMessage li").eq(10).find("p").html(result.TotalUsedBalance);
                        $(".roomMessage li").eq(11).find("p").html(result.BaseAmtBalance);
                        $(".roomMessage li").eq(12).find("p").html(result.Incamount);
                        $(".roomMessage li").eq(13).find("p").html(result.Market);
                        $(".roomMessage li").eq(14).find("p").html(result.CompanyName);
                        $(".roomMessage li").eq(15).find("p").html(result.Times);
                        $("#profileId").val(result.ProfileId);//   会员Id
                    }
                    else {
                        lAlert(data.Data);
                    }
                })
                .catch(function (error) {
                })
        }

    }
}

//获取房账信息
function getRommInfo(obj)
{
    if (event.keyCode == 13) {
        var mbrCardNo = $(obj).val();//会员卡 或者手机号码
        if (mbrCardNo != "") {
            var li = $(".payMode li");
            var _li;
            li.each(function () {
                if ($(this).is(".checked")) {
                    _li = $(this);
                    return false;
                }
            })
            var payUrl = $(_li).attr("data-payurl");
            var payRemark = $(_li).attr("data-payreamk");
            var parms = {
                url: "/PayMethod/RommInfo",
                type: 'post',
                dataType: "json",
                data: { payUrl: payUrl, roomNo: mbrCardNo, payRemark: payRemark }
            };
            JQajaxPromiseForLayer(parms)
                .then(function (data) {
                    if (data.Success) {
                        var result = JSON.parse(data.Data);
                        $(".roomMessage li").eq(0).find("p").html(result.GuestCname);
                        $(".roomMessage li").eq(1).find("p").html(result.ArrDate);
                        $(".roomMessage li").eq(2).find("p").html(result.Balance);
                        $(".roomMessage li").eq(3).find("p").html(result.ApprovalAmt);
                        $(".roomMessage li").eq(4).find("p").html(result.ApprovalAdj);
                        if (result.ApprovalAmt != undefined && result.ApprovalAdj != undefined) {
                            $(".roomMessage li").eq(5).find("p").html((result.ApprovalAmt) + (result.ApprovalAdj));
                        }
                        $(".roomMessage li").eq(6).find("p")(result.LimitAmount);
                        $(".roomMessage li").eq(7).find("p")(result.Payment);
                        $(".roomMessage li").eq(8).find("p")(result.Chargeamt);
                        $(".roomMessage li").eq(9).find("p")(result.ExcutiveRate);
                        $(".roomMessage li").eq(10).find("p")(result.EnableAmount);
                        $(".roomMessage li").eq(11).find("p")(result.Remark);
                        $(".roomMessage li").eq(12).find("p")(result.CashRemark);
                        //$("#labelRoom").val(result.GuestCname);
                     
                    }
                    else {
                        lAlert(data.Data);
                    }
                })
                .catch(function (error) {
                })
        }

    }
}