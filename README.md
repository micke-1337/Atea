# Atea

### Technical assignments: ###

### First task ###

Must use:
- Azure Function (Cloud/Local)
- Azure Storage (Cloud /Local storage emulator)
a. Table
b. Blob
- .Net Core 6

Achieve:
- Every minute, fetch data from https://api.publicapis.org/random?auth=null and store success/failure attempt log in the table and full payload in the blob.
- Create a GET API call to list all logs for the specific time period (from/to)
- Create a GET API call to fetch a payload from blob for the specific log entry
- Publish code on GitHub (public)

### Second task ###

Must use:
- ASP.NET CORE MVC (6)
- C#
- JavaScript

Achieve:

Using any public weather API receive data (country, city, temperature, clouds, wind speed) from at least 10 cities in 5 countries 
with periodical update 1/min,
store this data in the database
and show the 2 graphs:
- min temperature (Country\City\Temperature\Last update time)
- highest wind speed (Country\City\Wind Speed\Last update time)
- temperature & wind speed trend for last 2 hours on click for both previous graphs
