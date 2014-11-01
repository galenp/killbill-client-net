using System;
using System.Text;

namespace KillBill.Client.Net.Model
{
    public class Account : KillBillObject
    {
     
        public Guid AccountId { get; set; }
        public double AccountCBA { get; set; }

        public string Name { get; set; }
        public int FirstNameLength { get; set; }
        public string ExternalKey { get; set; }
        public string Email { get; set; }
        public int BillCycleDayLocal { get; set; }
        public string Currency { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string TimeZone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Locale { get; set; }
        public string Phone { get; set; }
        public bool IsMigrated { get; set; }
        public bool IsNotifiedForInvoices { get; set; }
        public double AccountBalance { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder("Account{");
            sb.Append("accountId='").Append(AccountId).Append('\'');
            sb.Append(", externalKey='").Append(ExternalKey).Append('\'');
            sb.Append(", accountCBA=").Append(AccountCBA);
            sb.Append(", accountBalance=").Append(AccountBalance);
            sb.Append(", name='").Append(Name).Append('\'');
            sb.Append(", firstNameLength=").Append(FirstNameLength);
            sb.Append(", email='").Append(Email).Append('\'');
            sb.Append(", billCycleDayLocal=").Append(BillCycleDayLocal);
            sb.Append(", currency='").Append(Currency).Append('\'');
            sb.Append(", paymentMethodId='").Append(PaymentMethodId).Append('\'');
            sb.Append(", timeZone='").Append(TimeZone).Append('\'');
            sb.Append(", address1='").Append(Address1).Append('\'');
            sb.Append(", address2='").Append(Address2).Append('\'');
            sb.Append(", postalCode='").Append(PostalCode).Append('\'');
            sb.Append(", company='").Append(Company).Append('\'');
            sb.Append(", city='").Append(City).Append('\'');
            sb.Append(", state='").Append(State).Append('\'');
            sb.Append(", country='").Append(Country).Append('\'');
            sb.Append(", locale='").Append(Locale).Append('\'');
            sb.Append(", phone='").Append(Phone).Append('\'');
            sb.Append(", isMigrated=").Append(IsMigrated);
            sb.Append(", isNotifiedForInvoices=").Append(IsNotifiedForInvoices);
            sb.Append('}');
            return sb.ToString();
        }
    }
}
