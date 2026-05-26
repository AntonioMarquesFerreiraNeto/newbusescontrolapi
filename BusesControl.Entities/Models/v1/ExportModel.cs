using BusesControl.Entities.Enums.v1;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1
{
    public class ExportModel
    {
        public Guid Id { get; set; }
        public ExportTypeEnum Type { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }
        public ExportStatusEnum Status { get; set; }
        [MaxLength(500)]
        public string? Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExportedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        [MaxLength(500)]
        public string? ErrorMessage { get; set; }
    }
}
