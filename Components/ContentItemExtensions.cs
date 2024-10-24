using DotNetNuke.Entities.Content;

namespace ToSic.Modules.ContentWorkflowDNNModule.Components
{
    public static class ContentItemExtensions
    {
        // Extension method to get the Version as an integer from ContentKey
        public static int GetVersion(this ContentItem contentItem)
            => contentItem.ContentItemId;

        // Extension method to get the Published status as a boolean from Metadata
        public static bool IsPublished(this ContentItem contentItem)
        {
            var publishedValue = contentItem.Metadata.Get(Constants.Published);
            if (string.IsNullOrEmpty(publishedValue)) return false;
            return bool.TryParse(publishedValue, out var published) && published;
        }
    }
}