// const express = require('express'); // for Common Scripts format.
import express from 'express';         // for Ecma-Script format.

const PORT = 3000;

const app = express();

app.set('view engine', 'ejs'); // ejs = template engine installed

app.use('/media', express.static('public'));

app.use((req, res, next) => {
    console.log(req.method);

    next();
});

app.get('/', (req, res) => {
    res.render('pages/index', {
        courseName: 'Node.js Basic',
        lessonName: 'Express.js Basic'
    });
});

app.listen(PORT, () => {
    console.log('Server successfuly started on port ' + PORT);
});
