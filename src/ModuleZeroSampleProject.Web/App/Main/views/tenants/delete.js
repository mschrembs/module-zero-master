(function () {
    var controllerId = 'app.views.tenants.delete';
    angular.module('app').controller(controllerId, [
        '$scope', '$location', 'abp.services.app.tenant', '$modalInstance', 'tenantId',
        function ($scope, $location, tenantService, $modalInstance, tenantId) {
            var vm = this;

            vm.tenant = null;

            var loadTenant = function () {
                abp.ui.setBusy(
                    null,
                    tenantService.getTenant({
                        id: tenantId
                    }).success(function (data) {
                        vm.tenant = data.tenant;
                    })
                );
            };

            loadTenant();

            vm.localize = abp.localization.getSource('ModuleZeroSampleProject');

            vm.delete = function () {
                tenantService
                    .deleteTenant(vm.tenant)
                    .success(function () {
                        $modalInstance.close();
                        abp.notify.info(abp.utils.formatString(vm.localize("TenantDeletedMessage", vm.tenant.name)));
                    });
            };

            vm.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
})();