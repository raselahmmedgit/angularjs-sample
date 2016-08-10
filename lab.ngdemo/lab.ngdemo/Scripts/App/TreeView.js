var treeViewApp = angular.module('TreeViewApp', []);

treeViewApp.factory('TreeViewService', ['$http', function ($http) {

    var treeViewService = {};
    treeViewService.getTreeViews = function () {
        return $http.get('/Home/TreeViewAjax');
    };
    return treeViewService;

}]);

treeViewApp.controller('TreeViewController', function ($scope, treeViewService) {

    getTreeViews();
    function getTreeViews() {
        treeViewService.getTreeViews()
            .success(function (dataList) {
                $scope.treeViews = dataList;
            })
            .error(function (error) {
                $scope.status = error;
                App.toastrNotifier($scope.status, false);
            });
    }
    
});
