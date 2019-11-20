namespace Ploutos.AutoJobs.Services
{
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Repository;
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class Nova88Service : INova88Service
    {
        public Task PushData()
        {
            var inputData = new DataTable();
            inputData.Columns.Add("LocationName");
            inputData.Columns.Add("CostRate");
            inputData.Rows.Add("ab", 1);
            inputData.Rows.Add("cd", 3);

            var criteria = new TestCriteria()
            {
                InputData = inputData
            };

            var repository = new BaseRepository();
            var result = repository.FetchData<TestModel>(criteria).Result.ToList();

            return null;
        }
    }
}
