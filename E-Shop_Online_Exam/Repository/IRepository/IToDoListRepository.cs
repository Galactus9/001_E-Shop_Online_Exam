using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface IToDoListRepository : IRepository<ToDoList>
    {
        void Update(ToDoList toDoList);

        Task<ICollection<ToDoList>> GetActiveToDoList();
        List<ToDoList> GetToDoListByMarkerId(string markerId);

        //Task<IEnumerable<ToDoList>> GetAllAs();


        
    }
}
