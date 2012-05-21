using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ActivatorClient
{
    public class BuildingSettings
    {
        public string BuildingID { get; set; }
        public Uri URI { get; set; }
        public int PollingInterval { get; set; }            // Seconds
        public string Secret { get; set; }

        public BuildingSettings(string StringLine)
        {
            string[] param = StringLine.Split( new char[] {'|'} );
            
            BuildingID = param[0];
            URI = new Uri(param[1]);
            PollingInterval = Int16.Parse(param[2]);
            Secret = param[3];
        }
    }

    /*
     Buildings file location: ~\My Documents\Buildings.settings
    */
    class Buildings
    {
        private Dictionary<string, BuildingSettings> FSettings = new Dictionary<string, BuildingSettings>();
        private static Buildings pBuildings;
        private const string BLDS_DATABASE_FILENAME = "Buildings.settings";

        public static Buildings Get
        {
            get
            {
                if (pBuildings == null)
                    pBuildings = new Buildings();
                return pBuildings;
            }
        }

        public static Dictionary<string, BuildingSettings> Settings
        {
            get
            {
                return Get.FSettings;
            }
        }

        public static void Load()
        {
            string file = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + '\\'
                + BLDS_DATABASE_FILENAME;
            foreach (var item in File.ReadAllLines(file))
            {
                BuildingSettings b = new BuildingSettings(item);
                Get.FSettings.Add(b.BuildingID, new BuildingSettings(item));
            }
        }
    }
}
