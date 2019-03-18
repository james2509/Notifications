# Notifications Microservice - Scenario
The Notifications microservice was built in Visual Studio 2017 and development database hosted on a SQL Azure database.

The Notifications microservice manages user notifications. It responds to system wide events and depending on the type of event it will generate a new user notification and store it in its store. 

When a user logs in, the user will fetch all of their notifications. A socket is opened between the notifications service and the user’s browser, which allows the notifications service to push new notifications to a user during their session.

# Setup
A blank database will need to created. 

The SQL server connection string needs to be updated in the appsettings.json file. The settings file should look like;
~~~~
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "SqlConnectionString": "YOUR_SQL_SERVER_CONNECTIOn_STRING",
  "AllowedHosts": "*"
}
~~~~

Once the connection string has been updated an EF Migration will need to be run. Open the solution up in Visual Studio and run Update-Database from the package manager console. 

This will also seed the following data:
* An appointment cancelled type is added to the NotificationsType table
* An appointment cancelled template is added to the NotificationsTemplate table
* Two test users are added to the NotificationUser tables, as follows:
  * james@test.com
  * natalia@test.com

# Calling the APIs
* GET https://host/api/notifications returns all notifcation
* GET https://host/api/notifications?userEmail=james@test.com returns all notifications for the user james@test.com
* POST https://host/api/notifications passing a payload in the following structure will add a notification and boradcast to the user if they have a socket connection:
~~~~
{
	"UserId": "james@test.com",
	"Data": {"AppointmentDateTime":"2020-04-17T17:24:01.5121422+00:00","OrganisationName":"Pinky Ponk","Reason":"Not working today"},
	"NotificationType": 1
}
~~~~


