Tech Stack
  -.NET Core → Backend framework
  -EF Core → ORM for database operations
  -Kafka → Message broker for event-driven communication
  -Microservices → Distributed architecture

Required Installations
  -Kafka & Zookeeper (for message brokering)
  -.NET Core SDK (for running microservices)
  -SQL Server (for database storage)
  
How to run the project: 
Start zookeeper

cd C:\Kafka
bin\windows\zookeeper-server-start.bat config\zookeeper.properties

Start Kafka
cd C:\kafka
bin\windows\kafka-server-start.bat config\server.properties

Create a topic (this is a one time step!)
cd c:\kafka
bin\windows\kafka-topics.bat --create --topic chat.created --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1

Run all microservices
dotnet run --project ChatAPI
dotnet run --project QueueService
dotnet run --project AgentService
dotnet run --project PollingService

API endpoints:
POST /api/chat/create -> when user initiate a chat session
GET /api/queue/pending  -> this checks if there's any pending chat sessions
GET /api/agent/assigned -> this checks any chat session that already has been assigned to an agent
GET /api/polling/active -> this checks if there's any active chat sessions
PUT api/chat/close -> this will close a chat session 

**Workflow:**
_**User Creates a Chat Session**_
POST /api/chat/create
Data is stored in:
ChatSessions table → Tracks chat details.
ChatQueue table → Tracks pending chats before assignment.
Overflow Handling:
If queue size is full, check office hours.
If during office hours → Assign chat to overflow team.
If not → Reject chat creation with a "Queue Full" response.

**System Checks Pending Chats**__
GET /api/queue/pending
Retrieves all chat sessions in ChatQueue where Processed = false.

**User Actively Chats**__
GET /api/polling/active
Checks if session is still active.
If no response in 3 polling attempts, chat is marked inactive.

**Chat Session is Closed**__
PUT /api/chat/close
Marks chat as IsActive = false in ChatSessions.
Agent's current chat count is reduced.
Agent is now available for new assignments.

**Edge Cases & Handling**
Queue is full	-> Assigns overflow agents (if office hours) or rejects chat
Agent shift ends -> Agent finishes current chats but does not get new ones
User stops responding	-> After 3 missed polls, chat is marked inactive
Agent limit reached	-> Chat stays in queue until an agent is free



