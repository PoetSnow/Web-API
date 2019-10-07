//添加数量
function _orderAdd(obj) {
    var num = Number(obj.prev('span').html());
    var status = $(obj).attr("data-tagcharge");
    var rowId = Number($(obj).parents("li").attr("data-rowid"));
    if (rowId < 0) {
        num++;
        obj.prev('span').html(num.toFixed(2));
        cmpDetail(obj, num.toFixed(2));
        subClassStyle();
        loadItemList(); //
        GetSumAmount(); //计算总金额
    }
    else {
        //不是保存状态
        layer.msg("已落单菜式不允许修改数量");
        return false;
    }
}
//减少数量
function _orderMinus(obj) {
    var num = Number(obj.next('span').html());
    var status = $(obj).attr("data-tagcharge");
    var itemId = $(obj).attr("data-itemId");
    var rowId = Number($(obj).parents("li").attr("data-rowid"));
    if (rowId < 0) {
        if (num >= 1) {
            num--;
            if (num > 0) {
                obj.next('span').html(num.toFixed(2));
            }
            else {
                obj.next('span').html("");
            }
        }
        cmpDetail(obj, num.toFixed(2));
        subClassStyle();
        loadItemList(); //
        GetSumAmount(); //计算总金额
    }
    else {
        //不是保存状态
        layer.msg("已落单菜式不允许修改数量");
        return false;
    }

}
//处理数量
function cmpDetail(obj, num) {
    // var num = Number(obj.next('span').html());
    var itemId = $(obj).attr("data-itemId");
    //操作的菜式还没有落单 ，直接修改存在前台的数据

    var itemList = storage.getItem("itemList");
    var arr = new Array();
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        var itemList = JSON.parse(jsonModel.itemList);
        for (var i = 0; i < itemList.length; i++) {
            var itemModel = itemList[i];
            if (itemModel.ItemId == itemId) {
                if (num != 0) {
                    itemModel.Quan = num;
                    arr.push(itemModel);
                    $(obj).parents(".pos-details").prev().find(".pos-num").html(num);
                } else {
                    //移除元素
                    $(obj).parents("li").remove();
                    // $(obj).parents(".pos-details").remove();
                }
            }
            else {
                arr.push(itemModel);
            }
        }
        var itemListModel = {
            tabId: jsonModel.tabId,
            itemList: JSON.stringify(arr)
        };
        storage.setItem("itemList", JSON.stringify(itemListModel));
    }
}

//处理要求
function _orderRequest(obj) {
    if (obj.is('.spec-active')) {
        obj.removeClass('spec-active');
    } else {
        obj.addClass('spec-active');        //

    }
    var status = $(obj).attr("data-tagcharge"); //状态

    var itemId = $(obj).attr("data-itemId");    //消费项目ID
    if (status == "4") {
        //新增菜式处理要求 修改缓存中的要求
        var itemList = storage.getItem("itemList");
        var arr = new Array();
        if (itemList != null) {
            var jsonModel = JSON.parse(itemList);//转换为json对象
            var itemList = JSON.parse(jsonModel.itemList);
            var requestName = "";//作法名称
            for (var i = 0; i < itemList.length; i++) {
                var itemModel = itemList[i];
                if (itemModel.ItemId == itemId) {
                    var request = $(obj).parent().find("a");
                    request.each(function () {
                        if ($(this).is('.spec-active')) {
                            requestName += $(this).attr("data-id") + ",";
                        }
                    })
                    itemModel.Request = requestName;
                    arr.push(itemModel);
                }
                else {
                    arr.push(itemModel);
                }
            }
            var itemListModel = {
                tabId: jsonModel.tabId,
                itemList: JSON.stringify(arr)
            };
            storage.setItem("itemList", JSON.stringify(itemListModel));
        }
    }
    else if (status == "0" || status == "1" || status == "2") {
        //状态为正常，例送，赠送的可以修改要求
        var s = $(obj).parents(".pos-details").parent();
        //给标识赋值 代表这个明细已经修改过
        $(s).attr("data-isModify", 1);
    }


}
//单位点击事件
function _orderSelectUnit(obj) {

    obj.addClass('spec-active').siblings().removeClass('spec-active')
    var status = $(obj).attr("data-tagcharge"); //状态

    var itemId = $(obj).attr("data-itemId");    //消费项目ID
    var unit = $(obj).attr("data-id");
    var unitName = $(obj).attr("data-name");
    var price = $(obj).attr("data-price");
    if (status == "4") {
        //新增菜式处理要求 修改缓存中的要求
        var itemList = storage.getItem("itemList");
        var arr = new Array();
        if (itemList != null) {
            var jsonModel = JSON.parse(itemList);//转换为json对象
            var itemList = JSON.parse(jsonModel.itemList);

            for (var i = 0; i < itemList.length; i++) {
                var itemModel = itemList[i];
                if (itemModel.ItemId == itemId) {

                    itemModel.UnitId = unit;
                    itemModel.UnitName = unitName;
                    itemModel.Price = price;
                    arr.push(itemModel);
                    $(obj).parents(".pos-details").prev().find(".pos-price").html(price);
                    $(obj).parents(".pos-details").prev().find(".pos-unit").html(unitName);
                }
                else {
                    arr.push(itemModel);
                }
            }
            var itemListModel = {
                tabId: jsonModel.tabId,
                itemList: JSON.stringify(arr)
            };
            storage.setItem("itemList", JSON.stringify(itemListModel));
        }
    }
    else if (status == "0" || status == "1" || status == "2") {
        //状态为正常，例送，赠送的可以修改要求
        var s = $(obj).parents(".pos-details").parent();
        //给标识赋值 代表这个明细已经修改过
        $(s).attr("data-isModify", 1);
        $(obj).parents(".pos-details").prev().find(".pos-price").html(price);
        $(obj).parents(".pos-details").prev().find(".pos-unit").html(unitName);
    }
    GetSumAmount();
}

