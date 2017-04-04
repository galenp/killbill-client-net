namespace KillBill.Client.Net.Model
{ 
    /**
    * Specifies how Subscription cancellation or plan change should operate
    */
    public enum BillingActionPolicy
    {
        /**
        * The cancellation or Plan change effectiveDate will occur at the start of the current invoiced service
        * period and that will trigger a full credit.
        */
         START_OF_TERM,

        /**
         * The cancellation or {@code Plan} change effectiveDate will occur at the end of the current invoiced service
         * period, and that will not trigger any proration and credit.
         */
        END_OF_TERM,

        /**
         * The cancellation or Plan change effectiveDate will occur at the requestedDate
         */
        IMMEDIATE,
        ILLEGAL
    }
}