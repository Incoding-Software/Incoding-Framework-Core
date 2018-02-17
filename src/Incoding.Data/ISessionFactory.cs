namespace Incoding.Data
{
    public interface ISessionFactory<out TSession> 
    {
        TSession Open(string connectionString);
    }
}