//作法点击事件
function _orderSelectAction(obj) {

    SetItemAction(obj);
    var actionGroup = $(obj).parents(".unit-flex").next().find("a");

    var thisActionName = $(obj).attr("data-name");
    var thisActionId = $(obj).attr("data-id");

    var parent = $(obj).parents("li");
    var rowid = $(parent).attr("data-rowid");

    var id = "addactionGrp_" + $(obj).attr("data-itemid") + "_" + rowid;

    var addGroupFlag = true;
    if (actionGroup.length == 1) {
        addActionGeoupHtml_Order(obj);
    }
    else {
        actionGroup.each(function () {
            if ($(this).attr("id") == id && $(this).is(".spec-active")) {
                addActionGeoupHtml_Order(obj);
            }

            if ($(this).is(".spec-active") && $(this).attr("id") != id) {
                SetItemActionB(obj, $(this).attr("data-id"));
                //选中的做法分组
                var groupActionId = $(this).attr("data-ids");
                var groupActionName = $(this).attr("data-names");
                var actionIdArr = groupActionId.split(','); //已有的做法
                var actioNameArr = groupActionName.split(','); //已有的做法

                var newActionArr = new Array(); //新的作法ID
                var newActionNameArr = new Array(); //新的作法名称

                var thisIndex = $.inArray(thisActionId, actionIdArr);   //判断选中的做法是否存在
                if (thisIndex >= 0) {
                    //存在
                    $(obj).removeClass("spec-active");
                    //作法ID
                    for (var i = 0; i < actionIdArr.length; i++) {
                        if (actionIdArr[i] != thisActionId) {
                            newActionArr.push(actionIdArr[i]);
                        }

                    }
                    //作法名称
                    for (var i = 0; i < actioNameArr.length; i++) {
                        if (actioNameArr[i] != thisActionName) {
                            newActionNameArr.push(actioNameArr[i]);
                        }
                    }
                    if (newActionArr.length > 0) {
                        $(this).attr("data-ids", newActionArr.join(','));
                        $(this).attr("data-names", newActionNameArr.join(','));
                        $(this).html(newActionNameArr.join(','));
                    }
                    else {
                        $("#" + id).addClass("spec-active");
                        $(this).remove();
                        return false;
                    }
                }
                else {
                    $(obj).addClass("spec-active");
                    $(this).html(groupActionName + "," + thisActionName);
                    $(this).attr("data-ids", groupActionId + "," + thisActionId);
                    $(this).attr("data-names", groupActionName + "," + thisActionName);
                }
            }
        })
    }
}
//添加做法分组html
function addActionGeoupHtml_Order(obj) {
    var actionId = $(obj).attr("data-id");
    var actionName = $(obj).attr("data-name");

    var parent = $(obj).parents("li");
    var rowid = $(parent).attr("data-rowid");
    var _obj = $("#addactionGrp_" + $(obj).attr("data-itemid") + "_" + rowid);
    var grpid = Number(getGrpId(_obj)) + 1;
    var html = "<a class='mui-btn ' onclick='_orderSelectActionGroup(this)' data-id='" + grpid + "' data-ids='" + actionId + "' data-names='" + actionName + "'>" + actionName + "</a>";

    $("#addactionGrp_" + $(obj).attr("data-itemid") + "_" + rowid).before(html);
    SetItemActionB(obj, grpid);

}

