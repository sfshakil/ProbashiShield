namespace ProbashiShield.Domain.Constants
{
    public static class CommonConstant
    {
        public static readonly string HighRiskAction = "টাকা প্রদানের আগে এজেন্সিকে ব্যাখ্যা দিতে বলুন অথবা বিএমইটি/রামরু-তে অভিযোগ করুন।";
        public static readonly string CautionAction = "এগোনোর আগে এজেন্সির কাছে বিস্তারিত জানতে চান।";
        public static readonly string VerifiedAction = "নথিটি সঠিক মনে হচ্ছে, আপনি এগোতে পারেন।";
        public enum VerificationVerdict
        {
            Verified,
            Caution,
            HighRisk
        }
    }
}
