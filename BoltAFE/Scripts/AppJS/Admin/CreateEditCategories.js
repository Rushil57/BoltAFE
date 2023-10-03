var afeTypes = '';
var afeCategories = '';
var tblTypesBodyEle = $('#tblTypes >  tbody');
var tblCategoryBodyEle = $('#tblCategory >  tbody');
var txtCategoryEle = $('#txtCategory');
var spanCatValEle = $('#spanCatVal');
var updateCategoryArr = [];
var updateTypesArr = [];


var mainAddTypeCategoryEle = $('#mainAddTypeCategory');
var typeEle = $('#type');
var codeEle = $('#code');
var grossAfeChkEle = $('#grossAfeChk');
var wiChkEle = $('#wiChk');
var nriChkEle = $('#nriChk');
var royChkEle = $('#royChk');
var netAfeChkEle = $('#netAfeChk');
var oilChkEle = $('#oilChk');
var gasChkEle = $('#gasChk');
var nglChkEle = $('#nglChk');
var boeChkEle = $('#boeChk');
var poChkEle = $('#poChk');
var rorChkEle = $('#rorChk');
var mroiChkEle = $('#mroiChk');
var spanTypeCategoryEle = $('#spanTypeCategory');
var spanTypeEle = $('#spanType');
var spanCodeEle = $('#spanCode');


$(document).ready(function () {
    $('#selectedMenu').text($('#menuAdmin').text());
    GetAllCategoriesTypes();
});

