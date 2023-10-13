const WebSocket = require('ws');
const http = require('http');
const { Pool } = require('pg');

// Define your PostgreSQL database connection configuration
const pool = new Pool({
    user: 'postgres',
    host: 'localhost', // Usually 'localhost' if it's running locally
    database: 'projectpro',
    password: 'zxcvbnm',
    port: 5432, // PostgreSQL default port
});

// Optional: Handle errors on the pool
pool.on('error', (err) => {
    console.error('Unexpected error on idle client', err);
    process.exit(-1);
});
// Create an HTTP server
const server = http.createServer((req, res) => {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('WebSocket server is running');
});

// Create a WebSocket server by passing the HTTP server
const wss = new WebSocket.Server({ server });

// Store connected clients (you can use a database for scalability)
const clients = new Set();



// Handle WebSocket connections
wss.on('connection', (ws) => {
    // Add the connected client to the set
    clients.add(ws);

    
    
    console.log(`Number of connected clients: ${wss.clients.size}`);
    if (wss.clients.size > 0) {
        function broadcastMessage(json) {
            const data = JSON.stringify(json);
            wss.clients.forEach((client) => {
                if (client.readyState === WebSocket.OPEN) {
                    // Send the message to other clients
                    
                    try {
                        client.send(data);
                       
                    } catch (error) {
                        // Handle the error when sending a message to a client
                        console.error("Error sending data to a client:", error);
                    }
                }
            });
        }

    } else {
        // There are no connected clients, so you cannot send any messages
        console.log("No clients are connected right now");
    }

    // Listen for messages from the client
    ws.on('message', async (message) => {
        try {
            // Parse the JSON message sent from the client
            const data = JSON.parse(message);
            console.log(data);
            // Extract the relevant fields
            const { number, projectId, id, messageText } = data;

            // Insert the extracted fields into the chat table
            await pool.query(
                'INSERT INTO chat (groupnumber, projectid, userid, messagetext) VALUES ($1, $2, $3, $4)',
                [number ,projectId, id, messageText]
            );
            broadcastMessage(data);
            // Send the complete data object as JSON to all connected clients
            
        } catch (error) {
            console.error('Error parsing or processing message:', error);
        }
    });



    // Handle WebSocket closing
    ws.on('close', () => {
        // Remove the disconnected client from the set
        clients.delete(ws);
    });
});


// Start the HTTP server on a port
const port = 3000;
server.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});
