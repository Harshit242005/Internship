// const WebSocket = require('ws');

// const server = new WebSocket.Server({ port: 3000 });

// // Event listener for when a client connects
// server.on('connection', (socket, request) => {
//     console.log('A client has connected');

//     // Event listener for when a client disconnects
//     socket.on('close', () => {
//         console.log('A client has disconnected');
//     });

//     // Event listener for when a message is received from the client
//     socket.on('message', (message) => {
//         console.log('Message from client:', message);

//         // Here, you can process the received message if needed
//     });
// });



const WebSocket = require('ws');

const server = new WebSocket.Server({ port: 3000 });

const connectedClients = new Set();

// Event listener for when a client connects
server.on('connection', (socket, request) => {
    console.log('A client has connected');
    connectedClients.add(socket);
    // Store the socket in the clients map
   

    // Event listener for when a client disconnects
    socket.on('close', () => {
        console.log('A client has disconnected');
        connectedClients.delete(socket);
    });

    // Event listener for when a message is received from the client
    socket.on('message', (message) => {
        console.log('Message from client:', message.toString());

        const response = 'Hello from the server!';
        socket.send(response);
        // Process the received message if needed
    });
});

// Function to send a message to all connected clients

