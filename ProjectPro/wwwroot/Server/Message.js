const express = require("express");
const { Pool } = require('pg'); // Import the pg module
const app = express();
const port = 8080;
const cors = require('cors');
// Define your PostgreSQL database connection configuration
const pool = new Pool({
    user: 'postgres',
    host: 'localhost', // Usually 'localhost' if it's running locally
    database: 'projectpro',
    password: 'zxcvbnm',
    port: 5432, // PostgreSQL default port
});
app.use(cors());
// Middleware to parse JSON data from the request body
app.use(express.json());

// Define a POST route for "/getMessages"
app.post("/getMessages", async (req, res) => {
    console.log("called");
    try {
        // Extract data sent from the client
        const { projectId, number } = req.body;

        // Query the database to get messages and user IDs
        const query = {
            text: 'SELECT messagetext, userid FROM chat WHERE groupnumber = $1 AND projectid = $2',
            values: [number, projectId],
        };

        const { rows } = await pool.query(query);

        // Send the retrieved data as a response
        const responseData = { message: "Data received successfully", data: rows };
        res.json(responseData);
    } catch (error) {
        console.error('Error retrieving data from the database:', error);
        res.status(500).json({ message: "Internal server error" });
    }
});

// Start the Express server
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});
