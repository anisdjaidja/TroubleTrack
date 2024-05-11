using MongoDB.Bson.Serialization.Attributes;

namespace TroubleTrack.Model
{
    public class Project
    {
        [BsonId]
        public int ID { get; set; }
        public string projectName { get; set; }
        
        public List<BugReport> Errors = new List<BugReport>();

        #region Stats
        public TimeSpan AverageResolutionTime { get { return TimeSpan.FromHours(
                                                            Errors.Average(x => x.ResolutionTime?.TotalHours) ?? 0); } }

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
