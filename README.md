# Snakes&Ladders API
by Evyatar Bitton


### Assignment requirements
1. Your web API should expose the following:
    * Add a new player:
        - Trigger a background process that starts rolling the dice (1-6) and move the player across the board accordingly.
        - The player will continue to play automatically until the game ends (i.e. the user reached/exceeded the last tile).
    * Get player status:
        - Return the player status, which can be Unrecognized / During game / Finished.
        - Return the score (which is the total dice thrown count).
        - Return an indication if the player is ranked #1 in the total score board
2. Implementation:
    * All data can be persisted in memory.
    * The API should be exposed and consumed using Swagger
    * The board data structure should be defined in the web server, implementation method is flexible. You can assume the it doesn’t change during a game.
    * The board should contain ladders and snakes:
        - When the player lands on a snakes head, he is transported to its tail.
        - When the player lands on a ladder’s bottom, he is transported to its top.
    * Each game doesn’t affect other games (e.g. players can be on the same tile)

### Additional assignments:
Add your own MySql DB:

    a. Create a new EF repository for games. Interface and models
    b. Define the entity to include the id, game uuid, startedAt(timestamp), player name, score, duration, state
    c. Define methods:
        AddGame(GameEntity game)
        EditGame(GameEntity game)
        DeleteGame(uuid)
        GetGameByUuid(uuid)
        GetGameByFilterOptions => Player name, isActive here use method Filter(Expression<Func<GameEntity, bool>> predicate)
    d. run migration to init the DB with the games table
Modify your API layer as needed to expose the new abilities.

Unit tests: Add unit tests for the services layer.

### Current Board Game
![Current Game Board](CurrentGameBoard.PNG)

Row and Column numbering starts from 0 and goes up to 6 (including!) = final cell coordinates are {6,6}

### Other design notes:
1. Movement on Even rows is done from left to right. Movement on Odd rows is done from right to left.
2. When reaching the end of a row, taking one step up (this up movement costs a step!)
3. Board game is currently constant and is created in the Boardgame class. This may be modified in the future, for example to read these settings from a file

### Architecture:
Project is separated into several parts:
1. Controllers: Define the available REST API of the game.
2. GameLogic: HostedServices that implement and play the actual game logic in the background.
3. Repositories: Access layer between the controller and the game logic. 
4. Models: The basic data structures that are used.
5. Setup files: Configurations, Program & Startup files (usual ASP.Net core API files that setup the web api, Swagger and so on).

The project is using a MySql DB that is created code-first through Entity Framework.