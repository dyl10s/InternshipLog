using InternshipJournals.Data.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InternshipJournals.Pages
{
    public partial class Export
    {
        [Inject]
        Account curAccount { get; set; }
        [Inject]
        Database Db { get; set; }

        [Inject]
        IJSRuntime jsRuntime { get; set; }

        DateTime searchTimeStart = DateTime.Now;
        DateTime searchTimeEnd = DateTime.Now;

        int selectedFilter = 0;
        DateTime _searchDate = DateTime.Now;
        DateTime searchDate { get { return _searchDate; } set { _searchDate = value; GetEntries(); } }

        Dictionary<DateTime, List<Data.Database.Entry>> entriesByDay { get
            {
                Dictionary<DateTime, List<Data.Database.Entry>> results = new Dictionary<DateTime, List<Data.Database.Entry>>();
                Entries.ForEach(x =>
                {
                    if (!results.ContainsKey(x.StartTime.Date))
                    {
                        results.Add(x.StartTime.Date, new List<Data.Database.Entry>());
                    }

                    results[x.StartTime.Date].Add(x);
                });
                return results;
            } 
        }

        List<Data.Database.Entry> Entries = new List<Data.Database.Entry>();

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                GetEntries();
            }
            base.OnAfterRender(firstRender);
        }

        void filterChanged(ChangeEventArgs e)
        {
            selectedFilter = Int32.Parse(e.Value.ToString());
            GetEntries();
        }

        void ExportPDF()
        {
            //Save the users Company and Name from last time
            Db.Update(curAccount);

            jsRuntime.InvokeVoidAsync("generatePDF", "exportData");
        }

        double GetEntrieHours()
        {
            TimeSpan count = new TimeSpan(0, 0, 0);
            Entries.ForEach(x =>
            {
                var results = x.EndTime - x.StartTime;
                count += results;
            });
            return count.TotalHours;
        }

        double GetEntrieHours(List<Data.Database.Entry> entryList)
        {
            TimeSpan count = new TimeSpan(0, 0, 0);
            entryList.ForEach(x =>
            {
                var results = x.EndTime - x.StartTime;
                count += results;
            });
            return count.TotalHours;
        }

        void GetEntries()
        {
            var curDate = searchDate;
            if (selectedFilter == 0)
            {
                var dayStart = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0);
                var dayEnd = new DateTime(curDate.Year, curDate.Month, curDate.Day, 23, 59, 59);

                searchTimeStart = dayStart;
                searchTimeEnd = dayEnd;

                Entries = Db.Fetch<Data.Database.Entry>
                    (
                    "SELECT * FROM Entries WHERE AccountId=@0 AND StartTime >= @1 AND StartTime <= @2 Order By StartTime DESC",
                    curAccount.AccountId, dayStart, dayEnd
                    );
            }
            else if (selectedFilter == 1)
            {
                var dayStart = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0);
                dayStart = dayStart.AddDays(-(curDate.DayOfWeek - DayOfWeek.Monday));
                var dayEnd = new DateTime(dayStart.Year, dayStart.Month, dayStart.Day, 23, 59, 59);
                dayEnd = dayEnd.AddDays(6);

                searchTimeStart = dayStart;
                searchTimeEnd = dayEnd;

                Entries = Db.Fetch<Data.Database.Entry>
                    (
                    "SELECT * FROM Entries WHERE AccountId=@0 AND StartTime >= @1 AND StartTime <= @2 Order By StartTime DESC",
                    curAccount.AccountId, dayStart, dayEnd
                    );
            }
            else if (selectedFilter == 2)
            {
                var dayStart = new DateTime(curDate.Year, curDate.Month, 1, 0, 0, 0);
                var dayEnd = dayStart.AddMonths(1).AddDays(-1);

                searchTimeStart = dayStart;
                searchTimeEnd = dayEnd;

                Entries = Db.Fetch<Data.Database.Entry>
                    (
                    "SELECT * FROM Entries WHERE AccountId=@0 AND StartTime >= @1 AND StartTime <= @2 Order By StartTime DESC",
                    curAccount.AccountId, dayStart, dayEnd
                    );
            }
            else if (selectedFilter == 3)
            {
                searchTimeStart = DateTime.MinValue;
                searchTimeEnd = DateTime.MaxValue;

                Entries = Db.Fetch<Data.Database.Entry>
                    (
                    "SELECT * FROM Entries WHERE AccountId=@0 Order By StartTime DESC",
                    curAccount.AccountId
                    );
            }
            else if (selectedFilter == 4)
            {
                var dayStart = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0);
                dayStart = dayStart.AddDays(-(curDate.DayOfWeek - DayOfWeek.Monday) - 7);

                var dayEnd = new DateTime(dayStart.Year, dayStart.Month, dayStart.Day, 23, 59, 59);
                dayEnd = dayEnd.AddDays(13);

                searchTimeStart = dayStart;
                searchTimeEnd = dayEnd;

                Entries = Db.Fetch<Data.Database.Entry>
                    (
                    "SELECT * FROM Entries WHERE AccountId=@0 AND StartTime >= @1 AND StartTime <= @2 Order By StartTime DESC",
                    curAccount.AccountId, dayStart, dayEnd
                    );
            }
            else
            {
                Entries = new List<Data.Database.Entry>();
            }
            this.StateHasChanged();
        }

        void NameChange(ChangeEventArgs e)
        {
            curAccount.Name = e.Value.ToString();
            //Save the users Company and Name from last time
            Db.Update(curAccount);
        }

        void CompanyChange(ChangeEventArgs e)
        {
            curAccount.Company = e.Value.ToString();
            //Save the users Company and Name from last time
            Db.Update(curAccount);
        }
    }
}
