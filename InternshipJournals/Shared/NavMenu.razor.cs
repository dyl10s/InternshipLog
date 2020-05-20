using InternshipJournals.Data.Database;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipJournals.Shared
{
    public partial class NavMenu
    {
        [Inject]
        Account account { get; set; }

        [Inject]
        NavigationManager nav { get; set; }
        bool dropDownClicked = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if(account == null || account.AccountId == 0)
                {
                    nav.NavigateTo("/Login");
                }
            }
            base.OnAfterRender(firstRender);
        }

        void Logout()
        {
            account.AccountId = 0;
            account.Username = null;
            nav.NavigateTo("/Login");
        }

        void switchDropdown()
        {
            dropDownClicked = !dropDownClicked;
        }

        void hideDropdown()
        {
            dropDownClicked = false;
        }
    }
}
