var EmployeeDataTablesFilterColumn = function () {

    //function dataTableSearchStopPropagation(event) {
    //    if (event.stopPropagation !== undefined) {
    //        event.stopPropagation();
    //    } else {
    //        event.cancelBubble = true;
    //    }
    //}

    var _validateForm = function () {

    };

    var _actionHandler = function () {

    };

    var _loadEmployeeDataTablesFilterColumnPatientDataTable = function (iDisplayLength, sAjaxSource) {

        //$('#employeeDataTable thead tr#dataTableFilterRow th').each(function () {
        //    var title = $('#employeeDataTable thead th').eq($(this).index()).text();
        //    $(this).html('<input type="text" onclick="dataTableSearchStopPropagation(event);" placeholder="' + title + '" />');
        //});

        var employeeDataTable = $('#employeeDataTable').DataTable({
            "bJQueryUI": true,
            "bAutoWidth": true,
            "sPaginationType": "full_numbers",
            "bPaginate": true,
            "iDisplayLength": iDisplayLength,
            "bSort": false,
            "bFilter": true,
            "bSortClasses": false,
            "lengthChange": false,
            "oLanguage": {
                "sLengthMenu": "Display _MENU_ records per page",
                "sZeroRecords": "Nothing found - Sorry",
                "sInfo": "Showing _START_ to _END_ of _TOTAL_ records",
                "sInfoEmpty": "Showing 0 to 0 of 0 records",
                "sInfoFiltered": "(filtered from _MAX_ total records)"
            },
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": sAjaxSource,
            "sServerMethod": "GET",
            "aoColumns": [
                //{ "sName": "Id", "bVisible": false, "bSearchable": false, "bSortable": false },
                  { "sName": "Name" },
                  { "sName": "EmailAddress" },
                  { "sName": "Mobile" },
                  {
                      "sName": "Id",
                    "bSearchable": false,
                    "bSortable": false,
                    "mRender": function (data, type, row) {

                        return '<a data-chart="bar" href="' + data + '" class="lnkEmployeeDataTablesFilterColumnPatientChart btn btn-sm btn-primary">Select</a>';

                    }
                }
            ],
            "initComplete": function (settings, json) {
                var filterLabel = '#employeeDataTable_filter label'
                //$(filterLabel).html().replace("Search:", '');
                //$(filterLabel).text().replace('"Search:"', '');
                //$('#employeeDataTable_filter label input').addClass('form-control dataTable-search');
                $(filterLabel).text('');
            }
        });

        // Apply the filter
        //$("#employeeDataTable thead input").on('keyup change', function () {
        //    employeeDataTable.column($(this).parent().index() + ':visible').search(this.value).draw();
        //});

        $("#employeeDataTable thead input").on('keyup', function () {
            employeeDataTable.column($(this).parent().index() + ':visible').search(this.value).draw();
        });

        $("#employeeDataTableSearch").on('keyup', function () {
            employeeDataTable.search(this.value).draw();
        });
    };

    var _initializeForm = function () {

    };

    var initializeEmployeeDataTablesFilterColumn = function () {
        _validateForm();
        _initializeForm();
        _actionHandler();
    };

    return {
        init: initializeEmployeeDataTablesFilterColumn,
        loadEmployeeDataTablesFilterColumnPatientDataTable: _loadEmployeeDataTablesFilterColumnPatientDataTable,
    };
}();