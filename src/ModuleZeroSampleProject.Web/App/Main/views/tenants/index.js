(function () {
    var controllerId = 'app.views.tenants.index';
    angular.module('app').controller(controllerId, [
        '$scope', 'abp.services.app.tenant', '$modal', 'ngTableParams', '$filter',
        function ($scope, tenantService, $modal, ngTableParams, $filter) {
            var vm = this;
            
            vm.localize = abp.localization.getSource('ModuleZeroSampleProject');

            vm.tenants = [];

            vm.showTenantDeleteDialog = function (event) {
                var modalInstance = $modal.open({
                    templateUrl: abp.appPath + 'App/Main/views/tenants/delete.cshtml',
                    controller: 'app.views.tenants.delete as vm',
                    size: 'md',
                    resolve: {
                        tenantId: function() {
                            return event.target.id;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.loadTenants();
                });
            };

            vm.loadTenants = function () {
                abp.ui.setBusy(
                    null,
                    tenantService.getTenants().success(function (data) {
                        vm.tenants = data.tenants;
                    })
                );
            };

            vm.loadTenants();

            $scope.tableParams = new ngTableParams({
                page: 1,            // show first page
                count: 10,           // count per page
                sorting: {
                    name: 'asc' // initial sorting
                }
            }, {
                total: vm.tenants.length, // length of data
                getData: function ($defer, params) {
                    var orderedData = params.sorting() ?
                                $filter('orderBy')(vm.tenants, params.orderBy()) :
                                vm.tenants;
                    $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });
        }
    ]);
})();