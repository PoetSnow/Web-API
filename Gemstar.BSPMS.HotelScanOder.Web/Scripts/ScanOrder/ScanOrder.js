var storage = window.sessionStorage;

$(document).ready(function () {
    loadItemList();
    subClassStyle();
});

function loadItemList() {
    var itemList = storage.getItem("itemList");
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        itemList = JSON.parse(jsonModel.itemList);
        //进入的餐台跟缓存的餐台Id 不一致 清空缓存数据
        var tabid = jsonModel.tabId;
        if (tabid != $("#Tabid").val()) {
            storage.removeItem("itemList");
            return false;
        }

        $(".mui-table-view").find("span").each(function () {
            if (itemList.length > 0) {
                for (var i = 0; i < itemList.length; i++) {
                    var item = itemList[i];
                    if ($(this).attr("data-id") == item.ItemId) {
                        $(this).html(item.Quan);
                        $(this).css("display", "block");
                        $(this).prev().css("display", "block");
                    }
                }
            }
            else {
                $(this).html("");
                $(this).css("display", "none");
                $(this).prev().css("display", "none");
            }
        });
        mui(".mui-numbox").numbox();
    }
    else {
        $(".mui-table-view").find("span").each(function () {
            $(this).html("");
            $(this).css("display", "none");
            $(this).prev().css("display", "none");
        });
        mui(".mui-numbox").numbox();
    }

}

//添加数量
function addQuantity(obj) {

    //  $("#itemid").val($(obj).attr("data-id"));
    var quantity = $(obj).parent().find('span').text();
    if (quantity == "" || parseInt(quantity) == 0) {
        quantity = 0;
    }
    var model = {
        ItemId: $(obj).attr("data-id"),
        Quan: parseInt(quantity) + 1,
        Tagcharge: 4,
        UnitId: $(obj).attr("data-unitId"),
        Price: $(obj).attr("data-price"),
        itemClassId: $(obj).attr("data-subclassId"),
        RowId: "",
        ActioName: "",     //作法
        ActioId: "",     //作法
        Request: "",     //要求
        IsLargess: $(obj).attr("data-isLargess"),   //是否可赠送
        IsMultiUnit: $(obj).attr("data-isMultiUnit"),   //是否多单位
        IsAutoaction: $(obj).attr("data-isAutoaction"),  //是否显示作法
        ItemName: $(obj).attr("data-itemName"),
        UnitName: $(obj).attr("data-unitName")
    };


    if (model.Quan > 0) {
        $(obj).parent().find('span').html(model.Quan)
        $(obj).parent().find('a').first().css("display", "block");
        $(obj).parent().find('span').css("display", "block");
    }
    else {
        $(obj).parent().find('span').text("");
        $(obj).parent().find('a').first().css("display", "none");
        $(obj).parent().find('span').css("display", "none");
    }
    AddlocalStorageItem(model);

}

/* 减少 */
function reductionQuantity(obj) {
    var quantity = $(obj).parent().find('span').text();

    if (quantity == "" || parseInt(quantity) == 0) {
        quantity = 0;
    }
    var model = {

        ItemId: $(obj).attr("data-id"),
        Quan: parseInt(quantity) - 1,
        Tagcharge: 4,
        UnitId: $(obj).attr("data-unitId"),
        Price: $(obj).attr("data-price"),
        itemClassId: $(obj).attr("data-subclassId"),
        RowId: "",
        ActioName: "",     //作法
        ActioId: "",     //作法
        Request: "",     //要求
        IsLargess: $(obj).attr("data-isLargess"),   //是否可赠送
        IsMultiUnit: $(obj).attr("data-isMultiUnit"),   //是否多单位
        IsAutoaction: $(obj).attr("data-isAutoaction"),  //是否显示作法
        ItemName: $(obj).attr("data-itemName"),
        UnitName: $(obj).attr("data-unitName")

    };

    if (model.Quan > 0) {
        $(obj).parent().find('span').html(model.Quan)
        $(obj).parent().find('a').first().css("display", "block");
        $(obj).parent().find('span').css("display", "block");
    }
    else {
        $(obj).parent().find('span').html("");
        $(obj).parent().find('a').first().css("display", "none");
        $(obj).parent().find('span').css("display", "none");


    }
    AddlocalStorageItem(model); //添加数据
}

