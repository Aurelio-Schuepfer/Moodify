using Microsoft.AspNetCore.Mvc;

namespace MoodifyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMoods()
        {
            var moods = new string[] { "Happy", "Sad", "Excited", "Angry" };
            return Ok(moods);
        }
    }
}
