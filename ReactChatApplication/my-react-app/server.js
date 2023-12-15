const express = require('express');
const http = require('http');
const WebSocket = require('ws');
const { MongoClient } = require('mongodb');
const bodyParser = require('body-parser');
const cors = require('cors');
const app = express();
const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

// Connection URI
const uri = 'mongodb+srv://agreharshit610:zzaassxx@reactchatapplication.uz2f3up.mongodb.net/';

// Create a new MongoClient
const client = new MongoClient(uri, { useNewUrlParser: true, useUnifiedTopology: true });

// Middleware to parse JSON data
app.use(bodyParser.json({ limit: '50mb' })); // Set the limit according to your needs
app.use(cors());
// Endpoint to handle form submissions
// ... (Previous server code)

app.post('/CreateUser', async (req, res) => {
  const { image, username, email, linkedinUrl, password } = req.body;

  try {
    // Convert base64 image to a buffer
    const imageBuffer = Buffer.from(image, 'base64');

    // Connect to the MongoDB server
    await client.connect();
    console.log('Connected to MongoDB');

    // Select the database 

    const database = client.db('ReactApplicationDatabase');

    // Select the collection
    const collection = database.collection('UserProfile');

    // Insert the user data as a document
    const result = await collection.insertOne({
      Image: imageBuffer,
      Username: username,
      Email: email,
      LinkedinUrl: linkedinUrl,
      Password: password
    });

    console.log('Inserted user data:', result);

    res.status(200).json({ message: "User Profile Created Successfully" });
  } catch (error) {
    console.error('Error saving user data:', error);
    res.status(500).json({ error: 'Internal Server Error' });
  } finally {
    // Close the MongoDB connection
    await client.close();
    console.log('Disconnected from MongoDB');
  }
});


app.post('/LoginUser', async (req, res) => {
  const { email, password } = req.body;
  console.log(email, password);
  try {
    // Connect to the MongoDB server
    await client.connect();
    console.log('Connected to MongoDB');

    // Select the database
    const database = client.db('ReactApplicationDatabase');

    // Select the collection
    const collection = database.collection('UserProfile');

    // Find the user with the given email and password
    const user = await collection.findOne({ Email: email, Password: password });

    if (user) {
      await collection.updateOne({ Email: email }, { $set: { Status: 'Online' } });

      // If a user is found, send a success response
      console.log(user.Username);

      res.json({ success: true, message: 'Login successful', userMe: user.Username });
    } else {
      // If no user is found, send an error response\
      res.json({ success: false, message: 'Invalid email or password' });
    }
  } catch (error) {
    console.error('Error during login:', error);
    res.status(500).json({ success: false, message: 'Internal Server Error' });
  } finally {
    // Close the MongoDB connection
    await client.close();
    console.log('Disconnected from MongoDB');
  }
});

// Endpoint to get user data
app.get('/GetUsers', async (req, res) => {
  try {
    // Connect to the MongoDB server
    await client.connect();
    console.log('Connected to MongoDB');

    // Select the database
    const database = client.db('ReactApplicationDatabase');

    // Select the collection
    const collection = database.collection('UserProfile');
    // Extract the email from the request query parameters
    const { email } = req.query;

    // Define the filter criteria to find documents without the specified email
    const filter = email ? { Email: { $ne: email } } : {};


    // Find documents in the collection based on the filter
    const users = await collection.find(filter, { projection: { _id: 0, Image: 1, Username: 1, Status: 1 } }).toArray();

    // Send the user data as a JSON response
    res.json(users);
  } catch (error) {
    console.error('Error fetching user data:', error);
    res.status(500).json({ error: 'Internal Server Error' });
  } finally {
    // Close the MongoDB connection
    await client.close();
    console.log('Disconnected from MongoDB');
  }
});

// Endpoint to get specific users based on search text
app.get('/GetSpecificUsers', async (req, res) => {
  try {
    // Connect to MongoDB
    await client.connect();

    // Select the database
    const database = client.db('ReactApplicationDatabase');

    // Select the collection
    const collection = database.collection('UserProfile');

    // Extract search text from the request query parameters
    const { Search } = req.query;
    console.log(`the search text is ${Search}`);

    // Create a regex pattern for case-insensitive search
    const regexPattern = new RegExp(Search, 'i');

    // Define the filter for the query
    const filter = {
      Username: { $regex: regexPattern },
    };

    // Query MongoDB for matching users
    const users = await collection.find(filter, { projection: { _id: 0, Image: 1, Username: 1, Status: 1 } }).toArray();
    console.log(`fetched users are ${users}`);
    // Send the filtered user data to the frontend
    res.json(users);
  } catch (error) {
    console.error('Error:', error);
    res.status(500).send('Internal Server Error');
  }
});

// Map to store WebSocket connections
const connections = new Map();
const login_connections = new Map();

