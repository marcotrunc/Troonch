using System;
using Troonch.Domain.Base.Entities;

namespace Troonch.Person.Domain.Entities
{
    public class User : PersonBaseEntity
    {
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public Guid? ChangePasswordToken { get; set; }
        public bool? IsChangePasswordTokenEnable { get; set; }
        public bool IsActive { get; set; }
    }
}
