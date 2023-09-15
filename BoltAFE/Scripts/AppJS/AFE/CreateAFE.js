var afeTypes = [];
var afeCategories = [];
var afeTypeSelectEle = $('#afeTypeSelect');
var afeCatSelectEle = $('#afeCatSelect');
var tblDocDtlBodyEle = $('#tblDocDtl >  tbody');
var tblAFECommBodyEle = $('#tblAFEComm > tbody');
var btnSaveCommentEle = $('#btnSaveComment');
var commentArr = [];
var docArr = [];
var btnSaveEle = $('#btnSave');
var users = [];
var usersModelBodyEle = $('#usersModelBody');
var docDescriptionEle = $('#docDescription');
var fileEle = $("#file");
$(document).ready(function () {
    $('#selectedMenu').text($('#menuCreateAFE').text());
    GetAFETypesAndCategories();
    GetUsers();
    bindAFEComment();
    getDocuments();
    if (Number(afeHDRIDEle.val()) == 0) {
        $('#createAFEBtn').prop('hidden', false)
    }
    else {
        $('#tblPrevAppDiv').prop('hidden', false)
    }
});

function GetUsers() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetUsers',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: {},
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            users = JSON.parse(data.users);
            users = users.filter(x => x.User_email != userEmail)
            var usersListBoxStr = '<select class="form-select cls-listBox" name="users" size="' + users.length +'">'
            for (var i = 0; i < users.length; i++) {
                let userID = users[i].User_ID;
                let userEmail = users[i].User_email;
                usersListBoxStr += ' <option value="' + userID + '">' + userEmail +'</option>'
            }
            usersListBoxStr += '</select>';
            usersModelBodyEle.html('');
            usersModelBodyEle.html(usersListBoxStr);
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function GetAFETypesAndCategories() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetAFETypesAndCategories',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: {},
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            afeTypes = JSON.parse(data.AFETypes);
            afeCategories = JSON.parse(data.AFECategories);

            let afeTypesStr = '<option value="0">-- Select --</option>';
            let afeCategoriesStr = '<option value="0">-- Select --</option>';

            for (var i = 0; i < afeTypes.length; i++) {
                let id = afeTypes[i].Afe_type_id;
                let type = isNullEmpty(afeTypes[i].Type) ? "" : afeTypes[i].Type;
                afeTypesStr += '<option value="' + id + '">' + type + '</option>';
            }
            for (var i = 0; i < afeCategories.length; i++) {
                let id = afeCategories[i].Afe_category_id;
                let category = isNullEmpty(afeCategories[i].Category) ? "" : afeCategories[i].Category;
                afeCategoriesStr += '<option value="' + id + '">' + category + '</option>';
            }
            afeTypeSelectEle.html('');
            afeCatSelectEle.html('');

            afeTypeSelectEle.html(afeTypesStr);
            afeCatSelectEle.html(afeCategoriesStr);
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function bindAFEComment() {
    var tblAFECommBodyStr = '';
    for (var i = 0; i < commentArr.length; i++) {
        let colorClass = i % 2 == 0 ? 'commentEvenBgColor' : 'commentOddBgColor';
        var currComment = isNullEmpty(commentArr[i].Message) ? "" : commentArr[i].Message;
        var currEmail = isNullEmpty(commentArr[i].User_email) ? "" : commentArr[i].User_email;
        var currDate = isNullEmpty(commentArr[i].Timestamp) ? "" : getFormattedDateTime(commentArr[i].Timestamp);

        if (currComment != "") {
            tblAFECommBodyStr += '<tr class="' + colorClass + '"><td><a href="#">' + currEmail + '</a>&nbsp; ' + currDate + '&nbsp; - ' + currComment + '</td></tr>';
        }
    }
    let colorClass = tblAFECommBodyEle.length % 2 == 0 ? 'commentEvenBgColor' : 'commentOddBgColor';
    tblAFECommBodyStr += '<tr style ="cursor: pointer"  onclick="addRowIntoCommentTbl(this)" id="trPlusRow" class="' + colorClass + '"><td>' + plusIcon + '</td></tr>';
    tblAFECommBodyEle.html('');
    tblAFECommBodyEle.html(tblAFECommBodyStr);
}
function bindAFEDoc() {
    var tblDocDtlBodyStr = '';
    for (var i = 0; i < docArr.length; i++) {
        let colorClass = i % 2 == 0 ? 'evenRowBgColor' : 'oddRowBgColor';
        let currDocPath = isNullEmpty(docArr[i].Doc_path) ? "" : docArr[i].Doc_path;
        let currDescription = isNullEmpty(docArr[i].Doc_description) ? "" : docArr[i].Doc_description;
        let currDocID = docArr[i].Afe_doc_id ;

        if (currDocPath != "") {
            tblDocDtlBodyStr += '<tr class="' + colorClass + '"><td>' + currDocPath + '</td><td>' + currDescription + '</td><td>' + eyeIcon + '</td><td>' + trashIcon + '</td></tr>';
        }
    }
    let colorClass = tblDocDtlBodyEle.length % 2 == 0 ? 'evenRowBgColor' : 'oddRowBgColor';
    tblDocDtlBodyStr += '<tr style ="cursor: pointer"  onclick="openFilePopup()" id="trPlusRowDoc" class="' + colorClass + '"><td colspan="4">' + plusIcon + '</td></tr>';
    tblDocDtlBodyEle.html('');
    tblDocDtlBodyEle.html(tblDocDtlBodyStr);
}
function addRowIntoCommentTbl(element) {
    btnSaveCommentEle.prop('disabled', false);
    $(element).before('<tr><td><textarea class="form-control"></textarea></td></tr>')
    $('#trPlusRow').prop('hidden', true);
}

