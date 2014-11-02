About
===
[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/galenp/killbill-client-net?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
I'm currently evaluating using the kill-bill billing and subscription system in a new .NET based product. There is currently no API library available for .net in the killbill org repo.

I'm going to use the existing JAVA api located [here](https://github.com/killbill/killbill-client-java) as a reference.

I'm not sure if I will complete the full functionality of this API if we decide not to go with KillBill.

[Client] NOTES
===

1. The tests are designed to connect to a live kill-bill instance. Some of them (in the creation folder) send and create real data and some of them assume certain data is installed (specific accountId's and such)... I know these are not proper unit tests of the API but they are helping me understand KB. It will be relatively trivial to mock out the live calls and convert the tests to proper unit tests.

2. Code Contracts (Java Preconditions).... These really need to be set up (they aren't yet) I'll probably get around it at some point. These would validate the API method parameters and fail gracefully with nicer error messages.

3. This api uses the RestSharp HTTP client library heavily. That's probably not ideal, especially since the HttpClient in .NET is pretty decent. Considering the WHOLE api really would require a major refactor to make it nice and asyncable. For where I'm at i'm happy for sync + library helpers while i'm evaluating.

4. RestSharp has a crappy Json serializer so I've also swapped in the JSON.Net serializer which has better options for controlling the format and allows easy JsonConverters for some of KillBills quirky format requirements (like DATES!)


[Server] Notes to install Kill-Bill in an Ubuntu VirtualBox
===
These are some notes from the deployment and integration experience of kill bill by a .NET developer with absolutely zero java development experience.

I'll write down my specific steps as I follow the Deployment guide located [here](http://docs.kill-bill.org/userguide.html#deployment "Kill Bill Deployment guide").

I'm attempting to install to a VirtualBox Ubuntu 14.04 instance.

This is not necessarily a fresh instance so I may miss some package installations steps that have already been installed. (Java 7, Ruby, Bundle, Gem etc)  [not that I really know what these things do yet ;)]

1. Download Killbill
---
The KB userguide says :
> Releases are available in Maven Central. Among other packaging formats, we provide a war with all dependencies. You can get the latest version here (download the war artifact).

Where I saw
> killbill-profiles-killbill-0.12.0-jar-with-dependencies.war	25-Oct-2014	361.5 M

```shell
cd ~
mkdir killbill
cd killbill
wget http://search.maven.org/remotecontent?filepath=org/kill-bill/billing/killbill-profiles-killbill/0.12.0/killbill-profiles-killbill-0.12.0-jar-with-dependencies.war
```

2. Database Setup
---
  1. Download the db sql (they call it ddl) from http://docs.kill-bill.org/schema.html
  2. PROBLEM: the link to version 12 is not correct, manually alter the link so it gets the correct version to http://docs.kill-bill.org/schemata/killbill-0.12.0.ddl
  

Installs MySQL Server and logs in as root user
```shell
sudo apt-get install mysql-server
mysql -uroot -p
```
Creates the KillBill mysql database and user KbUser
```sql
create database killbill;
grant all on killbill.* to 'kbuser'@'%' identified by 'kbpwd';
```

Downloads the SQL (ddl?) file and imports to the database
```shell
wget "http://docs.kill-bill.org/schemata/killbill-0.12.0.ddl"
mysql -ukbuser -pkbpwd < killbill-0.12.0.ddl
```


3. Plugins
---
I'll come back to this....

4. Container? Jetty?
---
Ok i dont really know what this Jetty business is. Going by the first line description on this [website](http://www.eclipse.org/jetty/) it like some sort of java framework stack for building websites that support plugins. Sounds a bit like Owin/Katana. Whatever... i'll install it.

I know enough basic linux to grasp all the below commands easily enough.

I'm going to attempt to install Jetty 8 stable.

 ```shell
 sudo apt-get install openjdk-7-jdk
 sudo mkdir /usr/java
 sudo ln -s /usr/lib/jvm/java-7-openjdk-amd64 /usr/java/default
 cd /opt
 sudo wget "http://eclipse.org/downloads/download.php?file=/jetty/stable-8/dist/jetty-distribution-8.1.16.v20140903.tar.gz&r=1"
 sudo mv download.php\?file\=%2Fjetty%2Fstable-8%2Fdist%2Fjetty-distribution-8.1.16.v20140903.tar.gz\&r\=1 jetty-distribution-8.1.16.v20140903.tar.gz
 sudo tar -xvf jetty-distribution-8.1.16.v20140903.tar.gz
 sudo mv jetty-distribution-8.1.16.v20140903 jetty
 sudo useradd jetty -U -s /bin/false
 sudo chown -R jetty:jetty /opt/jetty
 sudo cp /opt/jetty/bin/jetty.sh /etc/init.d/jetty
 sudo nano /etc/default/jetty
 ```
 Pasted into /etc/default/jetty
 ```shell
JAVA=/usr/bin/java # Path to Java
NO_START=0 # Start on boot
JETTY_HOST=0.0.0.0 # Listen to all hosts
JETTY_ARGS=jetty.port=8085
JETTY_USER=jetty # Run as this user
JETTY_HOME=/opt/jetty
 ```
 
 ```shell
 sudo service jetty start
 sudo update-rc.d jetty defaults
 sudo reboot
 ```
 
 On reboot check that Jetty is running
 ```shell
 service jetty check
 netstat -tln
 ```
 
 Apparently the jetty test app isnt secure 
 ```shell
 sudo rm *.war /opt/jetty/webapps/
 ```
 
 5. Kill bill into jetty!
 ---

```
 cd /opt/jetty
 sudo mv ~/killbill/killbill-0.12.0-with-dependencies.war ./webapps/root.war
 sudo chown -R jetty:jetty ./webapps/root.war
 sudo nano start.ini
```
Replace contents of start.ini with

```shell
# Kill Bill properties
-Dorg.killbill.dao.url=jdbc:mysql://127.0.0.1:3306/killbill
-Dorg.killbill.dao.user=killbill
-Dorg.killbill.dao.password=killbill

# Start classpath OPTIONS
OPTIONS=Server,resources,ext,plus,annotations

# Configuration files
etc/jetty.xml
etc/jetty-annotations.xml
etc/jetty-deploy.xml
etc/jetty-webapps.xml
```

This will take a fair few minutes to start up....
```shell
java -jar start.jar
````

6. SSL
---
Ctrl-C the running server

```shell
 sudo keytool -keystore keystore -alias jetty -genkey -keyalg RSA
 sudo mv keystore .\etc
```

I had issues getting SSL working.. i've left it off my development environment for now.




 
 
 


 

 
