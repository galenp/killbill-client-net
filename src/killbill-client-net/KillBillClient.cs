using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using KillBill.Client.Net.Infrastructure;
using KillBill.Client.Net.Model;

namespace KillBill.Client.Net
{
    public class KillBillClient
    {
        private readonly IKbHttpClient client;
        private static MultiMap<string>  DEFAULT_EMPTY_QUERY = new MultiMap<string>();
        public KillBillClient(IKbHttpClient client)
        {
            this.client = client;
        }

        //ACCOUNT
        //-------------------------------------------------------------------------------------------------------------------------------------

        public Account GetAccount(Guid accountId, bool withBalance = false, bool withCba = false)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId;

            var queryParams = new MultiMap<string>();
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE, withBalance ? "true" : "false");
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE_AND_CBA, withCba ? "true" : "false");

            return client.Get<Account>(uri, queryParams);
        }
        public Account GetAccount(string externalKey, bool withBalance = false, bool withCba = false)
        {
            var uri = KbConfig.ACCOUNTS_PATH;

            var queryParams = new MultiMap<string>();
            queryParams.Add(KbConfig.QUERY_EXTERNAL_KEY, externalKey);
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE, withBalance ? "true" : "false");
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE_AND_CBA, withCba ? "true" : "false");
            
            return client.Get<Account>(uri, queryParams);
        }

        public Account CreateAccount(Account account, string createdBy, string reason, string comment, IDictionary<string, string> additionalHeaders = null)
        {
            var options = ParamsWithAudit(createdBy, reason, comment);
            return client.Post<Account>(KbConfig.ACCOUNTS_PATH, account, options, null, true);
        }

        public Account UpdateAccount(Account account, string createdBy, string reason, string comment, IDictionary<string, string> additionalHeaders = null)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + account.AccountId;
            var options = ParamsWithAudit(createdBy, reason, comment);
            return client.Put<Account>(uri, account, options);
        }

        //BUNDLES
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Bundles GetAccountBundles(Guid accountId)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.BUNDLES;
            return client.Get<Bundles>(uri, DEFAULT_EMPTY_QUERY);
        }

        //INVOICE
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Invoice GetInvoice(string invoiceIdOrNumber, bool withItems, AuditLevel auditLevel)
        {
            var uri = KbConfig.INVOICES_PATH + "/" + invoiceIdOrNumber;

            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            options.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());


            return client.Get<Invoice>(uri, options);
        }

        public Invoice CreateInvoice(Guid accountId, Invoice invoice, DateTime futureDate, string createdBy, string reason,
            string comment)
        {
            var param = new MultiMap<string>();
            param.Add(KbConfig.QUERY_ACCOUNT_ID, accountId.ToString());
            param.Add(KbConfig.QUERY_TARGET_DATE, futureDate.ToDateString());
            var options = ParamsWithAudit(param, createdBy, reason, comment);

            return client.Post<Invoice>(KbConfig.INVOICES_PATH, invoice, options, DEFAULT_EMPTY_QUERY, true);
        }

        //INVOICES
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Invoices GetInvoicesForAccount(Guid accountId, bool withItems = false, bool unpaidOnly = false, AuditLevel auditLevel = AuditLevel.NONE)
        {

            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + KbConfig.INVOICES;

            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            options.Add(KbConfig.QUERY_UNPAID_INVOICES_ONLY, unpaidOnly.ToString());
            options.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            return client.Get<Invoices>(uri, options);
        }


        //INVOICE ITEM
        //-------------------------------------------------------------------------------------------------------------------------------------
        public List<InvoiceItem> CreateExternalCharge(IEnumerable<InvoiceItem> externalCharges, DateTime requestedDate,
            bool autoPay, string createdBy, string reason, string comment)
        {
            var externalChargesPerAccount = new Dictionary<Guid, Collection<InvoiceItem>>();
            
            foreach (var externalCharge in externalCharges)
            {
                if (externalCharge.AccountId == Guid.Empty)
                    throw new ArgumentException("InvoiceItem#accountId cannot be empty");

                if (string.IsNullOrEmpty(externalCharge.Currency))
                 throw new ArgumentException("InvoiceItem#currency cannot be empty");

                if (!externalChargesPerAccount.ContainsKey(externalCharge.AccountId))
                    externalChargesPerAccount.Add(externalCharge.AccountId, new Collection<InvoiceItem>());

                externalChargesPerAccount[externalCharge.AccountId].Add(externalCharge);
            }

            var createdExternalCharges = new List<InvoiceItem>();
            foreach (var accountId in externalChargesPerAccount.Keys)
            {
                var invoiceItems = CreateExternalCharges(accountId, externalChargesPerAccount[accountId], requestedDate, autoPay, createdBy, reason, comment);
                createdExternalCharges.AddRange(invoiceItems);
            }

            return createdExternalCharges;
        }

        private IEnumerable<InvoiceItem> CreateExternalCharges(Guid accountId, Collection<InvoiceItem> externalCharges, DateTime requestedDate, bool autoPay, string createdBy, string reason, string comment)
        {
            var uri = KbConfig.INVOICES_PATH + "/" + KbConfig.CHARGES + "/" + accountId;

            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_REQUESTED_DT, requestedDate.ToDateString());
            options.Add(KbConfig.QUERY_PAY_INVOICE, autoPay.ToString());
            
            var fullOptions = ParamsWithAudit(options, createdBy, reason, comment);

            return client.Post<List<InvoiceItem>>(uri, externalCharges, fullOptions);
        }

      
        //SUBSCRIPTION
        //-------------------------------------------------------------------------------------------------------------------------------------

        // subscription:create
        public Subscription CreateSubscription(Subscription subscription, string createdBy, string reason)
        {
            return CreateSubscription(subscription, null, createdBy, reason, null);
        }
        public Subscription CreateSubscription(Subscription subscription, string createdBy, string reason, string comment)
        {
            return CreateSubscription(subscription, null, createdBy, reason, comment);
        }

        public Subscription CreateSubscription(Subscription subscription, DateTime? requestedDate, string createdBy, string reason, string comment)
        {
            if (subscription == null)
                throw new ArgumentException("subscription");

            var param = new MultiMap<string>();
            if (requestedDate.HasValue)
                param.Add(KbConfig.QUERY_REQUESTED_DT, requestedDate.Value.ToDateTimeISO());

            var options = ParamsWithAudit(param, createdBy, reason, comment);

            return client.Post<Subscription>(KbConfig.SUBSCRIPTIONS_PATH, subscription, options, null, true);
        }

        // subscription:cancel


        private MultiMap<String> ParamsWithAudit(MultiMap<String> queryParams, string createdBy, string reason, string comment)
        {
            var queryParamsWithAudit = new MultiMap<string>();
            queryParamsWithAudit.PutAll(queryParams);
            queryParamsWithAudit.PutAll(ParamsWithAudit(createdBy, reason, comment));
            return queryParamsWithAudit;
        }

        private MultiMap<string> ParamsWithAudit(string createdBy,string reason, string comment)
        {
            var options = new MultiMap<string>();
            
            options.Add(KbConfig.AUDIT_OPTION_CREATED_BY, createdBy);
            options.Add(KbConfig.AUDIT_OPTION_COMMENT, comment);
            options.Add(KbConfig.AUDIT_OPTION_REASON, reason);

            return options;
        }
    }
}