using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Modules;
using System;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Personalization;
using DotNetNuke.Entities.Tabs.TabVersions;

namespace ToSic.Modules.ContentWorkflowDNNModule.Components
{
    internal class VersionableController
    {
        private readonly ContentTypeService _contentTypeService = new();
        private readonly IContentController _contentController = Util.GetContentController();


        #region IVersionable implementation

        /// <summary>This method deletes a concrete version of the module.</summary>
        /// <param name="moduleId">ModuleId.</param>
        /// <param name="version">Version number.</param>
        public void DeleteVersion(int moduleId, int version)
        {
            var deleteContentItem = _contentController.GetContentItem(version);
            if (deleteContentItem.IsPublished())
            {
                var publish = GetItems(moduleId).OrderByDescending(i => i.GetVersion()).FirstOrDefault(i => i.GetVersion() != version);
                PublishVersion(moduleId, publish.GetVersion());
            }
            _contentController.DeleteContentItem(deleteContentItem);
        }


        /// <summary>This method performs a rollback of a concrete version of the module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <param name="version">Version number that need to be rollback.</param>
        /// <returns>New version number created after the rollback process.</returns>
        public int RollBackVersion(int moduleId, int version)
        {
            PublishVersion(moduleId, version);
            return version;

            // Other strategy is to make a copy of rollback version and publish new copy
        }


        /// <summary>This method publishes a version of the module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <param name="version">Version number.</param>
        public void PublishVersion(int moduleId, int version)
        {
            // Unpublish all other versions
            foreach (var publishedContentItem in GetItems(moduleId).Where(i => i.Metadata[Constants.Published] == Convert.ToString(true) && i.ContentItemId != version))
            {
                publishedContentItem.Metadata[Constants.Published] = Convert.ToString(false);
                _contentController.UpdateContentItem(publishedContentItem);
            }

            // Publish selected version
            var contentItem = _contentController.GetContentItem(version);
            if (contentItem.IsPublished()) return;

            contentItem.Metadata[Constants.Published] = Convert.ToString(true);
            _contentController.UpdateContentItem(contentItem);
        }


        /// <summary>This method returns the version of the current published module version.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <returns>ContentItem current published content version.</returns>
        public ContentItem GetPublishedVersion(int moduleId)
            => GetItems(moduleId)
            .Where(i => i.IsPublished())
            .OrderByDescending(i => i.GetVersion())
            .FirstOrDefault();


        /// <summary>This method returns the latest version of the current module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <returns>ContentItem last content version.</returns>
        public ContentItem GetLatestVersion(int moduleId)
            => GetItems(moduleId)
            .OrderByDescending(i => i.GetVersion())
            .FirstOrDefault();

        #endregion


        public ContentItem CreateItem(ModuleInfo moduleInfo, string content)
        {
            var contentItem = new ContentItem
            { 
                ContentTypeId = _contentTypeService.ContentTypeId,
                TabID = moduleInfo.TabID,
                ModuleID = moduleInfo.ModuleID,
                StateID = ContentWorkflowStateId,
                Content = content
            };
            var newContentItemId = _contentController.AddContentItem(contentItem);
            
            moduleInfo.ModuleVersion = newContentItemId; // in this most simple implementation ModuleVersion is the same as ContentItemId

            TrackModuleModification(moduleInfo);

            return _contentController.GetContentItem(newContentItemId);
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var deleteContentItem = _contentController.GetContentItem(itemId);
            if (deleteContentItem.IsPublished())
            {
                var publish = GetItems(moduleId).OrderByDescending(i => i.GetVersion()).FirstOrDefault(i => i.GetVersion() != itemId);
                PublishVersion(moduleId, publish.GetVersion());
            }
            _contentController.DeleteContentItem(deleteContentItem);
        }

        public ContentItem GetItem(int moduleId) 
            => (!IsWorkflowEnabled || IsEditMode)
                ? GetLatestVersion(moduleId)
                : GetPublishedVersion(moduleId);

        internal bool IsEditMode => Personalization.GetUserMode() == PortalSettings.Mode.Edit;
        private bool IsVersioningEnabled => TabVersionSettings.Instance.IsVersioningEnabled(PortalSettings.Current.PortalId, TabController.CurrentPage.TabID);
        internal bool IsWorkflowEnabled => this.IsVersioningEnabled && TabWorkflowSettings.Instance.IsWorkflowEnabled(PortalSettings.Current.PortalId, TabController.CurrentPage.TabID);

        private IQueryable<ContentItem> GetItems(int moduleId)
            => _contentController.GetContentItemsByModuleId(moduleId).Where(ci => ci.ContentTypeId == _contentTypeService.ContentTypeId);

        public void UpdateItem(ModuleInfo moduleInfo, ContentItem contentItem)
        {
            contentItem.StateID = ContentWorkflowStateId;
            _contentController.UpdateContentItem(contentItem);
            moduleInfo.ModuleVersion = contentItem.GetVersion();
            TrackModuleModification(moduleInfo);
        }

        private void TrackModuleModification(ModuleInfo moduleInfo) => TabChangeTracker.Instance.TrackModuleModification(moduleInfo, moduleInfo.ModuleVersion, UserController.Instance.GetCurrentUserInfo().UserID);

        private int ContentWorkflowStateId => Null.NullInteger;

        private int Version(int version) => version < 1 ? 1 : version;
    }
}
