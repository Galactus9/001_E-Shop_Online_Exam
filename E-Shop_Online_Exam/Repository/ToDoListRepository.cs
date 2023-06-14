using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class ToDoListRepository : Repository<ToDoList>, IToDoListRepository
    {
        private readonly MyDbContext _context;
        public ToDoListRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<ToDoList>> GetActiveToDoList()
        {
            return await _context.Set<ToDoList>().Where(x => x.Status == ToDoListStatus.Open).ToListAsync();
        }
         
        public List<ToDoList> GetToDoListByMarkerId(string markerId) 
        {
            return _context.Set<ToDoList>().Where(x => x.Marker.Id == markerId)
                                           .Where(x => x.Status == ToDoListStatus.Open)
                                           .Include(x => x.Marker)
                                           .Include(x => x.Exam)
                                           .ToList();
        }
        public override async Task<IEnumerable<ToDoList>> GetAllAs()
        {
            return await _context.Set<ToDoList>().Include(x => x.Marker).Include(x => x.Exam).ToListAsync();
        }
        public override async Task<ToDoList> GetAs(int id)
        {
            return await _context.Set<ToDoList>().Include(x => x.Marker).Include(x => x.Exam).SingleAsync(x => x.Id == id);
        }
        public void Update(ToDoList toDoList)
        {
            _context.Update(toDoList);
        }
        
    }
}
