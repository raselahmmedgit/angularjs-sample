
var App = function () {

    var preloaderHandler = function (options) {
        options = $.extend(true, {}, options);
        options.animate = true;
        options.overlayColor = '#555';//'none';
        if (options.show) {
            var html = '';
            if (options.animate) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>' + '</div>';
            } //else if (options.iconOnly) {
            //    html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""></div>';
            //} else if (options.textOnly) {
            //    html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            //} else {
            //    html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            //}
            if (options.target) { // element blocking

                var el = $(options.target).is(document) ? $('body') : $(options.target);
                if (el.height() <= ($(window).height())) {
                    options.cenrerY = true;
                }
                el.block({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 9999,
                    centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                    css: {
                        top: '10%',
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            } else { // page blocking
                $.blockUI({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 9999,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            }
        } else {
            if (options.target) {
                var el = $(options.target).is(document) ? $('body') : $(options.target);
                el.unblock({
                    onUnblock: function () {
                        el.css('position', '');
                        el.css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        }
    };

    var modalHandler = function () {
        $('body').undelegate('.appModal', 'click').on('click', '.appModal', function (event) {

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


        //BUtton Click Animate Scroll to Destination
        $('a.scrollonclick[href*=#]:not([href=#])').click(function () {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top
                    }, 1000);
                    return false;
                }
            }
        });
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
            App.preloader({
                target: target,
                show: true
            });
        }

        return $.ajax({
            type: isPost ? "POST" : "GET",
            url: url,
            data: isPost ? JSON.stringify(data) : data,
            contentType: contentType,
            dataType: dataType,
            beforeSend: function (xhr) {
                App.preloader({
                    target: target,
                    show: true
                });
            },
            success: function (successData) {
                if (!isAsync) {
                    App.preloader({
                        target: target,
                        show: false
                    });
                }
                return typeof (callback) == 'function' ? callback(successData) : successData;
            },
            complete: function (xhr, status) {
                App.preloader({
                    target: target,
                    show: false
                });
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

    var loadDropdown = function (targetDropdown, dataSourceUrl, filterByValue, isSelect2, isPlaceholder, isMultiple, isTreeSelect2, selectedValue, isHideSearch) {
        var placeHolder = (typeof (isPlaceholder) == 'undefined' || (typeof (isPlaceholder) == 'boolean')) ? 'Select' : isPlaceholder;
        isPlaceholder = (typeof (isPlaceholder) == 'undefined' || (typeof (isPlaceholder) != 'boolean')) ? true : isPlaceholder;
        isSelect2 = (typeof (isSelect2) == 'undefined') ? true : isSelect2;
        isMultiple = (typeof (isMultiple) == 'undefined') ? false : isMultiple;
        isTreeSelect2 = (typeof (isTreeSelect2) == 'undefined') ? false : isTreeSelect2;
        isHideSearch = (isHideSearch == true) ? -1 : 10;

        App.sendAjaxRequest(dataSourceUrl, filterByValue, true, function (options) {
            var optionHtml = '';
            if (isSelect2) {
                $('#' + targetDropdown).select2('destroy');

                if ($.isArray(options) && (options.length > 0)) {

                    if ($('#' + targetDropdown).is("select")) {
                        optionHtml = (isPlaceholder) ? '<option></option>' : '';
                        if ($.isArray(options) && (options.length > 0)) {

                            $(options).each(function (index, option) {
                                var dataParam = '';
                                for (var pname in option) {
                                    if (option.hasOwnProperty(pname)) {

                                        if ($.inArray(pname, ['Value', 'Text']) == -1) {
                                            dataParam = dataParam + 'data-' + pname.toLowerCase() + '="' + option[pname] + '" ';
                                        }
                                    }
                                }
                                optionHtml += '<option value="' + option.Value + '" ' + dataParam + ((option.Value == selectedValue) ? " selected='selected'" : "") + '>' + option.Text + '</option>';
                            });
                        }
                        $('#' + targetDropdown).html(optionHtml);
                        $('#' + targetDropdown).select2({
                            placeholder: placeHolder,
                            minimumResultsForSearch: isHideSearch,
                            allowClear: true
                        });
                    } else {
                        var select2Options = [];
                        if (isTreeSelect2) {
                            select2Options = arrayToTree(options, null);
                        } else {
                            $(options).each(function (index, option) {
                                select2Options.push({ id: option.Value, text: option.Text + '' });
                            });
                        }
                        if (isPlaceholder) {
                            $('#' + targetDropdown).select2({
                                placeholder: placeHolder,
                                multiple: isMultiple,
                                data: select2Options,
                                minimumResultsForSearch: isHideSearch,
                                allowClear: true
                            });
                        } else {
                            $('#' + targetDropdown).select2({
                                multiple: isMultiple,
                                data: select2Options,
                                minimumResultsForSearch: isHideSearch,
                                allowClear: true
                            });
                        }
                    }

                } else {
                    if ($('#' + targetDropdown).is("select")) {
                        if (isPlaceholder) {
                            $('#' + targetDropdown).html('<option></option>');
                        } else {
                            $('#' + targetDropdown).html('');
                        }

                        $('#' + targetDropdown).select2({
                            placeholder: placeHolder,
                            minimumResultsForSearch: isHideSearch,
                            allowClear: true
                        });
                    } else {
                        $('#' + targetDropdown).select2({
                            placeholder: placeHolder,
                            multiple: isMultiple,
                            data: [],
                            minimumResultsForSearch: isHideSearch,
                            allowClear: true
                        });
                    }
                }
            } else {
                optionHtml = (isPlaceholder) ? '<option></option>' : '';
                if ($.isArray(options) && (options.length > 0)) {

                    $(options).each(function (index, option) {
                        optionHtml += '<option value="' + option.Value + '">' + option.Text + '</option>';
                    });

                }

                $('#' + targetDropdown).html(optionHtml);
            }
        }, true);
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

    var actionHandler = function () {
        modalHandler();
    };

    var initializeApp = function () {
        actionHandler();
    };

    return {
        init: initializeApp,
        modalShow: modalShow,
        modalHide: modalHide,
        preloader: preloaderHandler,
        sendAjaxRequest: sendAjaxRequest,
        loadDropdown: loadDropdown,
        toastrNotifier: toastrNotifier,
        toastrNotifierInfo: toastrNotifierInfo
    };
}();