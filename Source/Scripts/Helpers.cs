
public static class Helpers
{
    public static string NameAndType(INameable inameable)
    {
        return $"{inameable.ObjectName} {inameable.GetType()}";
    }
}
