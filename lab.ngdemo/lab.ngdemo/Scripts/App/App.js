
var angularModule = angular.module("App", []);

var appMessage = {
    Error: 'We are facing some problem while processing the current request. Please try again later.',
    NotFound: 'Requested object not found.',
    SaveSuccess: 'Save successfully.',
    UpdateSuccess: 'Update successfully.',
    DeleteSuccess: 'Delete successfully.'
};

var App = function () {

    var preloaderShow = function () {

        $('body').find('#appModalLoading').each(function () {
            var modal = $(this);
            modal.modal('show');
        });
    };

    var preloaderHide = function () {

        $('body').find('#appModalLoading').each(function () {
            var modal = $(this);
            modal.modal('hide');
        });
    };

    var modalHandler = function () {
        $('body').undelegate('.lnkAppModal', 'click').on('click', '.lnkAppModal', function (event) {

            event.preventDefault();
            var url = $(this).attr('href');
            var title = $(this).data('modal-title');
            var icon = $(this).data('modal-icon');
            var modalSize = '';
            var modalDialogSize = '';
            var isPost = false;
            if ($(this).data('ispost') == true) {
                isPost = true;
            }
            if ($(this).hasClass('modal-small')) {
                modalSize = 'bs-modal-sm';
                modalDialogSize = 'modal-sm';
            } else if ($(this).hasClass('modal-basic')) {
                modalSize = '';
                modalDialogSize = '';
            } else if ($(this).hasClass('modal-large')) {
                modalSize = 'bs-modal-lg';
                modalDialogSize = 'modal-lg';
            } else if ($(this).hasClass('modal-full')) {
                modalSize = '';
                modalDialogSize = 'modal-full';
            }

            $('body').find('#appModal').each(function () {
                var modal = $(this);
                (modalSize.length > 0) ? modal.addClass(modalSize) : '';
                (modalDialogSize.length > 0) ? modal.find('#appModalDialog').addClass(modalDialogSize) : '';
                modal.find('#appModalDialogTitle').html('<i class="' + icon + '"></i> ' + title);
                App.sendAjaxRequest(url, {}, isPost, function (result) {
                    modal.find('#appModalDialogContainer').html(result);
                    modal.modal('show');
                }, true, false);
            });
        });

    };

    var deleteHandler = function () {

        $('body').undelegate('.lnkAppDelete', 'click').on('click', '.lnkAppDelete', function (event) {

            event.preventDefault();
            var url = $(this).attr('href');
            var gridId = $(this).data('gridid');

            App.sendAjaxRequest(url, {}, true, function (result) {
                console.log(result);
            }, true, false);

        });

        return false;
    };

    var modalShow = function () {

        $('body').find('#appModal').each(function () {
            var modal = $(this);
            modal.modal('show');
        });
    };

    var modalHide = function () {

        $('body').find('#appModal').each(function () {
            var modal = $(this);
            modal.modal('hide');
        });
    };

    var sendAjaxRequest = function (url, data, isPost, callback, isAsync, isJson, target) {
        isJson = typeof (isJson) == 'undefined' ? true : isJson;
        var contentType = (isJson) ? "application/json" : "text/plain";
        var dataType = (isJson) ? "json" : "html";
        if (!isAsync) {
            App.preloaderShow();
        }

        return $.ajax({
            type: isPost ? "POST" : "GET",
            url: url,
            data: isPost ? JSON.stringify(data) : data,
            contentType: contentType,
            dataType: dataType,
            beforeSend: function (xhr) {
                App.preloaderShow();
            },
            success: function (successData) {
                if (!isAsync) {
                    App.preloaderHide();
                }
                return typeof (callback) == 'function' ? callback(successData) : successData;
            },
            complete: function (xhr, status) {
                App.preloaderHide();
            },
            error: function (exception) {
                return false;
            },
            async: isAsync
        });
    };

    var arrayToTree = function (arr, parent) {
        //arr.sort(function (a, b) { return parseInt(b.Level) - parseInt(a.Level) });
        var out = [];
        for (var i in arr) {
            if (arr[i].ParentId == parent) {
                var data = new Object();
                data.text = arr[i].Name;
                if (arr[i].Level == 3) {
                    data.id = arr[i].Id;
                } else {
                    var children = arrayToTree(arr, arr[i].Id);
                    if (children.length) {
                        data.children = children;
                    }
                }
                out.push(data);
            }
        }
        return out;
    };

    var loadDropdown = function (targetDropdown, dataSourceUrl, filterByValue) {

        App.sendAjaxRequest(dataSourceUrl, filterByValue, true, function (options) {
            var optionHtml = '';

            if ($.isArray(options) && (options.length > 0)) {

                $(options).each(function (index, option) {
                    optionHtml += '<option value="' + option.Value + '">' + option.Text + '</option>';
                });

            }

            $('#' + targetDropdown).html(optionHtml);

        }, true);

        $('#' + targetDropdown).val(0);

    };

    var toastrNotifier = function (msg, isSuccess) {
        toastr.clear();
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        if (isSuccess) {
            toastr['success'](msg, "Success !");
        } else {
            toastr['error'](msg, "Error !");
        }
    };

    var toastrNotifierInfo = function (msg) {
        toastr.clear();
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        toastr['info'](msg, "Info !");
    };

    var displayLength = function () {
        var _displayLength = 5;
        return _displayLength;
    };

    var actionHandler = function () {

        modalHandler();
        deleteHandler();

    };

    var initializeApp = function () {
        actionHandler();
    };

    var getAjax = function (getUrl, param) {

        $.getJSON(getUrl, param).done(function (data) {
            return data;
        }).fail(function (jqxhr, textStatus, error) {
            var msg = textStatus + ", " + error;
            toastr['error'](msg, "Error !");
        });

    };
    
    return {
        init: initializeApp,
        modalShow: modalShow,
        modalHide: modalHide,
        preloaderShow: preloaderShow,
        preloaderHide: preloaderHide,
        sendAjaxRequest: sendAjaxRequest,
        loadDropdown: loadDropdown,
        toastrNotifier: toastrNotifier,
        toastrNotifierInfo: toastrNotifierInfo,
        displayLength: displayLength,
        getAjax: getAjax
    };
}();