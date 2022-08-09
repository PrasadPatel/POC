namespace Consume_API.IRepository
{
    /// <summary>
    /// IRepository interface to implement the Repository Pattern
    /// </summary>
    /// <typeparam name="T">Entity e.g. Car,Employee</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get individual type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="Id">Id of type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>Type T / entity</returns>
        Task<T?> GetAsync(string url, int Id, string token);
        /// <summary>
        /// Get list of type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="token">Token</param>
        /// <returns>List of type T / entity</returns>
        Task<IEnumerable<T>?> GetAllAsync(string url, string token);
        /// <summary>
        /// Create type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="objToCreate">Type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>True if created; False otherwise</returns>
        Task<bool> CreateAsync(string url, T objToCreate, string token);
        /// <summary>
        /// Update type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="objToUpdate">Type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>True if updated; False otherwise</returns>
        Task<bool> UpdateAsync(string url, T objToUpdate, string token);
        /// <summary>
        /// Delete type T / entity
        /// </summary>
        /// <param name="url">Api endpoint</param>
        /// <param name="Id">Id of type T / entity</param>
        /// <param name="token">Token</param>
        /// <returns>True if deleted; False otherwise</returns>
        Task<bool> DeleteAsync(string url, int Id, string token);
    }
}