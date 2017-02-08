var app = angular.module('App', ['ngGrid']);

app.factory('StudentService', function ($http) {
    return {
        getPage: function (pageParams) {
            return $http.get('/Student/GetPage', pageParams);
        }
    };
});

app.controller('StudentController', function ($scope, $log, StudentService) {
    var binding;

    $scope.gridVals = {
        data: [], // data to be displayed in the grid
        total: 0, // total number of rows in the table matching the filters
        filterOptions: {
            filterText: "", // initial value of the filterText
            useExternalFilter: true // // I am using my own server-side filtering mechanism, not ng-grid's client side one
        },
        pagingOptions: {
            pageSizes: [5, 10, 20], // drop down with page sizes
            pageSize: 5, // default page size
            currentPage: 1 // default page number on load,
        },
        // sortInfo of the grid. Initially sort the grid by Id in ascending order
        sortInfo: {
            fields: ['Id'],
            directions: ['asc']
        }
    };

    // bindGrid function retrieves paging, sorting and searching data from
    // $scope variables and passes it to StudentService.getPage function
    // and then sets our grid data.
    function bindGrid() {
        binding = true;

        var gridVals = $scope.gridVals;

        var options = {
            page: gridVals.pagingOptions.currentPage,
            pageSize: gridVals.pagingOptions.pageSize,
            filterText: gridVals.filterOptions.filterText,
            sortColumn: gridVals.sortInfo.fields[0],
            sortDirection: gridVals.sortInfo.directions[0]
        };

        StudentService.getPage({ params: options }).then(function (data) {
            var pageResult = data.data;
            gridVals.pagingOptions.currentPage = pageResult.page;
            gridVals.pagingOptions.pageSize = pageResult.pageSize;
            gridVals.total = pageResult.total;
            gridVals.data = pageResult.data;
        }).finally(function () {
            binding = false;
        });
    }

    $scope.$watch('gridVals.pagingOptions', function (newVal, oldVal) {
        if (!binding) {
            if (newVal != oldVal && (newVal.pageSize != oldVal.pageSize)) {
                binding = true;
                $scope.gridVals.pagingOptions.currentPage = 1;
            }

            bindGrid();
        }
    }, true);

    $scope.$watch('gridVals.filterOptions.filterText', function (newVal, oldVal) {
        if (!binding)
            bindGrid();
    }, true);

    $scope.$watch('gridVals.sortInfo', function (newVal, oldVal) {
        // reset the selected page to 1 if/when they click on the sort column
        if (!binding) {
            if (newVal != oldVal && (newVal.fields[0] != oldVal.fields[0] || newVal.directions[0] != oldVal.directions[0])) {
                binding = true;
                $scope.gridVals.pagingOptions.currentPage = 1;
            }

            bindGrid();
        }
    }, true);

    // here we set the gridOptions property.  Note that we are using all the
    // properties defined above
    $scope.gridOptions = {
        data: 'gridVals.data',
        totalServerItems: 'gridVals.total',
        pagingOptions: $scope.gridVals.pagingOptions,
        filterOptions: $scope.gridVals.filterOptions,
        useExternalSorting: true,
        sortInfo: $scope.gridVals.sortInfo,
        enablePaging: true,
        showFooter: true,
        columnDefs: [
            { field: 'Id', displayName: 'ID' },
            { field: 'Name', displayName: 'Name' },
            { field: 'EmailAddress', displayName: 'Email' }
        ]
    };

    // call bindGrid
    bindGrid();
});