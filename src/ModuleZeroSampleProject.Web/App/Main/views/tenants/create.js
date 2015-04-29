(function () {
    var controllerId = 'app.views.tenants.create';
    angular.module('app').controller(controllerId, [
        '$scope', '$location', 'abp.services.app.tenant',
        function ($scope, $location, tenantService) {
            var vm = this;

            vm.tenant = {
                name: '',
                isActive: false
            };

            vm.localize = abp.localization.getSource('ModuleZeroSampleProject');

            vm.TenantCreate = function () {
                abp.ui.setBusy(
                    null,
                    tenantService
                    .createTenant(vm.tenant)
                    .success(function () {
                        abp.notify.info(abp.utils.formatString(vm.localize("TenantCreatedMessage"), vm.tenant.name));
                        $location.path('/tenants');
                    })
                );
            };
        }
    ]);
})();