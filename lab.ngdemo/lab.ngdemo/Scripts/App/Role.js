var angularModule = angular.module("App", []);

angularModule.service('RoleService', ['$http', function ($http) {

    var roleService = this;

    var urlBase = '/Role/';

    roleService.GetList = function () {
        return $http.get(urlBase + 'GetListAjax');
    };

    roleService.GetById = function (roleName) {
        return $http.get(urlBase + 'GetByIdAjax/?roleName=' + roleName);
    };

    roleService.Save = function (role) {
        return $http.post(urlBase + 'SaveAjax', { role: role });
    };

    roleService.Delete = function (roleName) {
        return $http.post(urlBase + 'DeleteAjax/?roleName=' + roleName);
    };

}]);

angularModule.controller('RoleController', ['RoleService', '$scope', function (RoleService, $scope) {

    var roleController = this;
    roleController.Current = false;
    roleController.Add = false;
    roleController.Edit = false;

    $scope.RoleForm = false;
    $scope.Roles = [];
    $scope.Role = {};

    getList();

    function getList() {
        RoleService.GetList().success(function (dataList) {
            $scope.Roles = dataList;
        }).error(function (error) {
            App.toastrNotifier(error, false);
        });
    };

    $scope.Add = function() {
        initialAdd();
    };

    function initialAdd() {
        resetRoleForm();
        roleController.Add = true;
        $scope.Action = "Add";
        $scope.RoleForm = true;
    }

    $scope.Edit = function (index) {
        initialEdit();
        roleController.Current = index;
        //angular.copy($scope.Roles[index], Current);
        angular.copy($scope.Roles[index], $scope.Role);
    };

    function initialEdit() {
        roleController.Edit = true;
        $scope.Action = "Edit";
        $scope.RoleForm = true;
    }

    $scope.Delete = function (index) {
        var roleName = $scope.Roles[index].RoleName;

        bootbox.confirm("Do you want to delete this ?", function (isConfirm) {
            if (isConfirm) {
                RoleService.Delete(roleName).success(function (data) {

                    //$scope.Roles.splice(index);

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

        RoleService.Save($scope.Role).success(function (data) {

            if (data.IsSuccess) {

                $scope.Reset();

                App.toastrNotifier(data.SuccessMessage, true);

                //if (roleController.Edit) {
                //    $scope.Roles[roleController.Current] = $scope.Role;
                //} else {
                //    $scope.Roles.push($scope.Role);
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
        resetRoleForm();
        $scope.RoleForm = false;
    };

    $scope.Refresh = function () {
        getList();
    };

    function resetRoleForm() {
        $scope.Role = {};
    }

}]);