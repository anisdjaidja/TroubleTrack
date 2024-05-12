# TroubleTrack 🐞 API DOCS 📄
The TroubleTrack API provides tools, and resources that enable you to report, update and track errors related to frontend development of your projects.
# Build
To build this project on Dotnet 8 core runtime you should make sure these required dependencies are listed in the *.cproj* file:
``` xml
    <PackageReference Include="Microsoft.AspNetCore.Authentication" Version="2.2.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.4" />
    <PackageReference Include="MongoDB.Driver" Version="2.25.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="6.5.0" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.5.1" />
```
If you are using VS Code, install these one by one in the terminal using this command ```nuget install <packageID | configFilePath> [options]```, consult officail docs : https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-install

If you are Using VS Studio, make sure you have the latest .net8 release and usually your IDE would auto install the listed packages on first build.

## Database
If you want to use MongoDB (the default databse for this api) make sure to update the *DbConfig.cs* file with the appropriate *ConnectionString*.
In case you prefere another database, consider upgrading to *EntityFramework Core* or making your own custom middleware for your database of choice and replace *MongoDatabase.cs* file.

Further more, if you intende to use this on production, start by introducing AudienceKey and Issuer(Provider) string and reference them in the JWT service configuration in *program.cs* file.
``` c#
public static class DbConfig
{
    public  const string DbConnectionString = "<YOUR CONNECTION STRING HERE>";
    public const string JwtKey = "<YOUR PRIVATE KEY HERE>";

    /// for production :
    /// Add Adience and Issiuer strings
}
```
### Cold Start behavior
The databse middleware in this api automatically reconnects to databse in case of downtime recovery, and subsecuently has a *Cold Start* behavior, read more :https://en.wikipedia.org/wiki/Cold_start_(computing)

In this case cold start is not a problem, but rather implemented as a mechanisme to avoid unessessary uptime since this is intented to be an internal API. 

![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/bfeff13a-d52b-4b93-8803-b1149b12accc)



#### Note : It is recommended to exclude the *DbConfig.cs* in the *.gitignore* file if you are forking this repo on *public*.

# Getting started

- The API only responds to HTTPS-secured communications. Any requests sent via HTTP return an HTTP 301 redirect to the corresponding HTTPS resources.

- The API returns request responses in JSON format. When an API request returns an error, it is sent in the JSON response as an error key.

- Some TroubleTrack API endpoints requires authentification and Authorised accounts are intenally managed, for full access please ask the maitenance team for a dedicated account.

## Authentication 🔐
- The TroubleTrack API uses JWT (Jason web token) for authentication.
You can generate a Token by using the Login endpoit

- For endpoints that require auth, you must include a Token in each request to the API as a Token Bearer (if you are using postman).

- If the provided user credentials are invalid you will receive an HTTP 401 Unauthorized response code.

### Login endpoint ```/api/projects/auth```
Request
``` json
Body:
{
  "email": "anis",
  "password": "123"
}
```
Response
``` json
Body:
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFuaXMiLCJuYmYiOjE3MTU0OTY0OTcsImV4cCI6MTcxNTUzOTY5NywiaWF0IjoxNzE1NDk2NDk3fQ.t2toJeI3ygLWbykJ2ugaDE7gfVpuMbXR7fCpWdx8IvE",
    "user": {
        "id": 0,
        "email": "anis",
        "password": "123"
    }
}
```
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/b7454bb7-0f88-49a8-8067-48fc8d1128d6)

## Endpoints ❇️

- These endpoints require authentification and can be accessed only by token bearers.
  
- An error HTTP 415 Unsupported media response means that the resquest body is not in json format or doesnt comply with data schema.
  
- An error HTTP 404 Not found response means that the specified route is not valid.

- An error HTTP 401 Unauthorized response means that you are not authorised to make such actions or your token has expired and you need to log in again.
  
- An HTTP 5xx response will almost never occure due to the internal database being auto-reconnected automatically.
  
- This API has a cold start behavior, meaning the very first request will take a brief time to reponde due to a fresh reconnection attempt to the database.
### Post project ```/api/projects```
Request
``` json
Body:
"Example project"
```
Response
``` json
Body:
{
    "id": 3,
    "projectName": "Example project",
    "averageResolutionTime": "00:00:00",
    "bugDistribution": {}
}
```

