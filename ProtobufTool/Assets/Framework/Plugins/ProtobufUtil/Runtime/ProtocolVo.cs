using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtobufPacking
{
    public class ProtocolVo
    {
        public int insideId;
        public int protocolId;
        public int[] clientTypes;
        public int[] senderTypes;
        public int[] receiverTypes;
        public string name;
        public string comment;
        public FieldVo[] fields;
    }
}