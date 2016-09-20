var Employee = function () {

    var _validateForm = function () {
        // Setup form validation on the #ContactCreateOrEdit element
        if ($().validate) {
            var form = $("#frmEmployee");
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', //help-block help-block-error default input error message class
                focusInvalid: false, // do not focus the last invalid input
                // Specify the validation rules
                rules: {
                    Name: {
                        required: true
                    },
                    EmailAddress: {
                        required: true
                    },
                    Mobile: {
                        required: true
                    }
                },
                errorPlacement: function (error, element) { // render error placement for each input type
                    //if (element.attr("name") == "gender") { // for uniform radio buttons, insert the after the given container
                    //    error.insertAfter("#form_gender_error");
                    //} else if (element.attr("name") == "payment[]") { // for uniform checkboxes, insert the after the given container
                    //    error.insertAfter("#form_payment_error");
                    //} else {
                    //    error.insertAfter(element); // for other inputs, just perform default behavior
                    //}
                    var errorContainer = element.parents('div.form-group');
                    errorContainer.append(error);
                    //error.insertAfter(element);
                },
                messages: {
                    // Specify the custom validation error messages
                    Name: {
                        required: "Name is required."
                    },
                    EmailAddress: {
                        required: "Email address is required."
                    },
                    Mobile: {
                        required: "Email address is required."
                    }
                },
                invalidHandler: function (event, validator) { //display error alert on form submit              
                    success.hide();
                    error.show();
                },
                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').addClass('has-error'); // set error class to the control group
                },
                unhighlight: function (element) { // revert the change done by hightlight
                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },
                success: function (label) {
                    label.closest('.form-group').removeClass('has-error'); // set success class to the control group
                },
                submitHandler: function (form) {
                    if ($('#btnSaveEmployee').length > 0) {
                        var url = "/Employee/SaveAjax";
                        $.post(url, $(form).serializeArray(),
                            function (result) {
                                if (result.IsSuccess) {
                                    App.modalHide();
                                    App.toastrNotifier(result.SuccessMessage, true);
                                } else {
                                    App.toastrNotifier(result.ErrorMessage, false);
                                }
                            });
                    } else {
                        form.submit(function (e) { });
                    }

                }
            });
        }
    };

    var _resetForm = function () {
        $(':input', '#frmEmployee')
          .removeAttr('checked')
          .removeAttr('selected')
          .not(':button, :submit, :reset, :hidden, :radio, :checkbox')
          .val('');
    };

    var _formHide = function () {
        $("#divEmployeeForm").hide();
    };

    var _formShow = function () {
        $("#divEmployeeForm").show();
    };

    var _loadEmployeeDataTable = $('#employeeDataTable').dataTable({
        "bJQueryUI": true,
        "bAutoWidth": true,
        "sPaginationType": "full_numbers",
        //"bPaginate": true,
        //"iDisplayLength": 2,
        "bSort": true,
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ records per page",
            "sZeroRecords": "Nothing found - Sorry",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ records",
            "sInfoEmpty": "Showing 0 to 0 of 0 records",
            "sInfoFiltered": "(filtered from _MAX_ total records)"
        },
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "/Employee/GetDataTableListAjax",
        "sServerMethod": "GET",
        "aoColumns": [
            { "sName": "Name" }
            , { "sName": "EmailAddress" }
            , { "sName": "Mobile" }
            , {
                "sName": "Id",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, row) {
                    //console.log(data);
                    //console.log(type);
                    //console.log(row);
                    return '<a class="lnkEmployeeDetail btn btn-info btn-sm" href=\"/Employee/Details/' +
                                data + '\" >Details</a>  ' +
                                '<a class="lnkEmployeeEdit btn btn-primary btn-sm" href=\"/Employee/Edit/' +
                                data + '\" >Edit</a>  ' +
                                '<a class="lnkEmployeeDelete btn btn-danger btn-sm" href=\"/Employee/Delete/' +
                                data + '\" >Delete</a>';

            }}
        ]
        //"columnDefs": [{
        //    "targets": 3,
        //    "data": null,
        //    "render": function (data, type, row) {
        //        //console.log(data);
        //        //console.log(type);
        //        //console.log(row);
        //        return '<a class="lnkEmployeeDetail btn btn-info btn-sm" href=\"/Employee/Details/' +
        //            data[3] + '\" >Details</a>  ' +
        //            '<a class="lnkEmployeeEdit btn btn-primary btn-sm" href=\"/Employee/Edit/' +
        //            data[3] + '\" >Edit</a>  ' +
        //            '<a class="lnkEmployeeDelete btn btn-danger btn-sm" href=\"/Employee/Delete/' +
        //            data[3] + '\" >Delete</a>';

        //    },
        //}]
    });

    var _actionHandler = function () {

        $(document).on("click", "#btnEmployeeAdd", function () {
            _resetForm();
            _formShow();
        });

        $(document).on("click", "#btnEmployeeCancel", function () {
            _resetForm();
            _formHide();
        });

        $(document).on("click", ".lnkEmployeeEdit", function () {
            _resetForm();
            _formShow();
            var id = $(this).data("id");
            if (id != null && id != "") {
                App.sendAjaxRequest('/Employee/GetByIdAjax', { id: id }, true, function (result) {
                    if (result != null) {
                        $("#Id").val(result.Id);
                        $("#Name").val(result.Name);
                        $("#EmailAddress").val(result.EmailAddress);
                        $("#Mobile").val(result.Mobile);
                    }
                }, true, true, null);
            } else {
                _formHide();
            }
        });

        $(document).on("click", ".lnkEmployeeDelete", function () {
            var id = $(this).data("id");
            if (id != null && id != "") {
                bootbox.confirm("Do you want to delete this ?", function (isConfirm) {
                    if (isConfirm) {
                        App.sendAjaxRequest('/Employee/DeleteAjax', { id: id }, true, function (result) {

                            if (result.IsSuccess) {
                                App.toastrNotifier(result.SuccessMessage, true);
                            } else {
                                App.toastrNotifier(result.ErrorMessage, false);
                            }

                        }, true, true, null);
                    }
                });
            }
        });

    };

    var _initializeForm = function () {
        _loadEmployeeDataTable();
    };

    var initializeEmployee = function () {
        _validateForm();
        _initializeForm();
        _actionHandler();
    };

    return {
        init: initializeEmployee
    };
}();