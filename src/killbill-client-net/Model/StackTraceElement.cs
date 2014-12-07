namespace KillBill.Client.Net.Model
{
    public class StackTraceElement
    {
        public string ClassName { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public string MethodName { get; set; }
        public bool NativeMethod { get; set; }
    }
}