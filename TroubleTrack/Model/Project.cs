using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace TroubleTrack.Model
{
    public class Project
    {
        [BsonId]
        public int ID { get; set; }
        public string projectName { get; set; }
        [IgnoreDataMember]
        public List<BugReport> Errors = new List<BugReport>();

        #region Stats
        [BsonIgnore]
        public TimeSpan AverageResolutionTime { get { return TimeSpan.FromHours(
                                                            Errors.Average(x => x.ResolutionTime?.TotalHours) ?? 0); } }
        [BsonIgnore]
        public Dictionary<string, int> BugDistribution {
            get {
                var result = new Dictionary<string, int>();
                foreach (var bug in Errors)
                {
                    if (result.ContainsKey(bug.Type))
                        result[bug.Type]++;
                    else
                        result.Add(bug.Type, 1);
                }
                return result;
            } 
        }
        #endregion
    }
}
