// ReSharper disable VirtualMemberCallInConstructor
namespace GarageBuddy.Data.Models
{
    using System;
    using System.Collections.Generic;

    using GarageBuddy.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
            this.Roles = new HashSet<IdentityUserRole<Guid>>();
            this.Claims = new HashSet<IdentityUserClaim<Guid>>();
            this.Logins = new HashSet<IdentityUserLogin<Guid>>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    }
}
