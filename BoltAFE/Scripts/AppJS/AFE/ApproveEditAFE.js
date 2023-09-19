var afeHdrs = [];
var tblAFEDtlBodyEle = $('#tblAFEDtl >  tbody');
var table = '';
$(document).ready(function () {
    $('#selectedMenu').text($('#menuApproveEditAFE').text());
    GetAllAFE();
});



function GetAllAFE() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/GetAllAFE',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: {},
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            destroyDataTable();
            afeHdrs = JSON.parse(data.AFEHdr);
            afeHdrsStr = '';
            let afeHDRIDArr = [];
            for (var i = 0; i < afeHdrs.length; i++) {
                let afeHDRID = afeHdrs[i].Afe_hdr_id;
                if ($.inArray(afeHDRID, afeHDRIDArr) !== -1) {
                    continue;
                }
                afeHDRIDArr.push(afeHDRID);
                let createdDate = toISOStringLocal(new Date(isNullEmptyValue(afeHdrs[i].Created_date)));
                let cretaedByEmail = isNullEmptyValue(afeHdrs[i].User_email);
                let cretaedBy = isNullEmptyValue(afeHdrs[i].Created_By);
                let afeName = isNullEmptyValue(afeHdrs[i].Afe_name)
                let afeType = isNullEmptyValue(afeHdrs[i].Type)
                let afeCategory = isNullEmptyValue(afeHdrs[i].Category);
                let afeDesc = isNullEmptyValue(afeHdrs[i].Description);
                let afeGrossAmount = isNullEmptyValue(afeHdrs[i].Gross_afe);
                let afeNetAmount = isNullEmptyValue(afeHdrs[i].Net_afe);
                let afePV10 = isNullEmptyValue(afeHdrs[i].Pv10);
                let afeROR = isNullEmptyValue(afeHdrs[i].Ror);
                let afeUserInbox = userEmail;
                let currentTrashIcon = Number(userIDEle.val()) == cretaedBy ? trashIcon : '';

                afeHdrsStr += '<tr><td><input type="hidden" value="' + afeHDRID + '">' + createdDate + '</td><td>' + cretaedByEmail + '</td><td>' + afeName + '</td><td>' + afeType + '</td><td>' + afeCategory + '</td><td>' + afeDesc + '</td><td>' + afeGrossAmount + '</td><td>' + afeNetAmount + '</td><td>' + afePV10 + '</td><td>' + afeROR + '</td><td>' + afeUserInbox + '</td><td onclick="window.location = \'/AFE/CreateAFE?afeHDR=' + afeHDRID + '\'">' + pencilIcon + '</td><td onclick="deleteAfeHDR('+ afeHDRID +')">' + currentTrashIcon + '</td></tr>';
            }
            tblAFEDtlBodyEle.html('');
            tblAFEDtlBodyEle.html(afeHdrsStr);
            createDataTable();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function deleteAfeHDR(afeHDRID) {
    if (afeHDRID > 0 && confirm('Are you sure do you want to delete this record?')) {
        deleteCancelAFEHDR(afeHDRID, true)
    }
}
function destroyDataTable() {
    if (table != '') {
        table.destroy();
    }
}

function createDataTable() {

    table = new DataTable('#tblAFEDtl', {
        dom: '<"top"i>rt<"bottom"flp><"clear">',
        pageLength: 25,
        paging: false,
        keys: true,
        searching: false,
        info: false
    });
}
