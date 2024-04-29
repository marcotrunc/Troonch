
using System.Collections.Generic;
using Troonch.Domain.Base.Entities;

namespace Troonch.Person.Domain.Entities
{
    public class AddressType : BaseEntity
    {
        public string Value { get; set; }

        #region Navigation Properties
            public ICollection<Address> Addresses { get; set; } = new List<Address>();
        #endregion
    }
}
