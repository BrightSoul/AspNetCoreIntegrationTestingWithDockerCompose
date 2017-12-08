# ASP.NET Core integration testing with docker-compose
This project demonstrates how an ASP.NET Core application developed on Windows can be tested on the Linux platform with minimal effort and by leveraging Docker containers.

# Prerequisites
 * 2GB of hard disk space for the docker images used in this example
 * Install Docker on Windows Pro/Enterprise: https://docs.docker.com/engine/installation/
 * Virtualization needs to be enabled at the BIOS level;
 * Hyper-V needs to be enabled as well, check here how to do it from the command line: https://stackoverflow.com/questions/30496116/how-to-disable-hyper-v-in-command-line#answer-35812945
 * The local drive needs to be shared (right click on the Docker icon that's in the systray, then click Settings -> Shared drives)


# The sample application
This solution is made of three projects:
 * An ASP.NET Core Web API sample application with just a ValuesController;
 * A unit test project, just for kicks;
 * An integration test project that's going to run when executing the `run-integration-tests.bat` from the command line;

 # The `run-integration-tests.bat` file
 This script will launch the `docker-compose` command to run the ASP.NET Core application in a container. As seen in the `docker-compose.yml` file, this application depends on a second container based on the Sql Server for Linux image by Microsoft. When first run, the `run-integration-tests.bat` script will cause the download of the `microsoft/aspnetcore` and `microsoft/mssql-server-linux` images (around 1.70GB total). Be patient while they're being downloaded.

 Integration tests will run as soon as the two containers have been created and a green message will appear in the output.

 ![Tests passed](output.png)

 Containers are recreated at each execution of the script, so that the test results can be deterministic.