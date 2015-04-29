using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ModuleZeroSampleProject.Tenants.Dto
{
    using ModuleZeroSampleProject.MultiTenancy;

    [AutoMapFrom(typeof(Tenant))]
    public class TenantDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}