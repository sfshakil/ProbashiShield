using System;

namespace ProbashiShield.Database.DBEntities
{
    public class AgencySyncLog
    {
        public long Id { get; set; }

        public DateTime SyncDate { get; set; }

        public int TotalRecords { get; set; }

        public int NewRecords { get; set; }

        public int UpdatedRecords { get; set; }

        public string? Status { get; set; }

        public string? Remarks { get; set; }
    }
}