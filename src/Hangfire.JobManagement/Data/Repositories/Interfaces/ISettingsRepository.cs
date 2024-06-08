using Hangfire.JobManagement.Data.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories.Interfaces
{
    internal interface ISettingsRepository
    {
        Task<IEnumerable<Setting>> GetAsync(CancellationToken cancellationToken = default);

        Task<Setting> GetByIdAsync(long id, CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<Setting> SaveAsync(Setting model, CancellationToken cancellationToken = default);

        Task<GlobalSettings> SaveAsync(GlobalSettings model, CancellationToken cancellationToken = default);

        Task<GlobalSettings> GetCompositeAsync(CancellationToken cancellationToken = default);
    }
}
