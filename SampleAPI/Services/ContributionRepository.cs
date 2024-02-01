using Classes;
using Data;
using Data.DTOs;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace Services
{
    public class ContributionRepository : RepositoryBase, IContributionRepository, IDisposable
    {
        private APIDbContext _context;
        public ContributionRepository(APIDbContext context)
        {
            _context = context;
        }
        public async Task<List<Data.DTOs.ViewContributionsList>> GetContributionsViewListAsync()
        {
            return await _context.ViewContributionsList.ToListAsync();
        }
        public async Task<List<Data.Entities.Contribution>> GetContributionsAsync()
        {
            return await _context.Contributions
                .Where(x => x.ProductId == Globals.ApplicationProductId).ToListAsync();

            // this works returns all the fund and contributor data for each contribution
            // need to modify DTOs.Contribution and Entities.Contribution to include Fund and Contributor entity if uncomment this.
            //return await _context.Contributions
            //    .Include(f => f.Fund)
            //    .Include(c => c.Contributor)
            //    .Where(x => x.ProductId == Globals.ApplicationProductId).ToListAsync();
        }
        public async Task<List<Data.Entities.Contribution>> GetContributionByContributorAsync(int ContributorId)
        {
            if (ContributorId == 0)
            {
                throw new ArgumentNullException(nameof(ContributorId));
            }
            return await _context.Contributions.Where(x => x.ProductId == Globals.ApplicationProductId && x.ContributorId == ContributorId).ToListAsync();
        }
        public async Task<Data.Entities.Contribution> GetContributionAsync(int ContributionId)
        {
            if (ContributionId == 0)
            {
                throw new ArgumentNullException(nameof(ContributionId));
            }
            return await _context.Contributions.FirstOrDefaultAsync(a => a.Id == ContributionId);
        }
        public async Task<bool> ContributionExistsAsync(int ContributionId)
        {
            if (ContributionId == 0)
            {
                throw new ArgumentNullException(nameof(ContributionId));
            }
            return await _context.Contributions.AnyAsync(a => a.Id == ContributionId);
        }
        public void AddContribution(Data.Entities.Contribution Contribution)
        {
            // Contribution is from Entities
            if (Contribution == null)
            {
                throw new ArgumentNullException(nameof(Contribution));
            }
            _context.Contributions.Add(Contribution);
        }
        public void UpdateContribution(Data.Entities.Contribution Contribution)
        {
            // no code in this implementation
        }
        public async Task<Data.Entities.Contribution> DeleteContribution(int ContributionId)
        {
            var result = await _context.Contributions.FirstOrDefaultAsync(a => a.Id == ContributionId);
            if (result != null)
            {
                _context.Contributions.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
        public async Task<List<Data.DTOs.ContributionChartData>> GetContributionChartDataAsync(int fundId, string selectedYear)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("SELECT MAX(CONVERT(varchar(12), DATENAME(MONTH, C.ContributionDate))) AS [Month],SUM(C.Amount) AS [TotalAmount] FROM Contribution ");
            sb.Append(string.Format("C  INNER JOIN Fund F ON C.FundId = F.Id WHERE C.FundId = {0} AND C.ProductId ='{1}'", fundId, Globals.ApplicationProductId));
            sb.Append(string.Format("AND SUBSTRING(CONVERT(nvarchar(22),  C.ContributionDate, 111), 1, 4) = {0} ", selectedYear));
            sb.Append("GROUP BY SUBSTRING(CONVERT(nvarchar(22),  C.ContributionDate, 111), 6, 2) ");
            sb.Append("ORDER BY SUBSTRING(CONVERT(nvarchar(22),  C.ContributionDate, 111), 6, 2) ");

            var dt = new DataTable();
            var connection = new SqlConnection(_context.Database.GetConnectionString());
            connection.Open();
            var cmd = new SqlCommand(); cmd.CommandText = sb.ToString(); cmd.CommandType = CommandType.Text; cmd.Connection = connection;
            var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            connection.Close();

            var returnList = new List<Data.DTOs.ContributionChartData>();

            foreach (DataRow row in dt.Rows)
            {
                var item = new Data.DTOs.ContributionChartData();
                item.Month = row["Month"].ToString();
                item.TotalAmount = (Decimal)row["TotalAmount"];
                returnList.Add(item);
            }
            return returnList;
        }
        public async Task<List<Data.DTOs.ContributionAnnualReportData>> GetContributionAnnualReportDataAsync(string selectedYear)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("SELECT C.Id, C.Name,FC.ContributionDate, F.Comments, FC.CheckNumber, FC.Amount, DATENAME(YEAR, FC.ContributionDate) AS[ContributionYear] ");
            sb.Append("FROM Contributor C ");
            sb.Append("INNER JOIN Contribution FC ON C.Id = FC.ContributorId ");
            sb.Append("INNER JOIN Fund F ON C.Id = F.Id ");
            sb.Append(string.Format("WHERE DATENAME(YEAR, FC.ContributionDate) =  '{0}' AND C.ProductId = '{1}' ", selectedYear, Globals.ApplicationProductId));
            sb.Append("ORDER BY C.Name, FC.ContributionDate");

            var dt = new DataTable();
            var connection = new SqlConnection(_context.Database.GetConnectionString());
            connection.Open();
            var cmd = new SqlCommand(); cmd.CommandText = sb.ToString(); cmd.CommandType = CommandType.Text; cmd.Connection = connection;
            var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            connection.Close();

            var returnList = new List<Data.DTOs.ContributionAnnualReportData>();

            foreach (DataRow row in dt.Rows)
            {
                var item = new Data.DTOs.ContributionAnnualReportData();
                item.Id = Convert.ToInt32(row["Id"]);
                item.Name = row["Name"].ToString();
                item.ContributionDate = (DateTime)row["ContributionDate"];
                item.Comments = row["Comments"].ToString();
                item.CheckNumber = row["CheckNumber"].ToString();
                item.Amount = (Decimal)row["Amount"];
                item.ContributionYear = row["ContributionYear"].ToString();
                returnList.Add(item);
            }
            return returnList;
        }
        public async Task<List<DailyContributionData>> GetDailyContributionDataAsync(DateTime selectedDate)
        {
            var sb = new System.Text.StringBuilder();
            var dt = new DataTable();
            var dt2 = new DataTable();
            var returnList = new List<DailyContributionData>();
            var item = new DailyContributionData();
            item.TotalContributionsAmount = 0;

            using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    sb.Clear();
                    sb.Append("SELECT ISNULL(SUM(Amount),0) AS [TotalAmount] FROM Contribution ");
                    sb.Append(string.Format("WHERE ProductId = '{0}' AND CONVERT(varchar(10), CAST(ContributionDate AS DATE), 101) = '{1}'",
                           Globals.ApplicationProductId, selectedDate.ToString("MM/dd/yyyy")));
                    cmd.CommandText = sb.ToString(); cmd.CommandType = CommandType.Text; cmd.Connection = connection;
                    var reader2 = await cmd.ExecuteReaderAsync();
                    dt2.Load(reader2);
                    item.TotalContributionsAmount = (Decimal)dt2.Rows[0]["TotalAmount"];
                }
                connection.Close();
            }
            returnList.Add(item);
            return returnList;
        }
        public async Task<IEnumerable<string>> GetContributionYearsAsync()
        {
            string sql = string.Format("SELECT DISTINCT(CONVERT(varchar(12), DATENAME(YEAR, C.ContributionDate))) AS [ContributionYear] FROM Contribution C  WHERE C.ProductId = '{0}' ORDER BY  CONVERT(varchar(12), DATENAME(YEAR, C.ContributionDate)) DESC", Globals.ApplicationProductId);
            var dt = new DataTable();
            var connection = new SqlConnection(_context.Database.GetConnectionString());
            connection.Open();
            var cmd = new SqlCommand(); cmd.CommandText = sql; cmd.CommandType = CommandType.Text; cmd.Connection = connection;
            var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            connection.Close();

            var returnList = new List<string>();
            if (dt.Rows.Count == 0)
            {
                returnList.Add(DateTime.Today.Year.ToString());
                return returnList;
            }

            foreach (DataRow row in dt.Rows)
            {
                returnList.Add(row[0].ToString());
            }
            return returnList;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}