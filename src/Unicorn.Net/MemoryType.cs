namespace Unicorn
{
    public enum MemoryType
    {
        Read = 16,
        Write,
        Fetch,

        ReadUnmapped,
        WriteUnmapped,
        FetchUnmapped,

        ReadProtected,
        WriteProceted,
        FetchProtected,

        ReadAfter
    }
}
