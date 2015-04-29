// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TenantAppService.cs" company="">
//   
// </copyright>
// <summary>
//   The tenant app service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ModuleZeroSampleProject.Tenants
{
    using System.Collections.Generic;
    using System.Linq;

    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;
    using Abp.Domain.Repositories;
    using Abp.UI;

    using ModuleZeroSampleProject.MultiTenancy;
    using ModuleZeroSampleProject.Tenants.Dto;

    using SimpleTenantSystem.Tenants.Dtos;

    /// <summary>
    /// The tenant app service.
    /// </summary>
    public class TenantAppService : ApplicationService, ITenantAppService
    {
        #region Fields

        /// <summary>
        /// The _ tenant repository.
        /// </summary>
        private readonly IRepository<Tenant> tenantRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantAppService"/> class.
        /// </summary>
        /// <param name="tenantRepository">
        /// The tenant repository.
        /// </param>
        public TenantAppService(IRepository<Tenant> tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get tenants.
        /// </summary>
        /// <returns>
        /// The <see cref="ListResultOutput"/>.
        /// </returns>
        public GetTenantsOutput GetTenants()
        {
            return new GetTenantsOutput
                       {
                           Tenants =
                               this.tenantRepository.GetAllList()
                               .MapTo<List<TenantDto>>()
                       };
        }

        public GetTenantOutput GetTenant(GetTenantInput input)
        {
            Tenant tenant =
                this.tenantRepository.GetAll()
                    .FirstOrDefault(q => q.Id == input.Id);

            if (tenant == null)
            {
                throw new UserFriendlyException("There is no such a question. Maybe it's deleted.");
            }

            return new GetTenantOutput { Tenant = tenant.MapTo<TenantDto>() };
        }

        public void UpdateTenant(UpdateTenantInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            Logger.Info("Updating a tenant for input: " + input);

            //Retrieving a tenant entity with given id using standard Get method of repositories.
            var tenant = this.tenantRepository.Get(input.Id);

            //Updating changed properties of the retrieved tenant entity.

            tenant.IsActive = input.IsActive;

            tenant.Name = input.Name;
            tenant.TenancyName = input.Name;

            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public void CreateTenant(CreateTenantInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            Logger.Info("Creating a tenant for input: " + input);

            //Creating a new Tenant entity with given input's properties
            var tenant = new Tenant(input.Name, input.Name) { IsActive = input.IsActive };

            //Saving entity with standard Insert method of repositories.
            tenantRepository.Insert(tenant);
        }

        public void DeleteTenant(UpdateTenantInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            Logger.Info("Updating a tenant for input: " + input);

            //Retrieving a tenant entity with given id using standard Get method of repositories.
            var tenant = this.tenantRepository.Get(input.Id);

            this.tenantRepository.Delete(tenant);
        }

        #endregion
    }
}