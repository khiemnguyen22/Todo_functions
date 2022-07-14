# Todo_functions

## Overview

Azure functions project handling basic requests for a Todo application :
-   Add/update/delete/get todos
- Store todos in a local blob storage
- Support user authentication and user authenticated functionalities for todos

## Functions

PostTodos: [POST] http://localhost:7077/api/postTodo

- create a new todo

GetTodos: [GET] http://localhost:7077/api/getTodos

- Get all todos

GetTodosById: [GET] http://localhost:7077/api/getTodoById/{id}

- Get todos by specified id

UpdateTodos: [PUT] http://localhost:7077/api/updateTodo/{id}

- Update an id-specified todo

DeleteTodos: [DELETE] http://localhost:7077/api/deleteTodo/{id}

- Delete an id-specified todo

PostUsers: [POST] http://localhost:7077/api/postUser

- Add a new user

GetUsers: [GET] http://localhost:7077/api/getUsers

- Get all users

PostAuthTodos: [POST] http://localhost:7077/api/postTodo/{userId}

- Create a new todo for an existing user

GetCompletedTodos: [GET] http://localhost:7077/api/getTodosCompleted

- Get all completed todos
    


## References

- https://github.com/markheath/funcs-todo-csharp 