//添加到缓存数据
function AddlocalStorageItem(model) {
    var itemListModel = {
        tabId: $("#Tabid").val(),
        itemList: ""
    };
    var itemList = storage.getItem("itemList");

    var addFlag = true; //用于控制是否可以修改当前model
    var arr = new Array();
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        var itemLists = JSON.parse(jsonModel.itemList);
        if (itemLists.length > 0) {
            for (var i = 0; i < itemLists.length; i++) {
                var item = itemLists[i];
                if (model.ItemId == item.ItemId) {
                    if (model.Quan != 0) {
                        item = model;
                        arr.push(item);
                    }
                    addFlag = false;
                }
                else {
                    arr.push(item);
                }
            }

            if (addFlag) {
                model.RowId = parseInt(getBillDetailListMaxNum()) - 1;
                arr.push(model);
            }
            itemListModel.itemList = JSON.stringify(arr);
            storage.setItem("itemList", JSON.stringify(itemListModel));
        }
        else {
            model.RowId = -1;
            arr.push(model);
            //不存在则直接添加
            itemListModel.itemList = JSON.stringify(arr);
            storage.setItem("itemList", JSON.stringify(itemListModel));
        }


    }
    else {
        model.RowId = -1;
        arr.push(model);
        //不存在则直接添加
        itemListModel.itemList = JSON.stringify(arr);
        storage.setItem("itemList", JSON.stringify(itemListModel));
    }
    subClassStyle();
}



//分类样式
function subClassStyle() {
    var itemList = storage.getItem("itemList");
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        var itemList = JSON.parse(jsonModel.itemList);
        var subclass = groupSubClass(itemList, "itemClassId");

        $(".mui-control-item").each(function () {
            $(this).find("span").first().hide();
        });
        for (var i = 0; i < subclass.length; i++) {
            if (parseInt(subclass[i].data.num) > 0) {
                $("#" + subclass[i].data.itemClassId).find("span").first().text(subclass[i].data.num);
                $("#" + subclass[i].data.itemClassId).find("span").first().show();
            }
            else {
                $("#" + subclass[i].data.itemClassId).find("span").first().hide();
            }

        }
    }
    else {
        $(".mui-control-item").each(function () {
            $(this).find("span").first().hide();
        });
    }


}

//分组
function groupSubClass(source, key) {
    var map = {},
        dest = [];
    for (var i = 0; i < source.length; i++) {
        var ai = source[i];
        if (!ai[key])
            throw new Error("需要分組的 key 不存在。");
        if (!map[ai[key]]) {
            var model = {
                itemClassId: ai.itemClassId,
                num: Number(ai.Quan)
            };
            dest.push({
                key: ai[key],
                data: model
            });
            map[ai[key]] = ai;
        }
        else {
            for (var j = 0; j < dest.length; j++) {
                var dj = dest[j];
                if (dj.data[key] == ai[key]) {
                    dj.key = ai[key];
                    dj.data.num += Number(ai.Quan);
                    //dj.data.push(ai);
                    break;
                }

            }
        }
    }
    return dest;
}

//获取账单明细中最大的行号
//修改从负数取  取最小的
function getBillDetailListMaxNum() {
    var itemList = storage.getItem("itemList");
    var arr = new Array();
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        var itemList = JSON.parse(jsonModel.itemList);
        for (var i = 0; i < itemList.length; i++) {
            arr.push(itemList[i].RowId);
        }
        return Math.min.apply(null, arr);//最大值。
    }
    return 1;
}

