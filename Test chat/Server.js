const express = require('express');
const mysql = require('mysql2');
const app = express();
const path = require('path');


app.use(function (req, res, next) {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type');
    next();
});

app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(express.static(path.join(__dirname, 'public')));
const connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'azxcvbnmlkjhgfds',
    database: 'ZEUS'
});



var userId = 0
app.get('/save-messages', function (req, res) {
    userId = req.query.userId; // Retrieve the userId from the query parameters
    const query = `SELECT * FROM CHAT WHERE IDTO = 2 OR IDFROM = ${userId}`;
    connection.query(query, function (error, results) {
        if (error) {
            console.error('Error retrieving messages:', error);
            return res.status(500).json({ message: 'Error retrieving messages' });
        }
        return res.status(200).json(results);
    });
});


app.post('/send-message', function (req, res) {
    const { text } = req.body;
    const query = `INSERT INTO CHAT (IdTo, IdFrom, Text) VALUES (${userId}, 2, ?)`;
    connection.query(query, [text], function (error, results) {
        if (error) {
            console.error('Error saving message:', error);
            return res.status(500).json({ message: 'Error saving message' });
        }

        return res.status(200).json({ message: 'Message saved successfully' });
    });
});

app.get('/save_1-messages', function (req, res) {
    userId = req.query.userId; // Retrieve the userId from the query parameters
    const query = `SELECT * FROM CHAT WHERE IDTO = 1 OR IDFROM = ${userId}`;
    connection.query(query, function (error, results) {
        if (error) {
            console.error('Error retrieving messages:', error);
            return res.status(500).json({ message: 'Error retrieving messages' });
        }
        return res.status(200).json(results);
    });
});




app.post('/send_1-message', function (req, res) {
    const { text } = req.body;
    const query = `INSERT INTO CHAT (IdTo, IdFrom, Text) VALUES (${userId}, 1, ?)`;
    connection.query(query, [text], function (error, results) {
        if (error) {
            console.error('Error saving message:', error);
            return res.status(500).json({ message: 'Error saving message' });
        }

        return res.status(200).json({ message: 'Message saved successfully' });
    });
});




app.get('/getUsers', function (req, res) {
    const query = 'SELECT Id, Nickname, Image FROM Users';
    connection.query(query, function (error, result) {
        if (error) {
            console.error('Error retrieveing the Users from database', error);
            return res.status(500).json({ message: 'Error getting back the Users' })
        }

        result.forEach(function (user) {
            if (user.Image) {
                user.Image = user.Image.toString('base64');
                console.log("Server side image passing is okay");
            }
        });

        return res.status(200).json(result);
    });
});


app.listen(3000, function () {
    console.log('Server is running on port 3000');
}); 
