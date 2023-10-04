var global_all_user_data = [];
var global_admin_user_data = [];
var global_manager_user_data = [];
var global_other_user_data = [];
var txtaddNewUserNameEle = $("#txtaddNewUserName");
var txtCardMemberNameEle = $("#txtCardMemberName");
var updateUserNameArr = [];
var mainAddRoleSelectEle = $('#mainAddRoleSelect');
var txtApproverEle = $('#txtApprover');
$(document).ready(function () {
    $('#selectedMenu').text($('#menuAdmin').text());
    GetAllUsers();

    $(document).on('click', '.all-users-list li', function () {
        $(".all-users-list li").removeClass("active");
        $(this).addClass("active");
    });

    $(document).on('click', '#btnLogout', function () {
        $.ajax('Login/Signout');
        sessionStorage.clear();
        window.location = "/Login";
    });

    $(document).on('click', '.fa-cog', function (event) {
        var userid = this.id;
        $("#ResetPasswordUserId").val(userid);
        $("#ResetPasswordUser").show();
    });

    $(document).on('click', '.all-users-list-actions button[type="button"]:first', function () {
        txtaddNewUserNameEle.val('');
        txtCardMemberNameEle.val('');
        txtApproverEle.val(0);
        if (!txtaddNewUserNameEle.siblings("span").hasClass('d-none')) {
            txtaddNewUserNameEle.siblings("span").addClass('d-none')
        }
        if (!txtCardMemberNameEle.siblings("span").hasClass('d-none')) {
            txtCardMemberNameEle.siblings("span").addClass('d-none')
        }
        $("#addNewUser").show();
    });

    $(document).on('click', '.delete-modal', function () {
        $("#" + $(this).closest('.modal').attr('id')).hide();
    });

    $(document).on('click', '#addNewUser button:last', function () {
        var val = txtaddNewUserNameEle.val().toLowerCase();
        var cardMemberName = txtCardMemberNameEle.val().toLowerCase();
        let txtApproverVal = txtApproverEle.val();
        let userRoleID = mainAddRoleSelectEle.val();
        var isReturn = false;
        if (!txtaddNewUserNameEle.siblings("span").hasClass('d-none')) {
            txtaddNewUserNameEle.siblings("span").addClass('d-none')
            txtaddNewUserNameEle.siblings("span").text('This field is required');
        }
        if (!txtCardMemberNameEle.siblings("span").hasClass('d-none')) {
            txtCardMemberNameEle.siblings("span").addClass('d-none')
            txtCardMemberNameEle.siblings("span").text('This field is required');
        }
        if (val == "") {
            if (txtaddNewUserNameEle.siblings("span").hasClass('d-none')) {
                txtaddNewUserNameEle.siblings("span").removeClass('d-none')
                txtaddNewUserNameEle.siblings("span").text('This field is required');
            }
            isReturn = true;
        }
        else if (!validateEmail(val)) {
            if (txtaddNewUserNameEle.siblings("span").hasClass('d-none')) {
                txtaddNewUserNameEle.siblings("span").removeClass('d-none')
                txtaddNewUserNameEle.siblings("span").text('Enter valid email');
            }
            isReturn = true;
        }
        if (cardMemberName == "") {
            if (txtCardMemberNameEle.siblings("span").hasClass('d-none')) {
                txtCardMemberNameEle.siblings("span").removeClass('d-none')
                txtCardMemberNameEle.siblings("span").text('This field is required');
            }
            isReturn = true;
        }
        if (global_all_user_data.length > 0) {
            var filterArr = filterArr = global_all_user_data.filter(v => v.User_email.toLowerCase() === val.toLowerCase());
            if (filterArr.length > 0) {
                if (txtaddNewUserNameEle.siblings("span").hasClass('d-none')) {
                    txtaddNewUserNameEle.siblings("span").removeClass('d-none')
                    txtaddNewUserNameEle.siblings("span").text('This email is already exist');
                }
                isReturn = true;
            }
        }
        if (!isNumber(txtApproverVal)) {
            isReturn = true;
            $('#txtApproverVal').removeClass('d-none').text('Enter only numeric value.');
        }
        if (isReturn) {
            return;
        }
        else {
            AddNewUser(val, cardMemberName, txtApproverVal, userRoleID);
            $("#addNewUser").hide();
        }
    });

    $(document).on('click', '#removeUser', function () {
        if ($(".all-users-list li.active").length > 0) {
            $("#deleteConfirmationId").val($(".all-users-list li.active").attr('id').split('_')[2]);
            $("#deleteConfirmationUser").show();
        }
    });

    $('input#txtSearchEmailUserMaster').keyup(function () {
        var searchText = $(this).val();
        $('#tblUsers > tbody > tr').each(function () {

            var currentLiText = $(this).text(),
                showCurrentLi = currentLiText.indexOf(searchText) !== -1;

            $(this).toggle(showCurrentLi);

        });
    });

    $(document).on('click', '#deleteConfirmationUser button.btn-danger', function () {
        RemoveUser($("#deleteConfirmationId").val());
        $("#deleteConfirmationId").val('');
        $("#deleteConfirmationUser").hide();
    });
    $(document).on('click', '#deleteConfirmationUser button.btn-warning', function () {

        $("#deleteConfirmationId").val('');
        $("#deleteConfirmationUser").hide();
    });

    $(document).on('click', '#ResetPasswordUser button.btn-primary', function () {
        ResetPassword($("#ResetPasswordUserId").val());
        $("#ResetPasswordUserId").val('');
        $("#ResetPasswordUser").hide();
    });
    $(document).on('click', '#ResetPasswordUser button.btn-default', function () {

        $("#ResetPasswordUserId").val('');
        $("#ResetPasswordUser").hide();
    });

});

