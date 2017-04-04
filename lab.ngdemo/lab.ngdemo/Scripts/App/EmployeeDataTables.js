var angularModule = angular.module("App", []);

angularModule.service('EmployeeService', ['$http', function ($http) {

    var employeeService = this;

    var urlBase = '/Employee/';

    employeeService.GetList = function () {
        return $http.get(urlBase + 'GetListAjax');
    };

    employeeService.GetById = function (id) {
        return $http.get(urlBase + 'GetByIdAjax/?id=' + id);
    };

    employeeService.Save = function (employee) {
        return $http.post(urlBase + 'SaveAjax', { employee: employee });
    };

    employeeService.Delete = function (id) {
        return $http.post(urlBase + 'DeleteAjax/?id=' + id);
    };

    var employeeDataTable;

    employeeService.LoadDataTable = function (iDisplayLength) {
        employeeDataTable = $('#employeeDataTable').dataTable({
            "bJQueryUI": true,
            "bAutoWidth": true,
            "sPaginationType": "full_numbers",
            "bPaginate": true,
            "iDisplayLength": iDisplayLength,
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
            "sAjaxSource": "/Employee/GetDataTablesAjax",
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
                        //console.log(data);
                        //console.log(type);
                        //console.log(row);

                        //return '<a class="lnkEmployeeDetail btn btn-info btn-sm" href=\"/Employee/Details/' +
                        //    data + '\" >Details</a>  ' +
                        //    '<a class="lnkEmployeeEdit btn btn-primary btn-sm" href=\"/Employee/Edit/' +
                        //    data + '\" >Edit</a>  ' +
                        //    '<a class="lnkEmployeeDelete btn btn-danger btn-sm" href=\"/Employee/Delete/' +
                        //    data + '\" >Delete</a>';

                        return '<a class="lnkEmployeeDetail btn btn-info btn-sm" ng-click="EmployeeController.Details()" href="javascript:;" >Details</a>  ' +
                            '<a class="lnkEmployeeEdit btn btn-primary btn-sm" ng-click="EmployeeController.Edit()" href="javascript:;" >Edit</a>  ' +
                            '<a class="lnkEmployeeDelete btn btn-danger btn-sm" ng-click="EmployeeController.Delete()" href="javascript:;" >Delete</a>';

                    }
                }
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
    };

    employeeService.ReLoadDataTable = function () {
        employeeDataTable.fnDraw();
    };

}]);

angularModule.controller('EmployeeController', ['EmployeeService', '$scope', function (EmployeeService, $scope) {

    var employeeController = this;
    employeeController.Current = false;
    employeeController.Add = false;
    employeeController.Edit = false;

    employeeController.DetailsUrl = "/Employee/Details/";
    employeeController.EditUrl = "/Employee/Edit/";
    employeeController.DeleteUrl = "/Employee/Delete/";

    $scope.EmployeeFormShow = false;
    $scope.Employees = [];
    $scope.Employee = {};

    $scope.EmployeeDataTable = {};

    //employeeList();
    //function employeeList() {
    //    EmployeeService.GetList().success(function (dataList) {
    //        $scope.Employees = dataList;
    //    }).error(function (error) {
    //        App.toastrNotifier(error, false);
    //    });
    //};

    var employeeDataTable;
    employeeList();
    function employeeList() {

        var iDisplayLength = 5;
        //EmployeeService.LoadDataTable(iDisplayLength);

        employeeDataTable = $('#employeeDataTable').dataTable({
            "bJQueryUI": true,
            "bAutoWidth": true,
            "sPaginationType": "full_numbers",
            "bPaginate": true,
            "iDisplayLength": iDisplayLength,
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
            "sAjaxSource": "/Employee/GetDataTablesAjax",
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
                        //console.log(data);
                        //console.log(type);
                        //console.log(row);

                        //return '<a class="lnkEmployeeDetail btn btn-info btn-sm" href=\"/Employee/Details/' +
                        //    data + '\" >Details</a>  ' +
                        //    '<a class="lnkEmployeeEdit btn btn-primary btn-sm" href=\"/Employee/Edit/' +
                        //    data + '\" >Edit</a>  ' +
                        //    '<a class="lnkEmployeeDelete btn btn-danger btn-sm" href=\"/Employee/Delete/' +
                        //    data + '\" >Delete</a>';

                        return '<a class="lnkEmployeeDetail btn btn-info btn-sm" ng-click="EmployeeController.Details()" href="javascript:;" >Details</a>  ' +
                            '<a class="lnkEmployeeEdit btn btn-primary btn-sm" ng-click="EmployeeController.Edit()" href="javascript:;" >Edit</a>  ' +
                            '<a class="lnkEmployeeDelete btn btn-danger btn-sm" ng-click="EmployeeController.Delete()" href="javascript:;" >Delete</a>';

                    }
                }
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

    };

    $scope.Add = function () {
        initialAdd();
        //EmployeeService.ReLoadDataTable();
    };

    function initialAdd() {
        resetEmployeeForm();
        employeeController.Add = true;
        $scope.Action = "Add";
        $scope.EmployeeFormShow = true;
    }

    $scope.SubmitEmployeeForm = function () {

        // check to make sure the form is completely valid
        if ($scope.EmployeeForm.$valid) {
            App.toastrNotifier('Hello!', true);
        } else {
            App.toastrNotifier('Hello!', false);
        }

    };

    $scope.Details = function (index) {
        initialEdit();
        employeeController.Current = index;
        //angular.copy($scope.Employees[index], Current);
        angular.copy($scope.Employees[index], $scope.Employee);
    };

    $scope.Edit = function (index) {
        initialEdit();
        employeeController.Current = index;
        //angular.copy($scope.Employees[index], Current);
        angular.copy($scope.Employees[index], $scope.Employee);
    };

    function initialEdit() {
        employeeController.Edit = true;
        $scope.Action = "Edit";
        $scope.EmployeeFormShow = true;
    }

    $scope.Delete = function (index) {
        var id = $scope.Employees[index].Id;

        bootbox.confirm("Do you want to delete this ?", function (isConfirm) {
            if (isConfirm) {
                EmployeeService.Delete(id).success(function (data) {

                    //$scope.Employees.splice(index);

                    if (data.IsSuccess) {
                        App.toastrNotifier(data.SuccessMessage, true);
                    } else {
                        App.toastrNotifier(data.ErrorMessage, false);
                    }

                }).error(function (error) {
                    App.toastrNotifier(error, false);
                });

                $scope.Refresh();
            }
        });


    };

    $scope.Save = function () {

        EmployeeService.Save($scope.Employee).success(function (data) {

            if (data.IsSuccess) {

                $scope.Reset();

                App.toastrNotifier(data.SuccessMessage, true);

                //if (employeeController.Edit) {
                //    $scope.Employees[employeeController.Current] = $scope.Employee;
                //} else {
                //    $scope.Employees.push($scope.Employee);
                //}

            } else {
                App.toastrNotifier(data.ErrorMessage, false);
            }

        }).error(function (error) {
            App.toastrNotifier(error, false);
        });

        $scope.Refresh();
    };

    $scope.Cancel = function () {
        $scope.Reset();
    };

    $scope.Reset = function () {
        resetEmployeeForm();
        $scope.EmployeeFormShow = false;
    };

    $scope.Refresh = function () {
        employeeController.ReLoadDataTable();
    };

    function resetEmployeeForm() {
        $scope.EmployeeForm.$setPristine();
        $scope.EmployeeForm.$setUntouched();
        $scope.EmployeeForm.$setSubmitted();
        $scope.Employee = {};
    }

}]);

