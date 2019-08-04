namespace XSing.Core.db.models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using DNS.Protocol;

    public class Record
    {
        [Key]
        public Guid UID { get; set; }
        public RecordType Type { get; set; }
        public string Domain { get; set; }
        public string Value { get; set; }
    }
}