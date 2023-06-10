using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var query = "SELECT * FROM Users";
        var users = await _dbConnection.QueryAsync<User>(query);

        return users.ToList();
    }

    public async Task<User> GetUserById(int id)
    {
        var query = "SELECT * FROM Users WHERE Id = @Id";
        var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });

        return user;
    }

    public async Task<User> CreateUser(User user)
    {
        var query = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email); SELECT LAST_INSERT_ID()";
        var userId = await _dbConnection.ExecuteScalarAsync<int>(query, user);

        user.Id = userId;

        return user;
    }
}