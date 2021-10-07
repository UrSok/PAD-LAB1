using System;

namespace PAD.LAB1.Shared.Models.Payload
{
    public class PayloadMessage
    {
        public DateTime DateTime { get; set; }
        public string Text { get; set; }

        public PayloadMessage()
        {
            DateTime = DateTime.Now;
        }
    }
}
