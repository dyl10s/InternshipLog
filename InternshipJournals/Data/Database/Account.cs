using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipJournals.Data.Database
{
    [TableName("Accounts")]
    [PrimaryKey("AccountId")]
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }

        public static byte[] HashPassword(string pass)
        {
            var hasher = new SHA512CryptoServiceProvider();
            var results = hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
            return results;
        }
    }
}
