import React, { useState, useEffect } from 'react';
import styles from './Styles/Home.module.css';
import axios from 'axios'; // Import axios
import { Link, useParams } from 'react-router-dom';

function Home() {
    const { nickname } = useParams();
    const [userData, setUserData] = useState({});
    const [blogPosts, setBlogPosts] = useState([]);
    // for adjust view
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


    useEffect(() => {
        // Fetch blog posts from your server
        axios.get('http://localhost:3000/blogs')
            .then((response) => {
                // Update the blogPosts state with the fetched data
                console.log(response.data);
                setBlogPosts(response.data);
            })
            .catch((error) => {
                console.error('Error fetching blog posts:', error);
            });
    }, []); // The empty dependency array ensures this effect runs once after mounting


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
                    <Link className="navbar-brand" to={`/Profile/${nickname}`}>
                        {userData.image && (
                            <img
                                className={styles.NavbarImage} // Make sure NavbarImage is defined in your CSS
                                src={`data:image/png;base64,${arrayBufferToBase64(
                                    userData.image.data
                                )}`}
                                alt="User's Profile"
                            />
                        )}
                    </Link>
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
                                <Link className="nav-link" to={`/Liked/${nickname}`}>
                                    Liked
                                </Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link" to={`/Bookmarks/${nickname}`}>
                                    Bookmarks
                                </Link>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <div className={styles.blogs}>

                {blogPosts.map((blogPost) => (

                    <Link to={`/Blog/${blogPost.number}/${nickname}`} className={styles.blogPostLink}>
                        <div key={blogPost.id} className={styles.blogPost}>

                            <div className={styles.blogPostHeadData}>
                                <div className={styles.imageSection}>
                                    <img
                                        className={styles.BlogPostImageData}
                                        src={`data:image/png;base64,${blogPost.Image}`}
                                        alt="User's Profile"
                                    />
                                </div>
                                <div className='BlogData'>
                                    <p className={styles.BlogDataNickname}>{blogPost.nickname}</p>
                                    <p className={styles.BlogDataHeading}>{blogPost.heading}</p>
                                    <p className={styles.BlogDataContent}>{blogPost.content}</p>
                                </div>
                            </div>
                            <div className={styles.BlogPostStatcis}>
                                <div className={styles.likeaAndViews}>
                                    <div className={styles.Views}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 25 25" fill="none">
                                            <path d="M12.5 9.375C11.6712 9.375 10.8763 9.70424 10.2903 10.2903C9.70424 10.8763 9.375 11.6712 9.375 12.5C9.375 13.3288 9.70424 14.1237 10.2903 14.7097C10.8763 15.2958 11.6712 15.625 12.5 15.625C13.3288 15.625 14.1237 15.2958 14.7097 14.7097C15.2958 14.1237 15.625 13.3288 15.625 12.5C15.625 11.6712 15.2958 10.8763 14.7097 10.2903C14.1237 9.70424 13.3288 9.375 12.5 9.375ZM12.5 17.7083C11.1187 17.7083 9.79391 17.1596 8.81716 16.1828C7.8404 15.2061 7.29167 13.8813 7.29167 12.5C7.29167 11.1187 7.8404 9.7939 8.81716 8.81715C9.79391 7.8404 11.1187 7.29167 12.5 7.29167C13.8813 7.29167 15.2061 7.8404 16.1829 8.81715C17.1596 9.7939 17.7083 11.1187 17.7083 12.5C17.7083 13.8813 17.1596 15.2061 16.1829 16.1828C15.2061 17.1596 13.8813 17.7083 12.5 17.7083ZM12.5 4.6875C7.29167 4.6875 2.84376 7.92708 1.04167 12.5C2.84376 17.0729 7.29167 20.3125 12.5 20.3125C17.7083 20.3125 22.1563 17.0729 23.9583 12.5C22.1563 7.92708 17.7083 4.6875 12.5 4.6875Z" fill="#699BF7" />
                                        </svg>
                                        <p className={styles.staticNumber}>{parseInt(blogPost.views / 2)}</p>
                                    </div>
                                    <div className={styles.Likes}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 25 25" fill="none">
                                            <path d="M17.7083 4.6875C15.5208 4.6875 13.5938 5.78125 12.5 7.5C11.4063 5.78125 9.47917 4.6875 7.29167 4.6875C3.85417 4.6875 1.04167 7.5 1.04167 10.9375C1.04167 17.1354 12.5 23.4375 12.5 23.4375C12.5 23.4375 23.9583 17.1875 23.9583 10.9375C23.9583 7.5 21.1458 4.6875 17.7083 4.6875Z" fill="#F44336" />
                                        </svg>
                                        <p className={styles.staticNumber}>{blogPost.likes}</p>
                                    </div>
                                </div>
                                <div>
                                    <p className={styles.BlogDataDate}>Post - {new Date(blogPost.date).toLocaleDateString()}</p>
                                </div>

                            </div>

                            {/* Include other content from the blog post as needed */}
                        </div>
                    </Link>

                ))}
            </div>


        </div>
    );
}

export default Home;
