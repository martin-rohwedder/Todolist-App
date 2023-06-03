# Todolist Application
This application is a simple Todo List application. It is made as a project for my developer portfolio, to show my skills in different technologies and software architecturing.

It has a web API which can communicate with the different frontend applications. this repository therefore consists of the backend which has a web API coded with `.NET 7`, and the frontend which consists of a `React.JS` web application which consumes from the API

The backend Web API can be a standalone application though, which you then can wire up with your own frontend.

---

## Setup Backend API

Here is a small guide on how to setup the Backend Todolist API.

1. Add the following `JwtSettings` In your `appsettings.json` and set your settings except for the SecretKey.

```json
"JwtSettings": {
    "SecretKey": "",
    "ExpiryMinutes": 60,
    "Issuer": "",
    "Audience": ""
}
```

2. Manage The applications `User Secrets` and add the JwtSettings `SecretKey`. The secret key has to be excactly 256 bits long (32 characters).

```json
"JwtSettings": {
    "SecretKey": "YOUR SUPER SECRET KEY"
}
```

---

&copy; 2023 [Martin Rohwedder](https://github.com/martin-rohwedder)