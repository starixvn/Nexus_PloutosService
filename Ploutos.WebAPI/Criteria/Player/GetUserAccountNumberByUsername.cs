namespace Ploutos.WebAPI.Criteria.Player
{
    using Fanex.Data.Repository;

    public class GetUserAccountNumberByUsername : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey() => "Player_GetUserAccountNumberByUsername";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Username);
    }
}