using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Modules;

namespace ToSic.Modules.ContentWorkflowDNNModule.Components
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for ContentWorkflowDNNModule
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class FeatureController : IUpgradeable, IVersionable
    {
        private readonly VersionableController _controller = new();

        // feel free to remove any interfaces that you don't wish to use
        // (requires that you also update the .dnn manifest file)

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        public string UpgradeModule(string version)
        {
            try
            {
                switch (version)
                {
                    case "00.00.01":
                        // run your custom code here
                        return "success";
                    default:
                        return "success";
                }
            }
            catch
            {
                return "failure";
            }
        }


        /// <summary>This method deletes a concrete version of the module.</summary>
        /// <param name="moduleId">ModuleId.</param>
        /// <param name="version">Version number.</param>
        public void DeleteVersion(int moduleId, int version)
            => _controller.DeleteVersion(moduleId, version);


        /// <summary>This method performs a rollback of a concrete version of the module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <param name="version">Version number that need to be rollback.</param>
        /// <returns>New version number created after the rollback process.</returns>
        public int RollBackVersion(int moduleId, int version)
            => _controller.RollBackVersion(moduleId, version);


        /// <summary>This method publishes a version of the module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <param name="version">Version number.</param>
        public void PublishVersion(int moduleId, int version)
            => _controller.PublishVersion(moduleId, version);


        /// <summary>This method returns the version number of the current published module version.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <returns>Version number of the current published content version.</returns>
        public int GetPublishedVersion(int moduleId)
            => _controller.GetPublishedVersion(moduleId).GetVersion();


        /// <summary>This method returns the latest version number of the current module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <returns>Version number of the current published content version.</returns>
        public int GetLatestVersion(int moduleId)
            => _controller.GetLatestVersion(moduleId).GetVersion();


        #endregion
    }
}