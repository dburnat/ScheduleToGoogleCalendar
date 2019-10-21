using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace ScheduleToGCalendar
{
    public class GoogleApi
    {
        private static readonly string[] Scopes = {CalendarService.Scope.Calendar};
        private const string ApplicationName = "WSEI Schedule import";
        private UserCredential _userCredential;
        private CalendarService _service;

        public void CreateGoogleToken(string credentialsPath)
        {
            try
            {
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read ))
                {
                    string credPath = "token.json";
                    _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public void CreateGoogleCalendarService()
        {
            try
            {
                _service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _userCredential,
                    ApplicationName = ApplicationName
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public void AddEventToCalendar(List<Lesson> lessons, int reminderMinutes = 15)
        {
            try
            {

                if (lessons != null && lessons.Count > 0)
                {
                    foreach(var lesson in lessons)
                    {
                        var ev = new Event();
                        var startLessonTime = DateTime.Parse(lesson.Date + " " + lesson.StartTime);
                        var endLessonTime = DateTime.Parse(lesson.Date + " " +  lesson.EndTime);
                        EventDateTime start = new EventDateTime{DateTime = startLessonTime};
                        EventDateTime end = new EventDateTime{DateTime = endLessonTime};
                        var reminder1 = new EventReminder {Method = "popup", Minutes = reminderMinutes};
                    
                        var eventReminderData = new Event.RemindersData {UseDefault = false, Overrides = new[] {reminder1}};

                        ev.Start = start;
                        ev.End = end;
                        ev.Summary = lesson.LessonName + " " + lesson.ClassRoom;
                        ev.Description = lesson.TeacherName + " " + lesson.Group;
                        ev.Reminders = eventReminderData;
                    


                        var calendarId = "primary";
                        Event recurringEvent = _service.Events.Insert(ev, calendarId).Execute();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}