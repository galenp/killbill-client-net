using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using KillBill.Client.Net.Infrastructure;
using KillBill.Client.Net.Model;
using RestSharp.Contrib;

namespace KillBill.Client.Net
{
    public class KillBillClient : IKillBillClient
    {
        private readonly IKbHttpClient client;
        private static MultiMap<string>  DEFAULT_EMPTY_QUERY = new MultiMap<string>();
        private const AuditLevel DEFAULT_AUDIT_LEVEL = AuditLevel.NONE;
        
       

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
            return client.PostAndFollow<Account>(KbConfig.ACCOUNTS_PATH, account, options, DEFAULT_EMPTY_QUERY);
        }

        public Account UpdateAccount(Account account, string createdBy, string reason, string comment, IDictionary<string, string> additionalHeaders = null)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + account.AccountId;
            var options = ParamsWithAudit(createdBy, reason, comment);
            return client.Put<Account>(uri, account, options);
        }

        public InvoicePayments GetInvoicePaymentsForAccount(Guid accountId, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.INVOICE_PAYMENTS;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, auditLevel.ToString());
            return client.Get<InvoicePayments>(uri, queryparams);
        }

        public Payments GetPaymentsForAccount(Guid accountId, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENTS;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, auditLevel.ToString());
            return client.Get<Payments>(uri, queryparams);
        }

        //ACCOUNTS
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Accounts GetAccounts(long offset = 0L, long limit = 100L, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + KbConfig.PAGINATION;

            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, auditLevel.ToString());
            return client.Get<Accounts>(uri, queryparams);
        }

        //ACCOUNT EMAIL
        //-------------------------------------------------------------------------------------------------------------------------------------
        public void AddEmailToAccount(AccountEmail email, string createdBy, string reason, string comment)
        {
            if (email == null)
                throw new ArgumentNullException("email");

            if (email.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#accountId cannot be empty");
            var uri = KbConfig.ACCOUNTS_PATH + "/" + email.AccountId + "/" + KbConfig.EMAILS;
            var options = ParamsWithAudit(createdBy, reason, comment);

            client.Post(uri, email, options);
        }

        public void RemoveEmailFromAccount(AccountEmail email, string createdBy, string reason, string comment)
        {
            if (email == null)
                throw new ArgumentNullException("email");

            if (email.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#accountId cannot be empty");
            
            var uri = KbConfig.ACCOUNTS_PATH + "/" + email.AccountId + "/" + KbConfig.EMAILS;
            var options = ParamsWithAudit(createdBy, reason, comment);
            client.Delete(uri, options, options);
        }

        //ACCOUNT EMAILS
        //-------------------------------------------------------------------------------------------------------------------------------------
        public AccountEmails GetEmailsForAccount(Guid accountId)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.EMAILS;
            return client.Get<AccountEmails>(uri, DEFAULT_EMPTY_QUERY);
        }

        //ACCOUNT TIMELINE
        //-------------------------------------------------------------------------------------------------------------------------------------
        public AccountTimeline GetAccountTimeline(Guid accountId, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.TIMELINE;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            return client.Get<AccountTimeline>(uri, queryparams);
        }

        //BUNDLE
        //-------------------------------------------------------------------------------------------------------------------------------------        
        public Bundle GetBundle(Guid bundleId)
        {
            var uri = KbConfig.BUNDLES_PATH + "/" + bundleId;
            return client.Get<Bundle>(uri, DEFAULT_EMPTY_QUERY);
        }

        public Bundle GetBundle(string externalKey)
        {
            var uri = KbConfig.BUNDLES_PATH;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_EXTERNAL_KEY, externalKey);
            return client.Get<Bundle>(uri, queryparams);
        }

        public Bundle TransferBundle(Bundle bundle, string createdBy, string reason, string comment)
        {
            if (bundle == null)
                throw new ArgumentNullException("bundle");

            if (bundle.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#accountId cannot be empty");

            if (bundle.BundleId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#bundleId cannot be empty");

            var uri = KbConfig.BUNDLES_PATH + "/" + bundle.BundleId;

            var options = ParamsWithAudit(createdBy, reason, comment);
            return client.PutAndFollow<Bundle>(uri, bundle, options, DEFAULT_EMPTY_QUERY);
        }

        //BUNDLES
        //-------------------------------------------------------------------------------------------------------------------------------------        
        public Bundles GetAccountBundles(Guid accountId)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.BUNDLES;
            return client.Get<Bundles>(uri, DEFAULT_EMPTY_QUERY);
        }

        public Bundles GetBundles(long offset = 0L, long limit = 100L, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.BUNDLES_PATH + "/" + KbConfig.PAGINATION;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryparams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            return client.Get<Bundles>(uri, queryparams);
        }

        public Bundles SearchBundles(string key, long offset = 0L, long limit = 100L, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var utf = Encoding.UTF8.GetBytes(key);
            var uri = KbConfig.BUNDLES_PATH + "/" + KbConfig.SEARCH + "/" + Encoding.UTF8.GetString(utf); ;
          
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryparams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            return client.Get<Bundles>(uri, queryparams);
        }

        
        //CREDITS
        //-------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates a credit against an account. This also creates a new invoice.
        /// </summary>
        /// <param name="credit">Credit Object</param>
        /// <returns>Processed credit object with populated invoice details.</returns>
        public Credit CreateCredit(Credit credit, string createdBy, string reason, string comment)
        {
            if (credit == null)
                throw new ArgumentNullException("credit");
            if (credit.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("Credit#accountId cannot be null");
            if (credit.CreditAmount <= 0)
                throw new ArgumentException("Credit#CreditAmount must be greater than 0");

            var options = ParamsWithAudit(createdBy, reason, comment);
            return client.PostAndFollow<Credit>(KbConfig.CREDITS_PATH, credit, options, DEFAULT_EMPTY_QUERY);
        }


        public Credit GetCredit(Guid creditId, AuditLevel auditLevel)
        {
            var uri = KbConfig.CREDITS_PATH + "/" + creditId;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            return client.Get<Credit>(uri, queryparams);
        }

        //INVOICE
        //-------------------------------------------------------------------------------------------------------------------------------------
        

        /// <summary>
        /// Triggers an invoice RUN!
        /// </summary>
        /// <remarks>Don't be fooled by the method name... this SHOULD NOT be used to create invoices. Invoices are created as a byproduct of other actions like 'Creating Credits', 'External Charges'</remarks>
        public Invoice CreateInvoice(Guid accountId, Invoice invoice, DateTime futureDate, string createdBy, string reason,
            string comment)
        {
            var param = new MultiMap<string>();
            param.Add(KbConfig.QUERY_ACCOUNT_ID, accountId.ToString());
            param.Add(KbConfig.QUERY_TARGET_DATE, futureDate.ToDateString());
            var options = ParamsWithAudit(param, createdBy, reason, comment);

            return client.PostAndFollow<Invoice>(KbConfig.INVOICES_PATH, invoice, options, DEFAULT_EMPTY_QUERY);
        }

        public Invoice GetInvoice(int invoiceNumber, bool withItems = false, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            return GetInvoiceByIdOrNumber(invoiceNumber.ToString(), withItems, auditLevel);
        }
        public Invoice GetInvoice(string invoiceIdOrNumber, bool withItems=false, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            return GetInvoiceByIdOrNumber(invoiceIdOrNumber, withItems, auditLevel);
        }
        public Invoice GetInvoiceByIdOrNumber(string invoiceIdOrNumber, bool withItems = false, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.INVOICES_PATH + "/" + invoiceIdOrNumber;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            queryparams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            return client.Get<Invoice>(uri, queryparams);
        }

        //INVOICES
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Invoices GetInvoicesForAccount(Guid accountId, bool withItems = false, bool unpaidOnly = false, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {

            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.INVOICES;

            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            options.Add(KbConfig.QUERY_UNPAID_INVOICES_ONLY, unpaidOnly.ToString());
            options.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            return client.Get<Invoices>(uri, options);
        }


        public Invoices SearchInvoices(string key, long offset = 0L, long limit= 100L, AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var utf = Encoding.UTF8.GetBytes(key);
            var uri = KbConfig.INVOICES_PATH + "/" + KbConfig.SEARCH + "/" + Encoding.UTF8.GetString(utf); ;
            var queryparams = new MultiMap<string>();
            queryparams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryparams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryparams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            return client.Get<Invoices>(uri, queryparams);
        }

        //INVOICE ITEM
        //-------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes an 'external charge' action... note if no InvoiceId is provided on each charge then the server will create a new invoice for the batch.
        /// </summary>
        /// <remarks>The currency on each charges needs to be the same as the currency on the referenced account.</remarks>
        /// <returns>List of processed charges with invoice references.</returns>
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

        //INVOICE EMAIL
        //-------------------------------------------------------------------------------------------------------------------------------------
        public InvoiceEmail GetEmailNotificationsForAccount(Guid accountId)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.EMAIL_NOTIFICATIONS;
            return client.Get<InvoiceEmail>(uri, DEFAULT_EMPTY_QUERY);
        }

        public void UpdateEmailNotificationsForAccount(InvoiceEmail invoiceEmail, string createdBy, string reason,
            string comment)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + invoiceEmail.AccountId + "/" + KbConfig.EMAIL_NOTIFICATIONS;
            var options = ParamsWithAudit(createdBy, reason, comment);
            client.Put(uri, invoiceEmail, options);
        }

        //PAYMENT
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, string createdBy, string reason, string comment)
        {
            return CreatePayment(accountId, null, paymentTransaction, new Dictionary<string, string>(), createdBy, reason, comment);
        }

        public Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, string createdBy, string reason, string comment)
        {
            return CreatePayment(accountId, null, paymentTransaction, pluginProperties, createdBy, reason, comment);
        }
        public Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, string createdBy, string reason, string comment)
        {
            return CreatePayment(accountId, paymentMethodId, paymentTransaction, new Dictionary<string, string>(), createdBy, reason, comment);
        }
        public Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, string createdBy, string reason, string comment)
        {

            var allowedTransactionTypes = new[] {"AUTHORIZE", "CREDIT", "PURCHASE"};
            if (accountId.Equals(Guid.Empty))
                throw new ArgumentException("createPayment#accountId must not be empty");

            if (paymentTransaction == null)
                throw new ArgumentNullException("paymentTransaction");

            if (!allowedTransactionTypes.Contains(paymentTransaction.TransactionType))
                throw new ArgumentException("Invalid paymentTransaction type " + paymentTransaction.TransactionType);

            if (paymentTransaction.Amount <= 0)
                throw new ArgumentException("PaymentTransaction#amount cannot be 0 or less");

            if (paymentTransaction.Currency == null)
                throw new ArgumentException("PaymentTransaction#currency cannot be null");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENTS;

            var param = new MultiMap<string>();

            if (paymentMethodId.HasValue)
                param.Add("paymentMethodId", paymentMethodId.ToString());

            StorePluginPropertiesAsParams(pluginProperties, ref param);
            
            var queryParams = ParamsWithAudit(param,createdBy,reason,comment);

            return client.PostAndFollow<Payment>(uri, paymentTransaction, queryParams, DEFAULT_EMPTY_QUERY);

        }


        //PAYMENT METHODS
        //-------------------------------------------------------------------------------------------------------------------------------------
        public PaymentMethod GetPaymentMethod(Guid paymentMethodId, bool withPluginInfo = false,
            AuditLevel auditLevel = DEFAULT_AUDIT_LEVEL)
        {
            var uri = KbConfig.PAYMENT_METHODS_PATH + "/" + paymentMethodId;

            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_WITH_PLUGIN_INFO, withPluginInfo.ToString());
            options.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            return client.Get<PaymentMethod>(uri, options);

        }

        public PaymentMethods GetPaymentMethodsForAccount(Guid accountId, bool withPluginInfo = false)  {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENT_METHODS;

            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_WITH_PLUGIN_INFO, withPluginInfo.ToString());

            return client.Get<PaymentMethods>(uri, options);
        }

       

        public PaymentMethod CreatePaymentMethod(PaymentMethod paymentMethod, string createdBy, string reason,
            string comment)
        {
            if (paymentMethod == null)
                throw new ArgumentNullException("paymentMethod");

            if (paymentMethod.AccountId.Equals(Guid.Empty) || string.IsNullOrEmpty(paymentMethod.PluginName))
                throw new ArgumentException("paymentMethod#accountId and paymentMethod#pluginName must not be empty");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + paymentMethod.AccountId + "/" + KbConfig.PAYMENT_METHODS;
            var queryparams = ParamsWithAudit(createdBy, reason, comment);
            return client.PostAndFollow<PaymentMethod>(uri, paymentMethod, queryparams, DEFAULT_EMPTY_QUERY);

        }

        public void DeletePaymentMethod(Guid paymentMethodId, bool deleteDefault, string createdBy, string reason, string comment,  bool forceDelete = false, bool forceDeleteDefault = false)
        {
            var uri = KbConfig.PAYMENT_METHODS_PATH + "/" + paymentMethodId;
            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_DELETE_DEFAULT_PM_WITH_AUTO_PAY_OFF, deleteDefault.ToString());
            options.Add(KbConfig.QUERY_FORCE_DEFAULT_PM_DELETION, forceDeleteDefault.ToString());
            var queryparams = ParamsWithAudit(options, createdBy, reason, comment);
            client.Delete(uri, queryparams);
        }


        //TENANT    
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Tenant CreateTenant(Tenant tenant, string createdBy, string reason, string comment)
        {
            if (tenant == null)
                throw new ArgumentNullException("tenant");

            if (string.IsNullOrEmpty(tenant.ApiKey) || string.IsNullOrEmpty(tenant.ApiSecret)) 
                throw new ArgumentException("tenant#apiKey and tenant#apiSecret must not be empty");

            
            var queryparams = ParamsWithAudit(createdBy, reason, comment);
            return client.PostAndFollow<Tenant>(KbConfig.TENANTS_PATH, tenant, queryparams, DEFAULT_EMPTY_QUERY);
        }


        public void DeleteCallbackNotificationForTenanr(Guid tenantId, string createdBy, string reason,
            string comment)
        {
            var queryparams = ParamsWithAudit(createdBy, reason, comment);
            var uri = KbConfig.TENANTS_PATH + "/" + KbConfig.LEGACY_REGISTER_NOTIFICATION_CALLBACK;
            client.Delete(uri, queryparams);
        }
        public TenantKey RegisterCallBackNotificationForTenant(string callback, string createdBy, string reason,
            string comment)
        {
            var options = new MultiMap<string>();
            options.Add(KbConfig.QUERY_NOTIFICATION_CALLBACK, callback);
            var queryparams = ParamsWithAudit(options, createdBy, reason, comment);
            var uri = KbConfig.TENANTS_PATH + "/" + KbConfig.REGISTER_NOTIFICATION_CALLBACK;
            return client.PostAndFollow<TenantKey>(uri, null, queryparams, DEFAULT_EMPTY_QUERY);
        }

        public TenantKey RetrieveRegisteredCallBacks()
        {
            var uri = KbConfig.TENANTS_PATH + "/" + KbConfig.REGISTER_NOTIFICATION_CALLBACK;
            return client.Get<TenantKey>(uri, DEFAULT_EMPTY_QUERY);
        }

        //SUBSCRIPTION
        //-------------------------------------------------------------------------------------------------------------------------------------

        // subscription:create
        public Subscription GetSubscription(Guid subscriptionId)
        {
            var uri = KbConfig.SUBSCRIPTIONS_PATH + "/" + subscriptionId;
            return client.Get<Subscription>(uri, DEFAULT_EMPTY_QUERY);
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

        // subscription:update


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

        private void StorePluginPropertiesAsParams(Dictionary<string, string> pluginProperties,
            ref MultiMap<string> queryParams)
        {
            foreach (var key in pluginProperties.Keys)
            {
                if (queryParams == null)
                    queryParams = new MultiMap<string>();

                queryParams.Add(KbConfig.QUERY_PLUGIN_PROPERTY, string.Format("{0}={1}", Encoding.UTF8.GetBytes(key), HttpUtility.UrlEncode(pluginProperties[key])));
            }
        }
    }
}