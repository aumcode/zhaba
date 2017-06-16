# Zhaba Issue Tracker

The project is under development.

To run the aplication it is necessary:
1. Create some directory (e.g. C:\Zhaba) with subdirectories `logs` and `obj`.
2. Add system environment variable `ZHABA_ROOT` that refers to the directory from the step 1.
3. To install database after compilation of the solution you should run `MakeSQLScripts.cmd` and `MakeZhabaDB.cmd` scripts which are located in `Source\Zhaba.DBAccess\Schema` directory. During the DB instalaltion default user `sa` with password `5HwxsG` will be setup.

After starting the application will be available at `http://localhost:8080`.
