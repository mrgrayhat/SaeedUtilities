namespace Saeed.Utilities.Gaurds
{
    public class Guard : IGuardClause
    {
        public static IGuardClause Against { get; } = new Guard();
    }
}