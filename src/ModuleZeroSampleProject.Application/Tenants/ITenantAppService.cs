using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ModuleZeroSampleProject.Tenants.Dto;

namespace ModuleZeroSampleProject.Tenants
{
    using SimpleTenantSystem.Tenants.Dtos;

    public interface ITenantAppService : IApplicationService
    {
        GetTenantsOutput GetTenants();

        GetTenantOutput GetTenant(GetTenantInput input);

        void UpdateTenant(UpdateTenantInput input);

        void CreateTenant(CreateTenantInput input);

        void DeleteTenant(UpdateTenantInput input);
    }
}
