# Smart Cache

Exposed endpoints:
http://localhost:5161/emails/{email}

HTTP Methods: ``GET``, ``POST``

Endpoints use Basic Authentication, please use the following username and password:
``admin:admin``

Email validation is performed on the passed email addresses. If email is not valid the server will respond with ``400 - Bad Request``

The ``.env`` file is provided as a template. Fill in the required credentials for the app to work with your Azure account.
(Or change app to use Orleans locally)

Grains are persisted to Azure Blob Storage every 5 minutes.

Docker image can be built and the run with the Dockerfile from the project.
CURL's for docker container:

GET email:
``curl -u admin:admin http://localhost:8080/emails/damjanh1@email.com -v``

POST email:
``curl -X POST -u admin:admin http://localhost:8080/emails/damjanh1@email.com -v``

Tests are in the accompanying project: SmartCache.Tests