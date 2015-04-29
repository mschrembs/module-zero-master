using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using ModuleZeroSampleProject.Configuration;
using ModuleZeroSampleProject.Roles.Dto;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Roles
{
    using ModuleZeroSampleProject.Authorization;

    [AbpAuthorize]
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRepository<Role> _RoleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RoleAppService(IRepository<Role> RoleRepository, IRepository<User, long> userRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _RoleRepository = RoleRepository;
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public PagedResultOutput<RoleDto> GetRoles(GetRolesInput input)
        {
            if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount = SettingManager.GetSettingValue<int>(MySettingProvider.RolesDefaultPageSize);
            }

            var RoleCount = _RoleRepository.Count();
            var Roles =
                _RoleRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToList();

            return new PagedResultOutput<RoleDto>
                   {
                       TotalCount = RoleCount,
                       Items = Roles.MapTo<List<RoleDto>>()
                   };
        }

        [AbpAuthorize("CanCreateRoles")] //An example of permission checking
        public async Task CreateRole(CreateRoleInput input)
        {
            await _RoleRepository.InsertAsync(new Role(input.Title, input.Text));
        }

        public GetRoleOutput GetRole(GetRoleInput input)
        {
            var Role =
                _RoleRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .Include(q => q.Answers)
                    .Include("Answers.CreatorUser")
                    .FirstOrDefault(q => q.Id == input.Id);

            if (Role == null)
            {
                throw new UserFriendlyException("There is no such a Role. Maybe it's deleted.");
            }

            if (input.IncrementViewCount)
            {
                Role.ViewCount++;
            }

            return new GetRoleOutput
                   {
                       Role = Role.MapTo<RoleWithAnswersDto>()
                   };
        }

        public VoteChangeOutput VoteUp(EntityRequestInput input)
        {
            var Role = _RoleRepository.Get(input.Id);
            Role.VoteCount++;
            return new VoteChangeOutput(Role.VoteCount);
        }

        public VoteChangeOutput VoteDown(EntityRequestInput input)
        {
            var Role = _RoleRepository.Get(input.Id);
            Role.VoteCount--;
            return new VoteChangeOutput(Role.VoteCount);
        }

        [AbpAuthorize("CanAnswerToRoles")]
        public SubmitAnswerOutput SubmitAnswer(SubmitAnswerInput input)
        {
            var Role = _RoleRepository.Get(input.RoleId);
            var currentUser = _userRepository.Get(CurrentSession.GetUserId());

            Role.AnswerCount++;

            var answer = _answerRepository.Insert(
                new Answer(input.Text)
                {
                    Role = Role,
                    CreatorUser = currentUser
                });

            _unitOfWorkManager.Current.SaveChanges();

            return new SubmitAnswerOutput
                   {
                       Answer = answer.MapTo<AnswerDto>()
                   };
        }

        public void AcceptAnswer(EntityRequestInput input)
        {
            var answer = _answerRepository.Get(input.Id);
            _RoleDomainService.AcceptAnswer(answer);
        }
    }
}