var afeTypes = [];
var afeCategories = [];
var afeTypesRecordDTL = [];
var afeTypeSelectEle = $('#afeTypeSelect');
var afeCatSelectEle = $('#afeCatSelect');
var tblDocDtlBodyEle = $('#tblDocDtl >  tbody');
var tblAFECommBodyEle = $('#tblAFEComm > tbody');
var btnSaveCommentEle = $('#btnSaveComment');
var commentArr = [];
var docArr = [];
var btnSaveEle = $('#btnSave');
var btnSubmitToEle = $('#btnSubmitTo');
var btnSaveHDREle = $('#btnSaveHDR');
var users = [];
var usersModelBodyEle = $('#usersModelBody');
var docDescriptionEle = $('#docDescription');
var fileEle = $("#file");
var btnCancelEle = $('#btnCancel');
var txtAFENameEle = $('#txtAFEName');
var txtAFENumEle = $('#txtAFENum');
var txtAfeDateEle = $('#txtAfeDate');
var txtAreaDescEle = $('#txtAreaDesc');

var txtGrossAFEEle = $('#txtGrossAFE');
var txtWIEle = $('#txtWI');
var txtNRIEle = $('#txtNRI');
var txtRoyEle = $('#txtRoy');
var txtNetAFEEle = $('#txtNetAFE');
var txtOilEle = $('#txtOil');
var txtGasEle = $('#txtGas');
var txtNGLEle = $('#txtNGL');
var txtBOEEle = $('#txtBOE');
var txtUPayoutEle = $('#txtUPayout');
var txtPV10Ele = $('#txtPV10');
var txtFDEle = $('#txtFD');
var txtRorEle = $('#txtRor');
var txtMroiEle = $('#txtMroi');
var afeDTLIDEle = $('#afeDTLID');
var btnApproveEle = $('#btnApprove');
var btnReturnToEle = $('#btnReturnTo');
var tblPrevAppBodyEle = $('#tblPrevApp >  tbody');

var approverAmountEle = $('#approverAmount');
var isApproveAFE = false;
var isApprovedAFE = false;
var lblAfeNameEle = $('#lblAfeName');
var lblAfeNumEle = $('#lblAfeNum');
var usersLabelEle = $('#usersLabel');

var uploadedFileWithZero = [];
var afeHDR = '';
var afeHdrAprvlHistory = ''
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
        getComments();
        GetAFE();
        $('#tblPrevAppDiv').prop('hidden', false)
    }
});


