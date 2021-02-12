function showModal(url, title) {
    $.ajax({
        method: "GET",
        url: url,
        success: function (response) {
            $("#form-modal .modal-body").html(response);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    });
}

AddProductAjaxPost = form => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $("#view-all").html(res.html);
                    $("#form-modal .modal-body").html('');
                    $("#form-modal .modal-title").html('');
                    $("#form-modal").modal('hide');
                } else
                    $("#form-modal .modal-body").html(res.html);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    catch (ex) {
        console.log(ex);
    }


    return false; //to prevent default form submit event
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $("#view-all").html(res.html);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        } catch (ex) {
            console.log(ex);
        }
    }

    //prevent default form submit event
    return false;
}


function pagination(pageId) {
    $.ajax({
        method: "GET",
        url: "/Product/GetProductsByPage",
        data: { pageId: pageId },
        success: function (res) {
            $("#products").html(res.html);
        }
    });
}

function SearchProduct(productName) {
    $.ajax({
        method: "GET",
        url: "/product/SearchProductByFilter",
        data: { productName: productName },
        success: function (res) {
            $("#products").html(res.html);
        }
    });
}


function GetCommentsOfProduct(productId, take) {
    $.ajax({
        method: "GET",
        url: "/Product/GetCommentsOfProduct",
        data: { productId: productId, take: take },
        success: function (res) {
            $("#showComments").html(res.html);
        }
    });
}


AddChildCategory = form => {
    try {
        $.ajax({
            method: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $("#view-all-childCategories").html(res.html);
                    $("#form-modal").modal('hide');
                } else
                    $("#form-modal .modal-body").html(res.html);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    catch (ex) {
        console.log(ex);
    }


    return false; //to prevent default form submit event
}