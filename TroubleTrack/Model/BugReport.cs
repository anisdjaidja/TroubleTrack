using MongoDB.Bson.Serialization.Attributes;

namespace TroubleTrack.Model
{
    public class BugReport
    {
        public BugReport()
        {
            Changelog = new();
        }
        [BsonId]
        public int ID { get; set; }

        public int ProjectID { get; set; }

        public string BugName { get { return $"Error{ProjectID}-{ID}"; } }

        public DateTime InitialReportDate { get; set; }

        public string Type { get; set; }

        public string? Summary { get; set; }

        public List<string> Changelog { get; set; }

        public bool IsFixed { get; set; }
    }
}
