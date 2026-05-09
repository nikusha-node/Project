namespace Project.Services.Interfaces;

internal interface IRepository<T>
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Delete(int id);
}