function RemoveUser(id) {
    if (id == undefined || id == 0 || id == "") {
        return;
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
        url: '/UserMaster/DeleteUser?id=' + id,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        async: false,
        data: {},
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            GetAllUsers();
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function ResetPassword(userid) {
    if (userid == undefined || userid == 0 || userid == "") {
        return;
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
        url: '/UserMaster/ResetPassword?UserEmail=' + userid,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: {},
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            alert(data.Message);

        },
        error: function (err) {
            console.log(err);
        }
    });
}
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

function GetRoleId(panel) {
    switch (panel) {
        case 'admin-panel':
            return 1;
        case 'manager-panel':
            return 2;
        case 'other-user-panel':
            return 3;
    }
}

function AddNewUser(userName, cardMemberName, approverVal,roleID) {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/UserMaster/AddNewUser',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: JSON.stringify({ 'userName': userName, 'cardMemberName': cardMemberName, 'approverVal': approverVal, 'roleID': roleID }),
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            GetAllUsers();
            alert(data.Message);

        },
        error: function (err) {
            console.log(err);
        }
    });
}

function BindAllUsers(data, globalAllRoles) {

    let rolesOptions = '';
    for (var i = 0; i < globalAllRoles.length; i++) {
        let roleID = globalAllRoles[i].RoleID;
        let roleName = isNullEmptyValue(globalAllRoles[i].RoleName);
        rolesOptions += '<option value="' + roleID +'" id="' + roleID + '">' + roleName + '</option>';
    }
    mainAddRoleSelectEle.html(rolesOptions);
    $("#txtSearchEmailUserMaster").val('');
    var _html = ``;
    $(".all-users-list ul").html('');
    data.sort(function (a, b) { return a.ID - b.ID });
    _html += "<table id='tblUsers' class='table'><thead><tr><th>Email</th><th>Card Member Full Name</th><th>Approver ($)</th><th>Role Assigned</th></tr></thead><tbody>";
    for (var i = 0; i < data.length; i++) {
        var userEmail = data[i].User_email;
        var userName = isNullEmptyValue(data[i].User_Name);
        let approverAmount = isNullEmptyDecValue(data[i].Approver_amount);
        let currRoleID = isNullEmptyDecValue(data[i].RoleID);
        
        let roleIndex = rolesOptions.indexOf('value="' + currRoleID + '"');
        let currRolesOptions = rolesOptions.substring(0, roleIndex)
            + ' selected ' + rolesOptions.substring(roleIndex);
        _html += `<tr><td><li class="ui-state-default float-start" id="all_user_` + data[i].User_ID + `">` + userEmail + `<img class = 'fa fa-key fa-cog' id="` + userEmail + `" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABmJLR0QA/wD/AP+gvaeTAAABT0lEQVRYhe2WTU7DMBCFPxCiElKDKLChO07ALdiyKRJHaIAekgooiwILFogl3IB2nbLIRDKWf5LGA5HgSVYsz4zfy4zHCfzjr2PDsbb6Sc5NZbIotgI2V3bawJnZWAZWRqBv3gqdLgF8L4NvriZAuxuAjpegE12gjtghTIkBcFzHMVmP14FmCXrABJgBSxkz4ArYDgWmyMAQeDb2sseT+KgI6Bnkb8AIOAD6wBnwKrZHPJloK2BikB867HvAu/hcagh4kPgLa/0WmMp8JD73GgIWEj+w1qfAjcwz8fnUELCU+Czgs1sJSN2GObAjm4cunVN5vriM62YgBwoZ44DfPvAhHHkqASHyE8oWzIBzg3xOpA1jow75WNbt2Dlw5HubpgIqAjudFXlBeTAXwJ34Ba/iprAFmSIL4DolWRMBBeWHJ4gUfz2+A1tr71//I/oCsWyCV9icYoQAAAAASUVORK5CYII=" style="height:24px;width:24px;" title="Reset Password"></i></li></td><td><input type="text"  id="` + userEmail + `" value='` + userName + `' class="form-control  userValue"></td><td><input type="number" value='` + approverAmount + `' class="form-control  approverAmount userValue"></td><td><select class="form-select userValue" id="select_` + userEmail + `" >` + currRolesOptions +`</select></td>`;
    }
    _html += "</tbody></table>"
    $(".all-users-list ul").append(_html);
    if (data.length > 0) {
        $(".all-users-list ul li:first").addClass("active");
    }

    $('.userValue').on('change focus blur keydown keyup keypress paste', function () {

        var thisTr = $(this).parent().parent();

        var value = thisTr.find('td:eq(1) >  input').val();
        var userEmail = thisTr.find('td:eq(0)').text();
        var approver = thisTr.find('td:eq(2) >  input').val();
        var roleID = thisTr.find('td:eq(3) >  select').val();
        let currUser = updateUserNameArr.filter(x => x.userEmail == userEmail);
        if (currUser.length > 0) {
            currUser[0].userName = value;
            currUser[0].approver = approver;
            currUser[0].roleID = roleID;
        }
        else {
            var currUserName = {
                userEmail: userEmail,
                userName: value,
                approver: approver,
                roleID: roleID
            }
            updateUserNameArr.push(currUserName);
        }
    })
}

