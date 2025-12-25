namespace MyFirstBackend.Controllers;
using Microsoft.AspNetCore.Mvc;
using MyFirstBackend.Data;
using TodoBackend.Models;

[ApiController]
[Route("api/todos")]
public class TodoController : ControllerBase
{
    private readonly TodoDbContext _context;

    public TodoController(TodoDbContext context)
    {
        _context = context;
    }

    // GET: api/todos
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.TodoItems.ToList());
    }

    // POST: api/todos
    [HttpPost]
    public IActionResult Create(TodoItem todo)
    {
        _context.TodoItems.Add(todo);
        _context.SaveChanges();
        return Ok(todo);
    }

    // PUT: api/todos/{id}
    [HttpPut("{id}")]
    public IActionResult Complete(int id)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null) return NotFound();

        todo.IsCompleted = true;
        _context.SaveChanges();
        return Ok(todo);
    }

    // DELETE: api/todos/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null) return NotFound();

        _context.TodoItems.Remove(todo);
        _context.SaveChanges();
        return NoContent();
    }
}

