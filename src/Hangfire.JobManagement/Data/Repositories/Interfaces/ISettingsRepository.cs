using Hangfire.JobManagement.Data.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories.Interfaces
{
    internal interface ISettingsRepository
    {
        Task<IEnumerable<Setting>> GetAsync(CancellationToken cancellationToken);

        Task<Setting> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task DeleteByIdAsync(long id, CancellationToken cancellationToken);

        Task SaveAsync(Setting model, CancellationToken cancellationToken);

        Task<GlobalSettings> GetCompositeAsync(CancellationToken cancellationToken);
    }
}
