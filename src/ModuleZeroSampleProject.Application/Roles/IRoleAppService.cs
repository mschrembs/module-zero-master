using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ModuleZeroSampleProject.Roles.Dto;

namespace ModuleZeroSampleProject.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        PagedResultOutput<RoleDto> GetRoles(GetRolesInput input);

        Task CreateRole(CreateRoleInput input);

        GetRoleOutput GetRole(GetRoleInput input);
    }
}
