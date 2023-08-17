// ReSharper disable VirtualMemberCallInConstructor
namespace GarageBuddy.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<Guid>, IAuditInfo, IDeletableEntity
    {
        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
