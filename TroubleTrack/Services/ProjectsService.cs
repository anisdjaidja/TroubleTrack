using MongoDB.Driver;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Linq;
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
        public async Task<BugReport?> INSERT_ERROR(int projectID, BugReport report)
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
                int newID = maxid ?? 1;
                report.ID = newID + 1;
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

        public async Task<Project?> DELETE(int projectID, int errorID)
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
    }
}
