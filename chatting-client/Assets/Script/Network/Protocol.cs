using System;
using System.Collections.Generic;

namespace Protocol {

    public class JsonPacket {
        public string Code { get; set; }
        public IPayload Payload { get; set; }
    }

}