﻿using System;
using System.Collections.Generic;
using ModAssistantPlus.Pages;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModAssistantPlus
{
    public class Mod
    {
        public string name;
        public string version;
        public string gameVersion;
        public string _id;
        public string authorId;
        public string uploadedDate;
        public string updatedDate;
        public Author author;
        public string description;
        public string link;
        public string category;
        public DownloadLink[] downloads;
        public bool required;
        public Dependency[] dependencies;
        public List<Mod> Dependents = new List<Mod>();
        public Mods.ModListItem ListItem;

        public class Author
        {
            public string _id;
            public string username;
            public string lastLogin;
        }

        public class DownloadLink
        {
            public string type;
            public string url;
            public FileHashes[] hashMd5;
        }

        public class FileHashes
        {
            public string hash;
            public string file;
        }

        public class Dependency
        {
            public string name;
            public string _id;
            public Mod Mod;
        }
    }
}
