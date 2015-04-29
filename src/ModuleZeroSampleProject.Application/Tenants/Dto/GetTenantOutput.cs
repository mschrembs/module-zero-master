using Abp.Application.Services.Dto;

namespace ModuleZeroSampleProject.Tenants.Dto
{
    public class GetTenantOutput : IOutputDto
    {
        public TenantDto Tenant { get; set; }
    }
}