using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDisciples.Backend
{
    public class ProjectsDataService
    {
        /// <summary>
        /// Gets the list of projects from the database
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ProjectDetails> GetProjects()
        {
            try
            {
                using (UniblueLabsEntities entities = new UniblueLabsEntities())
                {

                    return (from x in entities.Projects
                            select new ProjectDetails
                            {
                                ProjectId = x.id,
                                Name = x.Name,
                                Description = x.Description,
                                Link = x.Link,
                                Developer = x.Developers.Name,
                                DeveloperPic = x.Developers.Avatar,
                                DeveloperId = x.Developers.id
                            }).ToList(); ;
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// Saves a project in the database
        /// </summary>
        /// <param name="project">The project detail to save</param>
        /// <returns>Returns true if the insert was successful. False if it failed</returns>
        public static bool AddNewProject(ProjectDetails project)
        {
            try
            {
                using (UniblueLabsEntities entities = new UniblueLabsEntities())
                {
                    Projects newProject = new Projects
                    {
                        Name = project.Name,
                        Description = project.Description,
                        Link = project.Link,
                        Developers = entities.Developers.Where(x => x.id == project.DeveloperId).FirstOrDefault(),
                    };
                    entities.AddToProjects(newProject);
                    entities.SaveChanges();
                    project.ProjectId = newProject.id;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Saves a project in the database
        /// </summary>
        /// <param name="project">The project detail to save</param>
        /// <returns>Returns true if the insert was successful. False if it failed</returns>
        public static bool UpdateProject(ProjectDetails project)
        {
            try
            {
                using (UniblueLabsEntities entities = new UniblueLabsEntities())
                {
                    Projects projectToUpdate = entities.Projects.First(x => x.id == project.ProjectId);
                    if (projectToUpdate != null)
                    {
                        projectToUpdate.Name = project.Name;
                        projectToUpdate.Description = project.Description;
                        projectToUpdate.Link = project.Link;
                        projectToUpdate.Developers = entities.Developers.Where(x => x.id == project.DeveloperId).FirstOrDefault();
                    }
                    else
                        return false;

                    entities.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool DeleteProject(ProjectDetails projectDetails)
        {
            try
            {
                using (UniblueLabsEntities entities = new UniblueLabsEntities())
                {
                    var proj = entities.Projects.FirstOrDefault(x => x.id == projectDetails.ProjectId);
                    entities.DeleteObject(proj);
                    entities.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;

        }
    }
}
