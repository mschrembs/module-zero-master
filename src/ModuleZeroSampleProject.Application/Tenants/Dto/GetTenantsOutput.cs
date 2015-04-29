using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace SimpleTenantSystem.Tenants.Dtos
{
    using ModuleZeroSampleProject.Tenants.Dto;

    public class GetTenantsOutput : IOutputDto
    {
        public List<TenantDto> Tenants { get; set; } 
    }
}