import React, { useState } from 'react';
import axios from 'axios'; // Import Axios
import { useNavigate } from 'react-router-dom'; // Import useNavigate


function Login() {
    // State variables for email and password
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate(); // Create a navigate function

    // Function to handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            // Send the form data to the Express.js server
            const response = await axios.post('http://localhost:5000/LoginUser', {
                email,
                password,
            });

            // Handle the server response as needed
            console.log('Server response:', response.data);
            if (response.data.success) {
                console.log(response.data.userMe);
                // If login is successful, navigate to /ChatInterface
                navigate('/ChatInterface', {state: {email, userMe: response.data.userMe }});
            } else {
                // Handle unsuccessful login (show a message, etc.)
                console.log('Login failed:', response.data.message);
            }
        } catch (error) {
            // Handle errors
            console.error('Error submitting form:', error);
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <button type="submit">Login</button>
                </div>
            </form>
        </div>
    );
}

export default Login;
