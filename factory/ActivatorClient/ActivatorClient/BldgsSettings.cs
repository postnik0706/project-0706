using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ActivatorClient
{
    public class BuildingSettings
    {
        public int BUildingID { get; set; }
        public Uri URI { get; set; }
        public TimeSpan PollingInterval { get; set; }
    }

    class Buildings : IEnumerable<BuildingSettings>
    {
        private List<BuildingSettings> FSettings = new List<BuildingSettings>();
        private static Buildings pBuildings;
        private const string BLDS_DATABASE_FILENAME = "bldgs.settings";

        public IEnumerator<BuildingSettings> GetEnumerator()
        {
            return FSettings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return FSettings.GetEnumerator();
        }

        public static Buildings Get
        {
            get
            {
                if (pBuildings == null)
                    pBuildings = new Buildings();
                return pBuildings;
            }
        }

        static void Settings()
        {
            // It was decided to put the buildings database to the MyDocuments folder
            // of the current user, since it enforces some level of access limitation

            string file = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + BLDS_DATABASE_FILENAME;
            foreach (var item in File.ReadAllLines(file))
	        {
		        //Utilities.ShowInColor("
	        }
            //pBldgsSettings = new BldgsSettings();
        }
    }
}
