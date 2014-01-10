function DeleteConfirm(id) {
    allitem = document.getElementsByTagName("input");
    var len = allitem.length;
    var checked = false;
    for (i = 0; i < len; i++) {
        var checkbox = allitem[i];

        if (checkbox.type == "checkbox" && checkbox.id.endWith(id)) {
            if (checkbox.checked == true) {
                checked = true;
                break;
            }
        }
    }
    if (!checked) {
        alert("请至少选择一条记录！");
        return false;
    }
    else {
        return confirm('你确定删除选中的数据吗？');
    }
}

function DeleteConfirmByTitle(id, txt) {
    allitem = document.getElementsByTagName("input");
    var len = allitem.length;
    var checked = false;
    for (i = 0; i < len; i++) {
        var checkbox = allitem[i];

        if (checkbox.type == "checkbox" && checkbox.id.endWith(id)) {
            if (checkbox.checked == true) {
                checked = true;
                break;
            }
        }
    }
    if (!checked) {
        alert("请至少选择一条记录！");
        return false;
    }
    else {
        return confirm(txt);
    }
}

function ActionConfirm(id, action) {
    allitem = document.getElementsByTagName("input");
    var len = allitem.length;
    var checked = false;
    for (i = 0; i < len; i++) {
        var checkbox = allitem[i];

        if (checkbox.type == "checkbox" && checkbox.id.endWith(id)) {
            if (checkbox.checked == true) {
                checked = true;
                break;
            }
        }
    }
    if (!checked) {
        alert("请至少选择一条记录！");
        return false;
    }
    else {
        return confirm(action);
    }
}

function ForbidBackspace(txt) {
    if (document.readyState == "complete") {
        if (event.keyCode == 8 && txt.readOnly) {
            event.keyCode = 0;
            event.returnValue = false;
        }
    }
}


function OnEnterKey(keyCode, ok_id, cancel_id) {
    var btnOk = document.getElementById(ok_id);
    var btnCancel = document.getElementById(cancel_id);

    if (keyCode == 13) {
        if (document.activeElement.id == cancel_id) {
            btnCancel.click();
        }
        else {
            btnOk.click();
        }
    }
    else if (keyCode == 27) {

        btnCancel.click();
    }
}

function FireDefaultButton(event, target, cancelId) {
    if (event.keyCode == 13) {
        var src = event.srcElement || event.target;
        if (src.tagName.toLowerCase() == "input" && src.type.toLowerCase() == "submit") {
            if (src.id != target) {
                return true;
            }
        }

        if (!src || (src.tagName.toLowerCase() != "textarea")) {
            var defaultButton;
            if (__nonMSDOMBrowser) {
                defaultButton = document.getElementById(target);
            }
            else {
                defaultButton = document.all[target];
            }
            if (defaultButton && typeof (defaultButton.click) != "undefined") {
                defaultButton.click();
                event.cancelBubble = true;
                if (event.stopPropagation)
                    event.stopPropagation();
                return false;
            }
        }
    }
    else if (event.keyCode == 27) {
        var cancelButton;
        if (__nonMSDOMBrowser) {
            cancelButton = document.getElementById(cancelId);
        }
        else {
            cancelButton = document.all[cancelId];
        }
        if (cancelButton && typeof (cancelButton.click) != "undefined") {
            cancelButton.click();
            event.cancelBubble = true;
            if (event.stopPropagation)
                event.stopPropagation();
            return false;
        }
    }
    return true;
}

String.prototype.endWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substring(this.length - str.length) == str)
        return true;
    else
        return false;
    return true;
}

String.prototype.startWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substr(0, str.length) == str)
        return true;
    else
        return false;
    return true;
}
function editTXT(l, t) {

    var gl = document.getElementById(l);
    var gt = document.getElementById(t);
    if (gt.style.display == "none") {
        gt.style.display = "block";
    }


    if (gl.style.display != "none") {
        gl.style.display = "none";
    }

}

//messiliu

function Client_OnTreeNodeChecked(evt) {
    evt = window.event || evt;
    var objNode = evt.srcElement || evt.target;
    if (objNode.tagName == "INPUT" && objNode.type == "checkbox") {
        var objParentDiv = objNode.id.replace("CheckBox", "Nodes");
        if (objNode.checked == true) {
            setChildCheckState(objParentDiv, true);

            setParentCheckeState(objNode, true);
        }
        else {
            setChildCheckState(objParentDiv, false);

            if (!HasOtherChecked(objNode)) {
                setParentCheckeState(objNode, false);
            }
        }
    }
}

