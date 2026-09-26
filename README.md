===================================================================
                     FOLDER SYNCHRONIZER
-------------------------------------------------------------------
   Periodically synchronizes a source folder with a replica folder
===================================================================

HOW IT WORKS
-------------------------------------------------------------------
  1. Enter the source folder path
  2. Enter the replica folder path
  3. Enter the synchronization period in minutes
  4. Enter the log folder path

  The replica folder will be periodically updated to match
  the content of the source folder

NOTE
-------------------------------------------------------------------
  * Make sure all files in the replica folder are closed
    before starting the synchronization

  * Files in the source folder can remain open

  * Logs are written to a text file created in the specified
    log folder and are also displayed in the console

  * If a synchronization fails, the files in the source folder
    remain safe in their original location. Replica folder
    content may be left intact or found in a backup folder
    in the parent folder of the replica folder

-------------------------------------------------------------------
