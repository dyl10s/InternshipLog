using InternshipJournals.Data.Database;
using Microsoft.AspNetCore.Components;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipJournals.Pages
{
    public partial class Register
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

        void RegisterUser()
        {
            ErrorMessage = "";
            this.StateHasChanged();

            if (Username.Length < 6)
            {
                ErrorMessage = "Username must be atleast 6 characters";
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Password must be atleast 6 characters";
            }

            try
            {
                var resultAccount = Db.SingleOrDefault<Account>("SELECT * FROM Accounts WHERE Username = @0", Username);
                if (resultAccount == null)
                {
                    var newAccount = new Account()
                    {
                        Username = Username,
                        Password = Account.HashPassword(Password)
                    };
                    Db.Insert(newAccount);

                    curUser.AccountId = newAccount.AccountId;
                    curUser.Username = newAccount.Username;

                    nav.NavigateTo("/Entry");
                }
                else
                {
                    ErrorMessage = "This username is already in use";
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "An unknown error has occured. Please try again.";
            }
        }
    }
}