wss.on('connection', (ws) => {
  console.log('WebSocket connection opened');

  // Event listener for when the WebSocket connection is opened
  ws.on('message', async (message) => {
    console.log(`Received message: ${message}`);

    try {
      // Parse the JSON message
      const messageData = JSON.parse(message);

      // Extract data from the parsed message
      const { type } = messageData;
      console.log(`connected user ${messageData.username}`);

      // Handle different message types
      switch (type) {
        case 'userInfo':
          // Update or create a connection entry based on the user's name
          connections.set(messageData.username, ws);
          console.log(`All connections till now: ${[...connections.keys()]}`);
          // Check if the username is in the connections map before broadcasting
          if (login_connections.has(messageData.username)) {
            broadcastUserStatus(messageData.username, 'Online');
          }
          else {
            broadcastUserStatus(messageData.username, 'Offline');
          }
          break;

        case 'userMessage':
          // Handle userMessage type
          handleUserMessage(messageData);
          break;



        // Handle other message types as needed
        case 'myInfo':
          // Add yourself to the login_connections map
          login_connections.set(messageData.myUserName, ws);
          console.log(`Added ${messageData.myUserName} to login_connections`);

          // Now, you can use login_connections to send messages specifically to yourself
          // For example: login_connections.get(messageData.myUserName).send('Hello, yourself!');

          break;

        default:
          console.log('Unknown message type:', type);
          break;
      }

    } catch (error) {
      console.error('Error processing message:', error);
    }
  });



  ws.on('close', () => {
    console.log('WebSocket connection closed');

    // Remove the entry from the connections map when a connection is closed
    connections.forEach((value, key) => {
      if (value === ws) {
        console.log(`on close the values is ${value}`);
        console.log(`on close the key is ${key}`);
        connections.delete(key);
        broadcastUserStatus(key, 'Offline');
      }


    });
    login_connections.forEach((value, key) => {
      if (value === ws) {
        console.log(`on close the values is ${value}`);
        console.log(`on close the key is ${key}`);
        login_connections.delete(key);
      }
    });

  });
});


function broadcastUserStatus(username, status) {
  console.log(`for the user ${username} the status is ${status}`);
  wss.clients.forEach((client) => {
    if (client.readyState === WebSocket.OPEN) {
      client.send(JSON.stringify({
        type: 'userStatus',
        username,
        status,
      }));
    }
  });
}

async function handleUserMessage(messageData) {
  // Extract data from the userMessage
  const { UserFrom, UserTo, Message, Time } = messageData;

  // Log the received message
  console.log(`Received userMessage from ${UserFrom} to ${UserTo}: ${Message} at ${Time}`);

  // Send the userMessage to all connected users
  connections.forEach((socket, username) => {
    socket.send(JSON.stringify({
      type: 'userMessage',
      UserFrom: UserFrom,
      UserTo: UserTo,
      Message: Message,
      Time: Time,
    }));
  });

  // Assuming you have already connected to MongoDB using the 'client' instance
  await client.connect();

  // Select the database
  const database = client.db('ReactApplicationDatabase');

  // Select the collection
  const collection = database.collection('UserChat');

  // Create a document to insert into the collection
  const chatDocument = {
    UserFrom: UserFrom,
    UserTo: UserTo,
    Message: Message,
    Time: new Date(Time),
  };

  // Insert the document into the collection
  collection.insertOne(chatDocument, (err, result) => {
    if (err) {
      console.error('Error inserting document:', err);
      // Handle the error as needed
    } else {
      console.log('Document inserted successfully:', result);
      // Optionally, you can perform additional actions after the document is inserted
    }
  });
}



// Endpoint to handle GET request
app.post('/fetchUserChat', async (req, res) => {
  try {
    // Extract userFrom and userTo from req.query
    const { userFrom, userTo } = req.body;
    console.log(`userfrom: ${userFrom} and userTo: ${userTo}`);
    // Connect to MongoDB
    await client.connect();
    console.log('Connected to MongoDB');

    // Select the database
    const database = client.db('ReactApplicationDatabase');

    // Select the collection
    const collection = database.collection('UserChat');

    // Query the collection based on userFrom and userTo
    const query = {
      $or: [
        { UserFrom: userFrom, UserTo: userTo },
        { UserFrom: userTo, UserTo: userFrom },
      ],
    };
    console.log(query);

    const result = await collection.find(query).toArray();
    console.log(result);
    // Send the matching documents as the response
    res.json(result);
  } catch (error) {
    console.error('Error fetching user chat:', error);
    res.status(500).json({ error: 'Internal Server Error' });
  } finally {
    // Close the MongoDB connection when done
    await client.close();
    console.log('Disconnected from MongoDB');
  }
});


// Start the server
const port = 5000;
server.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});
