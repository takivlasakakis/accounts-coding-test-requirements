using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisplayAccountInfo.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Currency)]
        public decimal AmountDue { get; set; }
        [DataType(DataType.Date)]
        public DateTime PaymentDueDate { get; set; }
        public int AccountStatusId { get; set; }
        public string FormattedPaymentDueDate { get; set; }

        public static implicit operator UserAccount(List<UserAccount> v)
        {
            throw new NotImplementedException();
        }
    }
}
