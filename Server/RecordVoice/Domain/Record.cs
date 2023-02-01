using System;

namespace RecordVoice.Domain
{
    public class Record
    {
        public Guid Id { get; set; }
        public string IP { get; set; }
        public bool ApplicationOpened {get; set;}
        public bool Connected { get; set; }
        public bool Recording { get; set; }
    }
}