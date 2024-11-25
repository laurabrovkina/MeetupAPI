using Meetup.Aspire.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<MeetupAPI>("meetupapi");

builder.Build().Run();