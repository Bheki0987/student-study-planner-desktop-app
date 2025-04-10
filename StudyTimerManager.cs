using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentStudyPlanner
{
    public static class StudyTimerManager
    {
        private static List<StudySession> studySessions = new List<StudySession>();

        public static void AddStudySession(DateTime startTime, DateTime endTime)
        {
            studySessions.Add(new StudySession { StartTime = startTime, EndTime = endTime });
        }

        public static TimeSpan GetTotalStudyTimeForDay(DateTime date)
        {
            return studySessions
                .Where(s => s.StartTime.Date == date.Date)
                .Aggregate(TimeSpan.Zero, (total, session) => total + (session.EndTime - session.StartTime));
        }

        public static TimeSpan GetTotalStudyTimeForWeek(DateTime date)
        {
            var startOfWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            return studySessions
                .Where(s => s.StartTime >= startOfWeek && s.StartTime < endOfWeek)
                .Aggregate(TimeSpan.Zero, (total, session) => total + (session.EndTime - session.StartTime));
        }

        public static TimeSpan GetTotalStudyTimeForMonth(DateTime date)
        {
            var startOfMonth = new DateTime(date.Year, date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            return studySessions
                .Where(s => s.StartTime >= startOfMonth && s.StartTime < endOfMonth)
                .Aggregate(TimeSpan.Zero, (total, session) => total + (session.EndTime - session.StartTime));
        }
    }

    public class StudySession
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}