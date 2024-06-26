﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker_IheartRadio.Utils
{
    internal class Variables
    {
        public static int Total;
        public static int Hits = 0;
        public static int HitsPremiun = 0;
        public static int HitsFree = 0;
        public static int Invalids = 0;
        public static int Errors = 0;
        public static int Retries = 0;
        public static int Checked = 0;
        public static int AccIndex = 0;

        public static List<string> Proxies = new List<string>();
        public static string ProxyType = "";
        public static int ProxyIndex = 0;
        public static int ProxyTotal = 0;

        public static List<string> Combos = new List<string>();

        public static int Stop = 0;
        public static int CPM = 0;
        public static int CPMAux = 0;
        public static int Threads = 0;

        public static Random random = new Random();

        public static string GetLatestHit { get; set; }
        public static string GetComboName { get; set; }
        public static string CheckerTitle { get; set; }
    }
}
