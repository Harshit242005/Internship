
import React, { useState, useEffect } from 'react';
import styles from './Styles/Create.module.css';
import axios from 'axios'; // Import axios
import { Link, useParams, useNavigate } from 'react-router-dom';
function Create() {
    const { nickname } = useParams();
    const [userData, setUserData] = useState({});
    const navigate = useNavigate();
    useEffect(() => {
        // Send a GET request to retrieve user data by nickname
        axios.get(`http://localhost:3000/user/${nickname}`)
            .then((response) => {
                const userData = response.data;

                // Set the user data in state, including the image
                setUserData(userData);
            })
            .catch((error) => {
                console.error('Error:', error);
                // Handle errors, such as displaying a message or redirecting to an error page
            });
    }, [nickname]);

    
    const [image, setImage] = useState(null);
    const [heading, setHeading] = useState('');
    const [text, setText] = useState('');

    const handleImageChange = (e) => {
        setImage(e.target.files[0]);
    };

    const handleHeadingChange = (e) => {
        setHeading(e.target.value);
    };

    const handleTextChange = (e) => {
        setText(e.target.value);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Create a FormData object to send the data
        const formData = new FormData();
        formData.append('nickname', nickname);
        formData.append('image', image);
        formData.append('heading', heading);
        formData.append('text', text);

        try {
            // Send a POST request to your server with the FormData
            const response = await axios.post('http://localhost:3000/create', formData);

            if (response.data.message === 'Blog post created successfully') {
                // Handle success, e.g., redirect to a confirmation page or clear the form
                navigate(`/Home/${nickname}`);
            } else {
                setImage(null);
                setText('');
                setHeading('');
            }
        } catch (error) {
            console.error('Error creating blog post:', error);
            // Handle errors from the server
        }
    };

    // Define a function to convert the binary image data to a base64 URL
    function arrayBufferToBase64(buffer) {
        let binary = '';
        const bytes = new Uint8Array(buffer);
        for (let i = 0; i < bytes.byteLength; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return btoa(binary);
    }
    return (
        <div>
            <nav className="navbar navbar-expand-lg bg-body-tertiary">
                <div className="container-fluid">
                    <a className="navbar-brand" href="#">
                        {userData.Image && (
                            <img
                                className={styles.NavbarImage} // Make sure NavbarImage is defined in your CSS
                                src={`data:image/png;base64,${arrayBufferToBase64(
                                    userData.Image.data
                                )}`}
                                alt="User's Profile"
                            />
                        )}
                    </a>
                    <button
                        className="navbar-toggler"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#navbarSupportedContent"
                        aria-controls="navbarSupportedContent"
                        aria-expanded="false"
                        aria-label="Toggle navigation"
                    >
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                            <li className="nav-item">
                                <a className="nav-link active" aria-current="page" href="#">
                                    Blogs
                                </a>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link" to={`/Create/${nickname}`}>
                                    Create
                                </Link>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="#">
                                    Liked
                                </a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="#">
                                    Bookmarks
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <p className={styles.heading1}>Create a new blogpost</p>
            <form method='post' onSubmit={handleSubmit}>
                <div className={styles.Image}>
                    <label>Image for the blogpost to show</label>
                    <input type="file" onChange={handleImageChange} />
                </div>
                <div className={styles.heading2}>
                    <label>Heading for the blogpost</label>
                    <input type="text" placeholder='Type the heading...' autoComplete='off' value={heading} onChange={handleHeadingChange} />
                </div>
                <div className={styles.text}>
                    <label>Text for the blogpost</label>
                    <textarea placeholder='Text...' value={text} onChange={handleTextChange}></textarea>
                </div>
                <button className={styles.createButton} type='submit'>Create</button>
            </form>
        </div>
    )
}

export default Create