# TroubleTrack 🐞 API DOCS 📄
The TroubleTrack API provides tools, and resources that enable you to report, update and track errors related to frontend development of your projects.
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
Response
```
{
    "errorCount": 2,
    "trend": null,
    "averageResolutionTime": "14:50:12.1870000"
}
```
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/91255255-53a6-411f-ac8c-5727616aea76)

### Get project statistics ```/api/projects/{projectID}```
Response
```
{
    "id": 0,
    "projectName": "MyWebProject",
    "averageResolutionTime": "1.20:30:36.5610000",
    "bugDistribution": {
        "Performance": 1,
        "Buttons": 1
    }
}
```
![image](https://github.com/anisdjaidja/TroubleTrack/assets/58264397/74dae48f-9059-42d5-bda7-639de1e6c734)


## Disclaimer: this is a test project and not developed with production in mind. For any further questions regarding under the hood and api architechture please contact the author.

# Author
Anis Djaidja

IT engineer

anisdjaidja1@gmail.com