function GetAFE() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetAFE',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: { 'afeHDRID': Number(afeHDRIDEle.val()) },
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            afeHDR = JSON.parse(data.AFEHdr);
            afeHdrAprvlHistory = JSON.parse(data.AFEHdrAprvlHistory);
            $('input').tooltip('dispose');
            if (afeHDR.length > 0) {
                lblAfeNameEle.text(afeHDR[0].Afe_name);
                $('#lblAfeType').text(afeHDR[0].Type);
                $('#lblAfeCategory').text(afeHDR[0].Category);
                lblAfeNumEle.text(afeHDR[0].Afe_num);
                $('#lblAfeDate').text(getFormattedDate(afeHDR[0].Created_date));
                txtAreaDescEle.text(afeHDR[0].Description);
                afeDTLIDEle.val(afeHDR[0].Afe_econ_dtl_id);
                txtGrossAFEEle.val(afeHDR[0].Gross_afe);
                txtWIEle.val(afeHDR[0].Wi);
                txtNRIEle.val(afeHDR[0].Nri);
                txtRoyEle.val(afeHDR[0].Roy);
                txtNetAFEEle.val(afeHDR[0].Net_afe);
                txtOilEle.val(afeHDR[0].Oil);
                txtGasEle.val(afeHDR[0].Gas);
                txtNGLEle.val(afeHDR[0].Ngl);
                txtBOEEle.val(afeHDR[0].Boe);
                txtUPayoutEle.val(afeHDR[0].Und_po);
                txtPV10Ele.val(afeHDR[0].Pv10);
                txtFDEle.val(afeHDR[0].F_and_d);
                txtRorEle.val(afeHDR[0].Ror);
                txtMroiEle.val(afeHDR[0].Mroi);
            }
            if (afeHDR.length > 1) {
                txtAreaDescEle.attr('title', afeHDR[1].Description);
                txtGrossAFEEle.attr('title', afeHDR[1].Gross_afe);
                txtWIEle.attr('title', afeHDR[1].Wi);
                txtNRIEle.attr('title', afeHDR[1].Nri);
                txtRoyEle.attr('title', afeHDR[1].Roy);
                txtNetAFEEle.attr('title', afeHDR[1].Net_afe);
                txtOilEle.attr('title', afeHDR[1].Oil);
                txtGasEle.attr('title', afeHDR[1].Gas);
                txtNGLEle.attr('title', afeHDR[1].Ngl);
                txtBOEEle.attr('title', afeHDR[1].Boe);
                txtUPayoutEle.attr('title', afeHDR[1].Und_po);
                txtPV10Ele.attr('title', afeHDR[1].Pv10);
                txtFDEle.attr('title', afeHDR[1].F_and_d);
                txtRorEle.attr('title', afeHDR[1].Ror);
                txtMroiEle.attr('title', afeHDR[1].Mroi);
            }

            $("input").tooltip({
                hide: {
                    effect: "explode",
                    delay: 250
                }
            });
            tblPrevAppBodyEle.html('');
            tblPrevAppBodyStr = '';
            for (var i = 0; i < afeHdrAprvlHistory.length; i++) {
                let currAprlHistoryID = afeHdrAprvlHistory[i].Aprvl_hist_dtl_id;
                let currAprlUserID = afeHdrAprvlHistory[i].Approver_user_id;
                let currPreviousAprlEmail = isNullEmptyValue(afeHdrAprvlHistory[i].User_email);
                let currAprlDate = getFormattedDateTime(afeHdrAprvlHistory[i].Approved_date);
                tblPrevAppBodyStr += '<tr><input type="hidden" class="aprlHistoryID" value="' + currAprlHistoryID + '"><input class="aprlUserID" type="hidden" value="' + currAprlUserID + '"><td>' + currPreviousAprlEmail + '</td><td>' + currAprlDate + '</td></tr>'
            }
            tblPrevAppBodyEle.html(tblPrevAppBodyStr);
        },
        error: function (err) {
            console.log(err);
        }
    });
}
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
            bindUsersList();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function bindUsersList() {
    users = users.filter(x => x.User_email != userEmail)
    var usersListBoxStr = '<select class="form-select cls-listBox" name="users" size="' + users.length + '">'
    for (var i = 0; i < users.length; i++) {
        let userID = users[i].User_ID;
        let userEmail = users[i].User_email;
        usersListBoxStr += ' <option value="' + userID + '">' + userEmail + '</option>'
    }
    usersListBoxStr += '</select>';
    usersModelBodyEle.html('');
    usersModelBodyEle.html(usersListBoxStr);
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
    GetTypesRecordDetails();
}

