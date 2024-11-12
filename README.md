# ContentWorkflowDNNModule

A streamlined example of a DNN module with content workflow integration.

This module offers a straightforward implementation of a tab content workflow, facilitating content creation, editing, approval, and publishing within the DNN Platform.

## Current State

[DNN](https://github.com/dnnsoftware/Dnn.Platform) comes with a Content Workflow API that allows developers to integrate their modules with the DNN Platform's content workflow system.

Currently, the core DNN Text/HTML module does not support the DNN Content Workflow API. It has a rudimentary, custom workflow implementation that is planned to be replaced with a proper one in the future.

[2sxc](https://github.com/2sic/2sxc) is an open-source module that has supported the DNN Content Workflow for many years. It is a fantastic building element for any DNN website and is recognized by many web agencies that build web solutions for their clients. Developers can use 2sxc code to learn how the DNN Content Workflow is implemented and gain insights into its APIs, code structure, and useful comments.

The goal of this module is to provide a more streamlined example of how to integrate a DNN module with the Content Workflow API. The drawback of this example is that it is too simple for production use as it lacks features like caching, error handling, etc.

## How to Support Content Workflow API in a DNN Module

### Preconditions

A fully functional DNN module with a business controller class.

### Steps

1. **Detect if TabWorkflow is enabled in the DNN Platform.**
2. **Detect if the UI is in *Edit* mode.**
3. **Update the Model to support versioning:**
   - `int Version`
   - `bool IsPublished`
4. **Modify the repository to store draft version instances:**
   - The latest version instance can be used as the published version instance.
5. **[Optional] Implement version rollback support:**
   - The repository should preserve older published version instances.
6. **Notify TabWorkflow on changes in the module:**
   - Use `ITabChangeTracker.TrackModuleModification` to track changes when a module is modified on a page.
7. **Implement `IVersionable` in the DNN module controller (businessControllerClass).**

### `IVersionable` Interface

```cs
namespace DotNetNuke.Entities.Modules
{
    /// <summary>
    /// This interface allow the page to interact with the module to delete/rollback or publish a specific version.
    /// The module that wants to support page versioning needs to implement it in the Business controller.
    /// </summary>
    public interface IVersionable
    {
        /// <summary>This method deletes a concrete version of the module.</summary>
        /// <param name="moduleId">ModuleId.</param>
        /// <param name="version">Version number.</param>
        void DeleteVersion(int moduleId, int version);

        /// <summary>This method performs a rollback of a concrete version of the module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <param name="version">Version number that need to be rollback.</param>
        /// <returns>New version number created after the rollback process.</returns>
        int RollBackVersion(int moduleId, int version);

        /// <summary>This method publishes a version of the module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <param name="version">Version number.</param>
        void PublishVersion(int moduleId, int version);

        /// <summary>This method returns the version number of the current published module version.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <returns>Version number of the current published content version.</returns>
        int GetPublishedVersion(int moduleId);

        /// <summary>This method returns the latest version number of the current module.</summary>
        /// <param name="moduleId">Module Id.</param>
        /// <returns>Version number of the current published content version.</returns>
        int GetLatestVersion(int moduleId);
    }
}
```






### References:

- [[Enhancement]: Integration of Content Workflow API #6115](https://github.com/dnnsoftware/Dnn.Platform/issues/6115)
- [2sxc source](https://github.com/2sic/2sxc)

### TODO:
- [ ] Add more detailed documentation
- [ ] Add more detailed comments

