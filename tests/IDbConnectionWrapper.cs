using Npgsql;
using System.Data;
namespace tests;

public interface IDbConnectionWrapper : IDbConnection
{
    void Open();
    void Close();
    // Добавьте другие необходимые методы, если нужно
}

// Реализация интерфейса
public class NpgsqlConnectionWrapper : IDbConnectionWrapper
{
    private readonly NpgsqlConnection _connection;

    public NpgsqlConnectionWrapper(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public void Open() => _connection.Open();
    public void Close() => _connection.Close();

    // Реализация других методов IDbConnection
    public string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }

    public int ConnectionTimeout => _connection.ConnectionTimeout;
    
    public string Database => _connection.Database;

    public ConnectionState State => _connection.State;

    public IDbTransaction BeginTransaction() => _connection.BeginTransaction();

    public IDbTransaction BeginTransaction(IsolationLevel il) => _connection.BeginTransaction(il);

    public void ChangeDatabase(string databaseName) => _connection.ChangeDatabase(databaseName);

    public void Dispose() => _connection.Dispose();

    public IDbCommand CreateCommand() => _connection.CreateCommand();
}
