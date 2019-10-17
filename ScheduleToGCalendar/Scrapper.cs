using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Text;

namespace ScheduleToGCalendar
{
   public class Scrapper
   {
       public IEnumerable<IElement> TableElements;

       public async Task<string> ReadHtml(string path = "")
        {
            var source = File.ReadAllText(@"..\..\html\plan.html");
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(req => req.Content(source));
            TableElements = document.QuerySelectorAll(
                "tr.dxgvDataRow_Aqua > td.dxgv, tr.dxgvGroupRow_Aqua > td.dxgv");
            
            StringBuilder sb = new StringBuilder();
            foreach (var tableElement in TableElements)
            {
                sb.AppendLine(Regex.Replace(tableElement.TextContent, @"<[^>]+>|&nbsp;", "").Trim());
            }
            
            return sb.ToString();
        }

       public async  Task<string> ConvertHtmlToClass(IEnumerable<IElement> rows)
       {
           List<Lesson> lessons = new List<Lesson>();

           StringBuilder sb = new StringBuilder();
           foreach (var element in rows)
           {
               sb.AppendLine(Regex.Replace(element.TextContent.Replace(" ", " "), @"<[^>]+>|&nbsp;", "").Trim());
           }
           string[] lines = sb.ToString().Trim(' ').Split(Environment.NewLine.ToCharArray() , StringSplitOptions.RemoveEmptyEntries);


           sb.Clear();
           
           int dateIncrease = 0;
           string currentDate = lines[0];
           for (var i = 0; i < lines.Length - 10; i += 10)
           {
               try
               {
                   if (lines[i + dateIncrease] == "Brak" && lines[ i + 1 + dateIncrease].Contains("2019"))
                   {
                       if (i + dateIncrease > lines.Length)
                           break;
                       currentDate = lines[i + 1 +dateIncrease];
                       dateIncrease++;
                       Lesson lesson = new Lesson
                       {
                           Date = currentDate,
                           StartTime = lines[i +1 + dateIncrease % 10],
                           EndTime = lines[i + 2 + dateIncrease % 10],
                           TeacherName = lines[i + 4 + dateIncrease % 10],
                           LessonName = lines[i + 5  +dateIncrease %10],
                           Group = lines[i + 7 + dateIncrease % 10],
                           ClassRoom = lines[i + 8 + dateIncrease % 10]
                       };
                       lessons.Add(lesson);
                       sb.AppendLine(lesson.ToString());
                   
                   }
                   else
                   {
                       if (i + dateIncrease > lines.Length)
                           break;
                       Lesson lesson = new Lesson
                       {
                           Date = currentDate,
                           StartTime = lines[i +1 + dateIncrease % 10],
                           EndTime = lines[i + 2 + dateIncrease % 10],
                           TeacherName = lines[i + 4 + dateIncrease % 10],
                           LessonName = lines[i + 5  +dateIncrease %10],
                           Group = lines[i + 7 + dateIncrease % 10],
                           ClassRoom = lines[i + 8 + dateIncrease % 10]
                       };
                       lessons.Add(lesson);
                       sb.AppendLine(lesson.ToString());
                   
                   }
               }
               catch (Exception e)
               {
                   MessageBox.Show(e.Message);
                   throw;
               }
           }
           return sb.ToString();
       }
   }
}
