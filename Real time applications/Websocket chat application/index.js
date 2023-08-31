const express = require('express');
const app = express();
const cors = require('cors');
app.use(cors());
const bodyParser = require('body-parser');
const bcrypt = require('bcrypt');
// ...

app.use(bodyParser.urlencoded({ extended: true }));
const mysql = require('mysql2');

const connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'azxcvbnmlkjhgfds',
    database: 'chatx'
});

connection.connect((error) => {
    if (error) {
        console.error('Error connecting to the database:', error);
    } else {
        console.log('Connected to the database');
    }
});



const WebSocket = require('ws');

const server = new WebSocket.Server({ port: 8080 });

// Map to store connected clients along with user IDs
const connectedClients = new Map();

// Event listener for when a client connects
server.on('connection', (socket, request) => {
    console.log('A client has connected');

    // Store the socket in the clients map


    // Event listener for when a client disconnects
    socket.on('close', () => {
        console.log('A client has disconnected');
        connectedClients.delete(socket);
    });

    // Event listener for when a message is received from the client
    socket.on('message', (message) => {
        // this userId is the id of the person who log in the application
        const userId = message.toString();
        console.log("my id is :", userId);

        // Store the socket and user ID in the connectedClients Map
        connectedClients.set(userId, socket);
        console.log('Connected clients:');
        connectedClients.forEach((socket, userId) => {
            console.log(`User ID: ${userId}`);
        });

        const receivedData = JSON.parse(message);

        // Access the properties of the JSON object
        // these are the reciever id and the message we want to send
        const messageContent = receivedData.message;
        const personId = receivedData.personId;
        const myId = receivedData.senderId;

        const senderId = parseInt(myId, 10);
        const recieverId = parseInt(personId, 10);

        const insertQuery = `INSERT INTO chat (SenderId, ReceiverId, Message, Timestamp) VALUES (?, ?, ?, NOW())`;
        const values = [senderId, recieverId, messageContent];

        connection.query(insertQuery, values, (error, results) => {
            if (error) {
                console.error('Error inserting message into the database:', error);
            } else {
                console.log('Message inserted into the database');
            }
        });

        console.log('Message from client:', messageContent);
        console.log('Person ID:', personId);





        if (connectedClients.has(personId)) {
            const userSocket = connectedClients.get(personId);
            userSocket.send(messageContent);
        } else {
            console.log('Person ID not found in connected clients');
        }

    });
});




app.post('/signup', (req, res) => {
    const { username, password } = req.body;
    bcrypt.hash(password, 10, (err, hashedPassword) => {
        if (err) {
            console.error('Error hashing password:', err);
            return res.status(500).json({ success: false, message: 'Error creating user' });
        }

        const insertSignupQuery = `INSERT INTO user (username, password) VALUES (?, ?)`;

        connection.query(insertSignupQuery, [username, hashedPassword], (error, results) => {
            if (error) {
                console.error('Error inserting user:', error);
                return res.status(500).json({ success: false, message: 'Error creating user' });
            }

            return res.status(201).json({ success: true, message: 'User created successfully' });
        });
    });

});


app.post('/login', (req, res) => {
    const { username, password } = req.body;

    // Check if the username exists in the database
    const userQuery = `SELECT * FROM user WHERE username = ?`;

    connection.query(userQuery, [username], (error, results) => {
        if (error) {
            console.error('Error querying user:', error);
            return res.status(500).json({ success: false, message: 'Error during login' });
        }

        if (results.length === 0) {
            // User does not exist
            return res.status(401).json({ success: false, message: 'User not found' });
        }

        // User exists, compare hashed passwords
        const storedHashedPassword = results[0].password;

        bcrypt.compare(password, storedHashedPassword, (compareError, passwordsMatch) => {
            if (compareError) {
                console.error('Error comparing passwords:', compareError);
                return res.status(500).json({ success: false, message: 'Error during login' });
            }

            if (passwordsMatch) {
                // Successful login
                const userId = results[0].id;
                return res.status(200).json({ success: true, userId: userId }); // Send user ID in the response

            } else {
                // Invalid password
                return res.status(401).json({ success: false, message: 'Invalid credentials' });
            }
        });
    });
});

// query to send data to the client side
app.get('/users', (req, res) => {
    const sql = 'SELECT id, name FROM user';
    connection.query(sql, (err, results) => {
        if (err) {
            console.error('Error fetching user data:', err);
            res.status(500).json({ error: 'An error occurred' });
        } else {
            res.json(results);
        }
    });
});

app.get('/messages/:userId', (req, res) => {
    const userId = req.params.userId;
    const sql = 'SELECT Message FROM chat WHERE SenderId = ?';
    connection.query(sql, [userId], (err, results) => {
        if (err) {
            console.error('Error fetching chat messages:', err);
            res.status(500).json({ error: 'An error occurred' });
        } else {
            console.log("messages fetched successfully")
            res.json(results);
        }
    });
});



app.listen(3000, (req, res) => {
    console.log("server is running");
});