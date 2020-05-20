using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipJournals.Data.Database
{
    [TableName("Entries")]
    [PrimaryKey("EntryId")]
    public class Entry
    {
        public int EntryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Details { get; set; }
        public int AccountId { get; set; }
    }
}
