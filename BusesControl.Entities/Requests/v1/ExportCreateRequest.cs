using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1
{
    public class ExportCreateRequest
    {
        public ExportTypeEnum Type { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }
    }
}
