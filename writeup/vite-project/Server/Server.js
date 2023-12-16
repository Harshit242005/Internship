const express = require('express');
const bodyParser = require('body-parser');
const { Pool } = require('pg');
const cors = require('cors');
const app = express();
const multer = require('multer');

const bcrypt = require('bcrypt'); // Import bcrypt for password hashing

// PostgreSQL database connection configuration
const pool = new Pool({
    user: 'postgres',
    host: 'localhost', // or the host where your PostgreSQL server is running
    database: 'writeup', // Name of your database
    password: 'zxcvbnm',
    port: 5432, // PostgreSQL default port
});

// Middleware
app.use(bodyParser.json());
app.use(cors());
// Define API endpoints here
// Handle user signup
app.post('/signup', async (req, res) => {
    const { email, password } = req.body;

    // Validate email and password (add validation logic here)

    try {
        // Hash the user's password before storing it
        const hashedPassword = await bcrypt.hash(password, 10); // 10 is the number of salt rounds

        // Insert the user into the "Users" table with the hashed password
        const result = await pool.query(
            'INSERT INTO Users (Email, Password) VALUES ($1, $2) RETURNING *',
            [email, hashedPassword] // Store the hashed password
        );
        console.log(result.rows[0]);
        // Send a success response
        res.status(200).json({ id: result.rows[0].id});
    } catch (error) {
        console.error('Error creating user:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});


// Handle user login
app.post('/login', async (req, res) => {
    const { email, password } = req.body;

    // Validate email and password (add validation logic here)

    try {
        // Check if the user with the provided email exists
        const user = await pool.query('SELECT id, email, password FROM Users WHERE Email = $1', [email]);

        if (user.rows.length === 0) {
            // User with the provided email does not exist
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        // Compare the hashed password in the database with the provided password
        const isPasswordValid = await bcrypt.compare(
            password, // Provided password
            user.rows[0].password // Hashed password from the database
        );

        if (!isPasswordValid) {
            // Passwords do not match
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        // Passwords match, user is authenticated
        // You can generate a JWT or a session here to manage user sessions

        // Now, let's retrieve the user's nickname from the profile table
        const profile = await pool.query('SELECT nickname FROM profile WHERE id = $1', [user.rows[0].id]);

        // Send the user's nickname along with the success response
        res.status(200).json({ message: 'Login successful', nickname: profile.rows[0].nickname });
    } catch (error) {
        console.error('Error during login:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// now we will handle that upload profile section to uphold and save the data directly 
// Set up Multer for handling file uploads
const storage = multer.memoryStorage(); // Store the file in memory
const upload = multer({ storage: storage });

// Handle profile data upload
app.post('/uploadProfile', upload.single('image'), async (req, res) => {
    const { userId, nickname, about, profession } = req.body;

    // Get the image data from the uploaded file
    const image = req.file.buffer; // This is the binary image data

    try {
        // Insert the profile data into the "profile" table
        const result = await pool.query(

            'INSERT INTO profile (id, nickname, image, about, profession) VALUES ($1, $2, $3, $4, $5) RETURNING *',

            [userId, nickname, image, about, profession]
        );

        // Send a success response
        res.status(201).json(result.rows[0]);
    } catch (error) {
        console.error('Error uploading profile:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// to get some data and send it to the server 
// Handle GET request to retrieve user data by nickname
app.get('/user/:nickname', async (req, res) => {

    const { nickname } = req.params;

    try {
        // Query the database to retrieve user data, including the image, by nickname
        const user = await pool.query('SELECT image FROM profile WHERE nickname = $1', [nickname]);

        if (user.rows.length === 0) {
            // If no user with the provided nickname is found, return a 404 status
            return res.status(404).json({ error: 'User not found' });
        }

        // Extract the user data, including the image, from the query result
        const userData = user.rows[0];

        // Send the user data as a JSON response
        res.status(200).json(userData);
    } catch (error) {
        console.error('Error retrieving user data:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});


// Set up Multer for handling file uploads
const blogImageStorage = multer.memoryStorage(); // Store the file in memory
const blogImageUpload = multer({ storage: blogImageStorage });

// Handle blog post creation
app.post('/create', blogImageUpload.single('image'), async (req, res) => {
    try {
        const { nickname, heading, text } = req.body;
        console.log(`the nickname is ${nickname}`);
        // Get the image data from the uploaded file
        const image = req.file.buffer; // This is the binary image data

        // Step 1: Retrieve the user's ID from the "profile" table based on the nickname
        const userQuery = await pool.query('SELECT id FROM profile WHERE nickname = $1', [nickname]);

        if (userQuery.rows.length === 0) {
            // If no user with the provided nickname is found, return an error response
            return res.status(404).json({ error: 'User not found' });
        }

        // Get the user's ID
        const userId = userQuery.rows[0].id;
        console.log(`the id is ${userId}`);
        // Step 2: Insert the blog post into the "blog" table
        const blogQuery = await pool.query(
            'INSERT INTO blog ("id", "heading", "image", "content", "date", "views", "likes") VALUES ($1, $2, $3, $4, $5, $6, $7) RETURNING *',
            [userId, heading, image, text, new Date(), 0, 0]
        );

        // Send a success response or any relevant data back to the client
        res.status(201).json({ message: 'Blog post created successfully', blog: blogQuery.rows[0] });
    } catch (error) {
        console.error('Error creating blog post:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});


// query to get all the blogs from the table to show up in the component code
// Create a GET endpoint to fetch all blog posts
app.get('/blogs', async (req, res) => {
    try {
        // Query the database to retrieve all blog posts
        // const blogs = await pool.query(`
        // SELECT b.*, p.nickname, encode(p.image, 'base64') as "Image"
        // FROM blog AS b
        // INNER JOIN profile AS p ON b."id" = p.id`);
        const blogs = await pool.query(`
    SELECT b.*, p.nickname, encode(b."image", 'base64') as "Image"
    FROM public.blog AS b
    INNER JOIN public.profile AS p ON b."id" = p."id";
`);

        // console.log(blogs.rows);
        // Send the blog posts as a JSON response
        res.status(200).json(blogs.rows);
    } catch (error) {
        console.error('Error retrieving blog posts:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});


app.get('/blog/:number', async (req, res) => { // Updated route pattern to '/blog/:blogId'
    try {
        const { number } = req.params;
        console.log(`number is ${number}`);
        // console.log(`the number is ${number}`);
        const incrementViewsQuery = {
            text: 'UPDATE blog SET views = views + 1 WHERE number = $1',
            values: [number],
        };

        // Execute the query to increment views
        await pool.query(incrementViewsQuery);

        // Query the database to retrieve the blog data by its ID
        const query = {
            text: 'SELECT * FROM blog WHERE number = $1',
            values: [number],
        };

        const result = await pool.query(query);
        
        // Check if a blog with the specified ID exists
        if (result.rows.length === 0) {
            return res.status(404).json({ message: 'Blog not found' });
        }

        // Get the blog data from the first (and only) row of the result
        const blogData = result.rows[0];
        console.log(`the blog data is ${blogData}`);
        // Send the blog data as a JSON response
        res.status(200).json(blogData);
    } catch (error) {
        console.error('Error fetching blog data:', error);
        res.status(500).json({ message: 'Internal server error' });
    }
});


app.get('/blog/likeStatus/:number/:nickname', async (req, res) => {
    try {
      const { number, nickname } = req.params;
      console.log(number, nickname);
      // Query the profile table to get the id corresponding to the nickname
      const profileQuery = {
        text: 'SELECT id FROM profile WHERE nickname = $1',
        values: [nickname],
      };
  
      const profileResult = await pool.query(profileQuery);
  
      if (profileResult.rows.length === 0) {
        return res.status(404).json({ message: 'User not found' });
      }
  
      const userId = profileResult.rows[0].id;
      console.log(userId);
      // Query the likes table to check if there is a matching record
      const likeQuery = {
        text: 'SELECT * FROM likes WHERE blogid = $1 AND userid = $2',
        values: [number, userId],
      };
  
      const likeResult = await pool.query(likeQuery);
  
      // Check if a matching record exists and return true, otherwise return false
      const likeStatus = likeResult.rows.length > 0;
      console.log(likeStatus);
      res.status(200).json({ isLikedStatus: likeStatus });
    } catch (error) {
      console.error('Error checking like status:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  });
  


// like and neutral query codes endpoint creation 
// Define the route to increase likes and update Likes table
app.post('/blog/like/increase/:number/:nickname', async (req, res) => {
    try {
        const { number, nickname } = req.params;

        console.log(`the id is ${number} and the nickname is ${nickname}`);
        // Update the likes column in the blog table
        const updateBlogLikesQuery = {
            text: 'UPDATE blog SET likes = likes + 1 WHERE number = $1',
            values: [number],
        };

        // Execute the query to update likes in the blog table
        await pool.query(updateBlogLikesQuery);


        const getUserIdQuery = {
            text: 'SELECT id FROM profile WHERE nickname = $1',
            values: [nickname],
        };


        // Execute the query to get the user ID
        const result = await pool.query(getUserIdQuery);
        console.log(`the result is ${result}`);
        if (result.rows.length === 0) {
            return res.status(404).json({ message: 'User not found' });
        }

        const userId = result.rows[0].id;
        console.log(`getting id for the nickname is ${userId}`);
        // Insert a new row in the Likes table
        const insertLikesQuery = {
            text: 'INSERT INTO likes (blogid, userid) VALUES ($1, $2)',
            values: [number, userId],
        };

        // Execute the query to insert a new row in the Likes table
        await pool.query(insertLikesQuery);

        // Respond with a success message
        res.status(200).json({ message: 'Likes increased successfully' });
    } catch (error) {
        console.error('Error increasing likes:', error);
        res.status(500).json({ message: 'Internal server error' });
    }
});

app.post('/blog/like/decrease/:number/:nickname', async (req, res) => {
    try {
        const { number, nickname } = req.params;
        console.log(`the id is ${number} and the nickname is ${nickname}`);

        // Decrease the likes column in the blog table
        const decreaseBlogLikesQuery = {
            text: 'UPDATE blog SET likes = likes - 1 WHERE number = $1',
            values: [number],
        };

        // Execute the query to decrease likes in the blog table
        await pool.query(decreaseBlogLikesQuery);

        // Retrieve the user ID from the profile table using the nickname
        const getUserIdQuery = {
            text: 'SELECT id FROM profile WHERE nickname = $1',
            values: [nickname],
        };

        // Execute the query to get the user ID
        const result = await pool.query(getUserIdQuery);
        console.log(`the result is ${result}`);
        if (result.rows.length === 0) {
            return res.status(404).json({ message: 'User not found' });
        }

        const userId = result.rows[0].id;
        console.log(`getting id for the nickname is ${userId}`);

        // Delete the row from the Likes table where UserId matches
        const deleteLikesQuery = {
            text: 'DELETE FROM likes WHERE blogid = $1 AND userid = $2',
            values: [number, userId],
        };

        // Execute the query to delete the row from the Likes table
        await pool.query(deleteLikesQuery);

        // Respond with a success message
        res.status(200).json({ message: 'Likes decreased successfully' });
    } catch (error) {
        console.error('Error decreasing likes:', error);
        res.status(500).json({ message: 'Internal server error' });
    }
});


app.get('/blog/bookmarkStatus/:number/:nickname', async (req, res) => {
    try {
      const { number, nickname } = req.params;
      console.log(number, nickname);
      // Query the profile table to get the id corresponding to the nickname
      const profileQuery = {
        text: 'SELECT id FROM profile WHERE nickname = $1',
        values: [nickname],
      };
  
      const profileResult = await pool.query(profileQuery);
  
      if (profileResult.rows.length === 0) {
        return res.status(404).json({ message: 'User not found' });
      }
  
      const userId = profileResult.rows[0].id;
      console.log(userId);
      // Query the likes table to check if there is a matching record
      const likeQuery = {
        text: 'SELECT * FROM bookmarked WHERE blogid = $1 AND userid = $2',
        values: [number, userId],
      };
  
      const likeResult = await pool.query(likeQuery);
  
      // Check if a matching record exists and return true, otherwise return false
      const bookmarkStatus = likeResult.rows.length > 0;
      console.log(`bookmark status ${bookmarkStatus}`);
      res.status(200).json({ isBookmarkStatus: bookmarkStatus });
    } catch (error) {
      console.error('Error checking like status:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  });
  


// let's define the endpoint's for the bookmark
// like and neutral query codes endpoint creation 
// Define the route to increase likes and update Likes table
app.post('/blog/bookmark/add/:number/:nickname', async (req, res) => {
    try {
        const { number, nickname } = req.params;
        console.log(`the id is ${number} and the nickname is ${nickname}`);
        // Update the likes column in the blog table



        const getUserIdQuery = {
            text: 'SELECT id FROM profile WHERE nickname = $1',
            values: [nickname],
        };


        // Execute the query to get the user ID
        const result = await pool.query(getUserIdQuery);
        console.log(`the result is ${result}`);
        if (result.rows.length === 0) {
            return res.status(404).json({ message: 'User not found' });
        }

        const userId = result.rows[0].id;
        console.log(`getting id for the nickname is ${userId}`);
        // Insert a new row in the Likes table
        const insertbookmarkedQuery = {
            text: 'INSERT INTO bookmarked (blogid, userid) VALUES ($1, $2)',
            values: [number, userId],
        };

        // Execute the query to insert a new row in the Likes table
        await pool.query(insertbookmarkedQuery);

        // Respond with a success message
        res.status(200).json({ message: 'bookmarked increased successfully' });
    } catch (error) {
        console.error('Error increasing likes:', error);
        res.status(500).json({ message: 'Internal server error' });
    }
});

app.post('/blog/bookmark/remove/:number/:nickname', async (req, res) => {
    try {
        const { number, nickname } = req.params;


        // Retrieve the user ID from the profile table using the nickname
        const getUserIdQuery = {
            text: 'SELECT id FROM profile WHERE nickname = $1',
            values: [nickname],
        };

        // Execute the query to get the user ID
        const result = await pool.query(getUserIdQuery);
        console.log(`the result is ${result}`);
        if (result.rows.length === 0) {
            return res.status(404).json({ message: 'User not found' });
        }

        const userId = result.rows[0].id;
        console.log(`getting id for the nickname is ${userId}`);

        // Delete the row from the Likes table where UserId matches
        const deletebookmarkedQuery = {
            text: 'DELETE FROM bookmarked WHERE blogid = $1 AND userid = $2',
            values: [number, userId],
        };

        // Execute the query to delete the row from the Likes table
        await pool.query(deletebookmarkedQuery);

        // Respond with a success message
        res.status(200).json({ message: 'bookmarked decreased successfully' });
    } catch (error) {
        console.error('Error decreasing likes:', error);
        res.status(500).json({ message: 'Internal server error' });
    }
});

// endpoint to send more blogs form that same person
app.get('/ExtraBlog/:nickname', async (req, res) => {
    try {
        const { nickname } = req.params;
        // console.log(`the nickname is ${nickname}`);
        // First, get the user's ID from the profile table using their nickname
        const getUserIdQuery = {
            text: 'SELECT id FROM profile WHERE nickname = $1',
            values: [nickname],
        };

        // Execute the query to get the user's ID
        const userIdResult = await pool.query(getUserIdQuery);
        
        if (userIdResult.rows.length === 0) {
            return res.status(404).json({ message: 'User not found' });
        }

        const userId = userIdResult.rows[0].id;
        // console.log(`get user id is ${userId}`);
        // Now, fetch all blog data for the user with the matching ID
        const getBlogDataQuery = {
            text: 'SELECT * FROM blog WHERE id = $1',
            values: [userId],
        };

        // Execute the query to get the blog data
        const blogDataResult = await pool.query(getBlogDataQuery);

        // Check if any blog data was found
        if (blogDataResult.rows.length === 0) {
            return res.status(404).json({ message: 'No blog data found for this user' });
        }
        // console.log("extra blog data");
        // console.log(blogDataResult.rows);
        // Respond with the blog data
        res.status(200).json(blogDataResult.rows);
    } catch (error) {
        console.error('Error fetching blog data:', error);
        res.status(500).json({ message: 'Internal server error' });
    }
});


// get liked data 
app.get('/LikedBlogs/:nickname', async (req, res) => {
    try {
        const nickname = req.params.nickname;
        console.log(`the nickname is ${nickname}`);
        // Use the nickname to retrieve the user's ID from the profile table
        const profileQuery = await pool.query(
            'SELECT id FROM profile WHERE nickname = $1',
            [nickname]
        );

        if (profileQuery.rows.length === 0) {
            return res.status(404).json({ error: 'User not found' });
        }

        const userId = profileQuery.rows[0].id;
     
        console.log(`the user id is ${userId}`);
        // Use the user's ID to retrieve the blog IDs from the likes table
        const likesQuery = await pool.query(
            'SELECT blogid FROM likes WHERE userid = $1',
            [userId]
        );

        // Use the blog IDs to retrieve the blog data from the blog table
        const blogIds = likesQuery.rows.map((row) => row.blogid);
        console.log(`the blog id are ${blogIds}`);
        if (blogIds.length === 0) {
            // If the user hasn't liked any blogs, send a specific message to the front end
            console.log("there is not any blog that user have liked right now");
        }


        // Use the blog IDs to retrieve the blog data from the blog table
        const blogsQuery = await pool.query(
            'SELECT * FROM blog WHERE Number = ANY($1)',
            [blogIds]
        );

        const likedBlogs = blogsQuery.rows;
        console.log(likedBlogs);

        res.status(200).json(likedBlogs);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Internal server error' });
    }
});


app.get('/BookmarkedBlogs/:nickname', async (req, res) => {
    try {
        const nickname = req.params.nickname;
        console.log(`the nickname is ${nickname}`);
        // Use the nickname to retrieve the user's ID from the profile table
        const profileQuery = await pool.query(
            'SELECT id FROM profile WHERE nickname = $1',
            [nickname]
        );

        if (profileQuery.rows.length === 0) {
            return res.status(404).json({ error: 'User not found' });
        }

        const userId = profileQuery.rows[0].id;
     
        console.log(`the user id is ${userId}`);
        // Use the user's ID to retrieve the blog IDs from the likes table
        const likesQuery = await pool.query(
            'SELECT blogid FROM bookmarked WHERE userid = $1',
            [userId]
        );

        // Use the blog IDs to retrieve the blog data from the blog table
        const blogIds = likesQuery.rows.map((row) => row.blogid);
        console.log(`the blog id are ${blogIds}`);
        if (blogIds.length === 0) {
            // If the user hasn't liked any blogs, send a specific message to the front end
            console.log("there is not any blog that user have liked right now");
        }


        // Use the blog IDs to retrieve the blog data from the blog table
        const blogsQuery = await pool.query(
            'SELECT * FROM blog WHERE Number = ANY($1)',
            [blogIds]
        );

        const likedBlogs = blogsQuery.rows;
        console.log(likedBlogs);

        res.status(200).json(likedBlogs);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Internal server error' });
    }
});




// profile handling endpoint 
app.get('/profile/:nickname', async (req, res) => {
    try {
        const { nickname } = req.params;
        // console.log(`the nickname is ${nickname}`);

        // Query the database for the profile data matching the nickname
        const query = 'SELECT * FROM profile WHERE nickname = $1';
        const profileData = await pool.query(query, [nickname]);

        if (profileData.rows.length > 0) {
            // If a profile is found, send the first result as JSON response
            // console.log(profileData.rows[0]);
            res.json(profileData.rows[0]);
        } else {
            // If no matching profile is found, send a 404 response
            res.status(404).json({ error: 'Profile not found' });
        }
    } catch (error) {
        console.error('Error fetching profile data:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});


app.get('/profileBlog/:nickname', async (req, res) => {
    const { nickname } = req.params;
    console.log(nickname);
    try {
        // Step 1: Get the user's id from the profile table based on the nickname
        const profileQuery = await pool.query(
            'SELECT id FROM profile WHERE nickname = $1',
            [nickname]
        );

        if (profileQuery.rows.length === 0) {
            return res.status(404).json({ error: 'Profile not found' });
        }

        const userId = profileQuery.rows[0].id;
        console.log(`the userId is ${userId}`);
        // Step 2: Get all blogs that match the user's id from the blog table
        const blogsQuery = await pool.query(
            'SELECT * FROM blog WHERE id = $1',
            [userId]
        );

        const userBlogs = blogsQuery.rows;
        console.log("the blog data is");
        console.log(userBlogs);
        res.status(200).json(userBlogs);
    } catch (error) {
        console.error('Error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});



// Start the server
app.listen(3000, () => {
    console.log(`Server is running on port 3000`);
});
