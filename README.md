# FOLDER SYNCHRONIZER

## DESCRIPTION

  Periodically synchronizes a source folder with a replica folder. The synchronization creates a fresh copy of the source folder in every sync operation, guaranteeing that the content of the replica folder is recoverable.       Logs of the synchronization are written with a timestamp to the console and to a log folder. 

## INSTALLATION AND USAGE

Make sure to install .NET 10.0 or higher version. Clone this repository and run the console application. CommandLineParser NuGet package is installed for reading command line arguments

## LIMITS OF USE

Make sure no files in the replica folder and subfolders are open and no subfolders of the replica folder are open before starting the synchronization. Files in the source folder and its subfolders, and subfolders of the source folder can remain open.
When entering input data, the usual navigation between previous input values with the Up and Down arrows doesn't work. See more about this in the Technical notes section.

## CONTRIBUTING

Periodic run is implemented with a `PeriodocTimer`, and the actual synchronization happens in an async method. So that in case the synchronization lasts longer than the sync period specified by the user, a new syncing won't be started before the current one is done.

The synchronization itself follows these steps:

Read the content of the <source> folder and subfolders and store them in data model objects
Repeat the same for the <replica> folder
Create a temporary folder
Find the subfolders which only exist in the <replica> folder and not in the <source> folder and log deleting them
Find the files which only exist in the <replica> folder and not in the <source> folder and log deleting them
Iterate through the subfolders in the <source> folder. If they are not in the <replica> folder, copy them to tmp and log copying them
Iterate through the files in the <source> folder and its subfolders. If the files are not in the <replica> folder, copy them to tmp and log copying them. If they are there, but with a different size or last modified date, copy them from the <source> folder and log updating them to the source version
Rename <replica> to <replica>Backup
Rename the temporary folder to <replica>
Read the content of the new <replica> folder and subfolders and store them in data model objects
Update the last modified data of the subfolders in the new <replica> to match the last modified data of the subfolders in the <source> folder
Verify that the content of the new <replica> matches the content of <source>
Delete <replica>Backup

Exceptions were added to the data manipulation, so that different types of failures and handled gracefully, logged, and prevent the application from crashing.


I would be more than glad to discuss my implementation with a tech team of Veeam in a next round of interview.
