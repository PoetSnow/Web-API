var storage = window.sessionStorage;

$(document).ready(function () {
    loadItemList();
    subClassStyle();
});

function loadItemList() {
    var itemList = storage.getItem("itemList");
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        var itemList = JSON.parse(jsonModel.itemList);
        //进入的餐台跟缓存的餐台Id 不一致 清空缓存数据
        var tabid = jsonModel.tabId;
        if (tabid != $("#Tabid").val()) {
            storage.removeItem("itemList");
            return false;
        }

        $(".mui-table-view").find("span").each(function () {
            for (var i = 0; i < itemList.length; i++) {
                var item = itemList[i];
                if ($(this).attr("data-id") == item.ItemId) {
                    $(this).html(item.Quan);
                    $(this).css("display", "block");
                    $(this).prev().css("display", "block");
                }
            }
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
    //refreshItemPrice($(obj).attr("data-id"));
    //refreshTotalPrice();

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
        //var shopcart = $(obj).parents().find(".shopCart").attr("class");
        //if (shopcart == "shopCart") {
        //    $(obj).parents("li").remove();
        //}
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
        var itemList = JSON.parse(jsonModel.itemList);
        for (var i = 0; i < itemList.length; i++) {
            var item = itemList[i];
            if (model.ItemId == item.ItemId) {
                if (model.Quan != 0) {
                    item.Quan = model.Quan;
                    arr.push(item);
                }
                addFlag = false;
            }
            else {
                arr.push(item);
            }
        }

        if (addFlag) {
            model.RowId = parseInt(getBillDetailListMaxNum()) + 1;
            arr.push(model);
        }
        itemListModel.itemList = JSON.stringify(arr);
        storage.setItem("itemList", JSON.stringify(itemListModel));

    }
    else {
        model.RowId = 1;
        arr.push(model);
        //不存在则直接添加
        itemListModel.itemList = JSON.stringify(arr);
        storage.setItem("itemList", JSON.stringify(itemListModel));
    }
    subClassStyle();
}




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
                $("#" + subclass[i].data.subClassId).find("span").first().text(subclass[i].data.num);
                $("#" + subclass[i].data.subClassId).find("span").first().show();
            }
            else {
                $("#" + subclass[i].data.subClassId).find("span").first().hide();
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
                subClassId: ai.itemClassId,
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
function getBillDetailListMaxNum() {
    var itemList = storage.getItem("itemList");
    var arr = new Array();
    if (itemList != null) {
        var jsonModel = JSON.parse(itemList);//转换为json对象
        var itemList = JSON.parse(jsonModel.itemList);
        for (var i = 0; i < itemList.length; i++) {
            arr.push(itemList[i].RowId);
        }
        return Math.max.apply(null, arr);//最大值。
    }
    return 1;
}

/* ---------------------选择规格界面方法Begin-------------------------------------*/
var speceIndex;
function selectSpec(obj) {
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
                    if (itemList[i].Itemid == item.itemId) {
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
            layer.closeAll();
            //页面层
            speceIndex = layer.open({
                type: 1
                , anim: 'up'
                , content: perinfohtml
                , style: 'position:fixed; bottom:0; left:0; width: 100%; height: auto; padding:0; border:none;',
            });
            shopcartload();
            mui(".mui-numbox").numbox().setOption('setp', 1);
        })
        .catch(function (error) {
            // layer.msg(error, 1);
        })

    // GetSpenList($(obj).attr("data-id"))
}

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
    var requestStr = "";
    request.each(function () {
        if ($(this).attr("class").indexOf("spec-active") != "-1")
            requestStr += $(this).attr("data-id") + ",";
    })

    //作法
    var actionStr = "";
    var action = $("#specActionGroup a");
    action.each(function () {
        if ($(this).attr("id") != "addIgroupid") {
            actionStr += $(this).attr("data-id") + "/";
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
        ActioName: actionStr,     //作法
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
    layer.closeAll();
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

    //作法分组只有一个的时候 是默认的加号
    var className = "spec-active";
    //取消做法
    if ($(obj).attr("class").indexOf(className) != "-1") {
        $(obj).removeClass("spec-active");

        groupList.each(function () {
            var actionId = $(this).attr("data-ids");
            var actionName = $(this).attr("data-names");
            //处理选中的做法分组
            if ($(this).attr("class").indexOf(className) != "-1") {
                var actionArr = actionId.split(',');
                var actionNameArr = actionName.split(',');
                if (actionArr.length == 1) {
                    //分组作法只有一个值 直接移除整个分组
                    $(this).remove();
                }
                else {
                    var newActionIdArr = new Array();
                    var newActionNameArr = new Array();
                    //用新的数据接收修改后的做法
                    for (var i = 0; i < actionArr.length; i++) {
                        if (actionArr[i] != thisActionId) {
                            newActionIdArr.push(actionArr[i]);
                        }
                    }
                    for (var i = 0; i < actionNameArr.length; i++) {
                        if (actionNameArr[i] != thisActionName) {
                            newActionNameArr.push(actionNameArr[i]);
                        }
                    }
                    $(this).attr("data-ids", newActionIdArr.join(','));
                    $(this).attr("data-names", newActionNameArr.join(','));
                    $(this).html(newActionNameArr.join(','))
                }
            }

        })
    }
    else {
        //添加做法       
        if (groupList.length == 1) {
            addActionGeoupHtml(obj);
        }
        else {
            groupList.each(function () {
                var actionId = $(this).attr("data-ids");
                var actionName = $(this).attr("data-names");
                if ($(this).attr("id") == "addIgroupid" && $(this).attr("class").indexOf(className) != "-1") {
                    //添加一个新的分组作法
                    $(obj).addClass("spec-active");
                    $(this).removeClass("spec-active");
                    addActionGeoupHtml(obj);
                }
                if ($(this).attr("class").indexOf(className) != "-1") {
                    //往选中的做法分组里面添加做法
                    $(obj).addClass("spec-active");
                    actionName = actionName + "," + thisActionName;
                    actionId = actionId + "," + thisActionId;
                    $(this).attr("data-ids", actionId);
                    $(this).attr("data-names", actionName);
                    $(this).html(actionName)
                }
                else {
                    //移除其他分组存在的本做法
                    if ($(this).attr("id") != "addIgroupid") {
                        var actionId = $(this).attr("data-ids");
                        var actionName = $(this).attr("data-names");
                        var actionArr = actionId.split(',');
                        var actionNameArr = actionName.split(',');
                        var newActionIdArr = new Array();
                        var newActionNameArr = new Array();
                        //用新的数据接收修改后的做法
                        for (var i = 0; i < actionArr.length; i++) {
                            if (actionArr[i] != thisActionId) {
                                newActionIdArr.push(actionArr[i]);
                            }
                        }
                        for (var i = 0; i < actionNameArr.length; i++) {
                            if (actionNameArr[i] != thisActionName) {
                                newActionNameArr.push(actionNameArr[i]);
                            }
                        }
                        if (newActionIdArr.length >= 1) {
                            $(this).attr("data-ids", newActionIdArr.join(','));
                            $(this).attr("data-names", newActionNameArr.join(','));
                            $(this).html(newActionNameArr.join(','))
                        }
                        else {
                            //当前操作分组没有数据移除分组作法
                            $(this).remove();
                        }
                    }
                }

            })
        }
    }


}

//添加做法分组html
function addActionGeoupHtml(obj) {
    var actionId = $(obj).attr("data-id");
    var actionName = $(obj).attr("data-name");
    var html = "<a class='mui-btn spec-active' onclick='setGroup(this)' data-id='" + $("#igroupid").val() + "' data-ids='" + actionId + "' data-names='" + actionName + "'>" + actionName + "</a>";
    $("#addIgroupid").before(html);
    $(obj).addClass("spec-active");
    $("#addIgroupid").removeClass("spec-active")
}

/* ---------------------选择规格界面方法 END-------------------------------------*/