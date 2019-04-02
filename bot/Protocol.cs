using System.Collections.Generic;

namespace Protocol {

    public class JsonPacket {
        public int ID { get; set; }
        public string User { get; set; }
        public List<int> Infomations { get; set; }
    }

}