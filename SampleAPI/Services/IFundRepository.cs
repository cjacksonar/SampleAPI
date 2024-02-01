namespace Services
{
    public interface IFundRepository : IDisposable
    {
        public bool IsValidProductId(string productId);
        Task<List<Data.Entities.Fund>> GetFundsAsync();
        Task<Data.Entities.Fund> GetFundAsync(int FundId);
        Task<bool> FundExistsAsync(int FundId);
        void AddFund(Data.Entities.Fund Fund);    // Fund from Entities
        Task<Data.Entities.Fund> DeleteFund(int FundId);
        void UpdateFund(Data.Entities.Fund Fund);
        Task<bool> SaveChangesAsync();
    }
}
