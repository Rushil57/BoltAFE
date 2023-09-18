
function deleteCancelAFEHDR(afeHDRID,isDelete) {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/AFE/DeleteAFEHDR',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        async: false,
        data: JSON.stringify({ 'afeHDRID': afeHDRID }),
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            if (isDelete) {
                alert(data.data)
                GetAllAFE();
            }
            else {
                window.location.href = '/Dashboard';
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}