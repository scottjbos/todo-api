using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
	/// <summary>
    /// Provides API for managing Todo entities.
    /// </summary>
	[Produces("application/json")]
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
		/// <response code="200">Returns a list of todo items</response>
		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<List<TodoItem>> GetAll()
		{
			return _context.TodoItems.ToList();
		}

		/// <summary>
		/// Retrieves a Todo entity.
		/// </summary>
		/// <param name="id">todo item id</param>
		/// <returns>Returns a TodoItem</returns>
		/// <response code="200">Returns the created item</response>
		/// <response code="404">If the item is not found</response>
		[HttpGet("{id}", Name = "GetTodo")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
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
        /// <returns>A newly created TodoItem</returns>
		/// <response code="201">Returns the newly created item</response>
		/// <response code="400">If the item is null</response> 
		[HttpPost]
		[Consumes("application/json")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
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
        /// <returns>An updated TodoItem.</returns>
		/// <response code="200">Returns the updated item</response>
		/// <response code="404">If the id doesn't exist</response>
		[HttpPut("{id}")]
		[Consumes("application/json")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<TodoItem> Update(long id, TodoItem item)
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
			return todo;
		}

		/// <summary>
		/// Deletes a Todo entity.
		/// </summary>
		/// <param name="id">todo item id</param>
		/// <returns>No Content</returns>
		/// <response code="204">No content if the Todo is succesfully deleted</response>
		/// <response code="404">If the item is not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
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