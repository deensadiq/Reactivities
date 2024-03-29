using System.Linq;
using Application.Activities.Dtos;
using Application.Comments;
using Application.Profiles.Dtos;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            string currentUsername = null;
            CreateMap<Activity, ActivityDto>()
            .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.UserActivities!.FirstOrDefault(x => x.IsHost)!.AppUser!.UserName));
            CreateMap<Activity, UserActivityDto>()
            .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.UserActivities!.FirstOrDefault(x => x.IsHost)!.AppUser!.UserName));
            CreateMap<UserActivity, AttendeeDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser!.UserName))
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser!.DisplayName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser!.Bio))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser!.Photos!.FirstOrDefault(x => x.IsMain == true)!.Url))
            .ForMember(d => d.FollowingsCount, o => o.MapFrom(s => s.AppUser!.Followings!.Count))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser!.Followers!.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser!.Followers!.Any(x => x.Observer.UserName == currentUsername)));
            CreateMap<Comment, CommentDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Author!.UserName))
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author!.DisplayName))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<AppUser, Profiles.Profile>()
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername)));
        }
    }
}