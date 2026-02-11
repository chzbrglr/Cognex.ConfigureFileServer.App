This application serves to add the following functionality to the ISVS File Server.
- Auto-Start FTP server with Windows
- Encrypt FTP configuration to prevent users from reading the files
- Hide FTP Server in System Tray, independent of ISVS

Deployment Notes:
The FTP Configurator folder contains the required executables and deployed files. The batch file included will backup the existing File Server executable and replace with the updated one. The batch file can also be used to roll back any changes. Depending on the local/group policy, the Batch File, File Server Configurator and the FTP Launcher may need to run with administrator rights.
