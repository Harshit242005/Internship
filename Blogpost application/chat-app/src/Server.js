// here will go the express.js server code to send data and insert into the table 

import express from 'express';
import mysql from 'mysql2';
import cors from 'cors';
import path from 'path';
import fs from 'fs';
import multer from 'multer';
const app = express();
const port = 5000;
import nodemailer from 'nodemailer';

// MySQL connection configuration
const mysqlClient = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: 'azxcvbnmlkjhgfds',
  database: 'INTERNSHIP',
});

// Middleware to parse JSON data
app.use(express.json());
app.use(cors());
const upload = multer({ dest: 'uploads/' });

// ...

const transporter = nodemailer.createTransport({
  service: 'gmail',
  auth: {
    user: 'agreharshit610@gmail.com',
    pass: 'lbqxavlpxnewvczt'
  }
});


app.post('/signup', (req, res) => {
  // Retrieve email and password from the request body
  const { email, password } = req.body;

  // Generate a 6-digit verification code
  const verificationCode = Math.floor(100000 + Math.random() * 900000);

  const mailOptions = {
    from: 'agreharshit610@gmail.com',
    to: email,
    subject: 'Your Verification Code',
    text: `Hello, your verification code is: ${verificationCode}`
  };

  transporter.sendMail(mailOptions, (error, info) => {
    if (error) {
      console.log(error);
      res.status(500).send('Email sending failed.');
    } else {
      console.log('Email sent: ' + info.response);
      console.log(email, password, verificationCode);
      res.send({ email, password, verificationCode }); // Send the response with email, password, and verification code
    }
  });
});

app.post('/insertData', (req, res) => {
  const { email, password } = req.body;

  // Perform MySQL query to insert the data into the table
  const query = 'INSERT INTO Signup (Email, Password) VALUES (?, ?)';
  const values = [email, password];

  // Execute the query to insert data
  mysqlClient.query(query, values, (error, results) => {
    if (error) {
      console.error('Error inserting data:', error);
      res.status(500).send('Error inserting data.');
    } else {
      res.send('Data inserted successfully!');
    }
  });
});



app.post('/login', (req, res) => {
  const { email, password } = req.body;

  // Perform MySQL query to check if email and password match
  const query = 'SELECT Id FROM Signup WHERE Email = ? AND Password = ?';
  const values = [email, password];

  mysqlClient.query(query, values, (error, results) => {
    if (error) {
      console.error('Error logging in:', error);
      res.status(500).json({ error: 'Failed to log in' });
    } else {
      if (results.length === 1) {
        const id = results[0].Id;
        console.log('Login successful');
        res.status(200).json({ message: 'Login successful', id });
      } else {
        console.log('Invalid email or password');
        res.status(401).json({ error: 'Invalid email or password' });
      }
    }
  });
});

app.post('/create/:id', (req, res) => {
  const { id } = req.params;
  const { heading, description } = req.body;

  // Perform MySQL query to insert the data into the table
  const query = 'INSERT INTO Blogpost (Id, Heading, Description) VALUES (?, ?, ?)';
  const values = [id, heading, description];

  mysqlClient.query(query, values, (error, results) => {
    if (error) {
      console.error('Error signing up:', error);
      res.status(500).json({ error: 'Failed to sign up' });
    } else {
      console.log('Signup successful');
      res.status(200).json({ message: 'Signup successful' });
    }
  });
});

app.get('/cards', (req, res) => {
  // Perform MySQL query to retrieve data from the Blogpost table
  const query = 'SELECT * FROM Blogpost';

  mysqlClient.query(query, (error, results) => {
    if (error) {
      console.error('Error retrieving data:', error);
      res.status(500).json({ error: 'Failed to retrieve data' });
    } else {
      console.log('Data retrieved successfully');
      res.status(200).json(results);
    }
  });
});


app.post('/bookmark', (req, res) => {
  const number = req.body.number;
  const id = req.body.id;

  // Perform any desired operations with the number
  console.log(number);
  console.log(id);
  const query = `INSERT INTO Saved (Number, UserId) VALUES (?, ?)`
  mysqlClient.query(query, [number, id], (error, results) => {
    if (error) {
      console.error('Error inserting number:', error);
      // Handle the error if the insertion fails
      res.status(500).json({ message: 'Failed to insert number' });
    } else {
      console.log('Number inserted successfully');
      // Handle the success if the insertion is successful
      res.json({ message: 'Bookmark successful' });
    }
  });
  // Send a response back to the frontend

});

app.get('/savedCards', (req, res) => {
  const id = req.query.id;
  const query = `
    SELECT * 
    FROM Blogpost 
    WHERE Number IN (SELECT Number FROM Saved WHERE UserId = (?))
    
  `;

  mysqlClient.query(query, [id], (error, results) => {
    if (error) {
      console.error('Error retrieving data:', error);
      res.status(500).json({ error: 'Failed to retrieve data' });
    } else {
      console.log('Data retrieved successfully');
      res.status(200).json(results);
    }
  });
});




app.post('/delete', (req, res) => {
  const { cardNumber } = req.body;

  // Perform MySQL query to delete the number from the table
  const query = 'DELETE FROM Saved WHERE Number = ?';

  mysqlClient.query(query, [cardNumber], (error, results) => {
    if (error) {
      console.error('Error deleting number:', error);
      // Handle the error if the deletion fails
      res.status(500).json({ message: 'Failed to remove the blog from bookmark section' });
    } else {
      console.log('Removed');
      // Handle the success if the deletion is successful
      res.json({ message: 'Removed from bookmark' });
    }
  });
});


