


var Employee = function () {

    var _validateForm = function () {
        // Setup form validation on the #ContactCreateOrEdit element
        if ($().validate) {
            var form = $("#frmEmployeeEditor");
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', //help-block help-block-error default input error message class
                focusInvalid: false, // do not focus the last invalid input
                // Specify the validation rules
                rules: {
                    emp_name: {
                        required: true
                    },
                    emp_emailaddress: {
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
                    emp_name: {
                        required: "Name is required."
                    },
                    emp_emailaddress: {
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
                        var url = $(form).attr('action');
                        $.post(url, $(form).serializeArray(),
                            function (res) {
                                if (res.success) {

                                    App.modalHide();
                                    App.toastrNotifier(res.data, true);

                                } else {
                                    App.toastrNotifier(res.data, false);
                                }
                            });
                    } else {
                        form.submit(function (e) { });
                    }

                }
            });
        }
    };

    var _actionHandler = function () {

    };

    var _initializeForm = function () {

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