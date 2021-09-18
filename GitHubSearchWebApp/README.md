## How to run in Docker from the commandline

Build in container
```
docker build -t web_devteam .
```

to run

```
docker run -d -p 8081:80 --name web_container_devteam web_devteam
```

to stop container
```
docker stop web_container_devteam
```

to remove container
```
docker rm web_container_devteam
```

## Deploy to heroku

1. Create heroku account
2. Create application
3. Make sure application works locally in Docker


Login to heroku
```
heroku login
heroku container:login
```

Push container
```
heroku container:push -a webapp-github-dev-team web
```

Release the container
```
heroku container:release -a webapp-github-dev-team web
```