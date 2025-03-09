using AutoMapper;
using Entities;
using Meetup.Contracts.Models;

public class MeetupProfile : Profile
{
    public MeetupProfile()
    {
        CreateMap<Entities.Meetup, MeetupDetailsDto>()
            .ForMember(m => m.City, map => map.MapFrom(meetup => meetup.Location.City))
            .ForMember(m => m.PostCode, map => map.MapFrom(meetup => meetup.Location.PostCode))
            .ForMember(m => m.Street, map => map.MapFrom(meetup => meetup.Location.Street));

        CreateMap<MeetupDto, Entities.Meetup>();

        CreateMap<LectureDto, Lecture>()
            .ReverseMap();
    }
}