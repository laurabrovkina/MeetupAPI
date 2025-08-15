using Entities;
using Facet;
using Facet.Mapping;

namespace Mappings;

public class MeetupMapConfig : IFacetMapConfiguration<Meetup, MeetupResponse>
{
    public static void Map(Meetup source, MeetupResponse target)
    {
        target.FullLocation = source.Location.Street + ", " + source.Location.City + ", " + source.Location.PostCode;
    }
}

[Facet(typeof(Meetup), exclude: [nameof(Meetup.Id), nameof(Meetup.CreatedById), nameof(Meetup.CreatedBy), nameof(Meetup.Location)])]
public partial class MeetupResponse
{
    public string FullLocation { get; set; }
}