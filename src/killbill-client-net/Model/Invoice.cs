using System;
using System.Collections.Generic;
using KillBill.Client.Net.JSON;
using Newtonsoft.Json;

namespace KillBill.Client.Net.Model
{
    public class Invoice :KillBillObject
    {
         public double Amount {get; set;}
         public string Currency {get; set;}
         public double CreditAdj {get; set;}
         public double RefundAdj {get; set;}
         public Guid InvoiceId {get; set;}
         [JsonConverter(typeof(ShortDateTimeConverter))]
         public DateTime InvoiceDate {get; set;}
         [JsonConverter(typeof(ShortDateTimeConverter))]
         public DateTime TargetDate {get; set;}
         public int InvoiceNumber {get; set;}
         public double Balance {get; set;}
         public Guid AccountId {get; set;}
         public string ExternalBundleKeys {get; set;}
         public List<InvoiceItem> Items;
         public List<Credit> Credits {get; set;}
        }
}