function getDocuments() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetDocuments',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: { 'afeHDRID': 0 },
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            docArr = JSON.parse(data.docs)
            bindAFEDoc();
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function saveDocument() {
    var fileUpload = fileEle.get(0);
    var files = fileUpload.files;
    if (files.length == 0) {
        alert('Please select file.')
        return;
    }
    var formData = new FormData();
    formData.append("file", files[0]);
    formData.append("afeHDRID", 0);
    formData.append("docDiscription", docDescriptionEle.val());
    $.ajax({
        before: AddLoader(),
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        type: "POST",
        url: '/AFE/UploadFile',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            var newData = JSON.parse(data);
            alert(newData.data);
            if (newData.IsValid) {
                closeModel('importFile');
                getDocuments();
            }
        },
        error: function (e1, e2, e3) {
        }
    });
}


function saveComment() {
    var textareas = tblAFECommBodyEle.find('textarea');
    if (textareas.length > 0) {
        var lastTestArea = textareas[textareas.length - 1];
        var lastTestAreaComm = $(lastTestArea).val();
        let afeHDRID = 0;
        if (isNullEmpty(lastTestAreaComm)) {
            alert('Please enter comment.');
            return;
        }
        else {
            $.ajax({
                before: AddLoader(),
                complete: function () {
                    setTimeout(function () {
                        RemoveLoader();
                    }, 500);
                },
                type: "POST",
                url: '/AFE/SaveComment',
                data: JSON.stringify({ 'afeHDRID': afeHDRID, 'message': lastTestAreaComm }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (data) {
                    alert(data.data);
                    if (data.IsValid) {
                        commentArr.push({ ID: 0, afeHDRID: afeHDRID, User_ID: userIDEle.val(), Timestamp: toISOStringLocal(new Date()), Message: lastTestAreaComm, User_email: userEmail })
                        bindAFEComment();
                    }
                },
                error: function (e1, e2, e3) {
                }
            });
        }

    }
}
btnSaveEle.click(function () {
    openModel('usersModel');
})

function openFilePopup() {
    $('#file').val('');
    openModel('importFile');
}