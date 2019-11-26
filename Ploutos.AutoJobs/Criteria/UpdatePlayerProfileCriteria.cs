namespace Ploutos.AutoJobs.Criteria
{
    using Fanex.Data.Repository;
    using System.Data;

    public class UpdatePlayerProfileCriteria : NonQueryCommand
    {
        public string BrandCode { get; set; }
        public DataTable PlayerProfile { get; set; }
        public override string GetSettingKey() => "Plo_API_PlayerProfile_Update";
        public override bool IsValid() => true;
    }
}
