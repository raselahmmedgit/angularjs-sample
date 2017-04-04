var angularModule = angular.module('App', []);

angularModule.controller('EmployeeController', ['$scope', '$compile',
  function ($scope, $compile) {

      var employeeController = this;

      employeeController.Id = 0;
      employeeController.IsAdd = false;
      employeeController.IsEdit = false;
      employeeController.IsDelete = false;
      employeeController.IsDetails = false;

      $scope.EmployeeSaveShow = true;

      $scope.Employees = [];
      $scope.Employee = {};

      //Load DataTable
      var employeeDataTable;

      employeeList(App.displayLength());

      function employeeList(iDisplayLength) {

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

                          return '<a class="lnkEmployeeDetail btn btn-info btn-sm" ng-click="ctrl.Details(' + data + ')" href="javascript:;" >Details</a>  ' +
                              '<a class="lnkEmployeeEdit btn btn-primary btn-sm" ng-click="ctrl.Edit(' + data + ')" href="javascript:;" >Edit</a>  ' +
                              '<a class="lnkEmployeeDelete btn btn-danger btn-sm" ng-click="ctrl.Delete(' + data + ')" href="javascript:;" >Delete</a>';

                      }
                  }
              ],
              createdRow: function (row, data, dataIndex) {
                  $compile(angular.element(row).contents())($scope);
              }
          });

      };
      //Load DataTable

      //Add
      employeeController.Add = function () {
          initialAdd();
      };

      function initialAdd() {
          resetEmployeeForm();
          employeeController.IsAdd = true;
          $scope.Action = "Add";
          $scope.EmployeeFormShow = true;
      }

      employeeController.SubmitEmployeeForm = function () {

          if ($scope.EmployeeForm.$valid) {
              employeeSave($scope.Employee);
          }

      };
      //Add

      //Details
      employeeController.Details = function (id) {
          initialDetails();
          employeeController.Id = id;
          employeeGet(id);
      };

      function initialDetails() {
          employeeController.IsDetails = true;
          $scope.Action = "Details";
          $scope.EmployeeFormShow = true;
          $scope.EmployeeSaveShow = false;
      }
      //Details

      //Edit
      employeeController.Edit = function (id) {
          initialEdit();
          employeeController.Id = id;
          employeeGet(id);
      };

      function initialEdit() {
          employeeController.IsEdit = true;
          $scope.Action = "Edit";
          $scope.EmployeeFormShow = true;
      }
      //Edit

      //Delete
      employeeController.Delete = function (id) {
          initialDelete();
          employeeController.Id = id;

          bootbox.confirm("Do you want to delete this ?", function (isConfirm) {
              if (isConfirm) {
                  employeeDelete(id);
              }
          });


      };

      function initialDelete() {
          employeeController.IsDelete = true;
          $scope.Action = "Delete";
      }

      function employeeDelete(id) {
          App.sendAjaxRequest('/Employee/DeleteAjax', { id: id }, true, function (result) {

              if (result.IsSuccess) {
                  App.toastrNotifier(result.SuccessMessage, true);
                  reLoadDataTable();
              } else {
                  App.toastrNotifier(result.ErrorMessage, false);
              }

          }, true, true, null);
      }
      //Delete

      //Cancel
      employeeController.Cancel = function () {
          initialCancel();
      };

      function initialCancel() {
          resetEmployeeForm();
          $scope.EmployeeFormShow = false;
      };

      //Cancel

      //ReLoad DataTable
      employeeController.ReLoadDataTable = function () {
          employeeDataTable.fnDraw();
      };

      function reLoadDataTable() {
          employeeDataTable.fnDraw();
      }
      //ReLoad DataTable

      //Get
      function employeeGet(id) {
          App.sendAjaxRequest('/Employee/GetByIdAjax', { id: id }, false, function (result) {
              $scope.Employee = result;
          }, true, false, null);
          //data = App.getAjax('/Employee/GetByIdAjax', { id: id });
      }
      //Get

      //Save
      function employeeSave(model) {
          App.sendAjaxRequest('/Employee/SaveAjax', { employee: model }, true, function (result) {

              if (result.IsSuccess) {
                  App.toastrNotifier(result.SuccessMessage, true);
                  reLoadDataTable();
              } else {
                  App.toastrNotifier(result.ErrorMessage, false);
              }

          }, true, true, null);
      }
      //Save

      //Reset Form
      function resetEmployeeForm() {
          $scope.EmployeeForm.$setPristine();
          $scope.EmployeeForm.$setUntouched();
          $scope.EmployeeForm.$setSubmitted();
          $scope.Employee = {};
          reLoadDataTable();
      }
      //Reset Form
      
  }
]);