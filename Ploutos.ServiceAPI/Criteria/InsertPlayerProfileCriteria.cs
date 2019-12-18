namespace Ploutos.ServiceAPI.Criteria
{
    using System.Data;
    using Fanex.Data.Repository;

    public class InsertPlayerProfileCriteria : NonQueryCommand
    {
        public DataTable PlayerProfile { get; set; }
        public int Error { get; set; }
        public string Message { get; set; }

        public override string GetSettingKey() => "Plo_PlayerProfile_Insert";

        public override bool IsValid() => true;
    }
}