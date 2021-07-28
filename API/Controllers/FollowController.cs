using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Following;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseController
    {
        [HttpPost("{username}")]
        public async Task<ActionResult<Unit>> Follow(string username)
        {
            return await Mediator.Send(new FollowToggle.Command() { TragetUsername = username });
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<Application.Profiles.Profile>>> GetFollowings(string username, string predicate)
        {
            return await Mediator.Send(new List.Query() { Predicate = predicate, Username = username });
        }
    }
}