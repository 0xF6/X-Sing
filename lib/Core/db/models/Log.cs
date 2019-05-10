namespace XSing.Core.db.models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        [Key]
        public Guid UID { get; set; }
        public string Entity { get; set; }
        public int LogLevel { get; set; }
        public DateTime timestamp { get; set; }
    }
}