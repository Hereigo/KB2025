// const express = require('express'); // for Common Scripts format.
import express from 'express';         // for Ecma-Script format.
import './db.js';
import * as usersController from './src/users/users.controller.js';
import * as authenticationController from './src/authentication/authentication.controller.js';
import { errorLogger } from './src/errors/middlewares/error-logger.middleware.js';
import { standardErrorResponser } from './src/errors/middlewares/standard-error-responser.middleware.js';
import { authenticated } from './src/authentication/middlewares/authenticated.middleware.js';
import { hasRole } from './src/authorization/middlewares/has-role.middleware.js';
import { addCurrentUserIdToParams } from './src/authentication/middlewares/add-current-user-id-to-params.middleware.js';
import { PUBLIC_PORT, SESSION_SECRET_KEY, MONGODB_URI } from './config.js';
import cookieParser from 'cookie-parser';
import session from 'express-session';
import MongoStore from 'connect-mongo';

const PORT = PUBLIC_PORT;

const app = express();

app.set('view engine', 'ejs'); // ejs = template engine installed

app.use(express.json()); // express built-in middleware

app.use(cookieParser());

const sessionStore = new MongoStore({
    mongoUrl: MONGODB_URI,
    collectionName: 'sessions',
    ttl: 60 * 60,
});
 
app.use(session({
    secret: SESSION_SECRET_KEY, // used to sign the session ID cookie
    resave: false,              // don't save session if unmodified
    saveUninitialized: true,    // save new sessions
    cookie: { secure: false },  // set to "true" if using HTTPS
    store: sessionStore
}));

// {
//   _id: 'bZEUQL4OWq8AV1wWGtcuNOUfEJYwWRfJ',
//   expires: ISODate('2026-08-12T19:12:30.044Z'),
//   session: '{"cookie":{"originalMaxAge":null,"expires":null,"secure":false,"httpOnly":true,"path":"/"}}'
// }

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

app.post('/signin', authenticationController.signIn);
app.post('/signup', authenticationController.signUp);
app.post('/signout', authenticationController.signOut);

app.get('/users/me', authenticated, hasRole('limited_user'), addCurrentUserIdToParams, usersController.findById);
app.get('/users', authenticated, hasRole('admin'), usersController.findAll);
app.get('/users/:id', authenticated, hasRole('admin'), usersController.findById);
app.post('/users', authenticated, hasRole('admin'), usersController.create);
app.put('/users/:id', authenticated, hasRole('admin'), usersController.update);
app.delete('/users/:id', authenticated, hasRole('admin'), usersController.remove);

// custom error-handlers middlewares must be latest !
// (express.js has built-in errors-handler)
app.use(errorLogger);
app.use(standardErrorResponser);

app.listen(PORT, () => {
    console.log(`Server successfuly started on http://localhost:${PORT}`);
});
