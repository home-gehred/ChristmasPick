# ChristmasPick
The intention of this program is to provide a way to easily pick Christmas gift exchange names.

# Getting Started
The main project directory is ChristmasPick

1. git checkout -b xmas{year}
1. create backup of Adults/Archive.xml
1. create backup of Gehred/GehredFamily.xml
1. create backup of Kids/Archive.xml
1. Inspect Kids Archive.xml to make sure, no past years have been missed.
1. Inspect Adults Archive.xml to make sure no past years have been misseed.
1. Inspect GehredFamily.xml to make sure no marriages, new borns, or sadly deaths have been missed.
1. Open 'ChristmasPick' Solution using Visual Studio 2017 or Visual Studio Code
1. Build and run all unit tests. (Assume Success)
1. Open XmasPickTrialHarness project find XmasPickClient.cs
1. Modify/verify paths to adultArchivePath and kidArchivePath are correct
1. While you are at it make sure the path to the FileFamilyProvider is correct
1. Update the Christmas year
1. Once changes are made build XmasPickTrialHarness and open a cmd prompt in that directory
1. run XmasPickTrialHarness.exe
1. The UI is not ideal, but in essence it updates Kids\Archive.xml, Gehred\GheredFamily.xml, Adult\Archive.xml
1. Now, open the project XmasPickVerification project
1. Modify/verify the paths to adultArchivePath, kidArchivePath, and familyProvider
1. Modify the christmasThisYear variable to be correct
1. Build XmasPickVerification project
1. In cmd prompt run XmasPickVerification.exe read output (Assume Success)
1. Finally it time to create the official report!
1. Open the project XmasPickReportProgram project
1. In Program.cs modify the main function variables christmasThisYear, adultArchivePath, kidArchivePath and familyProvider
1. Build XmasPickReportProgram project
1. In cmd prompt run XmasPickReportProgram remeber to redirect output to a file. That is the master file!
1. Commit all changes to git hub.
1. Send out massive amount of emails and track who has responded.

## The Archive Folder
In the ChristmasPick folder there is a folder named Archive it contains the 'database' of people. Kids can migrate to Adults.
When adults no longer participate or passaway :-( they are removed. 

Archive
|--- Adults
|--- Gehred
|--- Kids

Each of folder contains a file called Archive.xml, that file contains the history of every Christmas. 
