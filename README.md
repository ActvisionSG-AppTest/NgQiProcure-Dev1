# ASP.NET ZERO

This repository is configured and used for AspNet Zero Team's development. 
It has been modified for Procurement System project.

# Angular and Node version

This project is using angular CLI version 8.3.23. Node version 14.9.0. If you are using a higher version you may try run it and resolve the dependencies.

# Steps to run in development environment
## Last Modified: 1 Sep 2020

1. Download all files ( you may use git clone or download method)

2. Use Visual Studio Code editor to open the Angular directory.

3. Open up terminal and type Yarn install. Note on the dependency errors and resolve them individually.

4. Type npm start

5. Create a database in SQL Server. You may download the latest version of SQL Server express.
Create a database called "Ng_QiProcure_Dev". Restore the database from the back up file in SQLDB folder as attached in the project.

6. Use Microsoft Visual Studio Professional 2019 and goto aspnet-core directory, select QiProcureDemo.All solution.

7 Press "F5" to run the project (You may use any internet browser). A "Swagger" webpage will be launched. Login as "admin". Password is 123qwe .

8. Launch a new tab and go to the following URL:
http://localhost/4200

9. Login as "admin". Password is 123qwe .






