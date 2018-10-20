using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
	/// <summary>
    /// Provides API for managing Todo entities.
    /// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class TodoController : ControllerBase
	{
		private readonly TodoContext _context;

		public TodoController(TodoContext context)
		{
			_context = context;

			if (_context.TodoItems.Count() == 0)
			{
				// Create a new TodoItem if collection is empty,
				// which means you can't delete all TodoItems.
				_context.TodoItems.Add(new TodoItem { Name = "Item1" });
				_context.SaveChanges();
			}
		}

		/// <summary>
		/// Retrieve a list of Todo entities.
		/// </summary>
		/// <returns>Returns a a list of Todos</returns>
		[HttpGet]
		public ActionResult<List<TodoItem>> GetAll()
		{
			return _context.TodoItems.ToList();
		}

		/// <summary>
		/// Retrieves a Todo entity.
		/// </summary>
		/// <param name="id">todo item id</param>
		/// <returns>Returns a Todo.  If the id doesn't exist, then a 404 Not Found is returned.</returns>
		[HttpGet("{id}", Name = "GetTodo")]
		public ActionResult<TodoItem> GetById(long id)
		{
			var item = _context.TodoItems.Find(id);
			if (item == null)
			{
				return NotFound();
			}
			return item;
		}

		/// <summary>
		/// Create a Todo entity.
		/// </summary>
		/// <param name="item">todo item</param>
        /// <returns>Returns the created Todo.</returns>
		[HttpPost]
		public IActionResult Create(TodoItem item)
		{
			_context.TodoItems.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
		}

		/// <summary>
		/// Updates a Todo entity.
		/// </summary>
		/// <param name="id">todo item id</param>
		/// <param name="item">todo item</param>
        /// <returns>Returns updated Todo.  If the id doesn't exist, then a 404 Not Found is returned.</returns>
		[HttpPut("{id}")]
		public IActionResult Update(long id, TodoItem item)
		{
			var todo = _context.TodoItems.Find(id);
			if (todo == null)
			{
				return NotFound();
			}

			todo.IsComplete = item.IsComplete;
			todo.Name = item.Name;

			_context.TodoItems.Update(todo);
			_context.SaveChanges();
			return NoContent();
		}

		/// <summary>
		/// Deletes a Todo entity.
		/// </summary>
		/// <param name="id">todo item id</param>
		/// <returns>Returns 204 No Content if Todo exist.  If the id doesn't exist, then a 404 Not Found is returned.</returns>
		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var todo = _context.TodoItems.Find(id);
			if (todo == null)
			{
				return NotFound();
			}

			_context.TodoItems.Remove(todo);
			_context.SaveChanges();
			return NoContent();
		}
	}
}