/* ---------------------选择规格界面方法Begin-------------------------------------*/
var speceIndex;
function selectSpec(obj, flag) {
    var parms = {
        url: "/InSingle/SpecList",
        type: 'post',
        dataType: "json",
        data: { itemId: $(obj).attr("data-id"), tabId: $("#Tabid").val(), refeId: $("#RefeId").val() }
    };
    JQajaxPromise(parms)
        .then(function (data) {
            var Unit = data.Data.Unit;  //单位
            var Amount = 0; //默认为数量为1的价格
            for (var i = 0; i < Unit.length; i++) {
                if (Unit[i].IsDefault == true) {
                    Amount = Unit[i].Price;
                }
            }
            var Request = data.Data.Request;    //要求
            var Action = data.Data.Action;      //作法
            //消费项目
            var item = {
                itemId: $(obj).attr("data-id"),

                itemName: $(obj).attr("data-name"),
                itemSubClass: $(obj).attr("data-subclassid"),
                IsLargess: $(obj).attr("data-isLargess"),   //是否可赠送
                IsMultiUnit: $(obj).attr("data-isMultiUnit"),   //是否多单位
                IsAutoaction: $(obj).attr("data-isAutoaction")  //是否显示作法

            };

            var itemList = storage.getItem("itemList");
            var jsonItem;   //缓存中的菜式
            if (itemList != null) {
                var jsonModel = JSON.parse(itemList);//转换为json对象
                var itemList = JSON.parse(jsonModel.itemList);
                for (var i = 0; i < itemList.length; i++) {
                    if (itemList[i].ItemId == item.itemId) {
                        jsonItem = itemList[i];
                    }
                }
            }
            //传送模板中需要的数据
            var _data = {
                item: item,
                unitList: Unit,
                requestList: Request,
                actionList: Action,
                amount: Amount
            };

            var perinfotmp = $("#sppttmp").html();
            var perinfohtml = juicer(perinfotmp, _data);
            if (flag == "1") {
                layer.closeAll();
            }
            //页面层
            speceIndex = layer.open({
                type: 1
                , anim: 'up'
                , content: perinfohtml
                , style: 'position:fixed; bottom:0; left:0; width: 100%; height: auto; padding:0; border:none;',
            });
            if (jsonItem) {
                loadSpecCache(jsonItem);
            }
            shopcartload();
            mui(".mui-numbox").numbox().setOption('setp', 1);
        })
        .catch(function (error) {
            // layer.msg(error, 1);
        })

    // GetSpenList($(obj).attr("data-id"))
}
var loadSpecCache = (x) => {
    //单位
    $("#specUnit>div[class='list']").find("a").attr("class", "mui-btn").parents("div:first").find("a[data-id='" + x.UnitId + "']").attr("class", "mui-btn spec-active");
    //已点做法
    $.each($.parseJSON(x.ActioName), function (i, d) {
        if ($("#specActionGroup>div[class='list']>a[data-id=" + d.groupid + "]").length > 0) { return; }
        var actionid = "", actionname = "";
        $.each($.parseJSON(x.ActioName), function (j, r) {
            if (r.groupid != d.groupid) {
                return;
            }
            if (actionid == "") {
                actionid = r.actionid;
                actionname = r.actionname;
            } else {
                actionid += "," + r.actionid;
                actionname += "," + r.actionname;
            }
        });
        $("#specActionGroup>div[class='list']").prepend("<a class=\"mui-btn\" onclick=\"setGroup(this)\" data-id=\"" + (++i) + "\" data-ids=\"" + actionid + "\" data-names=\"" + actionname + "\">" + actionname + "</a>");
    });
    //要求
    $.each(x.RequestIds.split(","), function (i, d) {
        $("#specRequest>div[class=list]>a[data-id='" + d + "']").addClass("spec-active");
    });
    //数量
    $("#specNum>div>span[class='inputSpan']").text(x.Quan);
    //价格
    GetSumSpece();
};
//规格界面减少数量
function downNum(obj) {
    var quantity = $(obj).parent().find('span').text();

    if (quantity == "" || parseInt(quantity) == 0) {
        quantity = 0;
    }
    quantity = parseInt(quantity);
    quantity -= 1;
    if (quantity > 0) {
        $(obj).parent().find('span').html(quantity)
        $(obj).parent().find('a').first().css("display", "block");
        $(obj).parent().find('span').css("display", "block");
    }
    else {
        $(obj).parent().find('span').html("");
        $(obj).parent().find('a').first().css("display", "none");
        $(obj).parent().find('span').css("display", "none");
    }
    GetSumSpece();
}

//添加数量
function addNum(obj) {
    var quantity = $(obj).parent().find('span').text();
    if (quantity == "" || parseInt(quantity) == 0) {
        quantity = 0;
    }
    quantity = parseInt(quantity);
    quantity += 1;
    if (quantity > 0) {
        $(obj).parent().find('span').html(quantity)
        $(obj).parent().find('a').first().css("display", "block");
        $(obj).parent().find('span').css("display", "block");
    }
    else {
        $(obj).parent().find('span').text("");
        $(obj).parent().find('a').first().css("display", "none");
        $(obj).parent().find('span').css("display", "none");
    }
    GetSumSpece();
}

