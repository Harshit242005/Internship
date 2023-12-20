const express = require('express');
const bodyParser = require('body-parser');
const multer = require('multer');
const cors = require('cors');
const app = express();

const port = 3000;
app.use(cors());
// Set up middleware
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

const storage = multer.memoryStorage();
const upload = multer({ storage: storage });
// connecting with the mongodb client to store the data
const { MongoClient } = require('mongodb');

const uri = 'mongodb+srv://agreharshit610:zzaassxx@reactchatapplication.uz2f3up.mongodb.net/';
const client = new MongoClient(uri, { useNewUrlParser: true, useUnifiedTopology: true });

// this is the code for the online user websocket
const http = require('http');
const WebSocket = require('ws');
const wsApp = express();
const wsServer = http.createServer(wsApp);
const wsPort = 3001;
const wss = new WebSocket.Server({ noServer: true });


wsServer.on('upgrade', (request, socket, head) => {
    wss.handleUpgrade(request, socket, head, (ws) => {
        wss.emit('connection', ws, request);
    });
});

// Map to store user online status
const userStatusMap = new Map();
const wsUsernameMap = new Map();


// // Handle WebSocket connections
// wss.on('connection', (ws) => {
//     // Handle messages from clients (e.g., login, logout)
//     ws.on('message', (message) => {
//         const { type, username } = JSON.parse(message);

//         if (type === 'login' || type === 'signup') {
//             // Set user status to 'Online'
//             userStatusMap.set(username, 'Online');
//             wsUsernameMap.set(ws, username);
//             console.log('after login or signing up');
//             console.log(`userStatusMap: ${userStatusMap}`);
//             console.log(`wsUsernameMap: ${wsUsernameMap}`);
//         } else if (type === 'logout') {
//             // Set user status to 'Offline'
//             userStatusMap.set(username, 'Offline');
//             wsUsernameMap.delete(ws);
//         }

//         // Broadcast the updated user status to all connected clients
//         broadcastUserStatus();
//     });

//     // Handle WebSocket close event
//     ws.on('close', () => {
//         // Assuming you have the username associated with the WebSocket connection
//         const username = wsUsernameMap.get(ws);
//         console.log(`the username we got for closing ${username}`);
//         wsUsernameMap.delete(ws);
//         if (username) {
//             // Set user status to 'Offline' when the connection is closed
//             userStatusMap.set(username, 'Offline');

//             // Broadcast the updated user status to all connected clients
//             broadcastUserStatus();
//         }
//     });
//     // Send the initial user status to the connected client
//     sendInitialUserStatus(ws);
// });

// user status updating in the map and in the clinet side broadcasting
// wss.on('connection', (ws) => {
//     let Username;  // Declare username variable outside the scope of the message event

//     // Handle messages from clients (e.g., login, logout)
//     ws.on('message', (message) => {
//         const { type, username } = JSON.parse(message);
//         if (type === 'login' || type === 'signup') {
//             // Set user status to 'Online'
//             Username = username;
//             userStatusMap.set(Username, 'Online');
//             wsUsernameMap.set(ws, Username);
//             console.log(`the username that would hold for both setting up and deleting: ${Username}`);
//             console.log('after login or signing up');
//             console.log(`userStatusMap: ${userStatusMap}`);
            
//         } else if (type === 'logout') {
//             // Set user status to 'Offline'
//             userStatusMap.set(Username, 'Offline');
//             wsUsernameMap.delete(ws);
//         }
//         // Broadcast the updated user status to all connected clients
//         broadcastUserStatus();
//     });

//     // Handle WebSocket close event
//     ws.on('close', () => {
//         // Assuming you have the username associated with the WebSocket connection
//         console.log(`the username we got for closing ${Username}`);
//         wsUsernameMap.delete(ws);
//         if (Username) {
//             // Set user status to 'Offline' when the connection is closed
//             userStatusMap.set(Username, 'Offline');
//             console.log(`after setting the user status to offline: ${userStatusMap}`);
//             // Broadcast the updated user status to all connected clients
//             broadcastUserStatus();
//         }
//     });

//     // Send the initial user status to the connected client
//     sendInitialUserStatus(ws);
// });