function getGrpId(obj) {
    //获取最大的分组ID
    var list = $(obj).parent().find("a");
    var arr = new Array();
    list.each(function () {
        if ($(this).attr("data-id") != "") {
            arr.push($(this).attr("data-id"));
        }
    })
    if (arr.length <= 0) {
        return 0;
    }
    return Math.max.apply(null, arr);//最大值。


}

//增加作法分组
function addGroup() {
    var igroupid = $("#Updateigroupid").val();
    igroupid = Number(igroupid) + 1;
    $("#igroupid").val(igroupid);

    $(".selectSpec-action .list").find("a").removeClass("spec-active");
    $(".selectSpec-actionGroup .list").find("a").removeClass("spec-active");
    $("#addIgroupid").addClass("spec-active");

    var groups = $(".selectSpec-actionGroup .list").find("a");
    var actions = $(".selectSpec-action .list").find("a");
    groups.each(function () {
        var group = $(this);
        //设置分组选中样式
        if (group.attr("data-id") == $("#igroupid").val()) {
            group.addClass("spec-active");

            //设置作法选中样式
            actions.removeClass("spec-active");
            actions.each(function () {
                var name = $(this).attr("data-name");
                if (group.attr("data-names").indexOf(name) != -1) {
                    $(this).addClass("spec-active");
                }
            });
        }
    });
}

//点击作法分组事件
function _orderSelectActionGroup(obj) {
    $(obj).addClass('spec-active').siblings().removeClass('spec-active');

    var action = $(obj).parents(".unit-flex").prev().find("a");
    if ($(obj).attr("data-ids") != "") {
        var idList = $(obj).attr("data-ids");

        var idArr = idList.split(',');
        action.each(function () {
            if ($.inArray($(this).attr("data-id"), idArr) >= 0) {   //判断是否存在
                $(this).addClass("spec-active");
            }
            else {
                $(this).removeClass("spec-active");
            }
        })
    }
    else {
        action.each(function () {
            $(this).removeClass("spec-active");
        })
    }
}

//已经落单菜式做法处理
function SetItemActionB(obj, groupid) {
    //获取当前明细的状态
    var status = $(obj).parents("li").attr("data-status");
    //正常，例送，赠送
    if (status == "0" || status == "1" || status == "2") {
        var s = $(obj).parents(".pos-details").parent();
        //给标识赋值 代表这个明细已经修改过
        $(s).attr("data-isModify", 1);
        var rowId = $(s).attr("data-rowId"); //当前操作的rowId

        var thisActionId = $(obj).attr("data-id");  //当前操作的做法ID
        var thisActionName = $(obj).attr("data-name");  //当前操作的做法ID
        var itemId = $(obj).attr("data-itemid");

        var actionStr = $("#input_" + itemId.trim() + "_" + rowId).val();   //获取到已有的做法
        var actionList = JSON.parse(actionStr);
        var addFlag = true; //标志是否添加新的做法
        var arr = new Array();
        for (var i = 0; i < actionList.length; i++) {
            var a = actionList[i];
            if (a.groupid == groupid && a.actionid == thisActionId) {
                if (a.status != "4") {
                    if (a.status == "3") {
                        //已经是取消状态
                        a.status = "0";
                    }
                    else {
                        a.status = 3;   //同时存在就做取消            
                    }
                    arr.push(a);
                }
                addFlag = false;
            }
            else {
                arr.push(a);
            }
        }
        if (addFlag == true) {
            var newModel = new setActionModel();
            newModel.rowid = rowId;
            newModel.groupid = groupid;
            newModel.actionid = thisActionId;
            newModel.actionname = thisActionName;
            newModel.pdano = GetPdaNo();
            arr.push(newModel);
        }

        $("#input_" + itemId.trim() + "_" + rowId).val(JSON.stringify(arr));

    }
}


