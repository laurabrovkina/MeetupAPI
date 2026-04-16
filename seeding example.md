```
            var locationFaker = new Faker<Location>()
                .UseSeed(555)
                .RuleFor(x => x.Id, x => x.IndexVariable)
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Street, f => f.Address.StreetName())
                .RuleFor(x => x.PostCode, f => f.Address.ZipCode());
            
            var locationToSeed = locationFaker.Generate(1);
            var contains = await context.Set<Location>().ContainsAsync(locationToSeed[0], cancellationToken: ct);
            if (!contains)
            {
                context.Set<Location>().AddRange(locationToSeed);
                await context.SaveChangesAsync(ct);
            }
            
            var location = await context.Set<Location>().FirstOrDefaultAsync(x => x.Id == locationToSeed[0].Id, ct);
        
            var lectureFaker = new Faker<Lecture>()
                .UseSeed(125)
                .RuleFor(x => x.Id, f => f.IndexVariable)
                .RuleFor(x => x.Author, f => f.Person.FullName)
                .RuleFor(x => x.Topic, f => f.Lorem.Word())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence());
            var lectureToSeed = lectureFaker.Generate(2);
            var containsLectures = await context.Set<Lecture>().ContainsAsync(lectureToSeed[0], cancellationToken: ct);
            if (!containsLectures)
            {
                context.Set<Lecture>().AddRange(lectureToSeed);
                await context.SaveChangesAsync(ct);
            }
            
            var lectures = context.Set<Lecture>().ToList();
        
            var meetupFaker = new Faker<Meetup>()
                .UseSeed(420)
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => f.Lorem.Word())
                .RuleFor(x => x.Organizer, f => f.Company.CompanyName())
                .RuleFor(x => x.IsPrivate, f => false)
                .RuleFor(x => x.Date, f => f.Date.Future())
                .RuleFor(x => x.Location, f => location)
                .RuleFor(x => x.Lectures, f => lectures);
            var meetupToSeed = meetupFaker.Generate(2);
            var containsMeetup = await context.Set<Meetup>().ContainsAsync(meetupToSeed[0], ct);
            if (!containsMeetup)
            {
                context.Set<Meetup>().AddRange(meetupToSeed);
                await context.SaveChangesAsync(ct);
            }



            /////
            /// 
            /// 

            
            var locationFaker = new Faker<Location>()
                .UseSeed(555)
                .RuleFor(x => x.Id, x => x.IndexVariable)
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Street, f => f.Address.StreetName())
                .RuleFor(x => x.PostCode, f => f.Address.ZipCode());
            
            var locationToSeed = locationFaker.Generate(1);
            var contains = context.Set<Location>().Contains(locationToSeed[0]);
            if (!contains)
            {
                context.Set<Location>().AddRange(locationToSeed);
                context.SaveChanges();
            }
            
            var location = context.Set<Location>().FirstOrDefault(x => x.Id == locationToSeed[0].Id);
            var lectureFaker = new Faker<Lecture>()
                .UseSeed(125)
                .RuleFor(x => x.Id, f => f.IndexVariable)
                .RuleFor(x => x.Author, f => f.Person.FullName)
                .RuleFor(x => x.Topic, f => f.Lorem.Word())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence());
            var lectureToSeed = lectureFaker.Generate(2);
            var containsLectures = context.Set<Lecture>().Contains(lectureToSeed[0]);
            if (!containsLectures)
            {
                context.Set<Lecture>().AddRange(lectureToSeed);
                context.SaveChangesAsync();
            }
            
            var lectures = context.Set<Lecture>().ToList();
            var meetupFaker = new Faker<Meetup>()
                .UseSeed(420)
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => f.Lorem.Word())
                .RuleFor(x => x.Organizer, f => f.Company.CompanyName())
                .RuleFor(x => x.IsPrivate, f => false)
                .RuleFor(x => x.Date, f => f.Date.Future())
                .RuleFor(x => x.Location, f => location)
                .RuleFor(x => x.Lectures, f => lectures);
            var meetupToSeed = meetupFaker.Generate(2);
            var containsMeetup = context.Set<Meetup>().Contains(meetupToSeed[0]);
            if (!containsMeetup)
            {
                context.Set<Meetup>().AddRange(meetupToSeed);
                context.SaveChangesAsync();
            }
```
```
warn: Microsoft.EntityFrameworkCore.Database.Transaction[30004]
db-migration  | Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
db-migration  | Savepoints are disabled because Multiple Active Result Sets (MARS) is enabled. If 'SaveChanges' fails, then the transaction cannot be automatically rolled back to a known clean state. Instead, the transaction should be rolled back by the application before retrying 'SaveChanges'. See https://go.microsoft.com/fwlink/?linkid=2149338 for more information and examples. To identify the code which triggers this warning, call 'ConfigureWarnings(w => w.Throw(SqlServerEventId.SavepointsDisabledBecauseOfMARS))'.
```