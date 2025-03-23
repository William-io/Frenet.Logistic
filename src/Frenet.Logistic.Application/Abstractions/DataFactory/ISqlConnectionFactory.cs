using System.Data;

namespace Frenet.Logistic.Application.Abstractions.DataFactory;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}