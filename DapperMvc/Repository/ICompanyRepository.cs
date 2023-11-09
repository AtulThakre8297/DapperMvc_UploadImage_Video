namespace DapperMvc.Repository
{
    public interface ICompanyRepository
    {
        Task CreateCompany(Company company);
        Task DeleteCompany(int id);
        Task<IEnumerable<Company>> GetCompanies();
        Task<Company> GetCompany(int? id);
        Task UpdateCompany(int id, Company company);
    }
}
