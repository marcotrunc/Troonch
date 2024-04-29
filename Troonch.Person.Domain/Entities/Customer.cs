using System.Collections.Generic;
using Troonch.Domain.Base.Entities;

namespace Troonch.Person.Domain.Entities
{
    public class Customer : PersonBaseEntity
    {
        // Aggiungere punti di vendita preferiti

        //Aggiungere il lead source
        
        // Aggiungere la company id 
        
        public string DoNotSendEmail { get; set; }
        public string DoNotSendSMS { get; set; }
        public string DoNotSendFax { get; set; }
        public bool IsRegisteredFromEcommerce { get; set; }
        public bool IsRegisteredFromPOS { get; set; }
        public bool IsRegisteredFromCRM { get; set; }
        

        #region Navigation Properties
            public ICollection<Address> Addresses { get; set; } = new List<Address>();
        #endregion
    }
}
