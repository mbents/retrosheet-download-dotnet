retrosheet-download-dotnet
==========================

This is a .NET console app to download and parse Retrosheet event files. You should be able to just run the executable using the command line parameters. It's required that you set a working directory using -wd. You will need to make sure the BEVENT and BGAME applications are located in the working directory. If you would like the output files to be saved to a different directory, you can use the -dir option. Otherwise the working directory will be used. Including the -d option will download the event files. If you include -y, the download will be limited to a specific year. Otherwise all event files will be downloaded, dating back to 1921. You can use -bevent to parse event files using the BEVENT application, and you can use -bgame to parse event files using the BGAME application. For a help screen, use the -h option. 

Here's a notice regarding use of Retrosheet data:
http://www.retrosheet.org/game.htm#Notice 
