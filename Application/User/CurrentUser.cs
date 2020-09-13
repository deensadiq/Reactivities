using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class CurrentUser
    {

        public class Query : IRequest<User> { }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                // handler loggic goes here
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "No login User found."});

                return new User() {
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
        }
    }
}