//选择要求
function selectRequest(obj) {
    var _class = $(obj).attr("class");
    //不存在添加样式
    if (_class.indexOf("spec-active") == "-1") {
        $(obj).addClass("spec-active");
    }
    else {
        $(obj).removeClass("spec-active");
    }

}

function selectUnit(obj) {
    $(".selectSpec-unit a").removeClass("spec-active");
    $(obj).addClass("spec-active");
    GetSumSpece();
}

//选择规格界面确定按钮事件
function submitSpece() {
    GetSumSpece();  //计算下总金额

    //单位
    var unitList = $("#specUnit a");
    var unit = "";
    var price = 0;
    var unitName = "";
    unitList.each(function () {
        if ($(this).attr("class").indexOf("spec-active") != "-1") {
            unit = $(this).attr("data-id");
            price = $(this).attr("data-price");
            unitName = $(this).html();
        }
    })

    //要求
    var request = $("#specRequest a");
    var requestIds = "";
    var requestStr = "";
    request.each(function () {
        if ($(this).attr("class").indexOf("spec-active") != "-1") {
            requestIds += $(this).attr("data-id") + ",";
            requestStr += $(this).html() + ",";
        }
    })

    //作法

    var action = $("#specActionGroup a");
    var actionList = new Array();

    action.each(function () {
        if ($(this).attr("id") != "addIgroupid") {
            var actionArr = $(this).attr("data-ids").split(',');
            var actionNameArr = $(this).attr("data-names").split(',');
            for (var i = 0; i < actionArr.length; i++) {
                var model = new setActionModel();
                model.groupid = $(this).attr("data-id");  //分组ID
                model.actionid = actionArr[i];
                model.actionname = actionNameArr[i];
                actionList.push(model);
            }
        }
    })

    //数量
    var num = $(".selectSpec .inputSpan").html();
    var itemId = $("#speceItemId").val();
    var subclassId = $("#speceitemSubClass").val();
    var isLargess = $("#speceisLargess").val();
    var isMultiUnit = $("#speceisMultiUnit").val();
    var isAutoaction = $("#speceisAutoaction").val();
    var itemName = $(".shopCart-top strong").html();


    var model = {
        ItemId: itemId,
        Quan: num,
        Tagcharge: 4,
        UnitId: unit,
        Price: price,
        itemClassId: subclassId,
        RowId: "",
        ActioName: actionList.length == 0 ? "" : JSON.stringify(actionList),     //作法
        ActioId: "",     //作法
        RequestIds: requestIds,   //要求Ids
        Request: requestStr,     //要求
        IsLargess: isLargess,   //是否可赠送
        IsMultiUnit: isMultiUnit,   //是否多单位
        IsAutoaction: isAutoaction, //是否显示作法
        ItemName: itemName,
        UnitName: unitName

    };
    layer.close(speceIndex);
    AddlocalStorageItem(model);

}
/*规格js*/
function closeSelectSpec() {
    layer.close(speceIndex);
}

//规格界面操作修金额
function GetSumSpece() {
    var num = $(".selectSpec .inputSpan").html();

    var priceList = $(".selectSpec-unit a ");
    var price = 0;
    priceList.each(function () {
        if ($(this).attr("class").indexOf("spec-active") != "-1")
            price = $(this).attr("data-price");
    })

    $(".selectSpec .price").html(num * price);
}


