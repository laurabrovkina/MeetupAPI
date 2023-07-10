# MeetupAPI
* This is a project about add/update/delete meetups API
* There are several branches on that repo, each one has a small project
#### Database
* As an improvement, the script and/or migration should be introduced to add Roles automatically to the database, 
the SQL query should be run manually for now:
```
INSERT INTO 
[MeetupDb].[dbo].[Roles]
VALUES ('User'),('Moderator'),('Admin')
```
#### Swagger
* Swagger documentation is set up and could be accessed using url `https://localhost:5001/swagger/index.html`
* This should be moved to the launching page in future as an improvement

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
* `load-testing` branch includes 5 files with tests for API performance written using k6 tool (Grafana Labs):

1. `load_test.js`
2. `sample_test.js`
3. `soak_test.js`
4. `spike_test.js`
5. `stress_test.js`

There is a [link](https://k6.io/docs/get-started/installation/) how to install k6
1. Install the tool on your PC
2. Run the test with providing the file name, e.g.:
```
k6 run sample_test.js
```

The tool has a nice UI, there is a clear picture in a console window:
![Screenshot of a results after running sample_test.js](Img/k6-screenshot.jpg)

* `minimal-api` - this branch holds implementation of Miniumal API using FastEndpoints. 

The project file could be found in `MeetupAPI.MinimalApi` folder. Swagger documentation is set up and could be accessed using url `https://localhost:5000/swagger/index.html`.