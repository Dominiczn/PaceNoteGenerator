using Pace_Note_Generator.Backend.Database;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Pace_Note_Generator
{
    public partial class App : Application
    {

        public App()
        {
            DataAccess.InitialiseDatabase();
        }
    }

}