function setGroup(obj) {
    $("#igroupid").val($(obj).attr("data-id"));
    $(".selectSpec-actionGroup .list").find("a").removeClass("spec-active");
    $(obj).addClass("spec-active");

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

//增加作法分组
function addGroup() {
    var igroupid = $("#igroupid").val();
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
//选中作法
function updateItemAction(obj) {
    var groupList = $("#specActionGroup a");    //分组集合

    var thisActionId = $(obj).attr("data-id");  //当前操作的做法ID
    var thisActionName = $(obj).attr("data-name");  //当前操作的做法名称
    var className = "spec-active";

    //作法分组只有一个的时候 是默认的加号
    if (groupList.length == 1) {
        addActionGeoupHtml(obj);
    }
    else {
        groupList.each(function () {
            if ($(this).attr("id") == "addIgroupid" && $(this).attr("class").indexOf(className) != "-1") {
                addActionGeoupHtml(obj);
            }

            if ($(this).attr("class").indexOf(className) != "-1" && $(this).attr("id") != "addIgroupid") {
                //选中的做法分组
                var groupActionId = $(this).attr("data-ids");
                var groupActionName = $(this).attr("data-names");
                var actionIdArr = groupActionId.split(','); //已有的做法
                var actioNameArr = groupActionName.split(','); //已有的做法

                var newActionArr = new Array(); //新的作法ID
                var newActionNameArr = new Array(); //新的作法名称

                if (groupActionId.indexOf(thisActionId) != "-1") {
                    $(obj).removeClass(className);
                    //作法ID
                    for (var i = 0; i < actionIdArr.length; i++) {
                        if (actionIdArr[i] != thisActionId) {
                            newActionArr.push(actionIdArr[i])
                        }

                    }
                    //作法名称
                    for (var i = 0; i < actioNameArr.length; i++) {
                        if (actioNameArr[i] != thisActionName) {
                            newActionNameArr.push(actioNameArr[i])
                        }
                    }
                    if (newActionArr != "") {
                        $(this).attr("data-ids", newActionArr.join(','));
                        $(this).attr("data-names", newActionNameArr.join(','));
                        $(this).html(newActionNameArr.join(','));
                    }
                    else {
                        $(this).remove();
                    }
                }
                else {
                    $(obj).addClass(className);
                    $(this).html(groupActionName + "," + thisActionName);
                    $(this).attr("data-ids", groupActionId + "," + thisActionId);
                    $(this).attr("data-names", groupActionName + "," + thisActionName);
                }
            }

        })
    }

}

//添加做法分组html
function addActionGeoupHtml(obj) {
    var actionId = $(obj).attr("data-id");
    var actionName = $(obj).attr("data-name");
    var html = "<a class='mui-btn ' onclick='setGroup(this)' data-id='" + $("#igroupid").val() + "' data-ids='" + actionId + "' data-names='" + actionName + "'>" + actionName + "</a>";
    $("#addIgroupid").before(html);
    addGroup();
    //$("#addIgroupid").removeClass("spec-active")
}

/* ---------------------选择规格界面方法 END-------------------------------------*/

function layerwindow(data) {
    var successtmp = $("#getsuccesstmp").html();

    var successtmphtml = juicer(successtmp, data);
    layer.closeAll();
    //页面层
    layer.open({
        type: 1
        , anim: 'up'
        , className: "getsuccess"
        , content: successtmphtml
        , shade: 'background-color: rgba(0,0,0,.2)'
    });
}

//作法Model
function setActionModel() {
    this.rowid = "";
    this.groupid = "";
    this.actionid = "";
    this.actionname = "";
    this.addprice = "";
    this.multiple = "";
    this.deptclassno = "";
    this.pdano = "";
    this.status = "1";
    this.opertype = "";
}


//账单明细
function setBillDetailModel(_model) {
    this.rowid = _model.RowId;
    this.itemid = _model.ItemId;
    this.itemname = _model.ItemName;
    this.unitid = _model.UnitId;
    this.quan = _model.Quan;
    this.quan2 = "";
    this.price = _model.Price;
    this.status = _model.Tagcharge;
    this.dcFlag = "D";
    this.request = _model.Request;
    this.actions = _model.ActioName;
    this.sp = "0";
    this.sd = "0";
    this.upid = "";
    this.place = "";        //客位
    this.salename = "";
    this.pdano = GetPdaNo();
    this.canReason = "";
    this.memo = "";
    this.OperType = "1";    //操作类型（1：新增，2：修改）
}

//function SetBillModel(_model) {
//    this.invno = _model.invno;                  //开台卡号
//    this.market = _model.market;                //营业经理
//    this.guestName = _model.guestName;          //客人姓名
//    this.iguest = _model.iguest;                //客人人数
//    this.orderNo = _model.orderNo;              //预订号(默认为空)
//    this.mobile = _model.mobile;                //手机号码
//    this.cardno = _model.cardno;                 //会员卡号
//    this.wxid = _model.wxid;                     //微信ID
//    this.wxorderid = _model.wxorderid;           //微信订单ID
//    this.pdano = _model.pdano;	                 //pda或自助机编号（默认为空）
//    this.tagcheckout = _model.tagcheckout;       //账单状态(0: 正常；1：买单；9：取消)
//    this.canReason = _model.canReason;           //账单取消原因
//    this.memo = _model.memo;                     //开台备注

//}