//新增菜式做法点击作法处理数据
function SetItemAction(_obj) {
    var thisItemId = $(_obj).attr("data-itemid");   //当前操作的itemId
    var thisActionId = $(_obj).attr("data-id");
    var thisActionName = $(_obj).attr("data-name");
    var status = $(_obj).attr("data-tagcharge");
    if (status == "4") {

        //获取当前选中的做法分组
        var actionGroup = $(_obj).parents(".unit-flex").next().find("a");
        var grpId = "-1";
        var actionFlag = true;        //标志是否是添加做法分组还是已经存在的做法分组

        actionGroup.each(function () {
            var parent = $(this).parents("li");
            var rowid = $(parent).attr("data-rowid");
            if ($(this).is(".spec-active") && $(this).attr("id") != "addactionGrp_" + thisItemId + "_" + rowid) {
                grpId = $(this).attr("data-id");
                actionFlag = false;
                return;
            }
            else if ($(this).is(".spec-active") && $(this).attr("id") == "addactionGrp_" + thisItemId + "_" + rowid) {
                grpId = Number(getGrpId($(this)));
                grpId++;
                actionFlag = true;
                return;
            }
        })
        var itemListModel = storage.getItem("itemList");
        var addFlag = true;
        var itemArr = new Array();
        if (itemListModel != null) {
            var jsonModel = JSON.parse(itemListModel);//转换为json对象
            var itemList = JSON.parse(jsonModel.itemList);
            for (var i = 0; i < itemList.length; i++) {
                var item = itemList[i];
                if (item.ItemId == thisItemId) {
                    var actionArr = new Array();
                    var actionNames = item.ActioName;
                    var actionJson;
                    if (actionNames != "") {
                        actionJson = JSON.parse(actionNames);
                    }
                    if (actionFlag == true) {
                        if (actionJson != null) {
                            for (var j = 0; j < actionJson.length; j++) {
                                actionArr.push(actionJson[j]);
                            }
                        }
                        if (grpId > -1) {
                            var model = new setActionModel();
                            model.groupid = grpId;  //分组ID
                            model.actionid = thisActionId;
                            model.actionname = thisActionName;
                            actionArr.push(model);
                        }


                    }
                    else {
                        if (actionJson != null) {
                            for (var j = 0; j < actionJson.length; j++) {
                                if (actionJson[j].groupid != grpId) {
                                    actionArr.push(actionJson[j]);
                                    addFlag = false;
                                }
                                else {
                                    if (actionJson[j].actionid != thisActionId) {
                                        actionArr.push(actionJson[j]);
                                        //  addFlag = true;
                                    }
                                    else {
                                        addFlag = false;
                                    }
                                }
                            }
                        }
                        if (addFlag) {
                            var model = new setActionModel();
                            model.groupid = grpId;  //分组ID
                            model.actionid = thisActionId;
                            model.actionname = thisActionName;
                            actionArr.push(model);
                        }
                    }
                    item.ActioName = JSON.stringify(actionArr);
                    itemArr.push(item);
                }
                else {
                    itemArr.push(item)
                }
            }
            var itemListModel = {
                tabId: jsonModel.tabId,
                itemList: JSON.stringify(itemArr)
            };

            storage.setItem("itemList", JSON.stringify(itemListModel));

        }
    }
}



//根据明细数据状态设置样式
function setOrderColor() {
    var orderList = $(".pos-tableList .pos-item");
    orderList.each(function () {
        var status = $(this).attr("data-tagcharge");

        if (status == "0") {
            $(this).addClass("normal-state");
        }
        else if (status == "1") {
            $(this).addClass("case-state");

        }
        else if (status == "2") {
            $(this).addClass("give-state");
        }
        else if (status == "4") {
            $(this).addClass("neworder-state");
        }
        else if (status == "51" || status == "52") {
            $(this).addClass("cancel-state");

        }

    })

}

//计算总金额
function GetSumAmount() {
    var sumAmount = 0;
    var sumNum = 0;
    $(".pos-tableList li").each(function () {
        var num = $(this).find(".pos-num").html();
        if ($(this).attr("data-status") == "0" || $(this).attr("data-status") == "4") {
            var price = $(this).find(".pos-price").html();

            var amount = Number(price) * Number(num);
            sumAmount += Number(amount);

        }
        sumNum += Number(num);
    });
    $(".footer-num").html("数量：" + sumNum);
    $(".footer-price p").html(sumAmount);
}


//落单