function GetTypesRecordDetails() {
    let currDate = txtAfeDateEle.val();
    let currMonth = new Date().getMonth() + 1;
    let currYear = new Date().getFullYear();
    if (!isNullEmpty(currDate)) {
        currDate = new Date(currDate);
        currMonth = currDate.getMonth() + 1;
        currYear = currDate.getFullYear();
    }
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetTypesRecordDetails',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: { 'month': currMonth, 'year': currYear },
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            afeTypesRecordDTL = JSON.parse(data.AFETypesRecordDTL);
            bindAFENumber();
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
    let currentDocIDArr = [];
    for (var i = 0; i < docArr.length; i++) {
        let colorClass = i % 2 == 0 ? 'evenRowBgColor' : 'oddRowBgColor';
        let currDocPath = isNullEmpty(docArr[i].Doc_path) ? "" : docArr[i].Doc_path;
        let currDescription = isNullEmpty(docArr[i].Doc_description) ? "" : docArr[i].Doc_description;
        let userID = docArr[i].User_id;
        let currDocID = Number(userIDEle.val()) == userID ? docArr[i].Afe_doc_id : 0;
        let currentTrashIcon = Number(userIDEle.val()) == userID ? trashIcon : '';

        if (currDocPath != "" && $.inArray(currDocID, currentDocIDArr) == -1) {
            currentDocIDArr.push(currDocID);
            tblDocDtlBodyStr += '<tr class="' + colorClass + '"><td>' + currDocPath + '</td><td>' + currDescription + '</td><td onclick="openDoc(\'' + docArr[i].Doc_path.substring(1) + '\')">' + eyeIcon + '</td><td onclick="deleteDoc(' + currDocID + ')">' + currentTrashIcon + '</td></tr>';
        }
    }
    let colorClass = tblDocDtlBodyEle.length % 2 == 0 ? 'evenRowBgColor' : 'oddRowBgColor';
    tblDocDtlBodyStr += '<tr style ="cursor: pointer"  onclick="openFilePopup()" id="trPlusRowDoc" class="' + colorClass + '"><td colspan="4">' + plusIcon + '</td></tr>';
    tblDocDtlBodyEle.html('');
    tblDocDtlBodyEle.html(tblDocDtlBodyStr);
}

