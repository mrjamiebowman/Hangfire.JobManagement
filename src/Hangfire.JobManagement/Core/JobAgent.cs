using Hangfire.Common;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangfire.JobManagement.Core
{
    public static class JobAgent
    {
        public const string tagRecurringJob = "recurring-job";
        public const string tagStopJob = "recurring-jobs-stop";

        public static void StartBackgroundJob(string JobId) {
            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction()) {
                transaction.RemoveFromSet(tagStopJob, JobId);
                transaction.AddToSet($"{tagRecurringJob}s", JobId);
                transaction.Commit();
            }
        }

        public static void StopBackgroundJob(string JobId) {
            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction()) {
                transaction.RemoveFromSet($"{tagRecurringJob}s", JobId);
                transaction.AddToSet($"{tagStopJob}", JobId);
                transaction.Commit();
            }
        }

        public static List<PeriodicJob> GetAllJobStopped() {
            var outPut = new List<PeriodicJob>();

            using (var connection = JobStorage.Current.GetConnection()) {
                var allJobStopped = connection.GetAllItemsFromSet(tagStopJob).ToList();

                allJobStopped.ForEach(jobId => {
                    var dto = new PeriodicJob();

                    var dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");
                    dto.Id = jobId;
                    dto.TimeZoneId = "UTC"; // Default

                    if (dataJob != null)
                    {
                        try
                        {
                            if (dataJob.TryGetValue("Job", out var payload) && !string.IsNullOrWhiteSpace(payload))
                            {
                                var invocationData = InvocationData.DeserializePayload(payload);
                                var job = invocationData.DeserializeJob();
                                dto.Method = job.Method.Name;
                                dto.Class = job.Type.Name;
                            }
                        }
                        catch (JobLoadException ex)
                        {
                            dto.Error = ex.Message;
                        }

                        if (dataJob.ContainsKey("TimeZoneId"))
                        {
                            dto.TimeZoneId = dataJob["TimeZoneId"];
                        }

                        if (dataJob.ContainsKey("NextExecution"))
                        {
                            var tempNextExecution = JobHelper.DeserializeNullableDateTime(dataJob["NextExecution"]);

                            dto.NextExecution = tempNextExecution.HasValue ? tempNextExecution.Value.ChangeTimeZone(dto.TimeZoneId).ToString("G") : "N/A";
                        }

                        if (dataJob.ContainsKey("LastJobId") && !string.IsNullOrWhiteSpace(dataJob["LastJobId"]))
                        {
                            dto.LastJobId = dataJob["LastJobId"];

                            var stateData = connection.GetStateData(dto.LastJobId);
                            if (stateData != null)
                            {
                                dto.LastJobState = stateData.Name;
                            }
                        }

                        if (dataJob.ContainsKey("Queue"))
                        {
                            dto.Queue = dataJob["Queue"];
                        }

                        if (dataJob.ContainsKey("Cron"))
                        {
                            dto.Cron = dataJob["Cron"];
                        }

                        if (dataJob.ContainsKey("LastExecution"))
                        {
                            var tempLastExecution = JobHelper.DeserializeNullableDateTime(dataJob["LastExecution"]);
                            dto.LastExecution = tempLastExecution.HasValue ? tempLastExecution.Value.ChangeTimeZone(dto.TimeZoneId).ToString("G") : "N/A";
                        }

                        if (dataJob.ContainsKey("CreatedAt"))
                        {
                            dto.CreatedAt = JobHelper.DeserializeNullableDateTime(dataJob["CreatedAt"]);
                            dto.CreatedAt = dto.CreatedAt.HasValue ? dto.CreatedAt.Value.ChangeTimeZone(dto.TimeZoneId) : new DateTime();
                        }

                        if (dataJob.TryGetValue("Error", out var error) && !string.IsNullOrEmpty(error))
                        {
                            dto.Error = error;
                        }

                        dto.Removed = false;
                        dto.JobState = "Stopped";

                        outPut.Add(dto);
                    }
                });
            }
            return outPut;
        }

        /// <summary>
        /// True when the job is currently stopped (it is in the stopped set).
        /// </summary>
        public static bool IsStopped(string jobId) {
            if (string.IsNullOrEmpty(jobId)) return false;

            using (var connection = JobStorage.Current.GetConnection()) {
                return connection.GetAllItemsFromSet(tagStopJob).Contains(jobId);
            }
        }

        /// <summary>
        /// The job (type, method and arguments) currently registered under this id, running or stopped.
        /// Null when there is none or it can no longer be loaded.
        /// </summary>
        public static Job GetRegisteredJob(string jobId) {
            if (string.IsNullOrEmpty(jobId)) return null;

            using (var connection = JobStorage.Current.GetConnection()) {
                var dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");

                if (dataJob == null || !dataJob.TryGetValue("Job", out var payload) || string.IsNullOrWhiteSpace(payload)) return null;

                try {
                    return InvocationData.DeserializePayload(payload).DeserializeJob();
                } catch (JobLoadException) {
                    return null;
                }
            }
        }

        /// <summary>
        /// Number of stopped jobs, without loading each job.
        /// </summary>
        public static long GetStoppedJobCount() => GetSetCount(tagStopJob);

        /// <summary>
        /// Number of managed jobs, running and stopped.
        /// </summary>
        public static long GetTotalJobCount() => GetSetCount($"{tagRecurringJob}s") + GetSetCount(tagStopJob);

        private static long GetSetCount(string key) {
            using (var connection = JobStorage.Current.GetConnection()) {
                if (connection is JobStorageConnection storageConnection) {
                    return storageConnection.GetSetCount(key);
                }

                return connection.GetAllItemsFromSet(key).Count;
            }
        }

        /// <summary>
        /// Loads a single stopped job for editing (full class name), or null when the job is not stopped.
        /// </summary>
        public static PeriodicJob GetStoppedJobForEdit(string jobId) {
            if (string.IsNullOrEmpty(jobId)) return null;

            using (var connection = JobStorage.Current.GetConnection()) {
                if (!connection.GetAllItemsFromSet(tagStopJob).Contains(jobId)) return null;

                var dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");
                if (dataJob == null) return null;

                var dto = new PeriodicJob { Id = jobId, JobState = "Stopped", Removed = false };

                try {
                    if (dataJob.TryGetValue("Job", out var payload) && !string.IsNullOrWhiteSpace(payload)) {
                        var job = InvocationData.DeserializePayload(payload).DeserializeJob();
                        dto.Method = job.Method.Name;
                        dto.Class = job.Type.FullName;
                    }
                } catch (JobLoadException ex) {
                    dto.Error = ex.Message;
                }

                if (dataJob.TryGetValue("Cron", out var cron)) dto.Cron = cron;
                if (dataJob.TryGetValue("Queue", out var queue)) dto.Queue = queue;
                if (dataJob.TryGetValue("TimeZoneId", out var timeZoneId)) dto.TimeZoneId = timeZoneId;
                if (dataJob.TryGetValue("LastJobId", out var lastJobId)) dto.LastJobId = lastJobId;

                return dto;
            }
        }

        public static bool IsValidJobId(string jobId, string tag = tagRecurringJob) {
            var result = false;
            using (var connection = JobStorage.Current.GetConnection()) {
                var job = connection.GetAllEntriesFromHash($"{tag}:{jobId}");

                result = job != null;
            }
            return result;
        }
    }
}
