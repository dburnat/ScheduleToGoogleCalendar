using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleToGCalendar
{
    public class Lesson
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TeacherName { get; set; }
        public string LessonName { get; set; }
        public string Group { get; set; }
        public string ClassRoom { get; set; }
        public string Date { get; set; }

        public override string ToString()
        {
            return ($"Date: {Date} \nStart: {StartTime} \nEnd: {EndTime} \nTeacher: {TeacherName} \nLesson: {LessonName} \nGroup: {Group} \nClass Room: {ClassRoom} \n");
        }
    }
}