### Post Bug ```/api/projects/{projectID}/errors```
Request
``` json
URL : /api/Projects/1/errors
Body:
{
  "initialReportDate": "2024-05-12T07:23:21.193Z",
  "summary": "Issue with latency",
  "type": "Performance",
  "severity": 0,
  "isFixed": false,
  "resolutionDate": null,
  "resolutionTime": null
}
```
Response
``` json
Body:
{
    "id": 0,
    "projectID": 1,
    "bugName": "Error 1-0",
    "initialReportDate": "2024-05-12T07:23:21.193Z",
    "summary": "Issue with latency",
    "type": "Performance",
    "severity": 0,
    "isFixed": false,
    "resolutionDate": null,
    "resolutionTime": null
}
```
### Get project bugs ```/api/projects/{projectID}/errors```
Response
``` json
[
    {
        "id": 1,
        "projectID": 0,
        "bugName": "Error 0-1",
        "initialReportDate": "2024-05-11T20:12:52.423Z",
        "summary": "Issue with latency",
        "type": "Performance",
        "severity": 2,
        "isFixed": false,
        "resolutionDate": null,
        "resolutionTime": null
    },
    {
        "id": 0,
        "projectID": 0,
        "bugName": "Error 0-0",
        "initialReportDate": "2024-05-11T00:44:22.651Z",
        "summary": "Problem with button x",
        "type": "Buttons",
        "severity": 1,
        "isFixed": true,
        "resolutionDate": "2024-05-12T21:14:59.212Z",
        "resolutionTime": "1.20:30:36.5610000"
    }
]
```
### Get bug ```/api/projects/{projectID}/errors/{errorID}```
Response
``` json
{
    "id": 0,
    "projectID": 0,
    "bugName": "Error 0-0",
    "initialReportDate": "2024-05-11T00:44:22.651Z",
    "summary": "Problem with button x",
    "type": "Buttons",
    "severity": 1,
    "isFixed": true,
    "resolutionDate": "2024-05-12T21:14:59.212Z",
    "resolutionTime": "1.20:30:36.5610000"
}
```
### Put bug ```/api/projects/{projectID}/errors/{errorID}```
Request
``` json
URL : /api/Projects/1/errors/0
Body :
{
  "initialReportDate": "2024-05-12T07:28:29.866Z",
  "summary": "Issue with latency",
  "type": "Performance",
  "severity": 0,
  "isFixed": true,
  "resolutionDate": "2024-05-15T07:23:21.193Z"
}
}
```
Response
```
200 OK
error Error 1-0 status updated
```
Postman demo :
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/c9e5ed08-0a1b-4b64-8537-c81ad004d0bb)
Updated bug :
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/95577d61-da88-4e9d-8dcb-fb01f2b8cfd6)

### Delete bug ```/api/projects/{projectID}/errors/{errorID}```
Request
``` json
URL : /api/Projects/1/errors/1
}
```
Response
```
200 OK
error Error 1-0 status deleted
```

## Statistics 📉
- These endpoints dont require authentification and can be accessed by anyone

### Get global overview ```/api/projects```
#### Note: Error sevirity is designed to return the entire bugReport entites classified by severity. this is intended to truely help indentify most urgent errors to prioritize. Returning error count doesnt make much sense here.
Response
```
{
    "errorCount": 3,
    "trend": {
        "5/12/2024": 1,
        "5/11/2024": 2,
        "5/10/2024": 0,
        "5/9/2024": 0,
        "5/8/2024": 0
    },
    "critical": [
        {
            "id": 1,
            "projectID": 0,
            "bugName": "Error 0-1",
            "initialReportDate": "2024-05-11T20:12:52.423Z",
            "summary": "Issue with latency",
            "type": "Performance",
            "severity": 2,
            "isFixed": false,
            "resolutionDate": null,
            "resolutionTime": null
        }
    ],
    "major": [
        {
            "id": 0,
            "projectID": 0,
            "bugName": "Error 0-0",
            "initialReportDate": "2024-05-11T00:44:22.651Z",
            "summary": "Problem with button x",
            "type": "Buttons",
            "severity": 1,
            "isFixed": true,
            "resolutionDate": "2024-05-12T21:14:59.212Z",
            "resolutionTime": "1.20:30:36.5610000"
        }
    ],
    "minor": [
        {
            "id": 0,
            "projectID": 1,
            "bugName": "Error 1-0",
            "initialReportDate": "2024-05-12T07:28:29.866Z",
            "summary": "Issue with latency",
            "type": "Performance",
            "severity": 0,
            "isFixed": true,
            "resolutionDate": "2024-05-15T07:23:21.193Z",
            "resolutionTime": "2.23:54:51.3270000"
        }
    ],
    "averageResolutionTime": "1.05:06:21.9720000"
}
```
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/279eebed-6440-4b48-99ba-c53dcd9a6361)


### Get project statistics ```/api/projects/{projectID}```
Response
```
{
    "id": 1,
    "projectName": "MyWebProject1",
    "averageResolutionTime": "2.23:54:51.3270000",
    "bugDistribution": {
        "Performance": 1
    },
    "errorRate": 0,
    "responseTime": 0,
    "upTime": 0
}
```
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/6a4098e3-02c9-49f4-aca9-19541d1c4f1a)



## Disclaimer: this is a test project and not developed with production in mind. For any further questions regarding under the hood and api architechture please contact the author.

# Author
Anis Djaidja

IT engineer

anisdjaidja1@gmail.com