app.post('/Userdata', upload.single('image'), (req, res) => {
  const { id, nickname } = req.body;
  const image = req.file;

  // Read the image file
  fs.readFile(image.path, (err, data) => {
    if (err) {
      console.error('Error reading image file:', err);
      return res.status(500).json({ error: 'Failed to process image' });
    }

    // Insert data into the database
    const sql = 'INSERT INTO User (Id, Nickname, Image) VALUES (?, ?, ?)';
    mysqlClient.query(sql, [id, nickname, data], (err, result) => {
      if (err) {
        console.error('Error inserting data into database:', err);
        return res.status(500).json({ error: 'Failed to save data' });
      }
      console.log('Data saved successfully');
      res.status(200).json({ message: 'Data saved successfully' });
    });
  });
});

app.get('/getImage/:id', (req, res) => {
  const id = req.params.id;

  mysqlClient.query('SELECT Image FROM User WHERE Id = ?', [id], (error, result) => {
    if (error) {
      console.error('Error fetching user image: ', error);
      res.status(500).send('Internal error in fetching the image')
      return
    }

    if (result.length === 0 || !result[0].Image) {
      const filePath = path.resolve('./assets/Example.jpg');
      res.sendFile(filePath);
      return;
    }

    const imageBuffer = result[0].Image;
    res.type('jpg');
    res.send(imageBuffer);
  });
});

app.get('/checkId/:id', (req, res) => {
  const { id } = req.params;

  const query = 'SELECT COUNT(*) AS count FROM User WHERE Id = ?';
  mysqlClient.query(query, [id], (error, results) => {
    if (error) {
      console.error('Error checking ID:', error);
      res.status(500).json({ error: 'Internal Server Error' });
      return;
    }

    const exists = results[0].count > 0;
    res.json({ exists });
  });
});

app.get('/checkLike', (req, res) => {
  const { number, userId } = req.query;
  const query = `SELECT LikeId FROM Likes WHERE BlogpostNumber = ? AND UserId = ?`;

  mysqlClient.query(query, [number, userId], (err, results) => {
    if (err) {
      console.error('Error querying database:', err);
      res.status(500).json({ error: 'Failed to check LikeId status.' });
      return;
    }
    console.log(results);
    if (results.length === 0) {
      res.json({ likeId: 0 }); // LikeId not found
    } else {
      const likeId = results[0].LikeId;
      res.json({ likeId });
    }
  });
});


app.post('/like', (req, res) => {
  const number = req.body.number;
  const id = req.body.id;

  // Increment the Likes count for the specified blog post in the Blogpost table
  const updateQuery = `UPDATE Blogpost SET Likes = Likes + 1 WHERE Number = ?`;
  mysqlClient.query(updateQuery, [number], (error, results) => {
    if (error) {
      console.error('Error updating likes:', error);
      res.status(500).json({ message: 'Failed to update likes' });
    } else {
      console.log('Likes updated successfully');

      // Insert data into the Likes table
      const insertQuery = `INSERT INTO Likes (LikeId, BlogpostNumber, UserId) VALUES (1, ?, ?)`;
      mysqlClient.query(insertQuery, [number, id], (error, results) => {
        if (error) {
          console.error('Error inserting like:', error);
          res.status(500).json({ message: 'Failed to insert like' });
        } else {
          console.log('Like inserted successfully');
          res.json({ message: 'Like successful' });
        }
      });
    }
  });
});

// here i will run two different query and in that both would be get method queries and in 
// firts would be to get the users data and second would be to use  the 
// second would be the the data of the blogpst that, that users have created 

app.get('/User/:id', (req, res) => {
  const id = req.params.id;

  // Execute the SQL query with the provided id
  const query = `
    SELECT Signup.Email, User.Nickname
    FROM Signup
    JOIN User ON Signup.Id = User.Id
    WHERE Signup.Id = ?`;

  mysqlClient.query(query, [id], (err, results) => {
    if (err) {
      console.error('Error executing SQL query:', err);
      res.status(500).json({ error: 'Internal server error' });
      return;
    }

    // Send the query results to the front-end
    res.json(results);
  });
});


app.get('/blogposts/:id', (req, res) => {
  const id = req.params.id;

  // Execute the SQL query to fetch blog posts with matching id
  const query = `
    SELECT Number, Heading, Description, Likes
    FROM Blogpost
    WHERE Id = ?`;

  mysqlClient.query(query, [id], (err, results) => {
    if (err) {
      console.error('Error executing SQL query:', err);
      res.status(500).json({ error: 'Internal server error' });
      return;
    }

    // Send the query results to the front-end
    res.json(results);
  });

  app.delete('/blogposts/:number', (req, res) => {
    const number = req.params.number;

    // Execute the SQL query to delete the blog post with matching number
    const query = `
      DELETE FROM Blogpost
      WHERE Number = ?`;

    mysqlClient.query(query, [number], (err, result) => {
      if (err) {
        console.error('Error executing SQL query:', err);
        res.status(500).json({ error: 'Internal server error' });
        return;
      }

      // Send a success message to the front-end
      res.json({ message: 'Blog post deleted successfully' });
    });
  });

});


app.get('/getCode', (req, res) => {
  const mailOptions = {
    from: 'your-gmail-username@gmail.com',
    to: 'recipient@example.com',
    subject: 'Your email subject',
    text: 'Hello, this is the email content.'
  };

  transporter.sendMail(mailOptions, (error, info) => {
    if (error) {
      console.log(error);
      res.status(500).send('Email sending failed.');
    } else {
      console.log('Email sent: ' + info.response);
      res.send('Email sent successfully!');
    }
  });
});


// Start the server
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
