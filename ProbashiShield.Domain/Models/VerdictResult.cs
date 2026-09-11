using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Models
{
    public class VerdictResult
    {
        public string Verdict { get; set; }
        public List<string> ReasonsBangla { get; set; } = new();
        public string SuggestedAction { get; set; } // proceed / ask agency to clarify / report to BMET
        public string Recommendation { get; set; } // raw
        public string BngRecommendation { get; set; } // translated recommendation in Bengali
    }
}
