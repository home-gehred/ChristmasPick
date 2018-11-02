# ChristmasPick
The intention of this program is to provide a way to easily pick Christmas gift exchange names.

# Getting Started
The main project directory is ChristmasPick

1. git checkout -b xmas{year}
1. create backup of Adults/Archive.xml
1. create backup of Gehred/GehredFamily.xml
1. create backup of Kids/Archive.xml
1. Inspect Kids Archive.xml to make sure, no new borns have been missed.
1. Inspect Adults Archive.xml to make sure no one who has passed away is in list.
1. Inspect GehredFamily.xml to make sure no marriages have been missed.

## The Archive Folder
In the ChristmasPick folder there is a folder named Archive it contains the 'database' of people. Kids can migrate to Adults.
When adults no longer participate or passaway :-( they are removed. 

Archive
|--- Adults
|--- Gehred
|--- Kids

Each of folder contains a file called Archive.xml, that file contains the history of every Christmas. 
