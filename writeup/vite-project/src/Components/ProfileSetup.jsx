import React, { useState } from 'react';
import styles from './Styles/ProfileSetup.module.css';
import Axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom'; // Import useParams

function ProfileSetup() {
    const { userId } = useParams(); // Get the userId from URL parameters
    const navigate = useNavigate();
    // State variables to store the data from input fields
    const [nickname, setNickname] = useState('');
    const [image, setImage] = useState(null);
    const [about, setAbout] = useState('');
    const [profession, setProfession] = useState('');

    // Event handler for the nickname input
    const handleNicknameChange = (e) => {
        setNickname(e.target.value);
    };

    // Event handler for the image input
    const handleImageChange = (e) => {
        // Handle image selection (e.target.files[0])
        setImage(e.target.files[0]);
    };

    // Event handler for the about input
    const handleAboutChange = (e) => {
        setAbout(e.target.value);
    };

    // Event handler for the profession input
    const handleProfessionChange = (e) => {
        setProfession(e.target.value);
    };

    // Function to handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Create a FormData object to send the data
        const formData = new FormData();
        formData.append('userId', userId); // Include userId in the FormData
        formData.append('nickname', nickname);
        formData.append('image', image);
        formData.append('about', about);
        formData.append('profession', profession);

        try {
            // Send a POST request to your server with the FormData
            const response = await Axios.post('http://localhost:3000/uploadProfile', formData);

            // Handle the response from the server as 
            // handle the success response from the server by redirecting to a new route and component 

            if (response.data) {
                // Assuming the server returns a success message or relevant data
                console.log('Success:', response.data);

                const { nickname } = response.data;

                navigate(`/Home/${nickname}`); // Include Nickname in the query parameter
            }

            else {            // Optionally, reset the form fields
                setNickname('');
                setImage(null);
                setAbout('');
                setProfession('');
            }
        } catch (error) {
            console.error('Error uploading profile:', error);
            // Handle errors from the server
        }
    };

    return (
        <div>
            <p className={styles.heading1}><span>w</span>riteup</p>
            <p className={styles.heading2}>Tell us a little about yourself</p>
            <form method='post' onSubmit={handleSubmit}>
                <div className={styles.nickname}>
                    <label>Choose a nickname</label>
                    <input
                        type='text'
                        placeholder='Type a nickname...'
                        autoComplete='off'
                        value={nickname}
                        onChange={handleNicknameChange}
                    />
                </div>
                <div className={styles.image}>
                    <label>Image for the profile</label>
                    <input
                        type="file"
                        accept="image/*"
                        onChange={handleImageChange}
                    />
                </div>
                <div className={styles.story}>
                    <label>Write something about yourself a little</label>
                    <textarea
                        placeholder='Type...'
                        value={about}
                        onChange={handleAboutChange}
                    ></textarea>
                </div>
                <div className={styles.identify}>
                    <label>Who you are?</label>
                    <input
                        type='text'
                        placeholder='Tell us about your work...'
                        autoComplete='off'
                        value={profession}
                        onChange={handleProfessionChange}
                    />
                </div>
                <button className={styles.uploadData} type='submit'>
                    Upload
                </button>
            </form>
        </div>
    );
}

export default ProfileSetup;
