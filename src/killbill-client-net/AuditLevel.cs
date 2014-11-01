namespace KillBill.Client.Net
{
    public enum AuditLevel
    {
        // All audits
        FULL,
        // Initial inserts only
        MINIMAL,
        // No audit
        NONE
    }
}