function getUniqueValuesTest(array, key) {
    var result = array.filter(function (el, i, x) {
        return x.some(function (obj, j) {
            return obj[key] === el[key] && (x = j);
        }) && i == x;
    });

    return result;
}


function updateUserName() {
    if (updateUserNameArr.length > 0) {
        $.ajax({
            before: AddLoader(),
            complete: function () {
                setTimeout(function () {
                    RemoveLoader();
                }, 500);
            },
            type: "POST",
            url: '/UserMaster/UpdateUserNames',
            data: JSON.stringify({ 'userNameArr': JSON.stringify(updateUserNameArr) }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: false,
            success: function (data) {
                alert(data.Message)
                if (data.IsValid) {
                    window.location.reload();
                }
            },
            error: function (e1, e2, e3) {
            }
        });
    }
    else {
        alert('No user name updated.');
    }
}


function GetAllUsers() {
    $.ajax({
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            setTimeout(function () {
                RemoveLoader();
            }, 500);
        },
        url: '/UserMaster/GetAllUsers',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        async: false,
        data: {},
        success: function (data) {
            if (!data.IsValid) {
                return;
            }
            globalAllUsers = JSON.parse(data.allUsers);
            globalAllRoles = JSON.parse(data.roles);
            BindAllUsers(globalAllUsers, globalAllRoles);
        },
        error: function (err) {
            console.log(err);
        }
    });
}