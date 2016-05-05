var TreeViewApp = angular.module('TreeViewApp', []);

TreeViewApp.factory('TreeViewService', ['$http', function ($http) {

    var TreeViewService = {};
    TreeViewService.getTreeViews = function () {
        return $http.get('/Home/TreeViewAjax');
    };
    return TreeViewService;

}]);

TreeViewApp.controller('TreeViewController', function ($scope, TreeViewService) {

    getTreeViews();
    function getTreeViews() {
        TreeViewService.getTreeViews()
            .success(function (dataList) {
                $scope.treeViews = dataList;
                console.log($scope.treeViews);
            })
            .error(function (error) {
                $scope.status = 'Unable to load customer data: ' + error.message;
                console.log($scope.status);
            });
    }
    
});
