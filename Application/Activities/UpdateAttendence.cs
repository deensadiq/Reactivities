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

namespace Application.Activities
{
    public class UpdateAttendence
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public CommandHandler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.Include(a => a.UserActivities)
                                                        .ThenInclude(u => u.AppUser)
                                                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "Activity Not Found" });

                var user = await _context.Users
                                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not Found" });

                var hostUsername = activity.UserActivities.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                var attendence = activity.UserActivities.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                if (attendence != null && hostUsername == user.UserName)
                {
                    activity.IsCancelled = !activity.IsCancelled;
                }

                if (attendence != null && hostUsername != user.UserName)
                {
                    activity.UserActivities.Remove(attendence);
                }

                if (attendence == null)
                {
                    attendence = new UserActivity()
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false,
                        DateJoined = DateTime.UtcNow
                    };

                    activity.UserActivities.Add(attendence);
                }

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                throw new Exception("Problem Updating attendence");
            }
        }
    }
}