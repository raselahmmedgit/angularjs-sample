var angularModule = angular.module("App", []);

angularModule.service('StudentService', ['$http', function ($http) {

    var studentService = this;

    var urlBase = '/Student/';

    studentService.GetList = function () {
        return $http.get(urlBase + 'GetCustomGridAjax');
    };

    studentService.GetById = function (id) {
        return $http.get(urlBase + 'GetByIdAjax/?id=' + id);
    };

    studentService.Save = function (student) {
        return $http.post(urlBase + 'SaveAjax', { student: student });
    };

    studentService.Delete = function (id) {
        return $http.post(urlBase + 'DeleteAjax/?id=' + id);
    };

}]);

angularModule.controller('StudentController', ['StudentService', '$scope', function (StudentService, $scope) {

    var studentController = this;
    studentController.Current = false;
    studentController.Add = false;
    studentController.Edit = false;

    $scope.GridLoadingShow = false;
    $scope.GridLoadingText = "Loading...";

    $scope.StudentFormShow = false;
    $scope.Students = [];
    $scope.Student = {};

    getList();

    function getList() {
        StudentService.GetList().success(function (dataList) {
            $scope.GridLoadingShow = true;

            $scope.Students = dataList;

            $scope.GridLoadingShow = false;
        }).error(function (error) {
            App.toastrNotifier(error, false);
        });
    };

    $scope.Search = function () {
        getList();
    };

    $scope.Add = function() {
        initialAdd();
    };

    function initialAdd() {
        resetStudentForm();
        studentController.Add = true;
        $scope.Action = "Add";
        $scope.StudentFormShow = true;
    }

    $scope.SubmitStudentForm = function () {

        // check to make sure the form is completely valid
        if ($scope.StudentForm.$valid) {
            App.toastrNotifier('Hello!', true);
        } else {
            App.toastrNotifier('Hello!', false);
        }

    };

    $scope.Edit = function (index) {
        initialEdit();
        studentController.Current = index;
        //angular.copy($scope.Students[index], Current);
        angular.copy($scope.Students[index], $scope.Student);
    };

    function initialEdit() {
        studentController.Edit = true;
        $scope.Action = "Edit";
        $scope.StudentFormShow = true;
    }

    $scope.Delete = function (index) {
        var id = $scope.Students[index].Id;

        bootbox.confirm("Do you want to delete this ?", function (isConfirm) {
            if (isConfirm) {
                StudentService.Delete(id).success(function (data) {

                    //$scope.Students.splice(index);

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

        StudentService.Save($scope.Student).success(function (data) {

            if (data.IsSuccess) {

                $scope.Reset();

                App.toastrNotifier(data.SuccessMessage, true);

                //if (studentController.Edit) {
                //    $scope.Students[studentController.Current] = $scope.Student;
                //} else {
                //    $scope.Students.push($scope.Student);
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
        resetStudentForm();
        $scope.StudentFormShow = false;
    };

    $scope.Refresh = function () {
        getList();
    };

    function resetStudentForm() {
        $scope.StudentForm.$setPristine();
        $scope.StudentForm.$setUntouched();
        $scope.StudentForm.$setSubmitted();
        $scope.Student = {};
    }

}]);