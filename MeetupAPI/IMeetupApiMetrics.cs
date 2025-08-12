public interface IMeetupApiMetrics
{
    void IncreaseMeetupRequestCount();
    MeetupApiMetrics.TrackedRequestDuration MeasureRequestDuration();
}