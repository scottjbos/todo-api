using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
	/// <summary>
    /// Provides API for managing Secret.
    /// </summary>
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
	public class SecretController : ControllerBase
	{
		/// <summary>
		/// Retrieve a secret code.
		/// </summary>
		/// <returns>Returns a secret string.</returns>
		/// <response code="200">Returns a secret string</response>
		[HttpGet]
		[ProducesResponseType(200)]
		public ActionResult<string> GetSecret()
		{
			return "secret code";
		}
    }
}