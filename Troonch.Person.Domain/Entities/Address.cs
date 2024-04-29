
using System;
using System.Drawing;
using Troonch.Domain.Base.Entities;

namespace Troonch.Person.Domain.Entities
{
    public class Address : BaseEntity
    {
        public int AddressNumber { get; set; }
        public Guid AddressTypeId { get; set; }
        public Guid? CustomerId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PostalCode { get; set; }
        public string Line {  get; set; }
        public string Region { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public bool IsDefault { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties
            public AddressType AddressType { get; set; }
            public Customer Customer { get; set; }
        #endregion
    }
}

