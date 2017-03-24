var pagination = (function() {

    var _checkHtmlInjection = function(settings) {
        var htmlspecialchars = { "&": "&amp;", "<": "&lt;", ">": "&gt;", "<<": "&laquo;", ">>": "&raquo;", '"': "&quot;" }

        $.each(htmlspecialchars, function(k, v) {
            settings.firstText = settings.firstText.replace(k, v);
            settings.prevText = settings.prevText.replace(k, v);
            settings.nextText = settings.nextText.replace(k, v);
            settings.lastText = settings.lastText.replace(k, v);
        });

        return settings;
    };

    var initializePagination = function(settings) {
        settings = jQuery.extend({
            pagingFor: {},
            recordPerPage: 10,              //Number of records per page
            numOfPagesTobeDisplayed: 10,         //Number of pages to be displayed
            totalNumberOfRecords: 10,     //Number of records to be handled
            currentPage: 0,
            numEdgeEntries: 0,
            linkTo: "#",
            firstText: "<<",
            prevText: "<",
            nextText: ">",
            lastText: ">>",
            ellipseText: "...",
            firstShowAlways: true,
            prevShowAlways: true,
            nextShowAlways: true,
            lastShowAlways: true,
            paramTobePassed: {},           //{LOOKUP_GROUP_ID: $("#LOOKUP_GROUP_ID").val()}
            url: 'null',
            preLoader: $('body'),
            callback: function () { return false; }
        }, settings || {});

        settings = _checkHtmlInjection(settings);

        /**
            *   Calculate Maximum Number of pages
            *   @return {int}
            */
        function getMaxNoOfPages() {
            return Math.ceil(settings.totalNumberOfRecords / settings.recordPerPage);
        }


        /**
        *   Generate star index and end index
        *   @return {array}
        */
        function generateStarEndIndex() {
            var neHalf = Math.ceil(settings.numOfPagesTobeDisplayed / 2);
            var np = getMaxNoOfPages();
            var upperLimit = np - settings.numOfPagesTobeDisplayed;
            var starIndex = currentPage > neHalf ? Math.max(Math.min(currentPage - neHalf, upperLimit), 0) : 0;
            var endIndex = currentPage > neHalf ? Math.min(currentPage + neHalf, np) : Math.min(settings.numOfPagesTobeDisplayed, np);
            return [starIndex, endIndex];
        }


        /**
        * function serverRequestHandler that interacts with server.
        *
        * Gets called every time the user clicks on a pagination link.
        *
        * param {int}pageIndex Start index of records
        * param {jQuery} jq the container with the pagination links as a jQuery object
        */
        function serverRequestHandler(pageIndex) {

            //settings.paramTobePassed.startIndex = (pageIndex * settings.recordPerPage) + 1;
            //settings.paramTobePassed.endIndex = Math.min((pageIndex + 1) * settings.recordPerPage, settings.totalNumberOfRecords);

            settings.paramTobePassed.iDisplayStart = pageIndex;

            JSON.stringify(settings.paramTobePassed);

            var t1 = +new Date();

            $.ajax({
                url: settings.url,
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(settings.paramTobePassed),
                contentType: 'application/json; charset=utf-8',
                crossDomain: true,
                xhrFields: {
                    withCredentials: true
                },
                beforeSend: function (xhr) {
                    RB.preloader({
                        target: settings.preLoader,
                        show: true
                    });
                },
                success: function (data) {
                    //var t2 = +new Date();
                    //console.log((t2 - t1) / 1000);
                    //var t3 = +new Date();
                    //jq.serverDataBindToViewOnlyGrid(data);
                    //var t4 = +new Date();
                    //console.log((t4 - t3) / 1000);

                    //var t5 = +new Date();
                    //jq.sortGrid(0, 0, 1);
                    //var t6 = +new Date();
                    //console.log((t6 - t5) / 1000);

                    if (typeof(settings.callback) == 'function') {
                        settings.callback(data);
                    }
                },
                complete: function (xhr, status) {
                    RB.preloader({
                        target: settings.preLoader,
                        show: false
                    });
                },
                error: function (data) {
                    $(this).html(JSON.stringify(data));
                }
            });

            // Prevent click event
            return false;
        }


        /**
        * This is the event handling function for the pagination links. 
        * @param {int} pageId The new page number
        */
        function pageSelected(pageId, evt) {
            currentPage = pageId;
            drawLinks();
            var continuePropagation = serverRequestHandler(pageId);

            if (!continuePropagation) {
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                } else {
                    evt.cancelBubble = true;
                }
            }
            return continuePropagation;
        }

        /**
        * This function inserts the pagination links into the container element
        */
        function drawLinks() {
            panel.empty();
            var interval = generateStarEndIndex();
            var np = getMaxNoOfPages();

            // This helper function returns a handler function that calls pageSelected with the right pageId
            var getClickHandler = function (pageId) {
                return function(evt) { return pageSelected(pageId, evt); };
            }

            // Helper function for generating a single link (or a span tag if it's the current page)
            var appendItem = function (pageId, appendopts) {
                pageId = pageId < 0 ? 0 : (pageId < np ? pageId : np - 1); // Normalize page id to sane value
                appendopts = jQuery.extend({ text: pageId + 1, classes: "" }, appendopts || {});
                if (pageId == currentPage) {
                    var lnk = jQuery("<li class='active'><a href='javascript:;'>" + (appendopts.text) + "</a></li>");
                }
                else {
                    var lnk = $("<a>" + (appendopts.text) + "</a>")
                    .bind("click", getClickHandler(pageId))
                    .attr('href', settings.linkTo.replace(/__id__/, pageId));
                    lnk = $('<li></li>').append(lnk);
                }
                //if (appendopts.classes) { lnk.addClass(appendopts.classes); }
                panel.append(lnk);
            }

            // Generate "Fist"-Link
            if (settings.firstText && (currentPage > 0 || settings.firstShowAlways)) {
                appendItem(0, { text: "<i class='fa fa-angle-double-left'></i>" }); //settings.firstText, classes: "first"
            }

            // Generate "Previous"-Link
            if (settings.prevText && (currentPage > 0 || settings.prevShowAlways)) {
                appendItem(currentPage - 1, { text: "<i class='fa fa-angle-left'></i>"}); //settings.prevText, classes: "prev" 
            }

            // Generate starting points
            if (interval[0] > 0 && settings.numEdgeEntries > 0) {
                var end = Math.min(settings.numEdgeEntries, interval[0]);
                for (var i = 0; i < end; i++) {
                    appendItem(i);
                }
                if (settings.numEdgeEntries < interval[0] && settings.ellipseText) {
                    jQuery("<li>" + settings.ellipseText + "</li>").appendTo(panel);
                }
            }

            // Generate interval links
            for (var i = interval[0]; i < interval[1]; i++) {
                appendItem(i);
            }

            // Generate ending points
            if (interval[1] < np && settings.numEdgeEntries > 0) {
                if (np - settings.numEdgeEntries > interval[1] && settings.ellipseText) {
                    jQuery("<li>" + settings.ellipseText + "</li>").appendTo(panel);
                }
                var begin = Math.max(np - settings.numEdgeEntries, interval[1]);
                for (var i = begin; i < np; i++) {
                    appendItem(i);
                }
            }

            // Generate "Next"-Link
            if (settings.nextText && (currentPage < np - 1 || settings.nextShowAlways)) {
            appendItem(currentPage + 1, { text: "<i class='fa fa-angle-right'></i>"}); //settings.nextText, classes: "next"
            }

            // Generate "Last"-Link
            if (settings.nextText && (currentPage < np - 1 || settings.lastShowAlways)) {
                appendItem(np, { text: "<i class='fa fa-angle-double-right'></i>" }); //settings.lastText, classes: "last"
            }
        }


        // Extract currentPage from options
        var currentPage = settings.currentPage;

        // Create a sane value for maxentries and recordPerPage
        settings.totalNumberOfRecords = (!settings.totalNumberOfRecords || settings.totalNumberOfRecords < 0) ? 1 : settings.totalNumberOfRecords;
        settings.recordPerPage = (!settings.recordPerPage || settings.recordPerPage < 0) ? 1 : settings.recordPerPage;

        // Store DOM element for easy access from all inner functions
        var panel = '';
        var grid = '';
        if (settings.pagingFor.length > 0) {
            panel = $('<ul/>', {
                class: 'pagination'
            });

            $(settings.pagingFor).siblings('div#divGridPaging').find('.pagination').remove();

            $(settings.pagingFor).siblings('div#divGridPaging').append(panel);
            
            grid = $(settings.pagingFor); //$(this);
        } else {
            console.log('Invalid pagination settings provided.');
            return false;
        }

        // Attach control functions to the DOM element 
        this.selectPage = function(pageId) { pageSelected(pageId); };

        this.prevPage = function () {
            if (currentPage > 0) {
                pageSelected(currentPage - 1);
                return true;
            } else {
                return false;
            }
        }

        this.nextPage = function () {
            if (currentPage < getMaxNoOfPages() - 1) {
                pageSelected(currentPage + 1);
                return true;
            } else {
                return false;
            }
        }

        // When all initialisation is done, draw the links
        drawLinks();

        // call callback function
        //serverRequestHandler(currentPage);//settings.callback(settings.paramTobePassed, settings.url, this);

    };

    return {
        init: initializePagination
    };
}());