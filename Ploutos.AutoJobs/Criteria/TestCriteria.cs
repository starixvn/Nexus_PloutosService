using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class TestCriteria : CriteriaBase
    {
        public DataTable InputData { get; set; }

        //public int NewParam { get; set; }

        public override string GetSettingKey() => "Test_Table_Param";
        //public override string GetSettingKey() => "NAP_Chatbot_GetDataToSync";
        

        public override bool IsValid() => true;
    }
}
