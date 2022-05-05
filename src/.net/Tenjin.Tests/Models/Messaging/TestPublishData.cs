using System;

namespace Tenjin.Tests.Models.Messaging
{
    public record TestPublishData
    {
        public int Value1 { get; set; }
        public Guid Value2 { get; set; }
    }
}
