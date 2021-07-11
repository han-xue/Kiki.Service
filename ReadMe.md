# Kiki courier service
A console app used for delivery cost and time estimation with offer. Calculation is provided by kiki.courierService.api. offer codes are stored in database

## Run the application from dev environment
* Deploy the database. Roundhouse is used for database versioning. If you haven't install RoundHouse, install it by running command "cinst roundhouse". Run CreateLocalDevDatabase.ps1 to generate the database. 
* Host the API. Either deploy it to a hosted environment, or for testing purpose, run it from visual studio
* Run the console app. The app is setted up to using the api hosted at https://localhost:44312, to use a different host, please change the value in the appsettings.json

## Potential improvement
* The console app is implemented to only read user input line by line. Could add function for reading for files
* The logic for delivery time estimation could be changed to reduce average delivery time per package.
* Could add functionality for adding new offer code. at the moment the only way is by adding a new sql script.
