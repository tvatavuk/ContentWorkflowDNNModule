using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;
using System.Linq;

namespace ToSic.Modules.ContentWorkflowDNNModule.Components;

internal class ContentTypeService
{
    private readonly IContentTypeController _contentTypeController = new ContentTypeController();

    public int ContentTypeId => (_contentTypeId == Null.NullInteger)
        ? _contentTypeId = GetContentType()?.ContentTypeId ?? CreateContentTypeId()
        : _contentTypeId;
    private int _contentTypeId = Null.NullInteger;

    private ContentType GetContentType()
        => _contentTypeController.GetContentTypes().FirstOrDefault(ct => ct.ContentType == Constants.ContentTypeName);

    private int CreateContentTypeId()
        => _contentTypeController.AddContentType(new ContentType { ContentType = Constants.ContentTypeName });
}