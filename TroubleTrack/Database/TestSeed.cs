using System;
using System.Collections.ObjectModel;
using TroubleTrack.Model;

namespace TroubleTrack.Database
{
    public class TestSeed
    {
        public ObservableCollection<Project> Projects;
        public TestSeed()
        {
            Projects = new ObservableCollection<Project>();
            for (int i = 0; i < 2; i++) 
            {
                var proj = new Project
                {
                    ID = i,
                    projectName = $"Project n{i}",
                };
                for (int j = 0; j < 5; j++)
                {
                    var error = new BugReport
                    {
                        ID = j,
                        ProjectID = proj.ID,
                        Summary = Summaries[j],
                        Type = "FrontEnd",
                        InitialReportDate = DateTime.Now,
                        Changelog = { $"Inital reviewing of {Summaries[j]}", },
                        IsFixed = false,
                    };
                    proj.Errors.Add(error);
                }
                Projects.Add(proj);
            }
        }
        private List<string> Summaries = new()
        {
            "Problem wity sound", "Issue regarding button 1", "Issue regarding button 2", "Unstable mailing feature", "Text net visible",
            "About Page freezing",
        };
        
    }
}
