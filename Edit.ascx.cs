using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using System;
using ToSic.Modules.ContentWorkflowDNNModule.Components;

namespace ToSic.Modules.ContentWorkflowDNNModule
{
    /// -----------------------------------------------------------------------------
    /// <summary>   
    /// The Edit class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from ContentWorkflowDNNModuleModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : ContentWorkflowDNNModuleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Implement your edit logic for your module
                if (!Page.IsPostBack)
                {
                    var controller = new VersionableController();
                    var contentItem = controller.GetItem(ModuleId);
                    txtDescription.Text = contentItem?.Content;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var controller = new VersionableController();
            var contentItem = controller.GetItem(ModuleId);
            if (contentItem == null || contentItem.IsPublished())
            {
                controller.CreateItem(ModuleConfiguration, txtDescription.Text.Trim());
            }
            else
            {
                contentItem.Content = txtDescription.Text.Trim();
                controller.UpdateItem(ModuleConfiguration, contentItem);
            }
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}