using System.Configuration;

namespace KillBill.Client.Net
{
    public static class KbConfig
    {
        public static string ServerUrl = "http://127.0.0.1:8080";
        public static string ApiKey = "admin";
        public static string ApiSecret = "password";

        public const string API_PREFIX = "";
        public const string API_VERSION = "/1.0";
        public const string API_POSTFIX = "/kb";

        public static string AUDIT_OPTION_CREATED_BY = "__AUDIT_OPTION_CREATED_BY";
        public static string AUDIT_OPTION_REASON = "__AUDIT_OPTION_REASON";
        public static string AUDIT_OPTION_COMMENT = "__AUDIT_OPTION_COMMENT";
        public static string TENANT_OPTION_API_KEY = "__TENANT_OPTION_API_KEY";
        public static string TENANT_OPTION_API_SECRET = "__TENANT_OPTION_API_SECRET";


        public static string PREFIX = API_PREFIX + API_VERSION + API_POSTFIX;
        /*
         * Resource paths
        */
        public static string ACCOUNTS = "accounts";
        public static string ACCOUNTS_PATH = PREFIX + "/" + ACCOUNTS;
        public static string BUNDLES = "bundles";
        public static string BUNDLES_PATH = PREFIX + "/" + BUNDLES;
        public static string CATALOG = "catalog";
        public static string CATALOG_PATH = PREFIX + "/" + CATALOG;
        public static string CHARGEBACKS = "chargebacks";
        public static string CHARGES = "charges";
        public static string CREDITS = "credits";
        public static string CREDITS_PATH = PREFIX + "/" + CREDITS;
        public static string CUSTOM_FIELDS = "customFields";
        public static string CUSTOM_FIELDS_PATH = PREFIX + "/" + CUSTOM_FIELDS;
        public static string EMAILS = "emails";
        public static string EMAIL_NOTIFICATIONS = "emailNotifications";
        public static string FORM = "form";
        public static string HOSTED = "hosted";
        public static string INVOICES = "invoices";
        public static string INVOICES_PATH = PREFIX + "/" + INVOICES;
        public static string DRY_RUN = "dryRun";
        public static string INVOICE_PAYMENTS = "invoicePayments";
        public static string INVOICE_PAYMENTS_PATH = PREFIX + "/" + INVOICE_PAYMENTS;
        public static string NOTIFICATION = "notification";
        public static string OVERDUE = "overdue";
        public static string PAGINATION = "pagination";
        public static string PAYMENTS = "payments";
        public static string PAYMENTS_PATH = PREFIX + "/" + PAYMENTS;
        public static string PAYMENT_GATEWAYS = "paymentGateways";
        public static string PAYMENT_GATEWAYS_PATH = PREFIX + "/" + PAYMENT_GATEWAYS;
        public static string PAYMENT_METHODS = "paymentMethods";
        public static string PAYMENT_METHODS_DEFAULT_PATH_POSTFIX = "setDefault";
        public static string PAYMENT_METHODS_PATH = PREFIX + "/" + PAYMENT_METHODS;
        public static string PLUGINS = "plugins";
        public static string PLUGINS_PATH = "/" + PLUGINS;
        public static string REFUNDS = "refunds";
        public static string REGISTER_NOTIFICATION_CALLBACK = "registerNotificationCallback";
        public static string LEGACY_REGISTER_NOTIFICATION_CALLBACK = "REGISTER_NOTIFICATION_CALLBACK";
        public static string SEARCH = "search";
        public static string SECURITY = "security";
        public static string SECURITY_PATH = PREFIX + "/" + SECURITY;
        public static string SUBSCRIPTIONS = "subscriptions";
        public static string SUBSCRIPTIONS_PATH = PREFIX + "/" + SUBSCRIPTIONS;
        public static string TAGS = "tags";
        public static string TAGS_PATH = PREFIX + "/" + TAGS;
        public static string TAG_DEFINITIONS = "tagDefinitions";
        public static string TAG_DEFINITIONS_PATH = PREFIX + "/" + TAG_DEFINITIONS;
        public static string TENANTS = "tenants";
        public static string TENANTS_PATH = PREFIX + "/" + TENANTS;
        public static string TIMELINE = "timeline";

        /*
         * Query parameters
         */
        public const string QUERY_ACCOUNT_ID = "accountId";
        public const string QUERY_ACCOUNT_WITH_BALANCE = "accountWithBalance";
        public const string QUERY_ACCOUNT_WITH_BALANCE_AND_CBA = "accountWithBalanceAndCBA";
        public const string QUERY_AUDIT = "audit";
        public const string QUERY_BILLING_POLICY = "billingPolicy";
        public const string QUERY_CALL_COMPLETION = "callCompletion";
        public const string QUERY_CALL_TIMEOUT = "callTimeoutSec";
        public const string QUERY_CUSTOM_FIELDS = "customFieldList";
        public const string QUERY_DELETE_DEFAULT_PM_WITH_AUTO_PAY_OFF = "deleteDefaultPmWithAutoPayOff";
        public const string QUERY_DRY_RUN = "dryRun";
        public const string QUERY_ENTITLEMENT_POLICY = "entitlementPolicy";
        public const string QUERY_EXTERNAL_KEY = "externalKey";
        public const string QUERY_INVOICE_WITH_ITEMS = "withItems";
        public const string QUERY_NOTIFICATION_CALLBACK = "cb";
        public const string QUERY_PAYMENT_EXTERNAL = "externalPayment";
        public const string QUERY_PAYMENT_METHOD_IS_DEFAULT = "isDefault";
        public const string QUERY_PAYMENT_PLUGIN_NAME = "pluginName";
        public const string QUERY_PAY_INVOICE = "payInvoice";
        public const string QUERY_PLUGIN_PROPERTY = "pluginProperty";
        public const string QUERY_REQUESTED_DT = "requestedDate";
        public const string QUERY_SEARCH_LIMIT = "limit";
        public const string QUERY_SEARCH_OFFSET = "offset";
        public const string QUERY_TAGS = "tagList";
        public const string QUERY_TARGET_DATE = "targetDate";
        public const string QUERY_UNPAID_INVOICES_ONLY = "unpaidInvoicesOnly";
        public const string QUERY_PAYMENT_METHOD_PLUGIN_NAME = "pluginName";
        public const string QUERY_WITH_PLUGIN_INFO = "withPluginInfo";

       /*
        * Metadata Additional headers
        */
        public static string HDR_CREATED_BY = "X-Killbill-CreatedBy";
        public static string HDR_REASON = "X-Killbill-Reason";
        public static string HDR_COMMENT = "X-Killbill-Comment";
        public static string HDR_PAGINATION_CURRENT_OFFSET = "X-Killbill-Pagination-CurrentOffset";
        public static string HDR_PAGINATION_NEXT_OFFSET = "X-Killbill-Pagination-NextOffset";
        public static string HDR_PAGINATION_TOTAL_NB_RECORDS = "X-Killbill-Pagination-TotalNbRecords";
        public static string HDR_PAGINATION_MAX_NB_RECORDS = "X-Killbill-Pagination-MaxNbRecords";
        public static string HDR_PAGINATION_NEXT_PAGE_URI = "X-Killbill-Pagination-NextPageUri";

        static KbConfig()
        {
            ServerUrl = ConfigurationManager.AppSettings["kb.api.url"] ?? ServerUrl;
            ApiKey = ConfigurationManager.AppSettings["kb.api.key"] ?? ApiKey;
            ApiSecret = ConfigurationManager.AppSettings["kb.api.secret"] ?? ApiSecret;
        }
    }
}