function beAlone() {
    var itemList = storage.getItem("itemList");
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        itemList = jsonModel.itemList;
    }
    var itemJson = JSON.parse(itemList);
    var detailArr = new Array();

    var htmlList = $(".pos-tableList li");
    htmlList.each(function () {
        var _this = $(this);
        var status = _this.attr("data-status");
        var itemId = _this.attr("data-id");
        var rowId = Number(_this.attr("data-rowid"));
        if (rowId < 0) {
            //新增菜式
            for (var i = 0; i < itemJson.length; i++) {
                var s1 = itemJson[i];
                if (itemId == s1.ItemId) {
                    var model = new setBillDetailModel(s1);
                    model.OpenId = $("#openId").val();
                    detailArr.push(model);
                }
            }
        }
        else {
            //修改过的菜式
            if (_this.attr("data-ismodify") == "1") {
                var price = Number(_this.find(".pos-price").html());
                var quan = Number(_this.find(".minus").next().html());
                var unitList = _this.find(".unit-flex").first().find("a");
                var rowId = _this.attr("data-rowid")
                var unitId = "";
                var price = 0;
                //单位
                unitList.each(function () {
                    var _a = $(this);
                    if (_a.is(".spec-active")) {
                        unitId = $(_a).attr("data-id");
                        price = $(_a).attr("data-price");
                    }
                })
                //要求
                var requestList = _this.find(".unit-flex").last().find("a");
                var request = "";
                var requestsplit = "";
                requestList.each(function () {
                    var _a = $(this);
                    if (_a.is(".spec-active")) {
                        request += requestsplit + $(_a).attr("data-names");
                        requestsplit = ","
                    }
                })
                //作法
                var action = $("#input_" + itemId + "_" + rowId).val();
                var s = {
                    RowId: rowId,
                    ItemId: itemId,
                    ItemName: _this.attr("data-itemName"),
                    Quan: quan,
                    Price: price,
                    ActioName: action,
                    Tagcharge: status,
                    UnitId: unitId,
                    Request: request
                };
                var newModel = new setBillDetailModel(s);
                newModel.OperType = 2;  //修改
                newModel.OpenId = $("#openId").val();  //修改
                detailArr.push(newModel);
            }

        }
    })
    if (detailArr.length > 0) {
        postJson(detailArr);
    }


}
//请求接口数据
function postJson(detailArr) {
    var openTab;
    if ($("#BillId").val() == "") {
        openTab = {
            invno: "",                  //开台卡号
            market: $("#Sale").val(),                //营业经理
            guestName: "",          //客人姓名
            iguest: $("#IGuest").val(),                //客人人数
            orderNo: "",              //预订号(默认为空)
            mobile: "",                //手机号码
            cardno: "",                 //会员卡号
            wxid: $("#openId").val(),                     //微信ID
            wxorderid: "",           //微信订单ID
            pdano: "",	                 //pda或自助机编号（默认为空）
            tagcheckout: "0",       //账单状态(0: 正常；1：买单；9：取消)
            canReason: "",           //账单取消原因
            memo: ""                     //开台备注
        }
    }
    else {
        openTab = {
            invno: "",                  //开台卡号
            market: "",                //营业经理
            guestName: "",          //客人姓名
            iguest: "",                //客人人数
            orderNo: "",              //预订号(默认为空)
            mobile: "",                //手机号码
            cardno: "",                 //会员卡号
            wxid: "",                     //微信ID
            wxorderid: "",           //微信订单ID
            pdano: "",	                 //pda或自助机编号（默认为空）
            tagcheckout: "",       //账单状态(0: 正常；1：买单；9：取消)
            canReason: "",           //账单取消原因
            memo: ""                    //开台备注
        }
    }
    var postUrl = "";
    if ($("#WxPaytype").val() == "1") {
        openTab.tagcheckout = 52;
        postUrl = "/InSingle_B/PayByWX";
    }
    else {
        postUrl = "/InSingle_B/InBillDetail";
    }

    //开台信息
    var openTabStr = JSON.stringify(openTab);
    //对象转换成字符串 传递到后台
    var PostStr = JSON.stringify(detailArr);
    var postModel = {
        billId: $("#BillId").val(),
        openTabStr: openTabStr,
        tabId: $("#Tabid").val(),
        refeId: $("#RefeId").val(),
        billDetailStr: PostStr,
        WxPaytype: 1
    }
    if ($("#WxPaytype").val() == "1") {
        payPost(postUrl, postModel);
    }
    else {
        var parms = {
            url: postUrl,
            type: 'post',
            dataType: "html",
            data: postModel
        };
        JQajaxPromiseForLayer(parms)
            .then(function (data) {
                if (data.Success) {
                    ////数据更新成功，清空本地数据
                    storage.removeItem("itemList");
                    $("#BillId").val(data.Data);
                    subClassStyle();
                    loadItemList(); //
                    GetSumAmount(); //计算总金额
                    GetOrderList(); //重新读取数据
                }
                else {
                    lAlert(data.Data)
                    return false;
                }

            })
            .catch(function (error) {
                // layer.msg(error, 1);
            })
    }
}
function payPost(postUrl, postModel) {
    var parms = {
        url: postUrl,
        type: 'post',
        dataType: "json",
        data: postModel
    };
    JQajaxPromiseForLayer(parms)
        .then(function (data) {
            if (data.Success) {
                ////数据更新成功，清空本地数据
                storage.removeItem("itemList");
                window.location.href = data.Data.Parameters;
            }
            else {
                lAlert(data.Data)
                return false;
            }

        })
        .catch(function (error) {
            // layer.msg(error, 1);
        })
}

