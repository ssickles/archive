using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using IdentityStream.DataAccess;
using IdentityStream.Server.Data;
using IdentityStream.Logging;

namespace LoadLotsOfData
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<AuthenticationTemplateData> templates = (from atd in (IEnumerable<AuthenticationTemplateData>)RepositoryFactory.AuthenticationTemplateRepository.GetAllAuthenticationTemplates()
                                                           where atd.AuthenticationUnitCode != "P1"
                                                           select atd).ToList();

            Random rndGroup = new Random();
            Random rndTemplate = new Random();
            Guid adminId = Guid.Empty;

            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine("Adding Identity " + i);
                IdentityData identity = new IdentityData()
                {
                    CountryCode = "US",
                    FirstName = "Scott" + i,
                    LastName = "Sickles" + i,
                    FullName = "Scott" + i + " Sickles" + i,
                    IdentityCode = i % 10 == 0 ? "E" : "C",
                    SourceId = i.ToString(),
                    GroupId = rndGroup.Next(26) + 1
                };
                identity.Uid = RepositoryFactory.IdentityRepository.Add(identity);
                Audit.AuditEvent(Audit.EventTypes.AddIdentity, string.Format("Identity {0} has been added.", identity.Uid), Guid.Empty);

                LoginData login1 = new LoginData()
                {
                    IdentityId = identity.Uid,
                    ApplicationCode = "T24",
                    LoginName = "AUTHOR",
                    Password = "123456",
                    RoleCode = "T24USER",
                    T24Id = i.ToString()
                };
                login1.Id = RepositoryFactory.LoginRepository.Add(login1);
                Audit.AuditEvent(Audit.EventTypes.AddLogin, string.Format("Login {0} has been added for Identity {1}.", login1.Id, identity.Uid), Guid.Empty);

                LoginData login2 = new LoginData()
                {
                    IdentityId = identity.Uid,
                    ApplicationCode = "LA",
                    LoginName = "system",
                    Password = "sa",
                    RoleCode = "LAUSER",
                    T24Id = i.ToString()
                };
                login2.Id = RepositoryFactory.LoginRepository.Add(login2);
                Audit.AuditEvent(Audit.EventTypes.AddLogin, string.Format("Login {0} has been added for Identity {1}.", login2.Id, identity.Uid), Guid.Empty);

                EnrollmentData enroll1 = new EnrollmentData()
                {
                    IdentityId = identity.Uid,
                    AdministratorId = adminId,
                    AuthenticationTypeCode = "FP"
                };
                enroll1.Uid = RepositoryFactory.EnrollmentRepository.Add(enroll1);
                Audit.AuditEvent(Audit.EventTypes.AddEnrollment, string.Format("Enrollment {0} has been added for Identity {1}.", enroll1.Uid, identity.Uid), Guid.Empty);
                //now add the templates to go along with the enrollment
                List<int> temps1 = new List<int>();
                for (int j = 0; j < 10; j++)
                {
                    int tempIndex;
                    do
                    {
                        tempIndex = rndTemplate.Next(templates.Count - 1);
                    } while (temps1.Contains(tempIndex));
                    temps1.Add(tempIndex);
                    AuthenticationTemplateData temp = new AuthenticationTemplateData()
                    {
                        AuthenticationUnitCode = templates[tempIndex].AuthenticationUnitCode,
                        EnrollmentId = enroll1.Uid,
                        Score = templates[tempIndex].Score,
                        Template = templates[tempIndex].Template
                    };
                    temp.Uid = RepositoryFactory.AuthenticationTemplateRepository.Add(temp);
                    Audit.AuditEvent(Audit.EventTypes.AddAuthenticationTemplate, string.Format("Authentication Template {0} has been added for Enrollment {1}.", temp.Uid, enroll1.Uid), Guid.Empty);
                }

                if (i % 5 == 0)
                {
                    EnrollmentData enroll2 = new EnrollmentData()
                    {
                        IdentityId = identity.Uid,
                        AdministratorId = adminId,
                        AuthenticationTypeCode = "FP"
                    };
                    enroll2.Uid = RepositoryFactory.EnrollmentRepository.Add(enroll2);
                    Audit.AuditEvent(Audit.EventTypes.AddEnrollment, string.Format("Enrollment {0} has been added for Identity {1}.", enroll2.Uid, identity.Uid), Guid.Empty);
                    //now add the templates to go along with the enrollment
                    List<int> temps2 = new List<int>();
                    for (int j = 0; j < 10; j++)
                    {
                        int tempIndex;
                        do
                        {
                            tempIndex = rndTemplate.Next(templates.Count - 1);
                        } while (temps2.Contains(tempIndex));
                        temps2.Add(tempIndex);
                        AuthenticationTemplateData temp = new AuthenticationTemplateData()
                        {
                            AuthenticationUnitCode = templates[tempIndex].AuthenticationUnitCode,
                            EnrollmentId = enroll2.Uid,
                            Score = templates[tempIndex].Score,
                            Template = templates[tempIndex].Template
                        };
                        temp.Uid = RepositoryFactory.AuthenticationTemplateRepository.Add(temp);
                        Audit.AuditEvent(Audit.EventTypes.AddAuthenticationTemplate, string.Format("Authentication Template {0} has been added for Enrollment {1}.", temp.Uid, enroll2.Uid), Guid.Empty);
                    }
                }

                //every fifth identity added becomes the enrollment officer for the next 5
                if (i % 5 == 0) adminId = identity.Uid;
            }
        }
    }
}