function GetAllCategoriesTypes() {
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
            let afeTypesStr = '';
            let afeCategoriesStr = '';
            let afeCategoriesSelect = '';
            let afeCategoriesOptions = '<option value="0"> -- Select -- </option>';

            for (var i = 0; i < afeCategories.length; i++) {
                let id = afeCategories[i].Afe_category_id;
                let category = isNullEmpty(afeCategories[i].Category) ? "" : afeCategories[i].Category;
                afeCategoriesStr += '<tr><td><input class="form-control cls-category" type="text" value="' + category + '" id="' + id + '"></td><td onclick="deleteCategory(' + id + ')">' + trashIcon + '</td>';
                afeCategoriesOptions += '<option value="' + id + '">' + category + '</option>'
            }
            afeCategoriesSelect = '<select class="form-select">' + afeCategoriesOptions + '</select>';

            mainAddTypeCategoryEle.html(afeCategoriesOptions);
            for (var i = 0; i < afeTypes.length; i++) {
                let id = afeTypes[i].Afe_type_id;
                let currCategoryID = afeTypes[i].CategoryID;
                let type = isNullEmpty(afeTypes[i].Type) ? "" : afeTypes[i].Type;
                let afeNumCode = isNullEmptyValue(afeTypes[i].Afe_num_code);

                let grossAfe = afeTypes[i].Include_gross_afe == 0 ? "F" : "T";
                let wi = afeTypes[i].Include_wi == 0 ? "F" : "T";
                let nri = afeTypes[i].Include_nri == 0 ? "F" : "T";
                let roy = afeTypes[i].Include_roy == 0 ? "F" : "T";
                let netAfe = afeTypes[i].Include_net_afe == 0 ? "F" : "T";
                let oil = afeTypes[i].Include_oil == 0 ? "F" : "T";
                let gas = afeTypes[i].Include_gas == 0 ? "F" : "T";
                let ngl = afeTypes[i].Include_ngl == 0 ? "F" : "T";
                let boe = afeTypes[i].Include_boe == 0 ? "F" : "T";
                let po = afeTypes[i].Include_po == 0 ? "F" : "T";
                let ror = afeTypes[i].Include_ror == 0 ? "F" : "T";
                let mroi = afeTypes[i].Include_mroi == 0 ? "F" : "T";


                let grossAfeCls = afeTypes[i].Include_gross_afe == 0 ? " txtRedBackground" : " txtGreenBackground";
                let wiCls = afeTypes[i].Include_wi == 0 ? " txtRedBackground" : " txtGreenBackground";
                let nriCls = afeTypes[i].Include_nri == 0 ? " txtRedBackground" : " txtGreenBackground";
                let royCls = afeTypes[i].Include_roy == 0 ? " txtRedBackground" : " txtGreenBackground";
                let netAfeCls = afeTypes[i].Include_net_afe == 0 ? " txtRedBackground" : " txtGreenBackground";
                let oilCls = afeTypes[i].Include_oil == 0 ? " txtRedBackground" : " txtGreenBackground";
                let gasCls = afeTypes[i].Include_gas == 0 ? " txtRedBackground" : " txtGreenBackground";
                let nglCls = afeTypes[i].Include_ngl == 0 ? " txtRedBackground" : " txtGreenBackground";
                let boeCls = afeTypes[i].Include_boe == 0 ? " txtRedBackground" : " txtGreenBackground";
                let poCls = afeTypes[i].Include_po == 0 ? " txtRedBackground" : " txtGreenBackground";
                let rorCls = afeTypes[i].Include_ror == 0 ? " txtRedBackground" : " txtGreenBackground";
                let mroiCls = afeTypes[i].Include_mroi == 0 ? " txtRedBackground" : " txtGreenBackground";

                let currCategoriesOptions = '';
                if (currCategoryID > 0) {
                    let categoryIndex = afeCategoriesSelect.indexOf(currCategoryID) + currCategoryID.toString().length + 1;
                    currCategoriesOptions = afeCategoriesSelect.substring(0, categoryIndex)
                        + ' selected ' + afeCategoriesSelect.substring(categoryIndex);
                }
                else {
                    currCategoriesOptions = afeCategoriesSelect;
                }
                let currentCategory =
                    afeTypesStr += '<tr><td class="categoroyID"><input type hidden class="typeID" value="' + id + '">' + currCategoriesOptions + '</td><td><input class="form-control  cls-type" type="text" value="' + type + '" id="' + id + '"></td><td><input class="form-control  cls-numcode" type="text" value="' + afeNumCode + '"></td><td><input class="form-control cls-afeField' + grossAfeCls + '" type="text" minlength="1" maxlength="1" value="' + grossAfe + '"></td><td><input class="form-control cls-afeField' + wiCls + '" type="text" minlength="1" maxlength="1" value="' + wi + '"></td><td><input class="form-control cls-afeField' + nriCls + '" type="text" minlength="1" maxlength="1" value="' + nri + '"></td><td><input class="form-control cls-afeField' + royCls + '" type="text" minlength="1" maxlength="1" value="' + roy + '"></td><td><input class="form-control cls-afeField' + netAfeCls + '" type="text" minlength="1" maxlength="1" value="' + netAfe + '"></td><td><input class="form-control cls-afeField' + oilCls + '" type="text" minlength="1" maxlength="1" value="' + oil + '"></td><td><input class="form-control cls-afeField' + gasCls + '" type="text" minlength="1" maxlength="1" value="' + gas + '"></td><td><input class="form-control cls-afeField' + nglCls + '" type="text" minlength="1" maxlength="1" value="' + ngl + '"></td><td><input class="form-control cls-afeField' + boeCls + '" type="text" minlength="1" maxlength="1" value="' + boe + '"></td><td><input class="form-control cls-afeField' + poCls + '" type="text" minlength="1" maxlength="1" value="' + po + '"></td><td><input class="form-control cls-afeField' + rorCls + '" type="text" minlength="1" maxlength="1" value="' + ror + '"></td><td><input class="form-control cls-afeField' + mroiCls + '" type="text" minlength="1" maxlength="1" value="' + mroi + '"></td><td onclick="deleteType(' + id + ')">' + trashIcon + '</td>';
            }

            tblTypesBodyEle.html(afeTypesStr);
            tblCategoryBodyEle.html('').html(afeCategoriesStr);

            $('.cls-category').on('change focus blur keydown keyup keypress paste', function () {
                let category = $(this).val();
                let categoryID = $(this).attr('id');
                let currCategory = updateCategoryArr.filter(x => x.Afe_category_id == categoryID);
                if (currCategory.length > 0) {
                    currCategory[0].category = category;
                }
                else {
                    var currCategoryArr = {
                        Category: category,
                        Afe_category_id: categoryID
                    }
                    updateCategoryArr.push(currCategoryArr);
                }
            })
            $('.cls-afeField').on('change focus blur keydown keyup keypress paste', function () {
                let value = $(this).val();
                if (value.toLowerCase() != 't' && value.toLowerCase() != 'f') {
                    $(this).val('');
                }
                updateTypeArr(this)
            })
            $('.categoroyID > select ,.cls-type,.cls-numcode').on('change focus blur keydown keyup keypress paste', function () {
                updateTypeArr(this)
            })
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function updateTypeArr(element) {
    let thisTr = $(element).parent().parent();
    let categoroyID = thisTr.find('.categoroyID > select').val();
    let typeID = thisTr.find('.typeID').val();
    let currType = thisTr.find('.cls-type').val();
    let currCode = thisTr.find('.cls-numcode').val();

    if (isNullEmpty(currType) || isNullEmpty(currCode) || categoroyID <= 0 || typeID <= 0) {
        return;
    }

    let currTypes = updateTypesArr.filter(x => x.Afe_type_id == typeID);
    if (currTypes.length > 0) {

        currTypes[0].Afe_type_id = typeID;
        currTypes[0].Type = currType;
        currTypes[0].Include_gross_afe = thisTr.find('td:eq(3) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_wi = thisTr.find('td:eq(4) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_nri = thisTr.find('td:eq(5) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_roy = thisTr.find('td:eq(6) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_net_afe = thisTr.find('td:eq(7) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_oil = thisTr.find('td:eq(8) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_gas = thisTr.find('td:eq(9) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_ngl = thisTr.find('td:eq(10) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_boe = thisTr.find('td:eq(11) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_po = thisTr.find('td:eq(12) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_pv10 = 0;
        currTypes[0].Include_f_and_d = 0;
        currTypes[0].Include_ror = thisTr.find('td:eq(13) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Include_mroi = thisTr.find('td:eq(14) >  input').val().toLowerCase() == "t" ? 1 : 0;
        currTypes[0].Afe_num_code = currCode;
        currTypes[0].CategoryID = categoroyID;
    }
    else {
        var currTypeArr = {
            Afe_type_id: typeID,
            Type: currType,
            Include_gross_afe: thisTr.find('td:eq(3) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_wi: thisTr.find('td:eq(4) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_nri: thisTr.find('td:eq(5) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_roy: thisTr.find('td:eq(6) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_net_afe: thisTr.find('td:eq(7) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_oil: thisTr.find('td:eq(8) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_gas: thisTr.find('td:eq(9) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_ngl: thisTr.find('td:eq(10) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_boe: thisTr.find('td:eq(11) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_po: thisTr.find('td:eq(12) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_pv10: 0,
            Include_f_and_d: 0,
            Include_ror: thisTr.find('td:eq(13) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Include_mroi: thisTr.find('td:eq(14) >  input').val().toLowerCase() == "t" ? 1 : 0,
            Afe_num_code: currCode,
            CategoryID: categoroyID,
        }
        updateTypesArr.push(currTypeArr);
    }
}

function addCategory() {
    openModel('categoryModel');
    spanCatValEle.addClass('d-none');
    txtCategoryEle.val('');
}

function saveCategory() {


    let isValidCategory = true;
    $('.cls-category').each(function () {
        if ($(this).val().toLowerCase().trim() == txtCategoryEle.val().toLowerCase().trim()) {
            isValidCategory = false;
        }
    })

    if (!isValidCategory) {
        alert('This category is already exists.Please enter unique name.');
        return;
    }
    AddLoader();
    setTimeout(function () {
        if (!isNullEmpty(txtCategoryEle.val())) {
            $.ajax({
                url: '/Admin/SaveCategory',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ 'category': txtCategoryEle.val() }),
                success: function (data) {
                    if (!data.IsValid) {
                        return;
                    }
                    alert(data.data);
                    window.location.reload();
                },
                error: function (err) {
                    console.log(err);
                    RemoveLoader();
                }
            });
        }
        else {
            spanCatValEle.removeClass('d-none');
        }
        RemoveLoader();
    }, 500);
}

function deleteCategory(categoryID) {
    AddLoader();
    setTimeout(function () {
        if (confirm('Are you sure do you want to delete this category ?') && categoryID > 0) {
            $.ajax({
                url: '/Admin/DeleteCategory',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ 'categoryID': categoryID }),
                success: function (data) {
                    if (!data.IsValid) {
                        return;
                    }
                    alert(data.data);
                    window.location.reload();
                },
                error: function (err) {
                    console.log(err);
                    RemoveLoader();
                }
            });
        }
        RemoveLoader();
    }, 500);
}

function updateCategory() {
    AddLoader();
    setTimeout(function () {
        if (confirm('Are you sure do you want to update categories ?') && updateCategoryArr.length > 0) {
            $.ajax({
                type: "POST",
                url: '/Admin/UpdateCategory',
                data: JSON.stringify({ 'categoryArr': JSON.stringify(updateCategoryArr) }),
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
                    RemoveLoader();
                }
            });
        }
        else {
            alert('No category updated.');
        }
        RemoveLoader();
    }, 500);
}



function addType() {
    openModel('typeModel');
    mainAddTypeCategoryEle.val(0);
    typeEle.val('');
    codeEle.val('');
    grossAfeChkEle.prop('checked', true);
    wiChkEle.prop('checked', true);
    nriChkEle.prop('checked', true);
    royChkEle.prop('checked', true);
    netAfeChkEle.prop('checked', true);
    oilChkEle.prop('checked', true);
    gasChkEle.prop('checked', true);
    nglChkEle.prop('checked', true);
    boeChkEle.prop('checked', true);
    poChkEle.prop('checked', true);
    rorChkEle.prop('checked', true);
    mroiChkEle.prop('checked', true);
    typeRemoveRequired();
}

function typeRemoveRequired() {
    spanTypeCategoryEle.addClass('d-none');
    spanTypeEle.addClass('d-none');
    spanCodeEle.addClass('d-none');
}

function saveType() {
    let isValidType = true;
    typeRemoveRequired();
    if (mainAddTypeCategoryEle.val() == 0) {
        spanTypeCategoryEle.removeClass('d-none');
        isValidType = false;
    }
    if (isNullEmpty(typeEle.val())) {
        spanTypeEle.removeClass('d-none');
        isValidType = false;
    }
    if (isNullEmpty(codeEle.val())) {
        spanCodeEle.removeClass('d-none');
        isValidType = false;
    }
    AddLoader();
    setTimeout(function () {
        if (isValidType) {
            let typeArr = {
                Afe_type_id: 0,
                Type: typeEle.val(),
                Include_gross_afe: grossAfeChkEle.is(':checked') ? 1 : 0,
                Include_wi: wiChkEle.is(':checked') ? 1 : 0,
                Include_nri: nriChkEle.is(':checked') ? 1 : 0,
                Include_roy: royChkEle.is(':checked') ? 1 : 0,
                Include_net_afe: netAfeChkEle.is(':checked') ? 1 : 0,
                Include_oil: oilChkEle.is(':checked') ? 1 : 0,
                Include_gas: gasChkEle.is(':checked') ? 1 : 0,
                Include_ngl: nglChkEle.is(':checked') ? 1 : 0,
                Include_boe: boeChkEle.is(':checked') ? 1 : 0,
                Include_po: poChkEle.is(':checked') ? 1 : 0,
                Include_pv10: 0,
                Include_f_and_d: 0,
                Include_ror: rorChkEle.is(':checked') ? 1 : 0,
                Include_mroi: mroiChkEle.is(':checked') ? 1 : 0,
                Afe_num_code: codeEle.val(),
                CategoryID: mainAddTypeCategoryEle.val(),
            }
            $.ajax({
                url: '/Admin/SaveType',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ 'type': JSON.stringify(typeArr) }),
                success: function (data) {
                    if (!data.IsValid) {
                        return;
                    }
                    alert(data.data);
                    window.location.reload();
                },
                error: function (err) {
                    console.log(err);
                    RemoveLoader();
                }
            });
        }
        RemoveLoader();
    }, 500);
}


function deleteType(typeID) {
    AddLoader();
    setTimeout(function () {
        if (confirm('Are you sure do you want to delete this type ?') && typeID > 0) {
            $.ajax({
                url: '/Admin/DeleteType',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ 'typeID': typeID }),
                success: function (data) {
                    if (!data.IsValid) {
                        return;
                    }
                    alert(data.data);
                    window.location.reload();
                },
                error: function (err) {
                    console.log(err);
                    RemoveLoader();
                }
            });
        }
        RemoveLoader();
    }, 500);
}

function updateTypes() {
    AddLoader();
    setTimeout(function () {
        if (confirm('Are you sure do you want to update types ?') && updateTypesArr.length > 0) {
            $.ajax({
                type: "POST",
                url: '/Admin/UpdateTypes',
                data: JSON.stringify({ 'typesArr': JSON.stringify(updateTypesArr) }),
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
                    RemoveLoader();
                }
            });
        }
        else {
            alert('No types updated.');
        }
        RemoveLoader();
    }, 500);
}