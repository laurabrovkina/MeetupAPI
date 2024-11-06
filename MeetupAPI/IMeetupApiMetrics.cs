using static MeetupAPI.MeetupApiMetrics;

namespace MeetupAPI;

public interface IMeetupApiMetrics
{
    void IncreaseMeetupRequestCount();
    TrackedRequestDuration MeasureRequestDuration();
}