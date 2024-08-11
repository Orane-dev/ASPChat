using System.Data;

namespace ASPChat.DAL.DalHelpers
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
    }
}
