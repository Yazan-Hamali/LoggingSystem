This app build using ABP framework that use DDD patern

-------------------------------------------------------------
Features:

Store logs in SQL DB

Store logs in LocalFiles

Sotre logs in S3Bucket (I used wasabisys.com that uses S3 protocol same as AWS S3)

---------------------------------------------

sys req:
.Net 7.0
---------------------------------------------

Testing steps:

1-Config app settings inside  LoggingSystem.HttpApi.Host project

2-Config DB conection string inside LoggingSystem.DbMigrator

3-Run LoggingSystem.DbMigrator project to start migrations and build Database

4-Make LoggingSystem.HttpApi.Host as startup

5-Run LoggingSystem.HttpApi.Host project

6-After running the project use the following credits to login

user:admin

pass:1q2w3E*
