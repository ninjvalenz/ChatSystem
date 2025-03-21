using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Command
{
    public class AssignChatCommand
    {
        public Guid SessionId { get; set; }

        public Guid AgentId { get; set; }
    }
}
