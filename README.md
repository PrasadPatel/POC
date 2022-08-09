*POC : Create a asp.net core project to consume external api endpoint (https://jsonplaceholder.typicode.com/todos) 
with HttpVerbs(get, post, delete, patch). Use logging frameworks(Serilog/NLog) for logging.

*Usecase:
-Suppose we have to perform operations on data received from multiple
api endpoints and expose our own endpoints

*What to expect from solution:
-.Net 6 framework
-Global usings
-Options pattern
-Repository pattern
-Serilog
-Request logging middleware
-Exception/global error handling middleware
-Healthchecks with UI
-Swagger UI

*Swagger : default
*Healthchecks url : ~/healthchecks-ui
*Serilog : "./bin/log.txt"

*Sonar report can be accessible on 
http://192.168.0.86:9000/dashboard?id=SCS-POC

***Future Scope:
-Authentication can be added

