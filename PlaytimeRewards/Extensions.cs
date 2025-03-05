namespace PlaytimeRewards;

public static class Extensions
{
    public static dynamic Choice(this Random rand, IEnumerable<dynamic> enumerable)
    {
        return enumerable.ElementAt(rand.Next(0, enumerable.Count()));
    }
}