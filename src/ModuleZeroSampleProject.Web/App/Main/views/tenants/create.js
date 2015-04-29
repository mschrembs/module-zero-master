(function () {
    var controllerId = 'app.views.tenants.create';
    angular.module('app').controller(controllerId, [
        '$scope', '$location', 'abp.services.app.tenant', '$modalInstance',
        function ($scope, $location, tenantService, $modalInstance) {
            var vm = this;

            vm.tenant = {
                name: '',
                isActive: false
            };

            vm.localize = abp.localization.getSource('ModuleZeroSampleProject');

            vm.save = function () {
                tenantService
                    .createTenant(vm.tenant)
                    .success(function () {
                        $modalInstance.close();
                        abp.notify.info(abp.utils.formatString(vm.localize("TenantCreatedMessage"), vm.tenant.name));
                    });
            };

            vm.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
})();