wss.on('connection', (ws) => {
    let Username;

    // Handle messages from clients (e.g., login, logout, refresh)
    ws.on('message', (message) => {
        const { type, username } = JSON.parse(message);

        if (type === 'login' || type === 'signup' || type === 'refresh') {
            // Set user status to 'Online'
            Username = username;
            userStatusMap.set(Username, 'Online');
            wsUsernameMap.set(ws, Username);
            console.log('after login, signing up, or refreshing');
            console.log(`userStatusMap: ${userStatusMap}`);
            console.log(`wsUsernameMap: ${wsUsernameMap}`);
        } else if (type === 'logout') {
            // Set user status to 'Offline'
            userStatusMap.set(Username, 'Offline');
            wsUsernameMap.delete(ws);
        }

        // Broadcast the updated user status to all connected clients
        broadcastUserStatus();
    });

    // Handle WebSocket close event
    ws.on('close', () => {
        // Assuming you have the username associated with the WebSocket connection
        console.log(`the username we got for closing ${Username}`);
        wsUsernameMap.delete(ws);

        if (Username) {
            // Set user status to 'Offline' when the connection is closed
            userStatusMap.set(Username, 'Offline');

            // Broadcast the updated user status to all connected clients
            broadcastUserStatus();
        }
    });

    // Send the initial user status to the connected client
    sendInitialUserStatus(ws);
});




// Broadcast user status to all connected clients
function broadcastUserStatus() {
    const onlineUsers = Array.from(userStatusMap.entries())
        .filter(([_, status]) => status === 'Online')
        .map(([username]) => username);
    console.log(`the onlineUsers values are ${onlineUsers}`);
    console.log(userStatusMap);
    // Broadcast the online users to all connected clients
    wss.clients.forEach((client) => {
        if (client.readyState === WebSocket.OPEN) {
            client.send(JSON.stringify({ type: 'userStatus', onlineUsers }));
        }
    });
}

// Send the initial user status to a newly connected client
function sendInitialUserStatus(ws) {
    const onlineUsers = Array.from(userStatusMap.entries())
        .filter(([_, status]) => status === 'Online')
        .map(([username]) => username);
    console.log(`online users ${onlineUsers}`);
    // Send the initial user status to the connected client
    ws.send(JSON.stringify({ type: 'userStatus', onlineUsers }));
}


// this is the code for signup and login in the application
async function connectToDatabase() {
    try {
        await client.connect();
        console.log('Connected to MongoDB');
    } catch (error) {
        console.error('Error connecting to MongoDB:', error);
    }
}
// this will connect me to the database 
connectToDatabase();

// Handle the signup POST request
app.post('/signup', upload.single('file'), (req, res) => {
    const file = req.file; // The file data
    const username = req.body.username; // The username
    const password = req.body.password; // The password

    // Set user status to 'Online' for the signup to show the status
    userStatusMap.set(username, 'Online');
    broadcastUserStatus();

    const imageBase64 = file ? file.buffer.toString('base64') : null;

    try {
        // Access your database and collection
        const database = client.db('UserStatus');
        const collection = database.collection('UserProfile');

        // Save the document to MongoDB 
        collection.insertOne({
            username: username,
            password: password,
            image: imageBase64,
        });

        res.status(200).json({ username: username });
    } catch (error) {
        console.error('Error saving document to MongoDB:', error);
        res.status(500).send('Internal Server Error');
    }
});

// Handle login POST request
app.post('/login', async (req, res) => {
    const username = req.body.username;
    const password = req.body.password;

    // Set user status to 'Online' when user login in the application
    userStatusMap.set(username, 'Online');
    broadcastUserStatus();

    console.log(`username is ${username} and password is ${password}`);
    try {
        // Access your database and collection
        const database = client.db('UserStatus');
        const collection = database.collection('UserProfile');

        // Perform login logic by querying the collection
        const user = await collection.findOne({ username: username, password: password });

        if (user) {
            // Login successful
            res.status(200).json({ message: 'Login successful', username: user.username });
        } else {
            // Invalid credentials
            res.status(401).json({ error: 'Invalid credentials' });
        }
    } catch (error) {
        console.error('Error during login:', error);
        res.status(500).json({ error: 'Internal Server Error' });
    }
});

// Handle GET request to fetch all users
app.get('/getusers', async (req, res) => {
    try {
        // Access your database and collection
        const database = client.db('UserStatus');
        const collection = database.collection('UserProfile');

        // Fetch all users from the collection
        const users = await collection.find().toArray();

        // Respond with the users array
        res.status(200).json(users);
    } catch (error) {
        console.error('Error fetching users:', error);
        res.status(500).json({ error: 'Internal Server Error' });
    }
});

wsServer.listen(wsPort, () => {
    console.log(`WebSocket server listening on port ${wsPort}`);
});

// Start the server
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});
