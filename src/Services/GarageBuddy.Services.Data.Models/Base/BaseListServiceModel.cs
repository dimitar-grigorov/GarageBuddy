namespace GarageBuddy.Services.Data.Models.Base
{
    using System;

    public class BaseListServiceModel
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
