using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtobufPacking
{
    public class ModuleVo
    {
        public int id;
        public string name;
        public string comment;
        public ProtocolVo[] protocols;
    }
}