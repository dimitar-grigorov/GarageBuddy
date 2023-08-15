namespace GarageBuddy.Common.Core.Enums
{
    public static class ReadOnlyOptionExtensions
    {
        public static bool AsBoolean(this ReadOnlyOption option)
        {
            return option == ReadOnlyOption.ReadOnly;
        }
    }
}
