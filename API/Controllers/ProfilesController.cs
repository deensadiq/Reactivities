using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Profiles;
using Application.Profiles.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseController
    {
        // Get : /api/profiles
        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Get(string username)
        {
            return await Mediator.Send(new Details.Query() { Username = username });
        }

        // Post: /api/profiles
        [HttpPut]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("{username}/activities")]
        public async Task<ActionResult<List<UserActivityDto>>> GetActivities(string username, [FromQuery] ProfileParams param)
        {
            return await Mediator.Send(new Activities.Query() { Username = username, Params = param });
        }
    }
}