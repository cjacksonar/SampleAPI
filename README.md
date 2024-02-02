Visual Studio 2022 17.8 is required for for .NET 8. 
Steps to use this sample application:
  1. Open Visual Studio 2022 and Clone Repository https://github.com/cjacksonar/SampleAPI
  2. Rebuild the entire solution
  3. Right-click SampleAPI.Database and select Publish, edit Target database connection, enter Database name, and click Publish to create the database as shown below.
     ![image](https://github.com/cjacksonar/SampleAPI/assets/34042711/ddc35ecc-654a-44d0-b170-ea47f52ee8a3)
  4. Change the connection string as necessary in SampleAPI/Classes/Globals.cs to match the created database connection string as shown below.
     ![image](https://github.com/cjacksonar/SampleAPI/assets/34042711/1d5b8a84-9467-44cb-8a9d-f82ee3166638)
  5. Select SampleAPI as startup project, select IIS Express from Visual Studio menu to run the project.
     ![image](https://github.com/cjacksonar/SampleAPI/assets/34042711/e6cb74a8-d756-4e8e-a089-5f7054dde210)
  6. Open Authentication and enter 1.0 in version, and copy sample request to request body  as shown below.
     ![image](https://github.com/cjacksonar/SampleAPI/assets/34042711/60d2fa55-7634-45d4-97e4-8fad2355d1d3)
   7. Click Execute. It should check the credentials in the database, which contains sample data, and return a token as shown below.
      ![image](https://github.com/cjacksonar/SampleAPI/assets/34042711/634fac41-5157-420c-bf79-26d4d54bedc2)
   8. Copy the token using the clipboard icon next to Download button as shown below.
      ![image](https://github.com/cjacksonar/SampleAPI/assets/34042711/dccdcae2-a4ce-4504-99cf-05663a81ab1c)
   9. Click Authorize button, paste the token, click Authorize, then Close.

You should now be able to perform the API actions now that you are authorized with the Authentication token.
Using Select a Definition, select 2.0. which allows you to view a list of states without authorization.
