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
       public IEnumerable<AngleSharp.Dom.IElement> RowElements;
       private IEnumerable<AngleSharp.Dom.IElement> GroupElements;

       public async Task<string> ReadHtml(string path = "")
        {
            var source = File.ReadAllText(@"..\..\html\plan.html");
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(req => req.Content(source));
            //RowElements = document.All.Where(m => m.ClassList.Contains("dxgvDataRow_Aqua"));
            //RowElements = document.QuerySelectorAll("tr.dxgvDataRow_Aqua");
            RowElements = document.QuerySelectorAll("tr.dxgvDataRow_Aqua > td.dxgv");
            GroupElements = document.QuerySelectorAll("tr.dxgvGroupRow_Aqua > td.dxgv");


            StringBuilder sb = new StringBuilder();
           
            foreach (var rowElement in RowElements)
            {
                sb.AppendLine(Regex.Replace(rowElement.TextContent, @"<[^>]+>|&nbsp;", "").Trim());
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
           for (var i = 0; i < lines.Length; i += 10)
           {
               Lesson lesson = new Lesson
               {
                   StartTime = lines[i % 10],
                   EndTime = lines[i + 1 % 10],
                   TeacherName = lines[i + 3 % 10],
                   LessonName = lines[i + 4 %10],
                   Group = lines[i + 6 % 10],
                   ClassRoom = lines[i + 7 % 10]
               };
               lessons.Add(lesson);
               sb.AppendLine(lesson.ToString());
           }

           return sb.ToString() + lessons.Count;
       }
   }
}
