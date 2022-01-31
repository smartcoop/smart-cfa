using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.Services;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Services;

public class AccountUserStrategy : IUserStrategy
{
    private readonly string _connectionString;

    public AccountUserStrategy(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<UserDto> GetAsync(string userId)
    {
        return new UserDto(userId, "Victor", "vD", ApplicationType.Account.Name);
        if (!int.TryParse(userId, out int id)) throw new Exception();
        var sql = @$"SELECT CAST([Id] AS NVARCHAR(10)) 'UserId'
                      ,[FirstName]
                      ,[LastName]
                      ,@ApplicationType 'ApplicationType'
                  FROM [dbo].[User] (NOLOCK) U
                    WHERE U.Id = @Id";
        await using var db = new SqlConnection(_connectionString);
        return await db.QuerySingleAsync<UserDto>(sql, new {Id = id, ApplicationType = ApplicationType.Account.Name});
    }
}
