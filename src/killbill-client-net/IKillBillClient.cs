using System;
using System.Collections.Generic;
using KillBill.Client.Net.Model;

namespace KillBill.Client.Net
{
    public interface IKillBillClient
    {
        Account GetAccount(Guid accountId, bool withBalance = false, bool withCba = false);
        Account GetAccount(string externalKey, bool withBalance = false, bool withCba = false);
        Account CreateAccount(Account account, string createdBy, string reason, string comment, IDictionary<string, string> additionalHeaders = null);
        Account UpdateAccount(Account account, string createdBy, string reason, string comment, IDictionary<string, string> additionalHeaders = null);
        Accounts GetAccounts(long offset = 0L, long limit = 100L, AuditLevel auditLevel = AuditLevel.NONE);
        Payments GetPaymentsForAccount(Guid accountId, AuditLevel auditLevel = AuditLevel.NONE);
        InvoicePayments GetInvoicePaymentsForAccount(Guid accountId, AuditLevel auditLevel = AuditLevel.NONE);
        void AddEmailToAccount(AccountEmail email, string createdBy, string reason, string comment);
        void RemoveEmailFromAccount(AccountEmail email, string createdBy, string reason, string comment);
        AccountEmails GetEmailsForAccount(Guid accountId);
        AccountTimeline GetAccountTimeline(Guid accountId, AuditLevel auditLevel = AuditLevel.NONE);
        Bundle GetBundle(Guid bundleId);
        Bundle GetBundle(string externalKey);
        Bundle TransferBundle(Bundle bundle, string createdBy, string reason, string comment);
        Bundles GetAccountBundles(Guid accountId);
        Bundles GetBundles(long offset = 0L, long limit = 100L, AuditLevel auditLevel = AuditLevel.NONE);
        Bundles SearchBundles(string key, long offset = 0L, long limit = 100L, AuditLevel auditLevel = AuditLevel.NONE);

        /// <summary>
        /// Creates a credit against an account. This also creates a new invoice.
        /// </summary>
        /// <param name="credit">Credit Object</param>
        /// <returns>Processed credit object with populated invoice details.</returns>
        Credit CreateCredit(Credit credit, string createdBy, string reason, string comment);

        Credit GetCredit(Guid creditId, AuditLevel auditLevel);

        /// <summary>
        /// Triggers an invoice RUN!
        /// </summary>
        /// <remarks>Don't be fooled by the method name... this SHOULD NOT be used to create invoices. Invoices are created as a byproduct of other actions like 'Creating Credits', 'External Charges'</remarks>
        Invoice CreateInvoice(Guid accountId, Invoice invoice, DateTime futureDate, string createdBy, string reason,
            string comment);

        Invoice GetInvoice(int invoiceNumber, bool withItems = false, AuditLevel auditLevel = AuditLevel.NONE);
        Invoice GetInvoice(string invoiceIdOrNumber, bool withItems=false, AuditLevel auditLevel = AuditLevel.NONE);
        Invoice GetInvoiceByIdOrNumber(string invoiceIdOrNumber, bool withItems = false, AuditLevel auditLevel = AuditLevel.NONE);
        Invoices GetInvoicesForAccount(Guid accountId, bool withItems = false, bool unpaidOnly = false, AuditLevel auditLevel = AuditLevel.NONE);
        Invoices SearchInvoices(string key, long offset = 0L, long limit= 100L, AuditLevel auditLevel = AuditLevel.NONE);

        /// <summary>
        /// Executes an 'external charge' action... note if no InvoiceId is provided on each charge then the server will create a new invoice for the batch.
        /// </summary>
        /// <remarks>The currency on each charges needs to be the same as the currency on the referenced account.</remarks>
        /// <returns>List of processed charges with invoice references.</returns>
        List<InvoiceItem> CreateExternalCharge(IEnumerable<InvoiceItem> externalCharges, DateTime requestedDate,
            bool autoPay, string createdBy, string reason, string comment);

        InvoiceEmail GetEmailNotificationsForAccount(Guid accountId);

        void UpdateEmailNotificationsForAccount(InvoiceEmail invoiceEmail, string createdBy, string reason,
            string comment);

        Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, string createdBy, string reason, string comment);
        Payment CreatePayment(Guid accountId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, string createdBy, string reason, string comment);
        Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, string createdBy, string reason, string comment);
        Payment CreatePayment(Guid accountId, Guid? paymentMethodId, PaymentTransaction paymentTransaction, Dictionary<string, string> pluginProperties, string createdBy, string reason, string comment);

        PaymentMethod GetPaymentMethod(Guid paymentMethodId, bool withPluginInfo = false,
            AuditLevel auditLevel = AuditLevel.NONE);

        PaymentMethods GetPaymentMethodsForAccount(Guid accountId, bool withPluginInfo = false);

        PaymentMethod CreatePaymentMethod(PaymentMethod paymentMethod, string createdBy, string reason,
            string comment);

        void DeletePaymentMethod(Guid paymentMethodId, bool deleteDefault, string createdBy, string reason, string comment, bool forceDelete = false, bool forceDeleteDefault = false);

        Tenant CreateTenant(Tenant tenant, string createdBy, string reason, string comment);

        void DeleteCallbackNotificationForTenanr(Guid tenantId, string createdBy, string reason,
            string comment);

        TenantKey RegisterCallBackNotificationForTenant(string callback, string createdBy, string reason,
            string comment);

        TenantKey RetrieveRegisteredCallBacks();
        Subscription GetSubscription(Guid subscriptionId);
        Subscription CreateSubscription(Subscription subscription, string createdBy, string reason, string comment);
        Subscription CreateSubscription(Subscription subscription, DateTime? requestedDate, string createdBy, string reason, string comment);
    }
}