# MeetupAPI
* This is a project about add/update/delete meetups API
* There are several branches on that repo, each one has a small project

#### Branches
* `ef-sp-migration` this is an example how to add stored procedures to your EF Migrations
There is a good [artical](https://clearmeasure.com/creating-stored-procs-in-ef-migrations/) to achieve that in your progect
Steps to add the migration:
1. Use the standard command
```
dotnet ef migrations add StoredProcs
```
2. Add the code to read `.sql` files and apply sql code to the `migrationBuilder` in the Up method
3. Make sure the `.sql` files are added as embedded resources
4. Update your database
```
dotnet ef database update
```
* `exception-without-throwing` it is another small project (ongoing) where instead of throwing exceptions, 
we would hadle bad state with ResultType (monadic transformations in C#).
This is [Original idea](https://www.youtube.com/watch?app=desktop&v=a1ye9eGTB98).