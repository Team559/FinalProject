# FinalProject

This project implements the REST and SOAP services for medico RFID.
To run the application::

SOFTWARE: Visual studio code, mysql, .net 3.1, postman

SERVICES:

1. The executable code is in rfidProject directory.
2. open the project in Visual Studio Code.
3. run the following line in terminal :
  dotnet run -p ./rfidProject.csproj --urls "http://0.0.0.0:4881"
4. your application will be hosted on localhost at port 4881.

DATABASE:

1. Install PHPMYADMIN on local.
2. It will host sql on localhost:3306
3. http://localhost:3306/phpmyadmin/
4. once database is up and running, import the data from attached medical_surgeries.sql file.

WROKING OF application:

1. from POSTMAN: 
   http://hostname:4881/surgeries :: this will return the list of the surgies in JSON Format.
   
   http://hostname:4881/Service.asmx :: This is soap services, which will return the list of staff members in XML FORMAT.