//赠送
function ZengSong(event) {
    var _this = event.parents("li");
    var status = $(_this).attr("data-status");
    var rowId = Number($(_this).attr("data-rowid"));
    if (rowId < 0) {
        //未落单的赠送 需要修改缓存中的数据
        var itemList = storage.getItem("itemList");
        var tabid = "";
        if (itemList != null) {
            var jsonModel = JSON.parse(itemList);//转换为json对象
            itemList = jsonModel.itemList;
            tabid = jsonModel.tabId;
        }

        var arr = new Array();
        var thisItemId = $(_this).attr("data-id");
        var itemJson = JSON.parse(itemList);
        for (var i = 0; i < itemJson.length; i++) {
            var item = itemJson[i];
            if (thisItemId == item.ItemId) {
                item.Tagcharge = 2;
                arr.push(item);
                //$(_this).attr("data-ismodify", 1);
            }
            else {
                arr.push(item);
            }
        }
        var itemListModel = {
            tabId: tabid,
            itemList: JSON.stringify(arr)
        };
        storage.setItem("itemList", JSON.stringify(itemListModel));
        $(_this).find(".pos-item").attr("class", "pos-item give-state")
    }
    else {
        if (status != "51" && status != "52") {
            //已落单的项目
            $(_this).attr("data-status", 2);
            $(_this).attr("data-ismodify", 1);
            $(_this).find(".pos-item").attr("class", "pos-item give-state")
        }

    }

}

function CancelItem(event) {
    var _this = event.parents("li");
    var status = $(_this).attr("data-status");
    var rowId = Number($(_this).attr("data-rowid"));
    if (rowId < 0) {    //rowId 小于0代表未落单
        //未落单的赠送 需要修改缓存中的数据
        var itemList = storage.getItem("itemList");
        var tabid = "";
        if (itemList != null) {
            var jsonModel = JSON.parse(itemList);//转换为json对象

            itemList = jsonModel.itemList;
            tabid = jsonModel.tabId;
        }
        var arr = new Array();
        var thisItemId = $(_this).attr("data-id");
        var itemJson = JSON.parse(itemList);
        for (var i = 0; i < itemJson.length; i++) {
            var item = itemJson[i];
            if (thisItemId != item.ItemId) {
                arr.push(item);
            }
        }

        var itemListModel = {
            tabId: tabid,
            itemList: JSON.stringify(arr)
        };
        storage.setItem("itemList", JSON.stringify(itemListModel));
        $(_this).remove();
        subClassStyle();
        loadItemList(); //
        GetSumAmount(); //计算总金额
        $(_this).find(".pos-item").attr("class", "pos-item cancel-state")
    }
    else {
        //已落单的项目
        if (status != "51" && status != "52") {
            $(_this).attr("data-status", 51);
            $(_this).attr("data-ismodify", 1);
            $(_this).find(".pos-item").attr("class", "pos-item cancel-state")
        }
    }

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



///买单
function PayMethod() {
    if ($("#WxPaytype").val() == "1") {
        //点菜支付模式
        beAlone();
    }
    else {
        var htmlList = $(".pos-tableList li");
        var flag = true;
        htmlList.each(function () {
            if ($(this).attr("data-status") == "4" || $(this).attr("data-ismodify") == "1" || Number($(this).attr("data-rowid")) < 0) {
                flag = false;
                return false;
            }
        });
        if (!flag) {
            lAlert("有未落单的菜式！");
            return false;
        }
        var postModel = {
            billId: $("#BillId").val(),
            openTabStr: "",
            tabId: $("#Tabid").val(),
            refeId: $("#RefeId").val(),
            billDetailStr: "",
            WxPaytype: 2
        }
        var postUrl = "/InSingle_B/PayByWX";
        payPost(postUrl, postModel);
    }
}