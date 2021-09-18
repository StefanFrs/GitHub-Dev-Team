using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHubSearchWebApp.Tests
{
    public class StartupTest
    {

        [Fact]
        public void CheckConversionToEfConnectionString()
        {
            //Assum
            string databaseUrl = "postgres://oyultvtdhnbkbi:5977ff0cf95d6104df51ab40a89d458552842ac8c1dc89564f78d3b4ad2c2277@ec2-34-227-120-94.compute-1.amazonaws.com:5432/dad7qhvjs5nlv6";

            //Act
            string convertedConnectionString = Startup.ConvertConnectionString(databaseUrl);

            //Assert
            Assert.Equal("Database=dad7qhvjs5nlv6; Host=ec2-34-227-120-94.compute-1.amazonaws.com; Port=5432; User Id=oyultvtdhnbkbi; Password=5977ff0cf95d6104df51ab40a89d458552842ac8c1dc89564f78d3b4ad2c2277; SSL Mode=Require; Trust Server Certificate=true;", convertedConnectionString);
        }
    }
}
