using ProbashiShield.Database.DBEntities.Common;

namespace ProbashiShield.Database.DBEntities
{
    public class JobCategory : BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}