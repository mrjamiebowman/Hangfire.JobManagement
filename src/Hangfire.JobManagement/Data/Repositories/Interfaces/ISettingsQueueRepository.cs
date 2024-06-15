using Hangfire.JobManagement.Data.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories.Interfaces;

public interface ISettingsQueueRepository
{
    Task<IEnumerable<SettingQueue>> GetAsync(CancellationToken cancellationToken = default);
}
