using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AngleSharp;

namespace ScheduleToGCalendar
{
   public class Scrapper
    {
        public async Task<string> ReadHtml(string path = "")
        {
            var source =
                System.IO.File.ReadAllText(
                    @"..\..\html\plan.html");
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);


            var document = await context.OpenAsync(req => req.Content(source));
            var row1 = document.All.Where(m => m.ClassList.Contains("dxgvDataRow_Aqua"));



            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();
            foreach (var element in row1)
            {
                sb.Append(element.TextContent + "\n");
            }


            return sb.ToString();


        }
    }
}
