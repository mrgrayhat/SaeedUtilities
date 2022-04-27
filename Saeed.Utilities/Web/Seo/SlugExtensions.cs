using DNTPersianUtils.Core;

namespace Saeed.Utilities.Web.Seo
{
    public static class SlugExtensions
    {

        public static string GenerateSlug(this string input)
        {
            return input
                .Replace(" ", "-")
                .ApplyCorrectYeKe();
        }


    }
}
