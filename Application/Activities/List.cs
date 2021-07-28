using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities.Dtos;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<PageList<ActivityDto>>
        {
            public ActivityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, PageList<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<PageList<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUsername = _userAccessor.GetCurrentUsername();

                var query = _context.Activities
                                    .Where(a => a.Date >= request.Params.StartDate)
                                    .OrderBy(c => c.Date)
                                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername })
                                    .AsQueryable();

                if (request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.UserActivities.Any(a => a.Username == currentUsername));
                }

                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == currentUsername);
                }

                return await PageList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);
            }
        }
    }
}