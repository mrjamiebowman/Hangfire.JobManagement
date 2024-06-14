using Hangfire.JobManagement.Data.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        public Task<IEnumerable<Setting>> GetAsync(CancellationToken cancellationToken = default);

        public Task<Setting> GetByIdAsync(long id, CancellationToken cancellationToken = default);

        public Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);

        public Task<Setting> SaveAsync(Setting model, CancellationToken cancellationToken = default);

        public Task<GlobalSettings> SaveAsync(GlobalSettings model, CancellationToken cancellationToken = default);

        public Task<GlobalSettings> GetCompositeAsync(CancellationToken cancellationToken = default);
    }
}
