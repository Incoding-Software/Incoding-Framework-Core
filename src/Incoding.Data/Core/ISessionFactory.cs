namespace Incoding.Data.Core
{
    #region << Using >>

    #endregion

    public interface ISessionFactory<out TSession> 
    {
        TSession Open(string connectionString);
    }
}