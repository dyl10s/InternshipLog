using InternshipJournals.Data.Database;
using Microsoft.AspNetCore.Components;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipJournals.Pages
{
    public partial class Entry
    {
        [Inject]
        Account curAccount { get; set; }
        [Inject]
        Database Db { get; set; }

        int? curEntryId { get; set; }
        DateTime newDate { get; set; } = DateTime.Now;
        DateTime newStart { get; set; } = DateTime.Now;
        DateTime newEnd { get; set; } = DateTime.Now;
        string newDetails { get; set; } = "";
        string ErrorMessage { get; set; } = "";

        DateTime searchTimeStart = DateTime.Now;
        DateTime searchTimeEnd = DateTime.Now;

        int selectedFilter = 0;
        DateTime _searchDate = DateTime.Now;
        DateTime searchDate {get { return _searchDate; } set { _searchDate = value; GetEntries(); } }

        List<Data.Database.Entry> Entries = new List<Data.Database.Entry>();
        bool ModalShow = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                clearModal();
                try
                {
                    GetEntries();
                }
                catch(Exception e)
                { 
                
                }
            }
            base.OnAfterRender(firstRender);
        }

        void filterChanged(ChangeEventArgs e)
        {
            selectedFilter = Int32.Parse(e.Value.ToString());
            GetEntries();
        }

        void GetEntries()
        {
            var curDate = searchDate;
            if(selectedFilter == 0)
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
            else if(selectedFilter == 1)
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

        void ShowModal()
        {
            ModalShow = true;
        }

        void HideModal()
        {
            ModalShow = false;
            clearModal();
        }

        void saveEntry()
        {
            try
            {
                var newEndDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, newEnd.Hour, newEnd.Minute, newEnd.Second);
                var newStartDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, newStart.Hour, newStart.Minute, newStart.Second);

                var newEntry = new Data.Database.Entry()
                {
                    AccountId = curAccount.AccountId,
                    Details = newDetails,
                    StartTime = newStartDate,
                    EndTime = newEndDate
                };

                if (curEntryId.HasValue)
                {
                    newEntry.EntryId = curEntryId.Value;
                    Db.Update(newEntry);
                }
                else
                {
                    Db.Insert(newEntry);
                }

                GetEntries();
                HideModal();
            }
            catch(Exception e)
            {
                ErrorMessage = "There was an error saving this to the database";
            }
        }

        void clearModal()
        {
            curEntryId = null;
            newDate = DateTime.Now;
            newStart = DateTime.Now;
            newStart = newStart.AddMinutes(-newStart.Minute);
            newEnd = DateTime.Now;
            newEnd = newEnd.AddMinutes(60 - newEnd.Minute);
            ErrorMessage = "";
            newDetails = "";
            this.StateHasChanged();
        }

        void Delete(Data.Database.Entry e)
        {
            try
            {
                Db.Delete(e);
                GetEntries();
            }
            catch { }
        }

        void Update(Data.Database.Entry e)
        {
            curEntryId = e.EntryId;
            newDate = e.StartTime;
            newStart = e.StartTime;
            newEnd = e.EndTime;
            newDetails = e.Details;
            ShowModal();
        }
    }
}
