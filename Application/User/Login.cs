using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class Login
    {

        public class Query : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly DataContext _context;
            public Handler(DataContext context, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                // handler loggic goes here
                var user = await _context.Users.Include(x => x.Photos)
                                               .FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.ToUpper());

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    // TODO: generate token
                    return new User()
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = user.Photos.FirstOrDefault(x => x.IsMain = true)?.Url
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}