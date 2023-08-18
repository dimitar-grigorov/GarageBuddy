namespace GarageBuddy.Web.ViewModels.Admin.Customer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Base;

    using GarageBuddy.Services.Mapping.TypeConverters;

    using Services.Data.Models.ApplicationUser;
    using Services.Data.Models.Customer;
    using Services.Mapping;

    using static Common.Constants.GlobalValidationConstants;
    using static GarageBuddy.Common.Constants.EntityValidationConstants.Customer;

    public class CustomerCreateOrEditViewModel :
        BaseCreateOrEditViewModel,
        IMapTo<CustomerServiceModel>,
        IMapFrom<CustomerServiceModel>,
        IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        [StringLength(CustomerNameMaxLength, MinimumLength = CustomerNameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DefaultAddressMaxLength)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(DefaultEmailMaxLength)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(CustomerPhoneMaxLength)]
        [Phone]
        public string? Phone { get; set; }

        [StringLength(CustomerCompanyNameMaxLength)]
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [StringLength(UrlMaxLength)]
        [Url]
        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }

        [StringLength(DefaultDescriptionMaxLength)]
        [Display(Name = "Customer description")]
        public string? Description { get; set; }

        [Display(Name = "User")]
        public Guid? ApplicationUserId { get; set; }

        public IEnumerable<UserSelectServiceModel> Users { get; set; }
            = new List<UserSelectServiceModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
