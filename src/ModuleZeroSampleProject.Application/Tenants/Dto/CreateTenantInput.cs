using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace SimpleTenantSystem.Tenants.Dtos
{
    public class CreateTenantInput : IInputDto
    {
        [Required]
        public string Name { get; set; }
        
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format("[CreateTenantInput > Name = {0}, IsActive = {1}]", this.Name, this.IsActive);
        }
    }
}