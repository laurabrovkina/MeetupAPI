var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MeetupAPI>("meetupapi");

builder.Build().Run();
