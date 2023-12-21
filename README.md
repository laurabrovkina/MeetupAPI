# MeetupAPI
* This is a project about add/update/delete meetups API
* There are several branches on that repo, each one has a small project
## Database
* As an improvement, the script and/or migration should be introduced to add Roles automatically to the database, 
the SQL query should be run manually for now:
```
INSERT INTO 
[MeetupDb].[dbo].[Roles]
VALUES ('User'),('Moderator'),('Admin')
```
## Swagger
* Swagger documentation is set up and could be accessed using url `https://localhost:5001/swagger/index.html`
* This should be moved to the launching page in future as an improvement

## Branches
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

* `started-with-mediatr` - this branch shows how to migrate to MediatR and use handlers, separating the project into vertical slices. If the functionality is just like that it might not have a lot of value. But with MediatR there are some interesting features become available where
we can set up behavior, and emit events using MediatR build-in functionality. To be continued...

## Dockerization
* The original idea was found on github, [Mssql-docker-initialization-demo](https://github.com/tometchy/Mssql-docker-initialization-demo):

Find out more details in the [article](https://www.softwaredeveloper.blog/initialize-mssql-in-docker-container).

To build image on your machine:
```
docker build -t db-meetup . --no-cache
``` 
`--no-cache` is especially useful when the image has already existed on your machine and you need to update it with new changes.

Then, run the container:
```
docker run -p 14033:1433 -d db-meetup
```

Next, you can establish a connection to the SQL Server using SQL Server Management Studio (SSMS). When doing so from your local machine, enter `localhost,14033` as the Server name. Opt for SQL Server Authentication and input the "sa" user along with the corresponding password specified in the Dockerfile.

## Health Checks
* There is a good [article](https://rmauro.dev/adding-health-checks-ui/) how to set up UI client for a health check. 
* Meetup project needs to be sure that db connection is up and running correctly.
* To make a call to health check,
```
https://localhost:5001/healthz
```
Then, you would see a more detailed response compare to a very standard one:
```
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0037176",
  "entries": {
    "Database": {
      "data": { },
      "duration": "00:00:00.0024150",
      "status": "Healthy",
      "tags": [ ]
    }
  }
}
```
* If you prefer to access the Health Check UI, there is a link for that:
```
https://localhost:5001/healthchecks-ui#/healthchecks
```
The Health Check UI page looks like that:
![Screenshot of tv](Img/health-check-ui-screenshot.jpg)

* Polling time between check is set to 15 seconds
* 60 entries is maximum history of checks
* API requests concurrency is set to 1

## Unit tests
#### Summary
The `UpdateUserValidatorTests` class contains unit tests for the `UpdateUserValidator` class. It tests the validation logic for the `RoleId` and `Email` properties of the `UpdateUserDto` class.

#### Example Usage
```csharp
// Arrange
var validator = new UpdateUserValidator(meetupContext);
var updateUserDto = new UpdateUserDto { RoleId = 111 }; // Assuming 111 is an invalid RoleId

// Act
var result = validator.TestValidate(updateUserDto);

// Assert
result.ShouldHaveValidationErrorFor(x => x.RoleId)
    .WithErrorMessage("Role doesn't exist");
```

### Code Analysis
#### Main functionalities
- The `ShouldHaveValidationError_WhenRoleIdDoesNotExist` method tests if a validation error is thrown when the `RoleId` property of the `UpdateUserDto` object does not exist in the database.
- The `ShouldHaveValidationError_WhenEmailDoesNotExist` method tests if a validation error is thrown when the `Email` property of the `UpdateUserDto` object does not exist in the database.
- The `ShouldNotHaveValidationError_WhenRoleIdAndEmailExist` method tests if no validation error is thrown when both the `RoleId` and `Email` properties of the `UpdateUserDto` object exist in the database.
___
#### Methods
- `ShouldHaveValidationError_WhenRoleIdDoesNotExist`: This method tests if a validation error is thrown when the `RoleId` property of the `UpdateUserDto` object does not exist in the database. It creates an instance of the `UpdateUserValidator` class, sets an invalid `RoleId` in the `UpdateUserDto` object, and validates it using the `TestValidate` method. It then asserts that a validation error is returned with the expected error message.
- `ShouldHaveValidationError_WhenEmailDoesNotExist`: This method tests if a validation error is thrown when the `Email` property of the `UpdateUserDto` object does not exist in the database. It follows a similar approach as the previous method, but sets an invalid email address in the `UpdateUserDto` object.
- `ShouldNotHaveValidationError_WhenRoleIdAndEmailExist`: This method tests if no validation error is thrown when both the `RoleId` and `Email` properties of the `UpdateUserDto` object exist in the database. It follows a similar approach as the previous methods, but sets valid values for both properties.
___
#### Fields
- `_fixture`: An instance of the `DatabaseFixture` class used for setting up the database context.
- `meetupContext`: The `MeetupContext` object used for database operations.
___
