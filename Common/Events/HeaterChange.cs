﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events {

    public class HeaterChange : IEventPayload {

        public int Index { get; set; }

        public bool IsEngaged { get; set; }
    }

    
}
