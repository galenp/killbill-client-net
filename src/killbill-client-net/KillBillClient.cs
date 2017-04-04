using System;
using System.CodeDom;
using System.Collections.Generic;
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
        private const AuditLevel DefaultAuditLevel = AuditLevel.NONE;

        public KillBillClient(IKbHttpClient client)
        {
            this.client = client;
        }

        public RequestOptions BaseOptions(string createdBy = null, string requestId = null, string reason = null, string comment = null)
        {
            return RequestOptions.Default().Extend()
                                    .WithCreatedBy(createdBy)
                                    .WithReason(reason)
                                    .WithComment(comment)
                                    .Build();
        }

        #region ACCOUNTS
        //ACCOUNTS
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Accounts GetAccounts(RequestOptions inputOptions)
        {
            return GetAccounts(0L, 100L, inputOptions);
        }


        public Accounts GetAccounts(long offset, long limit, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + KbConfig.PAGINATION;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryParams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Accounts>(uri, requestOptions);
        }
        #endregion

        #region ACCOUNT
        //ACCOUNT
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Account GetAccount(Guid accountId, RequestOptions inputOptions, bool withBalance = false, bool withCba = false)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE, withBalance ? "true" : "false");
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE_AND_CBA, withCba ? "true" : "false");

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Get<Account>(uri, requestOptions);
        }

        public Account GetAccount(string externalKey, RequestOptions inputOptions, bool withBalance = false, bool withCba = false)
        {
            var uri = KbConfig.ACCOUNTS_PATH;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_EXTERNAL_KEY, externalKey);
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE, withBalance ? "true" : "false");
            queryParams.Add(KbConfig.QUERY_ACCOUNT_WITH_BALANCE_AND_CBA, withCba ? "true" : "false");
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Get<Account>(uri, requestOptions);
        }

        public Account CreateAccount(Account account, RequestOptions inputOptions)
        {
            var followLocation = inputOptions.FollowLocation ?? true;
            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).Build();
            return client.Post<Account>(KbConfig.ACCOUNTS_PATH, account, requestOptions);
        }

        public Account UpdateAccount(Account account, RequestOptions inputOptions)
        {
            return UpdateAccount(account, false, inputOptions);
        }

        public Account UpdateAccount(Account account, bool treatNullAsReset, RequestOptions inputOptions)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + account.AccountId;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_ACCOUNT_TREAT_NULL_AS_RESET, treatNullAsReset ? "true" : "false");
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Put<Account>(uri, account, requestOptions);
        }


        public InvoicePayments GetInvoicePaymentsForAccount(Guid accountId, RequestOptions inputOptions, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.INVOICE_PAYMENTS;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<InvoicePayments>(uri, requestOptions);
        }

        public Payments GetPaymentsForAccount(Guid accountId, RequestOptions inputOptions, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENTS;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Payments>(uri, requestOptions);
        }


        #endregion


        //ACCOUNT EMAIL
        //-------------------------------------------------------------------------------------------------------------------------------------
        public void AddEmailToAccount(AccountEmail email, RequestOptions inputOptions)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            if (email.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#accountId cannot be empty");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + email.AccountId + "/" + KbConfig.EMAILS;

            client.Post(uri, email, inputOptions);
        }

        public void RemoveEmailFromAccount(AccountEmail email, RequestOptions inputOptions)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            if (email.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#accountId cannot be empty");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + email.AccountId + "/" + KbConfig.EMAILS + "/" +
                      HttpUtility.UrlEncode(email.Email);

            client.Delete(uri, inputOptions);
        }

        //ACCOUNT EMAILS
        //-------------------------------------------------------------------------------------------------------------------------------------

        public AccountEmails GetEmailsForAccount(Guid accountId, RequestOptions inputOptions)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.EMAILS;
            return client.Get<AccountEmails>(uri, inputOptions);
        }

        //ACCOUNT TIMELINE
        //-------------------------------------------------------------------------------------------------------------------------------------
        public AccountTimeline GetAccountTimeline(Guid accountId, RequestOptions inputOptions, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.TIMELINE;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<AccountTimeline>(uri, requestOptions);
        }

        //BUNDLE
        //-------------------------------------------------------------------------------------------------------------------------------------        
        public Bundle GetBundle(Guid bundleId, RequestOptions inputOptions)
        {
            var uri = KbConfig.BUNDLES_PATH + "/" + bundleId;
            return client.Get<Bundle>(uri, inputOptions);
        }

        public Bundle GetBundle(string externalKey, RequestOptions inputOptions)
        {
            var uri = KbConfig.BUNDLES_PATH;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_EXTERNAL_KEY, externalKey);

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            var bundles = client.Get<Bundles>(uri, requestOptions);

            return bundles?.First();
        }

        public Bundle TransferBundle(Bundle bundle, RequestOptions inputOptions)
        {
            if (bundle == null)
                throw new ArgumentNullException(nameof(bundle));

            if (bundle.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#accountId cannot be empty");

            if (bundle.BundleId.Equals(Guid.Empty))
                throw new ArgumentException("AccountEmail#bundleId cannot be empty");

            var uri = KbConfig.BUNDLES_PATH + "/" + bundle.BundleId;

            var followLocation = inputOptions.FollowLocation ?? true;
            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).Build();

            return client.Put<Bundle>(uri, bundle, requestOptions);
        }

        public Bundle CreateSubscriptionWithAddOns(IEnumerable<Subscription> subscriptions, RequestOptions inputOptions,
           DateTime? requestedDate = null, int? timeoutSec = null)
        {
            foreach (var subscription in subscriptions)
            {
                if (string.IsNullOrEmpty(subscription.PlanName))
                {
                    if (string.IsNullOrEmpty(subscription.ProductName))
                        throw new ArgumentException("Subscription#productName cannot be null");

                    if (string.IsNullOrEmpty(subscription.ProductCategory))
                        throw new ArgumentException("Subscription#productCategory cannot be null");

                    if (string.IsNullOrEmpty(subscription.ProductCategory))
                        throw new ArgumentException("Subscription#billingPeriod cannot be null");

                    if (string.IsNullOrEmpty(subscription.PriceList))
                        throw new ArgumentException("Subscription#priceList cannot be null");

                    if (subscription.ProductCategory == "BASE" && subscription.AccountId.Equals(Guid.Empty)) {
                        throw new ArgumentException("Account#accountId cannot be null for base subscription");
                    }
                }              
            }

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            if (timeoutSec.HasValue && timeoutSec.Value > 0)
            {
                queryParams.Add(KbConfig.QUERY_CALL_COMPLETION, "true");
                queryParams.Add(KbConfig.QUERY_CALL_TIMEOUT, timeoutSec.Value.ToString());
            }

            if (requestedDate.HasValue)
                queryParams.Add(KbConfig.QUERY_REQUESTED_DT, requestedDate.Value.ToDateString());

            //var httpTimeout = Math.Max(KbConfig.DEFAULT_HTTP_TIMEOUT_SEC, timeoutSec ?? 0);
            var uri = KbConfig.SUBSCRIPTIONS_PATH + "/createEntitlementWithAddOns";
            var followLocation = inputOptions.FollowLocation ?? true;
            var requestOptions =
                inputOptions.Extend().WithQueryParams(queryParams).WithFollowLocation(followLocation).Build();

            return client.Post<Bundle>(uri, subscriptions, requestOptions);
        }

        //BUNDLES
        //-------------------------------------------------------------------------------------------------------------------------------------        
        public Bundles GetAccountBundles(Guid accountId, RequestOptions inputOptions)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.BUNDLES;
            return client.Get<Bundles>(uri, inputOptions);
        }

        public Bundles GetAccountBundles(Guid accountId, string externalKey, RequestOptions inputOptions)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.BUNDLES;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_EXTERNAL_KEY, externalKey);
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Bundles>(uri, requestOptions);
        }


        public Bundles GetBundles(RequestOptions inputOptions, long offset = 0L, long limit = 100L, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.BUNDLES_PATH + "/" + KbConfig.PAGINATION;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryParams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Get<Bundles>(uri, requestOptions);
        }

        public Bundles SearchBundles(string key, RequestOptions inputOptions, long offset = 0L, long limit = 100L, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.BUNDLES_PATH + "/" + KbConfig.SEARCH + "/" + HttpUtility.UrlEncode(key);

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryParams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Bundles>(uri, requestOptions);
        }


        //CREDIT
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Credit CreateCredit(Credit credit, bool autoCommit, RequestOptions inputOptions)
        {
            if (credit == null)
                throw new ArgumentNullException(nameof(credit));
            if (credit.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("Credit#accountId cannot be null");
            if (credit.CreditAmount <= 0)
                throw new ArgumentException("Credit#CreditAmount must be greater than 0");

            var followLocation = inputOptions.FollowLocation ?? true;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_AUTO_COMMIT, autoCommit.ToString());
            
            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).WithQueryParams(queryParams).Build();

            return client.Post<Credit>(KbConfig.CREDITS_PATH, credit, requestOptions);
        }


        public Credit GetCredit(Guid creditId, RequestOptions inputOptions, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.CREDITS_PATH + "/" + creditId;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Get<Credit>(uri, requestOptions);
        }

        //INVOICE
        //-------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Triggers an invoice RUN!
        /// </summary>
        /// <remarks>Don't be fooled by the method name... this SHOULD NOT be used to create invoices. Invoices are created as a byproduct of other actions like 'Creating Credits', 'External Charges'</remarks>
        public Invoice CreateInvoice(Guid accountId, DateTime futureDate, RequestOptions inputOptions)
        {
            var followLocation = inputOptions.FollowLocation ?? true;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_ACCOUNT_ID, accountId.ToString());
            queryParams.Add(KbConfig.QUERY_TARGET_DATE, futureDate.ToDateString());

            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).WithQueryParams(queryParams).Build();
            
            return client.Post<Invoice>(KbConfig.INVOICES_PATH, null, requestOptions);
        }

        public Invoice GetInvoice(int invoiceNumber, RequestOptions inputOptions, bool withItems = false, bool withChildrenItems = false, AuditLevel auditLevel = DefaultAuditLevel)
        {
            return GetInvoiceByIdOrNumber(invoiceNumber.ToString(), inputOptions, withItems, withChildrenItems, auditLevel);
        }

        public Invoice GetInvoice(string invoiceIdOrNumber, RequestOptions inputOptions, bool withItems = false, bool withChildrenItems = false, AuditLevel auditLevel = DefaultAuditLevel)
        {
            return GetInvoiceByIdOrNumber(invoiceIdOrNumber, inputOptions, withItems, withChildrenItems, auditLevel);
        }

        public Invoice GetInvoiceByIdOrNumber(string invoiceIdOrNumber, RequestOptions inputOptions, bool withItems = false, bool withChildrenItems = false, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.INVOICES_PATH + "/" + invoiceIdOrNumber;
        
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            queryParams.Add(KbConfig.QUERY_INVOICE_WITH_CHILDREN_ITEMS, withChildrenItems.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Invoice>(uri, requestOptions);
        }

        //INVOICES
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Invoices GetInvoices(RequestOptions inputOptions)
        {
            return GetInvoices(true, 0L, 100L, inputOptions);
        }

        public Invoices GetInvoices(bool withItems, long offset, long limit, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE)
        {
            var uri = KbConfig.INVOICES_PATH + "/" + KbConfig.PAGINATION;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryParams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryParams.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Invoices>(uri, requestOptions);
        }

        public Invoices GetInvoicesForAccount(Guid accountId, RequestOptions inputOptions, bool withItems = false, bool unpaidOnly = false, bool includeMigrationInvoices = false, AuditLevel auditLevel = DefaultAuditLevel)
        {

            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.INVOICES;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_INVOICE_WITH_ITEMS, withItems.ToString());
            queryParams.Add(KbConfig.QUERY_UNPAID_INVOICES_ONLY, unpaidOnly.ToString());
            queryParams.Add(KbConfig.QUERY_WITH_MIGRATION_INVOICES, includeMigrationInvoices.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<Invoices>(uri, requestOptions);
        }

        public Invoices SearchInvoices(string key, RequestOptions inputOptions)
        {
            return SearchInvoices(key, 0L, 100L, DefaultAuditLevel, inputOptions);
        }

        public Invoices SearchInvoices(string key, long offset, long limit, RequestOptions inputOptions)
        {
            return SearchInvoices(key, offset, limit, DefaultAuditLevel, inputOptions);
        }

        public Invoices SearchInvoices(string key, long offset, long limit, AuditLevel auditLevel, RequestOptions inputOptions)
        {
            var utf = Encoding.UTF8.GetBytes(key);
            var uri = KbConfig.INVOICES_PATH + "/" + KbConfig.SEARCH + "/" + Encoding.UTF8.GetString(utf);

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_SEARCH_OFFSET, offset.ToString());
            queryParams.Add(KbConfig.QUERY_SEARCH_LIMIT, limit.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Get<Invoices>(uri, requestOptions);
        }

        //INVOICE ITEM
        //-------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes an 'external charge' action... note if no InvoiceId is provided on each charge then the server will create a new invoice for the batch.
        /// </summary>
        /// <remarks>The currency on each charges needs to be the same as the currency on the referenced account.</remarks>
        /// <returns>List of processed charges with invoice references.</returns>

        public List<InvoiceItem> CreateExternalCharges(IEnumerable<InvoiceItem> externalCharges, DateTime? requestedDate,
            bool autoPay, bool autoCommit, RequestOptions inputOptions)
        {
            return CreateExternalCharges(externalCharges, requestedDate, autoPay, autoCommit, null, null, inputOptions);
        }

        public List<InvoiceItem> CreateExternalCharges(IEnumerable<InvoiceItem> externalCharges, DateTime? requestedDate,
                bool autoPay, bool autoCommit, string paymentExternalKey, string transactionExternalKey, RequestOptions inputOptions)
        {
            var externalChargesPerAccount = new Dictionary<Guid, List<InvoiceItem>>();

            foreach (var externalCharge in externalCharges)
            {
                if (externalCharge.AccountId == Guid.Empty)
                    throw new ArgumentException("InvoiceItem#accountId cannot be empty");

                if (string.IsNullOrEmpty(externalCharge.Currency))
                    throw new ArgumentException("InvoiceItem#currency cannot be empty");

                if (!externalChargesPerAccount.ContainsKey(externalCharge.AccountId))
                    externalChargesPerAccount.Add(externalCharge.AccountId, new List<InvoiceItem>());

                externalChargesPerAccount[externalCharge.AccountId].Add(externalCharge);
            }

            var createdExternalCharges = new List<InvoiceItem>();
            foreach (var accountId in externalChargesPerAccount.Keys)
            {
                var invoiceItems = CreateExternalCharges(accountId, externalChargesPerAccount[accountId], requestedDate, autoPay, autoCommit, paymentExternalKey, paymentExternalKey, inputOptions);
                createdExternalCharges.AddRange(invoiceItems);
            }

            return createdExternalCharges;
        }

        private IEnumerable<InvoiceItem> CreateExternalCharges(Guid accountId, IEnumerable<InvoiceItem> externalCharges, DateTime? requestedDate, bool autoPay, bool autoCommit, string paymentExternalKey, string transactionExternalKey, RequestOptions inputOptions)
        {
            var uri = KbConfig.INVOICES_PATH + "/" + KbConfig.CHARGES + "/" + accountId;

            var queryParams = new MultiMap<string>();

            if (requestedDate.HasValue)
            {
                queryParams.Add(KbConfig.QUERY_REQUESTED_DT, requestedDate.Value.ToDateString());
            }

            queryParams.Add(KbConfig.QUERY_PAY_INVOICE, autoPay.ToString());
            queryParams.Add(KbConfig.QUERY_AUTO_COMMIT, autoCommit.ToString());

            if (!string.IsNullOrEmpty(paymentExternalKey))
            {
                queryParams.Add(KbConfig.QUERY_PAYMENT_EXT_KEY, paymentExternalKey);
            }

            if (!string.IsNullOrEmpty(transactionExternalKey))
            {
                queryParams.Add(KbConfig.QUERY_TRANSACTION_EXT_KEY, transactionExternalKey);
            }

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Post<List<InvoiceItem>>(uri, externalCharges, requestOptions);
        }

        //INVOICE EMAIL
        //-------------------------------------------------------------------------------------------------------------------------------------
        public InvoiceEmail GetEmailNotificationsForAccount(Guid accountId, RequestOptions inputOptions)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.EMAIL_NOTIFICATIONS;
            return client.Get<InvoiceEmail>(uri, inputOptions);
        }

        public void UpdateEmailNotificationsForAccount(InvoiceEmail invoiceEmail, RequestOptions inputOptions)
        {
            if (invoiceEmail.AccountId.Equals(Guid.Empty))
                throw new ArgumentException("invoiceEmail#AccountId can not be empty");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + invoiceEmail.AccountId + "/" + KbConfig.EMAIL_NOTIFICATIONS;
            client.Put(uri, invoiceEmail, inputOptions);
        }

        //PAYMENT
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, RequestOptions inputOptions)
        {
            return CreatePayment(accountId, null, paymentTransaction, null, new Dictionary<string, string>(), inputOptions);
        }

        public Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, RequestOptions inputOptions)
        {
            return CreatePayment(accountId, null, paymentTransaction, null, pluginProperties, inputOptions);
        }
        public Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, RequestOptions inputOptions)
        {
            return CreatePayment(accountId, paymentMethodId, paymentTransaction, null, new Dictionary<string, string>(), inputOptions);
        }
        public Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, RequestOptions inputOptions)
        {
            return CreatePayment(accountId, paymentMethodId, paymentTransaction, null, pluginProperties, inputOptions);
        }
        public Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, List<string> controlPluginNames, Dictionary<string, string> pluginProperties, RequestOptions inputOptions)
        {

            var allowedTransactionTypes = new[] { "AUTHORIZE", "CREDIT", "PURCHASE" };
            if (accountId.Equals(Guid.Empty))
                throw new ArgumentException("createPayment#accountId must not be empty");

            if (paymentTransaction == null)
                throw new ArgumentNullException(nameof(paymentTransaction));

            if (!allowedTransactionTypes.Contains(paymentTransaction.TransactionType))
                throw new ArgumentException("Invalid paymentTransaction type " + paymentTransaction.TransactionType);

            if (paymentTransaction.Amount <= 0)
                throw new ArgumentException("PaymentTransaction#amount cannot be 0 or less");

            if (paymentTransaction.Currency == null)
                throw new ArgumentException("PaymentTransaction#currency cannot be null");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENTS;

            var param = new MultiMap<string>().Create(inputOptions.QueryParams);

            if (paymentMethodId.HasValue)
                param.Add("paymentMethodId", paymentMethodId.ToString());

            if (controlPluginNames != null && controlPluginNames.Count > 0) {
                param.PutAll(KbConfig.CONTROL_PLUGIN_NAME, controlPluginNames);
            }
                

            StorePluginPropertiesAsParams(pluginProperties, ref param);
            var followLocation = inputOptions.FollowLocation ?? true;
            var requestOptions = inputOptions.Extend().WithQueryParams(param).WithFollowLocation(followLocation).Build();

            return client.Post<Payment>(uri, paymentTransaction, requestOptions);
        }


        //PAYMENT METHODS
        //-------------------------------------------------------------------------------------------------------------------------------------
        public PaymentMethod GetPaymentMethod(Guid paymentMethodId, RequestOptions inputOptions, bool withPluginInfo = false, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.PAYMENT_METHODS_PATH + "/" + paymentMethodId;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_WITH_PLUGIN_INFO, withPluginInfo.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            
            return client.Get<PaymentMethod>(uri, requestOptions);

        }

        public PaymentMethods GetPaymentMethodsForAccount(Guid accountId, RequestOptions inputOptions, Dictionary<string, string> pluginProperties = null,  bool withPluginInfo = false, AuditLevel auditLevel = DefaultAuditLevel)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENT_METHODS;

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_WITH_PLUGIN_INFO, withPluginInfo.ToString());
            queryParams.Add(KbConfig.QUERY_AUDIT, auditLevel.ToString());
            StorePluginPropertiesAsParams(pluginProperties, ref queryParams);
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<PaymentMethods>(uri, requestOptions);
        }



        public PaymentMethod CreatePaymentMethod(PaymentMethod paymentMethod, RequestOptions inputOptions)
        {
            if (paymentMethod == null)
                throw new ArgumentNullException(nameof(paymentMethod));

            if (paymentMethod.AccountId.Equals(Guid.Empty) || string.IsNullOrEmpty(paymentMethod.PluginName))
                throw new ArgumentException("paymentMethod#accountId and paymentMethod#pluginName must not be empty");

            var uri = KbConfig.ACCOUNTS_PATH + "/" + paymentMethod.AccountId + "/" + KbConfig.PAYMENT_METHODS;
            var followLocation = inputOptions.FollowLocation ?? true;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_PAYMENT_METHOD_IS_DEFAULT, paymentMethod.IsDefault ? "true" : "false");

            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).WithQueryParams(queryParams).Build();

            return client.Post<PaymentMethod>(uri, paymentMethod, requestOptions);
        }

        public void DeletePaymentMethod(Guid paymentMethodId, RequestOptions inputOptions, bool deleteDefault = false, bool forceDeleteDefault = false)
        {
            var uri = KbConfig.PAYMENT_METHODS_PATH + "/" + paymentMethodId;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_DELETE_DEFAULT_PM_WITH_AUTO_PAY_OFF, deleteDefault.ToString());
            queryParams.Add(KbConfig.QUERY_FORCE_DEFAULT_PM_DELETION, forceDeleteDefault.ToString());
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            client.Delete(uri, requestOptions);
        }

        public void UpdateDefaultPaymentMethod(Guid accountId, Guid paymentMethodId, RequestOptions inputOptions)
        {
            var uri = KbConfig.ACCOUNTS_PATH + "/" + accountId + "/" + KbConfig.PAYMENT_METHODS + "/" + paymentMethodId + "/" + KbConfig.PAYMENT_METHODS_DEFAULT_PATH_POSTFIX;
            client.Put(uri, null, inputOptions);
        }

        //TENANT    
        //-------------------------------------------------------------------------------------------------------------------------------------
        public Tenant CreateTenant(Tenant tenant, RequestOptions inputOptions, bool useGlobalDefault = true)
        {
            if (tenant == null)
                throw new ArgumentNullException(nameof(tenant));

            if (string.IsNullOrEmpty(tenant.ApiKey) || string.IsNullOrEmpty(tenant.ApiSecret))
                throw new ArgumentException("tenant#apiKey and tenant#apiSecret must not be empty");

            var followLocation = inputOptions.FollowLocation ?? true;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_TENANT_USE_GLOBAL_DEFAULT, useGlobalDefault.ToString());
            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).WithQueryParams(queryParams).Build();
            return client.Post<Tenant>(KbConfig.TENANTS_PATH, tenant, requestOptions);
        }


        public void UnregisterCallbackNotificationForTenant(Guid tenantId, RequestOptions inputOptions)
        {
            var uri = KbConfig.TENANTS_PATH + "/" + KbConfig.LEGACY_REGISTER_NOTIFICATION_CALLBACK;
            client.Delete(uri, inputOptions);
        }


        public TenantKey RegisterCallBackNotificationForTenant(string callback, RequestOptions inputOptions)
        {
            var uri = KbConfig.TENANTS_PATH + "/" + KbConfig.REGISTER_NOTIFICATION_CALLBACK;
            var followLocation = inputOptions.FollowLocation ?? true;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add(KbConfig.QUERY_NOTIFICATION_CALLBACK, callback);
            var requestOptions = inputOptions.Extend().WithFollowLocation(followLocation).WithQueryParams(queryParams).Build();
            return client.Post<TenantKey>(uri, null, requestOptions);
        }

        public TenantKey GetCallbackNotificationForTenant(RequestOptions inputOptions)
        {
            var uri = KbConfig.TENANTS_PATH + "/" + KbConfig.REGISTER_NOTIFICATION_CALLBACK;
            return client.Get<TenantKey>(uri, inputOptions);
        }

        //CATALOG
        //-------------------------------------------------------------------------------------------------------------------------------------
        public List<Catalog> GetCatalogJson(RequestOptions inputOptions, DateTime? requestedDate = null)
        {
            var uri = KbConfig.CATALOG_PATH;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            if (requestedDate.HasValue)
            {
                queryParams.Add(KbConfig.QUERY_DELETE_DEFAULT_PM_WITH_AUTO_PAY_OFF, requestedDate.Value.ToDateString());
            }

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Get<List<Catalog>>(uri, requestOptions);

        }

        //SUBSCRIPTION
        //-------------------------------------------------------------------------------------------------------------------------------------

        // subscription:create
        public Subscription GetSubscription(Guid subscriptionId, RequestOptions inputOptions)
        {
            var uri = KbConfig.SUBSCRIPTIONS_PATH + "/" + subscriptionId;
            return client.Get<Subscription>(uri, inputOptions);
        }

        // PLAN DETAIL
        public List<PlanDetail> GetBasePlans(RequestOptions inputOptions)
        {
            var uri = KbConfig.CATALOG_PATH + "/availableBasePlans";
            return client.Get<List<PlanDetail>>(uri, inputOptions);
        }

        public List<PlanDetail> GetAvailableAddons(string baseProductName, RequestOptions inputOptions)
        {
            var uri = KbConfig.CATALOG_PATH + "/availableAddons";

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            queryParams.Add("baseProductName", baseProductName);

            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();

            return client.Get<List<PlanDetail>>(uri, requestOptions);
        }

        public Subscription CreateSubscription(Subscription subscription, RequestOptions inputOptions, DateTime? requestedDate = null, bool? isMigrated = null)
        {
            //var httpTimeout = KbConfig.DEFAULT_HTTP_TIMEOUT_SEC;
            var followLocation = inputOptions.FollowLocation ?? true;

            if (subscription.PlanName == null)
            {
                if (subscription.AccountId.Equals(Guid.Empty))
                    throw new ArgumentException("Subscription#accountId cannot be empty");

                if (string.IsNullOrEmpty(subscription.ProductName))
                    throw new ArgumentException("Subscription#productName cannot be null");

                if (string.IsNullOrEmpty(subscription.ProductCategory))
                    throw new ArgumentException("Subscription#productCategory cannot be null");

                if (string.IsNullOrEmpty(subscription.BillingPeriod))
                    throw new ArgumentException("Subscription#billingPeriod cannot be null");

                if (string.IsNullOrEmpty(subscription.PriceList))
                    throw new ArgumentException("Subscription#priceList cannot be null");
            }

            if (subscription.ProductCategory == "BASE")
            {
                if (subscription.AccountId.Equals(Guid.Empty))
                    throw new ArgumentException("Subscription#accountId cannot be empty");
            }

            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
            

            //if (timeoutSec.HasValue)
            //{
            //    queryParams.Add(KbConfig.QUERY_CALL_COMPLETION, timeoutSec.Value > 0 ? "true" : "false");
            //    queryParams.Add(KbConfig.QUERY_CALL_TIMEOUT, timeoutSec.Value.ToString());
            //    timeoutSec = timeoutSec.Value;
            //}
            
            if (requestedDate.HasValue)
            {
                queryParams.Add(KbConfig.QUERY_REQUESTED_DT, requestedDate.Value.ToShortDateString());
            }

            if (isMigrated.HasValue)
            {
                queryParams.Add(KbConfig.QUERY_MIGRATED, isMigrated.ToString());
            }

           
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).WithFollowLocation(followLocation).Build();

            //return client.Post<Subscription>(KbConfig.SUBSCRIPTIONS_PATH, subscription, requestOptions, httpTimeout);
            return client.Post<Subscription>(KbConfig.SUBSCRIPTIONS_PATH, subscription, requestOptions);
        }

        // subscription:update

        public Subscription UpdateSubscription(Subscription subscription, RequestOptions inputOptions, BillingActionPolicy? billingPolicy = null, DateTime? requestedDate = null, bool? isMigrated = null)
        {
            if (subscription.SubscriptionId.Equals(Guid.Empty))
                throw new ArgumentException("Subscription#subscriptionId cannot be empty");

            if (subscription.PlanName == null)
            {
                if (string.IsNullOrEmpty(subscription.ProductName))
                    throw new ArgumentException("Subscription#productName cannot be null");

                if (string.IsNullOrEmpty(subscription.ProductCategory))
                    throw new ArgumentException("Subscription#productCategory cannot be null");

                if (string.IsNullOrEmpty(subscription.BillingPeriod))
                    throw new ArgumentException("Subscription#billingPeriod cannot be null");

                if (string.IsNullOrEmpty(subscription.PriceList))
                    throw new ArgumentException("Subscription#priceList cannot be null");
            }

            var uri = KbConfig.SUBSCRIPTIONS_PATH + "/" + subscription.SubscriptionId;
            var queryParams = new MultiMap<string>().Create(inputOptions.QueryParams);
           
            //if (timeoutSec.HasValue)
            //{
            //    queryParams.Add(KbConfig.QUERY_CALL_COMPLETION, timeoutSec.Value > 0 ? "true" : "false");
            //    queryParams.Add(KbConfig.QUERY_CALL_TIMEOUT, timeoutSec.Value.ToString());
            //    timeoutSec = timeoutSec.Value;
            //}

            if (requestedDate.HasValue)
            {
                queryParams.Add(KbConfig.QUERY_REQUESTED_DT, requestedDate.Value.ToShortDateString());
            }

            if (billingPolicy.HasValue)
            {
                queryParams.Add(KbConfig.QUERY_BILLING_POLICY, billingPolicy.ToString());
            }
            var requestOptions = inputOptions.Extend().WithQueryParams(queryParams).Build();
            return client.Put<Subscription>(uri, subscription, requestOptions);
        }


        // subscription:cancel


        private void StorePluginPropertiesAsParams(Dictionary<string, string> pluginProperties,
            ref MultiMap<string> queryParams)
        {
            if (pluginProperties == null)
                return;

            foreach (var key in pluginProperties.Keys)
            {
                if (queryParams == null)
                    queryParams = new MultiMap<string>();

                queryParams.Add(KbConfig.QUERY_PLUGIN_PROPERTY, $"{Encoding.UTF8.GetBytes(key)}={HttpUtility.UrlEncode(pluginProperties[key])}");
            }
        }

    }
}