//判断是否有并行的其他节点被选中
function HasOtherChecked(objNode) {
    var objParentDiv = WebForm_GetParentByTagName(objNode, "div");

    var chks = objParentDiv.getElementsByTagName("INPUT");
    for (var i = 0; i < chks.length; i++) {
        if (chks[i].checked && chks[i].id != objNode.id) {
            return true;
        }
    }
    return false;
}

//设置父节点
function setParentCheckeState(objNode, chkstate) {
    try {
        var objParentDiv = WebForm_GetParentByTagName(objNode, "div");

        if (objParentDiv == null || objParentDiv == "undefined ") {
            return;
        }
        else {
            var objParentChkId = objParentDiv.id.replace("Nodes", "CheckBox");
            var objParentCheckBox = document.getElementById(objParentChkId);

            if (objParentCheckBox) {
                objParentCheckBox.checked = chkstate;
                setParentCheckeState(objParentDiv, chkstate);
            }
        }
    }
    catch (e) { }
}

//设置子节点
function setChildCheckState(nodeid, chkstate) {
    var node = document.getElementById(nodeid);

    if (node) {
        var chks = node.getElementsByTagName("INPUT");
        for (var i = 0; i < chks.length; i++) {
            chks[i].checked = chkstate;
        }
    }
}


//messiliu

//输入整数(带小数点)
function Num_KeyDown(obj) {
    if (!(event.keyCode == 9) && !(event.keyCode == 110) && !(event.keyCode == 190) && !(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}


function searchableListBox(controller) {

    var aspController = { txtText: null, btnSearch: null, upList: null, listBox: null, hfValue: null, hfText: null };
    if (!$.isEmptyObject(controller)) {

        //        $(controller.txtTextId) = $(controller.txtTextId) || null;
        //        $(controller.btnSearchId) = $(controller.btnSearchId) || null;
        //        $(controller.upListId) = $(controller.upListId) || null;
        //        //$(controller.listBoxId) = $(controller.listBoxId) || null;
        //        $(controller.hfValueId) = $(controller.hfValueId) || null;
        //        $(controller.hfTextId) = $(controller.hfTextId) || null;

        aspController.onSelected = function () {
            var select = $(controller.listBoxId).find('option:selected');
            var text = select.text();
            $(controller.hfValueId).val(select.val());
            $(controller.hfTextId).val(text);
            $(controller.txtTextId).val(text);
            $(controller.upListId).hide();
        };


        $(controller.upListId).hide();

        $(document).bind("click", function (event) {
            $(".listpanel").hide();
            if ($(event.target).closest(".searchablelistbox").length > 0) {
                $(controller.upListId).show();
            }
            else {
                //var ss = $(controller.txtTextId).val();
            }
        });



        $(controller.listBoxId).die("keydown").live("keydown", function (event) {
            if (event.keyCode == 13) {
                var select = $(controller.listBoxId).find('option:selected');
                select.click();
                return false;
            }
        });

        if ($.browser.msie) {
            $(controller.listBoxId).die("click").live("click", function () {
                aspController.onSelected();
                return false;
            });
        }
        else {
            $(controller.listBoxId).find("option").die("click").live("click", function () {
                aspController.onSelected();
                return false;
            });
        }


        $(controller.txtTextId).die("focus").live("focus", function () {
            var ss = $(controller.listBoxId).find('option')
            if (ss.length < 2) {
                $(controller.hfTextId).val("");
                 controller.fromFocus = true;
                $(controller.btnSearchId).click();
            }
            else {
                $(controller.upListId).show();
            }
            if ($.browser.msie) {
                $(this).select();
            }

        });

        $(controller.txtTextId).die("click").live("click", function () {
            if (!$.browser.msie) {
                $(this).select();
            }
            return false;
        });

        $(controller.btnSearchId).die("click").live("click", function () {
            if (controller.fromFocus) {
                controller.fromFocus = false;
            }
            else {
                $(controller.hfTextId).val($(controller.txtTextId).val());
            }
           
        });


        $(controller.txtTextId).die("keydown").live("keydown", function (event) {
            if (event.keyCode == 13) {
                $(controller.btnSearchId).click();
                return false;
            }
            else if (event.keyCode == 40) {
                var items = $(controller.listBoxId).find("option");
                if (items.length > 0) {
                    items.first().attr("SELECTED", "SELECTED");
                }
                $(controller.listBoxId).focus();
            }
        });
    }
}

