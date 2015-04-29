(function () {
    var controllerId = 'app.views.tenants.update';
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

            vm.update = function () {
                tenantService
                    .updateTenant(vm.tenant)
                    .success(function () {
                        $modalInstance.close();
                        abp.notify.info(abp.utils.formatString(vm.localize("TenantUpdatedMessage")));
                    });
            };

            vm.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
})();