﻿namespace GarageBuddy.Web.ViewModels.Admin.BrandModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Common.Constants;
    using GarageBuddy.Services.Mapping.TypeConverters;

    using Services.Data.Models.Vehicle.Brand;
    using Services.Data.Models.Vehicle.BrandModel;
    using Services.Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.BrandModel;

    public class BrandModelCreateOrEditViewModel : IMapTo<BrandModelServiceModel>, IMapFrom<BrandModelServiceModel>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        [StringLength(BrandModelNameMaxLength, MinimumLength = BrandModelNameMinLength)]
        public string ModelName { get; set; } = null!;

        [Display(Name = "Deactivated")]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Deleted On")]
        public string? DeletedOn { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public Guid BrandId { get; set; }

        public IEnumerable<BrandSelectServiceModel> Brands { get; set; }
            = new List<BrandSelectServiceModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}