var afeTypes = '';
var afeCategories = '';
var tblTypesBodyEle = $('#tblTypes >  tbody');
var tblCategoryBodyEle = $('#tblCategory >  tbody');
var txtCategoryEle = $('#txtCategory');
var spanCatValEle = $('#spanCatVal');
var updateCategoryArr = [];
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

            //for (var i = 0; i < afeTypes.length; i++) {
            //    let id = afeTypes[i].Afe_type_id;
            //    let type = isNullEmpty(afeTypes[i].Type) ? "" : afeTypes[i].Type;
            //    afeTypesStr += '<option value="' + id + '">' + type + '</option>';
            //}
            for (var i = 0; i < afeCategories.length; i++) {
                let id = afeCategories[i].Afe_category_id;
                let category = isNullEmpty(afeCategories[i].Category) ? "" : afeCategories[i].Category;
                afeCategoriesStr += '<tr><td><input class="form-control cls-category" type="text" value="' + category + '" id="' + id + '"></td><td onclick="deleteCategory(' + id + ')">' + trashIcon + '</td>';
            }
            //afeTypeSelectEle.html(afeTypesStr);
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
        },
        error: function (err) {
            console.log(err);
        }
    });
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
        if (confirm('Are you sure do you want to update categories ?') &&updateCategoryArr.length > 0) {
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