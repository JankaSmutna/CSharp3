namespace ToDoList.Persistence.Repositories;

public interface IRepository<T>
where T : class
{
    public void Create(T item);

    public IEnumerable<T> Read();

    public T? ReadById(int id);

    public bool Update(int id, T updatedItem);

    public bool Delete(int id);

}
