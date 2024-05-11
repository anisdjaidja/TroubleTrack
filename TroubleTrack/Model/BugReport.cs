using Amazon.Runtime.Internal.Transform;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TroubleTrack.Model
{
    public class BugReport
    {
        [BsonId]
        [SwaggerSchema(ReadOnly = true)]
        public int ID { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public int ProjectID { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string BugName => $"Error {ProjectID}-{ID}";

        public DateTime InitialReportDate { get; set; }

        public string? Summary { get; set; }

        [BsonIgnoreIfDefault]
        public string Type { get; set; } = "General";

        [BsonIgnoreIfDefault]
        public int Severity { get; set; } = 0;

        [BsonIgnoreIfDefault]
        public bool IsFixed { get; set;} = false;
        [BsonIgnoreIfNull]
        [SwaggerSchema(Nullable = true)]
        public DateTime? ResolutionDate { get; set; } = null;
        [SwaggerSchema(ReadOnly = true, Nullable = true)]
        public TimeSpan? ResolutionTime { get { return ResolutionDate - InitialReportDate; } }
    }
}
