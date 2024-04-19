using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troonch.Application.Base.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
}
