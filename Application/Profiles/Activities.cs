using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Profiles.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Activities
    {
        public class Query : IRequest<List<UserActivityDto>>
        {
            public string Username { get; set; }
            public ProfileParams Params { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, List<UserActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public QueryHandler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<UserActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                                                .Where(a => a.UserActivities.Any(x => x.AppUser.UserName == request.Username))
                                                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

                if (request.Params.FutureEvents && !request.Params.PastEvents && !request.Params.IsHost)
                {
                    activities = activities.Where(a => a.Date >= request.Params.StartDate).ToList();
                }

                if (request.Params.PastEvents && !request.Params.FutureEvents && !request.Params.IsHost)
                {
                    activities = activities.Where(a => a.Date < request.Params.StartDate).ToList();
                }

                if (request.Params.IsHost && !request.Params.FutureEvents && !request.Params.PastEvents)
                {
                    activities = activities.Where(a => a.HostUsername == request.Username).ToList();
                    // activities = DynamicFilter<UserActivityDto>.FilterBy(activities, "HostUsername", request.Username, "equalTo").ToList();
                }

                return activities;
            }
        }
    }
}