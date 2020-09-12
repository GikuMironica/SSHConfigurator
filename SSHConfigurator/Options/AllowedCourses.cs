using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Options
{
    /// <summary>
    /// This is a configuration class containing the options related to the courses the user must be enrolled in.
    /// Can be injected with Dependency Injection in the required services.
    /// </summary>
    public class AllowedCourses
    {
        public List<string> CourseNames { get; set; }
    }
}
