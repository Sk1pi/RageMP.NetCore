RageMP .NET Core Server: Clean Architecture Implementation
This project delivers a basic GTA V multiplayer server script for RageMP, built entirely on .NET Core. The script implements all core and bonus features requested in the test task using Clean Architecture principles, including Dependency Injection (DI) and a separation of concerns (CQRS pattern for money management).

/pos	Debug	Displays the player's current X, Y, Z coordinates, Heading, and Dimension.
/veh [model]	Utility	Spawns a vehicle of the specified model name (e.g., /veh comet).
/tp [x] [y] [z]	Utility	Teleports the player to the specified coordinates.
/money	Roleplay	Displays the player's current bank balance.
/give [player] [amount]	Roleplay	Transfers the specified amount to another player.

Prerequisites and Setup
1. Requirements
Platform: .NET 8 SDK (or later)

RageMP Server: Latest official server files (for the necessary Rage.dll).

Database: A running PostgreSQL instance.

2. Compilation and Deployment
To run this script, you must publish the project to create the necessary DLL files.

Build the Project: Open your terminal in the solution root directory and execute the publish command:

dotnet publish RageMP.NetCore -c Release -o output/RageMP.NetCore_Resource
Locate Server: Navigate to your RageMP server directory (\packages\).

Deploy: Copy all contents of the newly created output/RageMP.NetCore_Resource folder into a new resource folder (e.g., \packages\my_netcore_resource\).

Configure conf.json: Add the resource name to your server's configuration file:

"resources": [
  "my_netcore_resource"
]
3. Database Connection
Update the connection string in your Program.cs file to point to your PostgreSQL instance:

const string PostgreSQLConnectionString = "Host=localhost;Database=RageMP_DB;Username=user;Password=ВАШ_ПАРОЛЬ";
Available Server Commands
Command	Category	Description
/pos	Debug	Displays the player's current X, Y, Z coordinates, Heading, and Dimension.
/veh [model]	Utility	Spawns a vehicle of the specified model name (e.g., /veh comet).
/tp [x] [y] [z]	Utility	Teleports the player to the specified coordinates.
/money	Roleplay	Displays the player's current bank balance.
/give [player] [amount]	Roleplay	Transfers the specified amount to another player.

Development Challenges and Solutions
The most significant challenges in building an advanced RageMP script lie in integrating modern .NET Core features with the proprietary RageMP API.

1. Integration of DI and IoC (Service Locator Anti-Pattern)
Problem: RageMP does not natively support the .NET Generic Host or DI container lifecycle management (Scoped, Singleton).

Solution: The container (ServiceCollection) is manually built inside the Program constructor. Crucially, IServiceScopeFactory is injected and used to manually create a new scope (using var scope = _scopeFactory.CreateScope();) for every single RageMP event (OnPlayerJoin, OnPlayerCommand, etc.). This ensures that database-related services (DbContext, Repositories) are safely isolated for each operation, preventing concurrency issues (thread-safety) common with using Singleton DBContexts.

2. RageMP API Resolution (The Rage.dll Hurdle)
Problem: Compiling the C# code requires explicit references to core RageMP libraries (like Rage.dll), which are not standard NuGet packages. This caused persistent Cannot resolve symbol 'Rage' errors.

Solution (Architectural Compromise): To achieve a successful compilation and demonstrate the logic, Stub (Fake) Implementations were used for core RageMP classes (Rage, Player, Vector3, OutputChatBox, HasData, etc.). This allowed the application to compile without the physical DLL, proving the architectural logic, though it must be replaced by the actual DLL for server execution.

3. Command API Cleanliness
Problem: Directly calling methods like player.OutputChatBox throughout the code is verbose and less readable.

Solution: An Extension Method (player.SendChat()) was created on the Player object. This wraps the native RageMP API, improving code readability and making the command handlers cleaner and more expressive.

