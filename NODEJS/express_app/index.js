// const express = require('express'); // for Common Scripts format.
import express from 'express';         // for Ecma-Script format.
import './db.js';
// import { create, findAll, findById, remove, update } from './src/users/users.controller.js';
import * as usersController from './src/users/users.controller.js';
import { errorLogger } from './src/errors/middlewares/error-logger.middleware.js';
import { standardErrorResponser } from './src/errors/middlewares/standard-error-responser.middleware.js';

const PORT = 3000;

const app = express();

app.set('view engine', 'ejs'); // ejs = template engine installed

app.use(express.json()); // express built-in middleware

app.use('/media', express.static('public'));

app.use((req, res, next) => { // logging middleware
    console.log(req.method);

    next();
});

app.get('/', (req, res) => {
    res.render('pages/index', {
        courseName: 'Node.js Basic',
        lessonName: 'Express.js Basic'
    });
});

app.get('/users', usersController.findAll);
app.get('/users/:id', usersController.findById);
app.post('/users', usersController.create);
app.put('/users/:id', usersController.update);
app.delete('/users/:id', usersController.remove);

// custom error-handlers middlewares must be latest!
// (express.js has built-in errors-handler)
app.use(errorLogger);
app.use(standardErrorResponser);

app.listen(PORT, () => {
    console.log(`Server successfuly started on http://localhost:${PORT}`);
});
