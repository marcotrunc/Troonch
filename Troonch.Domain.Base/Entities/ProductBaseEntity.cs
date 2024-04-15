using System;
using System.Collections.Generic;
using System.Text;

namespace Troonch.Domain.Base.Entities
{
    public class ProductBaseEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public string CoverImageLink { get; set; }
    }
}
