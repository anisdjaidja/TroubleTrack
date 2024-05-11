using MongoDB.Bson.Serialization.Attributes;

namespace TroubleTrack.Model
{
    public class BugReport
    {
        [BsonId]
        public int ID { get; set; }

        public int ProjectID { get; set; }

        public string BugName { get { return $"Error{ProjectID}-{ID}"; } }

        public DateTime InitialReportDate { get; set; }

        public DateTime? ResolutionDate { get; set; } = null;

        public TimeSpan? ResolutionTime => ResolutionDate - InitialReportDate;

        public string Type { get; set; }

        public int Severity { get; set; } = 0;

        public string? Summary { get; set; }

        public bool IsFixed { get; set;}

    }
}
