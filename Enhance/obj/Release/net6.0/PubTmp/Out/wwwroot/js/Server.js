// here i will handle the upcoming server side code
const express = require('express');
const mysql = require('mysql2');
const app = express();
const bodyParser = require('body-parser');
app.use((req, res, next) => {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type, Authorization');
    next();
});
app.use(bodyParser.json());
const connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'azxcvbnmlkjhgfds',
    database: 'ZEUS'
});

connection.connect((err) => {
    if (err) throw err;
    console.log("Database is connected");
});

// Define a route to handle the POST request
app.post('/chat', (req, res) => {
    const selectedIdTo = req.body.idTo;
    const message = req.body.message;
    const idFrom = req.body.idFrom;
    // Insert the data into your table
    const query = `INSERT INTO Chat (IDTO, IDFROM, Text) VALUES (?, ?, ?)`;
    connection.query(query, [selectedIdTo, idFrom, message], (err, result) => {
        if (err) {
            console.error('Error executing MySQL query:', err);
            res.status(500).send('Error inserting data into table');
        } else {
            console.log('Data inserted into table successfully');
            res.status(200).send('Data inserted into table');
        }
    });
});

app.get('/chat', (req, res) => {
    const selectedIdTo = req.query.idTo;
    const idFrom = req.query.idFrom;

    // Retrieve the chat messages from the table
    const query = `SELECT Text FROM Chat WHERE IDTO = ? AND IDFROM = ?`;
    connection.query(query, [idFrom, selectedIdTo], (err, results) => {
        if (err) {
            console.error('Error executing MySQL query:', err);
            res.status(500).send('Error retrieving chat messages');
        } else {
            console.log('Chat messages retrieved successfully');
            res.json(results);
        }
    });
});


app.listen(3000, () => {
    console.log("Server is started");
});

