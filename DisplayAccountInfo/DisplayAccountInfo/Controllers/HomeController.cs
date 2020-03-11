using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DisplayAccountInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DisplayAccountInfo.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<ActionResult> Index()
        {
            ViewBag.Message = "Your application description page.";
            try
            {
                List<UserAccount> requestForUserAccounts = await RequestToURL();

                FormatUserAccountsPhoneNumber(requestForUserAccounts);
                FormatCurrency(requestForUserAccounts);
                FormatDateTime(requestForUserAccounts);

                return View(requestForUserAccounts);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }
        }
        static async Task<List<UserAccount>> RequestToURL()
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://frontiercodingtests.azurewebsites.net/api/accounts/getall");
            response.EnsureSuccessStatusCode();
            string usrActString = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var userAccounts = JsonConvert.DeserializeObject<List<UserAccount>>(usrActString, settings);

            return userAccounts;
        }
        public List<UserAccount> FormatUserAccountsPhoneNumber(List<UserAccount> userAccountsList)
        {
            foreach (var user in userAccountsList)
            {
                user.PhoneNumber = Regex.Replace(user.PhoneNumber, @"(\d{1,3})(\d{0,3})(\d{0,4})", " ($1) $2-$3");
            }
            return userAccountsList;
        }

        public List<UserAccount> FormatCurrency(List<UserAccount> userAccountsList)
        {
            foreach (var user in userAccountsList)
            {
                var newAmount = user.AmountDue.ToString("C", new CultureInfo("en-US"));
                var priceAsDecimal = Decimal.Parse(newAmount, NumberStyles.Currency);
                user.AmountDue = priceAsDecimal;
            }
            return userAccountsList;
        }
        public List<UserAccount> FormatDateTime(List<UserAccount> userAccountsList)
        {
            foreach (var user in userAccountsList)
            {
                user.FormattedPaymentDueDate = user.PaymentDueDate.Date.ToString("MM/dd/yyyy");

            }
            return userAccountsList;
        }
    }
}
