using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Following
{
    public class List
    {
        public class Query : IRequest<List<Profiles.Profile>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Profiles.Profile>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<Profiles.Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profiles.Profile>();
                var currentUsername = _userAccessor.GetCurrentUsername();

                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await _context.UserFollowings.Where(x => x.Target.UserName == request.Username)
                        .Select(x => x.Observer)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currentUsername })
                        .ToListAsync();
                        break;
                    case "following":
                        profiles = await _context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                        .Select(x => x.Target)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currentUsername })
                        .ToListAsync();
                        break;
                }

                return profiles;
            }
        }
    }
}