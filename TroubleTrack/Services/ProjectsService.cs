using MongoDB.Driver;
using TroubleTrack.Database;
using TroubleTrack.Model;

namespace TroubleTrack.Services
{
    public class ProjectsService
    {
        public MongoClient DBClient;
        public IMongoDatabase mongoDB;
        public IMongoCollection<Project> ProjectCollection;

        public ProjectsService(MongoDatabase databaseConnector)
        {
            while (true)
            {
                if (databaseConnector.Connected)
                    break;
            }
            DBClient = databaseConnector.DBclient;
            mongoDB = DBClient.GetDatabase("TroubleTrackDB");
            ProjectCollection = mongoDB.GetCollection<Project>("Projects");
        }

        #region QUERIES
        public async Task<List<Project>> GetAllProjects()
        {
            var all = await ProjectCollection.FindAsync(_ => true);
            return all.ToList();
        }
        public async Task<List<BugReport>> GetAllErrors()
        {
            var projects = await GetAllProjects();
            List<BugReport> bugReports = new();
            foreach (var project in projects)
            {
                bugReports.AddRange(project.Errors);
            }
            return bugReports;
        }
        public async Task<Project?> GetProjectByID(int id)
        {
            var result = await ProjectCollection.FindAsync<Project>(x => x.ID == id);
            return result.FirstOrDefault();
        }
        public async Task<BugReport?> GetErrorByID(int projectId, int id)
        {
            var proj = await GetProjectByID(projectId);
            if (proj == null)
                return null;
            return proj.Errors.Where(e => e.ID == id).FirstOrDefault();
        }
        #endregion

        public async Task<Project?> INSERT_PROJECT(Project? project)
        {
            if (project == null)
                return null;
            try
            {
                var all = ProjectCollection.Find(_ => true).ToList();
                int? maxid = all.Max(u => (int?)u.ID);
                int newID = maxid ?? -1;
                project.ID = newID + 1;
                await ProjectCollection.InsertOneAsync(project);
                return project;
            }
            catch
            {
                return null;
            }
        }
        public async Task<BugReport?> INSERT_ERROR(int projectID, BugReport report, int custom_id = -1)
        {
            if (report == null)
                return null;
            var filter = Builders<Project>
            .Filter
            .Eq(a => a.ID, projectID);
            var update = Builders<Project>
            .Update
               .Push(a => a.Errors, report);
            try
            {
                var project = await GetProjectByID(projectID);
                int? maxid = project.Errors.Max(br => (int?)br.ID);
                int newID = maxid ?? -1;
                report.ID = newID + 1;
                if (custom_id > -1) { report.ID = custom_id; }
                report.ProjectID = projectID;
                
                project.Errors.Add(report);
                await ProjectCollection.FindOneAndUpdateAsync(filter, update);
                return report;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Project?> DELETE_ERROR(int projectID, int errorID)
        {
            var filter = Builders<Project>
            .Filter
            .Eq(a => a.ID, projectID);

            var Deletionfilter = Builders<BugReport>
            .Filter
            .Eq(a => a.ID, errorID);

            var update = Builders<Project>
            .Update
               .PullFilter(a => a.Errors, Deletionfilter);
            try
            {
                return await ProjectCollection.FindOneAndUpdateAsync(filter, update);
            }
            catch
            {
                return null;
            }
        }

        public async Task<BugReport?> UPDATE_ERROR(int projectID, int errorID, BugReport report)
        {
            //ensure correct id's when updating
            report.ID = errorID;
            report.ProjectID = projectID;

            try
            {
                if (await DELETE_ERROR(projectID, errorID) == null)
                    return null;
                return await INSERT_ERROR(projectID, report, errorID);
            }
            catch
            {
                return null;
            }
        }
    }
}
