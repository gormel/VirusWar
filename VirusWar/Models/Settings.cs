using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace VirusWar.Models
{
    public static class Settings
    {
        public static string DevServerRoot => ConfigurationManager.AppSettings["DevServerRoot"];
    }
}