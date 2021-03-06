﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fhir.Anonymizer.DataFactoryTool
{
    public class ActivityInputData
    {
        public string SourceStorageConnectionString { get; set; }
        public string DestinationStorageConnectionString { get; set; }
        public string SourceContainerName { get; set; }
        public string DestinationContainerName { get; set; }
        public string SourceFolderPath { get; set; }
        public string DestinationFolderPath { get; set; }
    }
}
