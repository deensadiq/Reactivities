using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Following
{
    public class FollowToggle
    {
        public class Command : IRequest<Unit>
        {
            public string TragetUsername { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var target = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.TragetUsername);

                if (target == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Target = "Not Found" });

                var following = await _context.UserFollowings.FindAsync(target.Id, observer.Id);

                if (following == null)
                {
                    following = new UserFollowing()
                    {
                        Target = target,
                        Observer = observer
                    };

                    _context.UserFollowings.Add(following);
                }
                else
                {
                    _context.UserFollowings.Remove(following);
                }

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                throw new Exception("Problem Updating User Followings");
            }
        }
    }
}