import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Link, useParams } from 'react-router-dom';
import styles from './Styles/Liked.module.css';

function Bookmark() {
    const { nickname } = useParams();

    const [likedBlogPosts, setLikedBlogPosts] = useState([]);

    useEffect(() => {
        // Send a GET request to retrieve liked blog posts by nickname
        axios.get(`http://localhost:3000/BookmarkedBlogs/${nickname}`)
            .then((response) => {
                const likedPostsData = response.data;

                // Assuming the response is an array of liked blog posts
                setLikedBlogPosts(likedPostsData);


            })
            .catch((error) => {
                console.error('Error:', error);
                // Handle errors, such as displaying a message or redirecting to an error page
            });
    }, [nickname]);

    // Function to display base64-encoded image data
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
            <Link to={`/Home/${nickname}`}><button className={styles.backButton}>Back</button></Link>
            <p className={styles.LikedBlogheader}>Bookamrked Blogs By {nickname}</p>
            <div className={styles.LikedBlogs}>

                {likedBlogPosts.map((post, index) => (
                    <div key={index} className={styles.LikedBlog}>
                        <div className={styles.LikedBlogHeadData}>
                            {post.image && (
                                <img
                                    src={`data:image/png;base64,${arrayBufferToBase64(post.image.data)}`}
                                    alt="Blog Image"
                                    className={styles.LikedBlogImage}
                                />
                            )}
                            <div className={styles.LikedBlogtextData}>
                                <h2>{post.heading}</h2>
                                <p className={styles.LikedBlogTextContent}>{post.content}</p>
                            </div>
                            <div className={styles.statcis}>
                                <div className={styles.views}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 25 25" fill="none">
                                        <path d="M12.5 9.375C11.6712 9.375 10.8763 9.70424 10.2903 10.2903C9.70424 10.8763 9.375 11.6712 9.375 12.5C9.375 13.3288 9.70424 14.1237 10.2903 14.7097C10.8763 15.2958 11.6712 15.625 12.5 15.625C13.3288 15.625 14.1237 15.2958 14.7097 14.7097C15.2958 14.1237 15.625 13.3288 15.625 12.5C15.625 11.6712 15.2958 10.8763 14.7097 10.2903C14.1237 9.70424 13.3288 9.375 12.5 9.375ZM12.5 17.7083C11.1187 17.7083 9.79391 17.1596 8.81716 16.1828C7.8404 15.2061 7.29167 13.8813 7.29167 12.5C7.29167 11.1187 7.8404 9.7939 8.81716 8.81715C9.79391 7.8404 11.1187 7.29167 12.5 7.29167C13.8813 7.29167 15.2061 7.8404 16.1829 8.81715C17.1596 9.7939 17.7083 11.1187 17.7083 12.5C17.7083 13.8813 17.1596 15.2061 16.1829 16.1828C15.2061 17.1596 13.8813 17.7083 12.5 17.7083ZM12.5 4.6875C7.29167 4.6875 2.84376 7.92708 1.04167 12.5C2.84376 17.0729 7.29167 20.3125 12.5 20.3125C17.7083 20.3125 22.1563 17.0729 23.9583 12.5C22.1563 7.92708 17.7083 4.6875 12.5 4.6875Z" fill="#699BF7" />
                                    </svg>
                                    <p>{parseInt(post.views / 2)}</p>
                                </div>
                                <div className={styles.likes}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 25 25" fill="none">
                                        <path d="M17.7083 4.6875C15.5208 4.6875 13.5938 5.78125 12.5 7.5C11.4063 5.78125 9.47917 4.6875 7.29167 4.6875C3.85417 4.6875 1.04167 7.5 1.04167 10.9375C1.04167 17.1354 12.5 23.4375 12.5 23.4375C12.5 23.4375 23.9583 17.1875 23.9583 10.9375C23.9583 7.5 21.1458 4.6875 17.7083 4.6875Z" fill="#F44336" />
                                    </svg>
                                    <p>{post.likes}</p>
                                </div>
                                <div>
                                    <p>Post - {new Date(post.date).toLocaleDateString()}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Bookmark