using Hangfire.JobManagement.Data.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories.Interfaces;

public interface ISettingsQueueRepository
{
    Task<IEnumerable<SettingQueue>> GetAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<SettingQueue> SaveAsync(SettingQueue model, CancellationToken cancellation = default);
}