function openDoc(filePath) {
    let fileExtension = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
    filePath = 'https://localhost:44320/' + filePath;
    let currSrc = filePath;
    if (fileExtension == "doc" || fileExtension == "docx" || fileExtension == "xls" || fileExtension == "xlsx" || fileExtension == "csv") {
        currSrc = 'https://view.officeapps.live.com/op/embed.aspx?src=' + filePath;
    }
    openModel('loadFileModel');
    $('#loadFileIFream').attr('src', currSrc);
}
function deleteDoc(docID) {
    if (docID > 0 && confirm('Are you sure do you want to delete this record?')) {
        $.ajax({
            beforeSend: function () {
                AddLoader();
            },
            complete: function () {
                setTimeout(function () {
                    RemoveLoader();
                }, 500);
            },
            url: '/AFE/DeleteDocument',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'GET',
            async: false,
            data: { 'afeHDRID': afeHDRIDEle.val(), docID: docID },
            success: function (data) {
                if (!data.IsValid) {
                    return;
                }
                uploadedFileWithZero = uploadedFileWithZero.filter(x => x.Afe_doc_id != docID);
                getDocuments();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
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
        data: { 'afeHDRID': afeHDRIDEle.val() },
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            docArr = JSON.parse(data.docs)
            for (var i = 0; i < uploadedFileWithZero.length; i++) {
                docArr.push(uploadedFileWithZero[i]);
            }
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
                uploadedFileWithZero.push({ Afe_doc_id: newData.createdFileID, Afe_hdr_id: afeHDRIDEle.val(), User_id: userIDEle.val(), Doc_path: './' + newData.folderPath.replace("\\", "/"), Doc_description: docDescriptionEle.val() })
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

function getComments() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetComments',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: { 'afeHDRID': afeHDRIDEle.val() },
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            commentArr = JSON.parse(data.comments)
            bindAFEComment();
        },
        error: function (err) {
            console.log(err);
        }
    });
}
btnSaveEle.click(function () {
    openUserModel()
})

function openUserModel() {
    openModel('usersModel');
    $('.cls-listBox').val(0);
}
function openFilePopup() {
    $('#file').val('');
    docDescriptionEle.val('');
    openModel('importFile');
}

btnCancelEle.click(function () {
    deleteCancelAFEHDR(0, false)
})

btnSaveHDREle.click(function () {
    saveHDRAndDTL(false);
})
btnSubmitToEle.click(function () {
    saveHDRAndDTL(true);
})

function saveHDRAndDTL(isSubmitTo) {
    AddLoader();
    setTimeout(function () {
        let isValidSave = true;
        let isValidSubmitTo = true;
        let isValidStr = 'Please enter or select ';
        let isValidStrMessage = ' ';
        let isValidSubmitToMessage = ' ';
        let currAfeType = !isApproveAFE ? Number(afeTypeSelectEle.find(':selected').val()) : 0;
        let currCat = !isApproveAFE ? Number(afeCatSelectEle.find(':selected').val()) : 0;
        let currName = !isApproveAFE ? txtAFENameEle.val() : lblAfeNameEle.text();
        let currNum = !isApproveAFE ? txtAFENumEle.val() : lblAfeNumEle.text();
        let currDate = txtAfeDateEle.val();
        let inboxUserID = Number($('.cls-listBox').val());
        let currInboxUserID = isSubmitTo ? inboxUserID : userIDEle.val();
        let currInboxUserEmail = isSubmitTo ? $('.cls-listBox').find(':selected').text() : userEmail;

        if (!isApproveAFE) {
            if (currAfeType == 0) {
                isValidSave = false;
                isValidStrMessage += 'AFE Type';
            }
            if (currCat == 0) {
                isValidSave = false;
                isValidStrMessage += ' ,AFE Category';
            }
            if (isNullEmpty(currName)) {
                isValidSave = false;
                isValidStrMessage += ' ,AFE Name'
            }
            if (isNullEmpty(currNum)) {
                isValidSave = false;
                isValidStrMessage += ' ,AFE Number'
            }
            if (isNullEmpty(currDate)) {
                isValidSave = false;
                isValidStrMessage += ' ,AFE Date'
            }
        }
        if (isSubmitTo && inboxUserID == 0) {
            isValidSubmitTo = false;
            isValidSubmitToMessage += 'If you need to submit your AFE to other user then Please select user.'
        }
        if (isValidSave && isValidSubmitTo) {

            let afeHDR = {
                Afe_hdr_id: Number(afeHDRIDEle.val()),
                Afe_name: currName,
                Afe_type_id: isNullEmpty(currAfeType) ? 0 : currAfeType,
                Afe_category_id: isNullEmpty(currCat) ? 0 : currCat,
                Afe_num: currNum,
                Created_date: currDate,
                Inbox_user_id: currInboxUserID,
                Inbox_user_email: currInboxUserEmail
            }

            let afeHDRDTL = {
                Afe_econ_dtl_id: Number(afeDTLIDEle.val()),
                Afe_hdr_id: Number(afeHDRIDEle.val()),
                Description: txtAreaDescEle.val(),
                Gross_afe: isNullEmptyDecValue(txtGrossAFEEle.val()),
                Wi: isNullEmptyDecValue(txtWIEle.val()),
                Nri: isNullEmptyDecValue(txtNRIEle.val()),
                Roy: isNullEmptyDecValue(txtRoyEle.val()),
                Net_afe: isNullEmptyDecValue(txtNetAFEEle.val()),
                Oil: isNullEmptyDecValue(txtOilEle.val()),
                Gas: isNullEmptyDecValue(txtGasEle.val()),
                Ngl: isNullEmptyDecValue(txtNGLEle.val()),
                Boe: isNullEmptyDecValue(txtBOEEle.val()),
                Und_po: isNullEmptyDecValue(txtUPayoutEle.val()),
                Pv10: isNullEmptyDecValue(txtPV10Ele.val()),
                F_and_d: isNullEmptyDecValue(txtFDEle.val()),
                Ror: isNullEmptyDecValue(txtRorEle.val()),
                Mroi: isNullEmptyDecValue(txtMroiEle.val()),
                Changed_date: new Date(),
            }

            $.ajax({
                type: "POST",
                url: '/AFE/SaveHDRAndDTL',
                data: JSON.stringify({ 'afeHDR': JSON.stringify(afeHDR), 'afeHDRDTL': JSON.stringify(afeHDRDTL), 'isApproveAFE': isApproveAFE }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                cache: false,
                success: function (data) {
                    alert(data.data);
                    if (data.IsValid) {
                        if (isApprovedAFE) {
                            approveAFE()
                        }
                        else {
                            window.location.href = '/Dashboard'
                        }
                    }
                },
                error: function (e1, e2, e3) {
                }
            });
        }
        else {
            if (!isValidSave) {
                firstCommaIndex = isValidStrMessage.indexOf(',') + 1
                if (firstCommaIndex < 4) {
                    isValidStrMessage = isValidStrMessage.substring(firstCommaIndex, isValidStrMessage.length)
                }
                alert(isValidStr + isValidStrMessage);
            }
            if (!isValidSubmitTo) {
                alert(isValidSubmitToMessage);
            }
        }

        setTimeout(function () {
            RemoveLoader();
        }, 500);
    }, 500);
}


btnApproveEle.click(function () {
    isApproveAFE = true;
    if (Number(approverAmountEle.val()) >= Number(txtNetAFEEle.val())) {
        saveHDRAndDTL(false);
        isApprovedAFE = true;
    }
    else {
        usersLabelEle.text('Approve');
        btnSubmitToEle.text('Submit To');
        btnSaveHDREle.remove();
        openUserModel();
        bindUsersList();
    }
})

btnReturnToEle.click(function () {
    isApproveAFE = true;
    usersLabelEle.text('Return To');
    btnSubmitToEle.text('Return To');
    btnSaveHDREle.remove();
    openUserModel();
    bindReturnUsersList();
});

function bindReturnUsersList() {


    let uniqueReturnToUsers = [];
    usersListBoxOptionStr = '';
    for (var i = 0; i < afeHdrAprvlHistory.length; i++) {
        let userID = afeHdrAprvlHistory[i].User_ID;
        if ($.inArray(userID, uniqueReturnToUsers) !== -1) {
            continue;
        }
        uniqueReturnToUsers.push(userID);
        let userEmail = afeHdrAprvlHistory[i].User_email;
        usersListBoxOptionStr += ' <option value="' + userID + '">' + userEmail + '</option>'
    }
    var usersListBoxStr = '<select class="form-select cls-listBox" name="users" size="' + uniqueReturnToUsers.length + '">' + usersListBoxOptionStr + '</select>';
    usersModelBodyEle.html('');
    usersModelBodyEle.html(usersListBoxStr);
}
function approveAFE() {
    $.ajax({
        before: AddLoader(),
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        type: "POST",
        url: '/AFE/ApproveAFE',
        data: JSON.stringify({ 'afeHDRID': Number(afeHDRIDEle.val()) }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        cache: false,
        success: function (data) {
            alert(data.data);
            if (data.IsValid) {
                window.location.href = '/AFE/ApproveEditAFE'
            }
        },
        error: function (e1, e2, e3) {
        }
    });
}

function bindAFENumber() {
    let currAfeTypeID = Number(afeTypeSelectEle.find(':selected').val());
    let currentAFENumerDTL = afeTypesRecordDTL.filter(x => x.Afe_type_id == currAfeTypeID);
    let afeTypesForCode = afeTypes.filter(x => x.Afe_type_id == currAfeTypeID);
    let currentDate = new Date();


    let currDate = txtAfeDateEle.val();
    if (!isNullEmpty(currDate)) {
        currentDate = new Date(currDate);
    }

    let currentAFENumerDTLCount = currentAFENumerDTL.length > 0 ? (currentAFENumerDTL[0].count + 1) : 01;
    if (afeTypesForCode.length > 0) {
        let afeNumber = afeTypesForCode[0].Afe_num_code + (currentDate.getMonth() + 1).toLocaleString('en-US', {
            minimumIntegerDigits: 2,
            useGrouping: false
        }) + currentDate.getFullYear() + "-" + currentAFENumerDTLCount.toLocaleString('en-US', {
            minimumIntegerDigits: 2,
            useGrouping: false
        });
        txtAFENumEle.val(afeNumber)
    }
}

txtAfeDateEle.change(function () {
    GetTypesRecordDetails();
})