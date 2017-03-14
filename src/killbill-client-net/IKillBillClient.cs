using System;
using System.Collections.Generic;
using KillBill.Client.Net.Infrastructure;
using KillBill.Client.Net.Model;

namespace KillBill.Client.Net
{
    public interface IKillBillClient
    {
        //REQUEST OPTIONS
        RequestOptions BaseOptions(string requestId, string createdBy, string reason = null, string comment = null);
       
        // ACCOUNT
        Account GetAccount(Guid accountId, RequestOptions inputOptions, bool withBalance = false, bool withCba = false);
        Account GetAccount(string externalKey, RequestOptions inputOptions, bool withBalance = false, bool withCba = false);
        Account CreateAccount(Account account, RequestOptions inputOptions);
        Account UpdateAccount(Account account, RequestOptions inpuOptions);
        Account UpdateAccount(Account account, bool treatNullAsReset, RequestOptions inpuOptions);
    

        //ACCOUNTS
        Accounts GetAccounts(RequestOptions requestOptions);
        Accounts GetAccounts(long offset, long limit, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE);

        //ACCOUNT EMAILS
        AccountEmails GetEmailsForAccount(Guid accountId, RequestOptions inputOptions);
        void AddEmailToAccount(AccountEmail email, RequestOptions inputOptions);
        void RemoveEmailFromAccount(AccountEmail email, RequestOptions inputOptions);

        //ACCOUNT TIMELINE
        AccountTimeline GetAccountTimeline(Guid accountId, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE);

        //PAYMENTS
        Payments GetPaymentsForAccount(Guid accountId, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE);


        //INVOICE PAYMENTS
        InvoicePayments GetInvoicePaymentsForAccount(Guid accountId, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE);

        //BUNDLE
        Bundle GetBundle(Guid bundleId, RequestOptions inputOptions);
        Bundle GetBundle(string externalKey, RequestOptions inputOptions);
        Bundle TransferBundle(Bundle bundle, RequestOptions inputOptions);

        //BUNDLES
        Bundles GetAccountBundles(Guid accountId, RequestOptions inputOptions);
        Bundles GetBundles(RequestOptions inputOptions, long offset = 0L, long limit = 100L, AuditLevel auditLevel = AuditLevel.NONE);
        Bundles SearchBundles(string key, RequestOptions inputOptions, long offset = 0L, long limit = 100L, AuditLevel auditLevel = AuditLevel.NONE);

        //CREDIT
        /// <summary>
        /// Creates a credit against an account. This also creates a new invoice.
        /// </summary>
        Credit CreateCredit(Credit credit, bool autoCommit, RequestOptions inputOptions);
        Credit GetCredit(Guid creditId, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE);

        //INVOICE
        /// <summary>
        /// Triggers an invoice RUN!
        /// </summary>
        /// <remarks>Don't be fooled by the method name... this SHOULD NOT be used to create invoices. Invoices are created as a byproduct of other actions like 'Creating Credits', 'External Charges'</remarks>
        Invoice CreateInvoice(Guid accountId, DateTime futureDate, RequestOptions inputOptions);
        //TODO implement CreateDryRunInvoice
        //Invoice CreateDryRunInvoice(Guid accountId, DateTime? futureDate, InvoiceDryRun dryRunInfo, RequestOptions requestOptions);
        Invoice GetInvoice(int invoiceNumber, RequestOptions inputOptions, bool withItems = false, bool withChildrenItems = false, AuditLevel auditLevel = AuditLevel.NONE);
        Invoice GetInvoice(string invoiceIdOrNumber, RequestOptions inputOptions, bool withItems=false, bool withChildrenItems = false, AuditLevel auditLevel = AuditLevel.NONE);
        
        //INVOICES
        Invoices GetInvoices(RequestOptions inputOptions);
        Invoices GetInvoices(bool withItems, long offset, long limit, RequestOptions inputOptions, AuditLevel auditLevel = AuditLevel.NONE);
        Invoices GetInvoicesForAccount(Guid accountId, RequestOptions inputOptions, bool withItems = false, bool unpaidOnly = false, bool includeMigrationInvoices = false, AuditLevel auditLevel = AuditLevel.NONE);
        Invoices SearchInvoices(string key, RequestOptions inputOptions);
        Invoices SearchInvoices(string key, long offset, long limit, RequestOptions inputOptions);

        //INVOICE ITEM
        /// <summary>
        /// Executes an 'external charge' action... note if no InvoiceId is provided on each charge then the server will create a new invoice for the batch.
        /// </summary>
        /// <remarks>The currency on each charges needs to be the same as the currency on the referenced account.</remarks>
        /// <returns>List of processed charges with invoice references.</returns>
        List<InvoiceItem> CreateExternalCharges(IEnumerable<InvoiceItem> externalCharges, DateTime? requestedDate, bool autoPay, bool autoCommit, RequestOptions inputOptions);
        List<InvoiceItem> CreateExternalCharges(IEnumerable<InvoiceItem> externalCharges, DateTime? requestedDate,
            bool autoPay, bool autoCommit, string paymentExternalKey, string transactionExternalKey,
            RequestOptions inputOptions);

        //INVOICE EMAIL
        InvoiceEmail GetEmailNotificationsForAccount(Guid accountId, RequestOptions inputOptions);
        void UpdateEmailNotificationsForAccount(InvoiceEmail invoiceEmail, RequestOptions inputOptions);

        //PAYMENT
        Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, RequestOptions inputOptions);
        Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, RequestOptions inputOptions);
        Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, RequestOptions inputOptions);
        Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, RequestOptions inputOptions);

        Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, List<string> controlPluginNames, Dictionary<string, string> pluginProperties, RequestOptions inputOptions);

        //PAYMENT METHOD
        PaymentMethod GetPaymentMethod(Guid paymentMethodId, RequestOptions inputOptions, bool withPluginInfo = false,
            AuditLevel auditLevel = AuditLevel.NONE);
        PaymentMethods GetPaymentMethodsForAccount(Guid accountId, RequestOptions inputOptions, Dictionary<string, string> pluginProperties = null, bool withPluginInfo = false, AuditLevel auditLevel = AuditLevel.NONE);

        PaymentMethod CreatePaymentMethod(PaymentMethod paymentMethod, RequestOptions inputOptions);
        void DeletePaymentMethod(Guid paymentMethodId, RequestOptions inputOptions, bool deleteDefault = false, bool forceDeleteDefault = false);
        void UpdateDefaultPaymentMethod(Guid accountId, Guid paymentMethodId, RequestOptions inputOptions);

        //TENANT
        Tenant CreateTenant(Tenant tenant, RequestOptions inputOptions, bool useGlobalDefault = true);
        void UnregisterCallbackNotificationForTenant(Guid tenantId, RequestOptions inputOptions);
        
        //TENANT KEY
        TenantKey RegisterCallBackNotificationForTenant(string callback, RequestOptions inputOptions);
        TenantKey GetCallbackNotificationForTenant(RequestOptions inputOptions);

        //SUBSCRIPTION
        Subscription GetSubscription(Guid subscriptionId, RequestOptions inputOptions);
        Subscription CreateSubscription(Subscription subscription, RequestOptions inputOptions, DateTime? requestedDate = null, bool? isMigrated = null);
    }
}