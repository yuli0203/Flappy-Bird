

public static class StringExtentions 
{
    public static bool IsNullOrEmpty(this string str)
    {
        return str == null || str == "";
    }
}
