using Dapper;
using DapperMvc.Context;
using System.Data;

namespace DapperMvc.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
         readonly DapperContext context;

        public CompanyRepository(DapperContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = "SELECT * FROM Companies";

            using (var connection = context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);
                return companies.ToList();
            }
        }






        public async Task<Company> GetCompany(int? id)
        {
            var query = "SELECT * FROM Companies WHERE Id = @Id";

            using (var connection = context.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id });
                return company;
            }
        }






        public async Task CreateCompany(Company company)
        {
            var query = "INSERT INTO Companies (CompanyName, CompanyAddress, Country,GlassdoorRating) VALUES (@CompanyName, @CompanyAddress, @Country, @GlassdoorRating)";

            var parameters = new DynamicParameters();
            parameters.Add("CompanyName", company.CompanyName, DbType.String);
            parameters.Add("CompanyAddress", company.CompanyAddress, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);
            parameters.Add("GlassdoorRating", company.GlassdoorRating, DbType.Int32);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }



        public async Task UpdateCompany(int id, Company company)
        {
            var query = "UPDATE Companies SET CompanyName = @CompanyName, CompanyAddress = @CompanyAddress, Country = @Country, GlassdoorRating = @GlassdoorRating WHERE Id = @Id";

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    CompanyName = company.CompanyName,
                    CompanyAddress = company.CompanyAddress,
                    Country = company.Country,
                    GlassdoorRating = company.GlassdoorRating,
                    Id = id
                });
            }
        }




        public async Task DeleteCompany(int id)
        {

            var query = "DELETE FROM Companies WHERE Id = @Id";
            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }




    }
}
