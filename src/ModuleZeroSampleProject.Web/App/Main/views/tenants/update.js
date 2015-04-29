(function () {
    var controllerId = 'app.views.tenants.update';
    angular.module('app').controller(controllerId, [
        '$state', '$location', 'abp.services.app.tenant',
        function ($state, $location, tenantService) {
            var vm = this;

            vm.tenant = null;

            var loadTenant = function () {
                abp.ui.setBusy(
                    null,
                    tenantService.getTenant({
                        id: $state.params.tenantId
                    }).success(function (data) {
                        vm.tenant = data.tenant;
                    })
                );
            };

            loadTenant();

            vm.localize = abp.localization.getSource('ModuleZeroSampleProject');

            vm.TenantUpdate = function () {
                abp.ui.setBusy(
                    null,
                    tenantService
                    .updateTenant(vm.tenant)
                    .success(function () {
                        abp.notify.info(abp.utils.formatString(vm.localize("TenantUpdatedMessage")));
                        $location.path('/tenants');
                    })
                );
            };
        }
    ]);
})();