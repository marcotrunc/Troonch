﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces
{
    public interface IProductBrandRepository : IBaseRepository<ProductBrand>
    {
        Task<bool> IsUniqueNameAsync(Guid? id,string name);
        Task<bool> IsDeletableAsync(Guid id);
    }
}
