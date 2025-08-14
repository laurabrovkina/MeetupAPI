using AutoMapper;
using Entities;
using MeetupAPI.Models;

public class MeetupProfile : Profile
{
    public MeetupProfile()
    {
        CreateMap<MeetupRequest, Meetup>();
    }
}