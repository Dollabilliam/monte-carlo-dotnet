# welcome to monte-carlo-dotnet! 

## Requirements
.NET 6 and C# 10

## How To Run
Clone this repo, open it in your IDE of choice, and then click "Run". 

A Swagger page should then open in your browser running on the port defined in` launchSettings.json` and allow you to interact with these endpoints:
* `POST`: `/simulation/start-simulation`
  * Request
  ```json
  {
    "T": 120,
    "S": 1000,
    "assets": [
        {
          "name": "Asset 1",
          "p0": 190.0,
          "m": "0.5%",
          "s": "10%"
        },
        {
          "name": "Asset 2",
          "p0": 100.0,
          "m": "0.2%",
          "s": "1%"
        },
        {
          "name": "Asset 3",
          "p0": 75.25,
          "m": "0.4%",
          "s": "8%"
        }
    ]
  }
  ```
  * Response
  ```json
  {
    "simulationIds": [
        "3b1c1277-e9e1-4d3b-8339-a9ca1d4af319_Asset 1",
        "557104d6-0a10-4708-be41-01f2c25e76db_Asset 2",
        "5d0e0586-c85d-4063-b974-208229e87fbd_Asset 3"
      ]
  }
  ```
* `GET` `/simulation/get-statuses`
  * Request
  `'https://localhost:7289/simulation/get-statuses?SimulationIds=3b1c1277-e9e1-4d3b-8339-a9ca1d4af319_Asset%201'`
  * Response
  ```json
  [
    {
      "simulationId": "3b1c1277-e9e1-4d3b-8339-a9ca1d4af319_Asset 1",
      "scenarioCount": 999
    }
  ]
  ```
* `GET` `/simulation/get-results`
  * Request
    `'https://localhost:7289/simulation/get-results?SimulationIds=3b1c1277-e9e1-4d3b-8339-a9ca1d4af319_Asset%201'`
  * Response
  ```json
  [
    {
      "simulationId": "3b1c1277-e9e1-4d3b-8339-a9ca1d4af319_Asset 1",
      "quantiles": {
          "firstQuantileReturn": 17.390310895879136,
          "fifthQuantileReturn": 21.148225095560502
      }
    }
  ]
  ```
The `launchSettings.json` file may be configured to your liking if you'd like to change aspects of how the program runs.

### Note

This is a "happy path" service, so please play don't try any edge cases as those most likely aren't handled

You're able to configure how faster the QueueWorker runs by configuring its delay `WorkerDelay` in `appsettings.json`. If nothing is provided, the worker will run every 10000 ms.

## Author's Note
The `QueueWorker`


## Further Improvements
* Introduce...
  * FluentValidation for validating requests sent to endpoints
  * NUnit for testing components
* Add some more logging perhaps
* Document interfaces, methods, etc
* Migrate toward scalable architecture as defined in diagram below
  * The REST service already communicates in this manner; this diagram is more about showing which components should be introduced to make it faster and scalable

![pefect-world-diagram](system-design.png "pefect-world-diagram")