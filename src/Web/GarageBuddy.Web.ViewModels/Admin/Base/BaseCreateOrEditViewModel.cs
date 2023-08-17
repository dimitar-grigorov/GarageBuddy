namespace GarageBuddy.Web.ViewModels.Admin.Base
{
    using System.ComponentModel.DataAnnotations;

    public class BaseCreateOrEditViewModel
    {
        [Display(Name = "Deactivated")]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Deactivated On")]
        public string? DeletedOn { get; set; }
    }
}
