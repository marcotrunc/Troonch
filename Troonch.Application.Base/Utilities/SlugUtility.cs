
using Slugify;

namespace Troonch.Application.Base.Utilities;

public static class SlugUtility
{
    public static string GenerateSlug(string param)
    {
        var slugHelper = new SlugHelper();
        return slugHelper.GenerateSlug(param);
    }
}
