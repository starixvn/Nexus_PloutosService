namespace Ploutos.AutoJobs.Criteria
{
    using Fanex.Data.Repository;

    public class GetPlayerProfileCriteria : CriteriaBase
    {
        public string BrandCode { get; set; }
        public int TotalRows { get; set; }
        public override string GetSettingKey() => "API_PlayerProfile_SelPush";
        public override bool IsValid() => true;
    }
}
