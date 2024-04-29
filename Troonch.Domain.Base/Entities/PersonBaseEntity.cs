using System;

namespace Troonch.Domain.Base.Entities
{
    public class PersonBaseEntity : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string IsEmailVerified { get; set; }
        public string IsPhoneNumberVerified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
