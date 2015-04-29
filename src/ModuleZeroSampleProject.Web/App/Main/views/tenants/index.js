(function () {
    var controllerId = 'app.views.tenants.index';
    angular.module('app').controller(controllerId, [
        '$scope', 'abp.services.app.tenant', '$modal',
        function ($scope, tenantService, $modal) {
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
        }
    ]);
})();