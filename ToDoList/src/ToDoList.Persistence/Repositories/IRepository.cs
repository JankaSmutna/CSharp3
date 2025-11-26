namespace ToDoList.Persistence.Repositories;

public interface IRepository<T>
where T : class
{
    public void Create(T item);

    public IEnumerable<T> Read();

    public T? ReadById(int id);

    public bool UpdateById(int id, T updatedItem);

    public bool DeleteById(int id);

}
