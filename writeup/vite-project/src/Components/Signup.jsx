import { React, useState } from 'react'
import styles from './Styles/Signup.module.css';
import Axios from 'axios'; // Import Axios for making HTTP requests
import { useNavigate } from 'react-router-dom';

function Signup() {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    // Event handler for email input
    const handleEmailChange = (e) => {
        setEmail(e.target.value);
    };

    // Event handler for password input
    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
    };


    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            // Send a POST request to your server with the email and password
            const response = await Axios.post('http://localhost:3000/signup', {
                email,
                password,
            });

            // Handle the response from the server as needed
            if (response.data.id > 0) {
                const userId = response.data.id; // Assuming the server sends the 'id' in the response

                // Navigate to the ProfileSetup route with the userId parameter
                navigate(`/ProfileSetup/${userId}`);

            }
            else {

                // Optionally, you can reset the form inputs after successful submission
                setEmail('');
                setPassword('');
            }
        } catch (error) {
            console.error('Error signing up:', error);
            // Handle errors from the server
        }
    };


    return (
        <div className={styles.Container}>
            <div className={styles.LeftContainer}>
                <p className={styles.heading1}>Writeup</p>

                <form method='post' onSubmit={handleSubmit}>
                    <label>Signup</label>
                    <div className={styles.email}>
                        <label>Email</label>
                        <input type="email" placeholder='Type your email...' value={email} onChange={handleEmailChange} />
                    </div>
                    <div className={styles.password}>
                        <label>Password</label>
                        <input type="password" placeholder='Type your password...' value={password} onChange={handlePasswordChange} />
                    </div>
                    <button className={styles.submitButton} type='submit'>Signup</button>
                </form>
            </div>
            <div className={styles.RightContainer}>
                <div className={styles.About}>
                    <p className={styles.heading2}><span>w</span>riteup</p>
                    <p className={styles.text4}>An easy application to start blogging and engaging with friends and making new ones</p>
                </div>
            </div>
        </div>
    )
}

export default Signup