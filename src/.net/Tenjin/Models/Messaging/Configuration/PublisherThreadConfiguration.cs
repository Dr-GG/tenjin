﻿using Tenjin.Enums.Messaging;

namespace Tenjin.Models.Messaging.Configuration
{
    public class PublisherThreadConfiguration
    {
        public PublisherThreadMode Mode { get; set; }
        public int? NumberOfThreads { get; set; }
    }
}
