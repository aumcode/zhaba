# Zhaba Issue Tracker

### Overview
Zhaba Issue Tracker is a tool which can be used to track bugs and feature requests during product development. At first is should be noted that Zhaba was developed as internally tool for our company, Agnicore Inc., but you can fully use it at your own business.  The tracker allows you to get rid of the use ofspreadsheets, sticky notes, and email to keep track of bugs, ideas, and requests. Now you can do all that things in just one uncomplicated, central hub. The tracker provides simple workflow and easy filtering. Zhaba allows you to view a list of issues, see individual activity from any day, or gather quick reports.


### Installation
1. Create some directory (e.g. C:\Zhaba) with subdirectories `logs` and `obj`.
2. Add system environment variable `ZHABA_ROOT` that refers to the directory from the step 1.
3. Create environment variables MYSQL_ADMIN_USERID and MYSQL_ADMIN_PASSWORD which contain your MySQL credentials.
4. Compile the project.
5. Run `MakeSQLScripts.cmd` and then `MakeZhabaDB.cmd` scripts which are located in `Source\Zhaba.DBAccess\Schema` directory.

After starting the application will be available at `http://localhost:8080` with default user `admin` with password `5HwxsG`. 
