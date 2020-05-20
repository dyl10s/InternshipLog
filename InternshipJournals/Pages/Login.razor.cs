using InternshipJournals.Data.Database;
using Microsoft.AspNetCore.Components;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipJournals.Pages
{
    public partial class Login
    {
        [Inject]
        Database Db { get; set; }

        [Inject]
        NavigationManager nav { get; set; }

        [Inject]
        Account curUser { get; set; }

        string Username;
        string Password;
        string ErrorMessage;

        void LoginUser()
        {
            ErrorMessage = "";
            this.StateHasChanged();

            try
            {
                var resultAccount = Db.SingleOrDefault<Account>("SELECT * FROM Accounts WHERE Username = @0", Username);
                if (resultAccount != null && resultAccount.Password.SequenceEqual(Account.HashPassword(Password)))
                {
                    curUser.AccountId = resultAccount.AccountId;
                    curUser.Username = resultAccount.Username;
                    curUser.Password = resultAccount.Password;
                    curUser.Company = resultAccount.Company;
                    curUser.Name = resultAccount.Name;

                    nav.NavigateTo("/Entry");
                }
                else
                {
                    ErrorMessage = "Invalid Username or Password";
                }
            }
            catch(Exception e)
            {
                ErrorMessage = "An unknown error has occured. Please try again.";
            }
        }
    }
}
