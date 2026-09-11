using System;

namespace ProbashiShield.Domain.ViewModels.Admin
{
    public class JobCategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
