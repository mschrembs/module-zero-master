using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace SimpleTenantSystem.Tenants.Dtos
{
    using ModuleZeroSampleProject.Tenants;

    /// <summary>
    /// This DTO class is used to send needed data to <see cref="ITenantAppService.UpdateTenant"/> method.
    /// 
    /// Implements <see cref="IInputDto"/>, thus ABP applies standard input process (like automatic validation) for it. 
    /// Implements <see cref="ICustomValidate"/> for additional custom validation.
    /// </summary>
    public class UpdateTenantInput : IInputDto
    {
        [Range(1, int.MaxValue)] //Data annotation attributes work as expected.
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format("[UpdateTenantInput > TenantId = {0}, Name = {1}, IsActive = {2}]", this.Id, this.Name, this.IsActive);
        }
    }
}