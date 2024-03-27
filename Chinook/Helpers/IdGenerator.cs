namespace Chinook.Helpers
{
    public static class IdGenerator
    {
        public static long GenerateUniqueId()
        {
            return DateTime.UtcNow.Ticks;
        }
    }
}
