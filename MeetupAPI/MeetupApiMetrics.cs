using System;
using System.Diagnostics.Metrics;

public class MeetupApiMetrics : IMeetupApiMetrics
{
    public const string MeterName = "MeetupAPI";

    private readonly Counter<long> _meetupRequestCounter;
    private readonly Histogram<double> _meetupRequestDuration;

    public MeetupApiMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _meetupRequestCounter = meter.CreateCounter<long>(
            "meetupAPI.meetup_requests.create_meetup.count");

        _meetupRequestDuration = meter.CreateHistogram<double>(
            "meetupAPI.meetup_requests.duration");
    }

    public void IncreaseMeetupRequestCount()
    {
        _meetupRequestCounter.Add(1);
    }

    public TrackedRequestDuration MeasureRequestDuration()
    {
        return new TrackedRequestDuration(_meetupRequestDuration);
    }

    public class TrackedRequestDuration : IDisposable
    {
        private readonly Histogram<double> _histogram;
        private readonly long _requestStartTime = TimeProvider.System.GetTimestamp();

        public TrackedRequestDuration(Histogram<double> histogram)
        {
            _histogram = histogram;
        }

        public void Dispose()
        {
            var elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime);
            _histogram.Record(elapsed.TotalMicroseconds);
        }
    }
}