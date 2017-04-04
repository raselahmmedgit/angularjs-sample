var app = angular.module('App', []);

app.controller('EmployeeController', ['$scope', '$compile',
  function ($scope, $compile) {
      var employeeController = this;

      $('#report').DataTable({
          data: [{
              "LastName": "Doe",
              "Link": "<button type=\"button\" ng-click=\"ctrl.dataTablesAlert('hello')\">Test Alert</a>"
          }],
          columns: [{
              "title": "Last Name",
              "data": "LastName"
          }, {
              "title": "Actions",
              "data": "Link"
          }],
          createdRow: function (row, data, dataIndex) {
              $compile(angular.element(row).contents())($scope);
          }
      });

      employeeController.buttonAlert = function () {
          $('#buttondiv').addClass('success');
      };

      employeeController.htmlAlert = function () {
          $('#htmltablediv').addClass('success');
      };

      employeeController.dataTablesAlert = function (id) {
          $('#datatablediv').addClass('success');
      };

  }
]);