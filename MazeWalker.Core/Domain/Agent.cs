using System;

namespace MazeWalker.Core.Domain
{
    public class Agent
    {
        public Agent(int agentId, string agentName)
        {
            AgentId = agentId;
            AgentName = agentName ?? throw new ArgumentNullException(nameof(agentName));
        }

        public int AgentId { get; }
        public string AgentName { get; }
    }
}