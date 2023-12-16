import React, { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import axios from 'axios';
import styles from './Styles/Profile.module.css';

function Profile() {
    const { nickname } = useParams();
    const [profileBlogData, setProfileBlogData] = useState([]);
    const [profileData, setProfileData] = useState(null);
    // state to store the data that will come from the 

    useEffect(() => {
        axios.get(`http://localhost:3000/profileBlog/${nickname}`)
            .then((response) => {
                const profileBlogData = response.data;
                setProfileBlogData(profileBlogData);
            })
            .catch((error) => {
                console.error("Error is : ", error);
            })
    }, [nickname]);

    useEffect(() => {
        axios.get(`http://localhost:3000/profile/${nickname}`)
            .then((response) => {
                const userData = response.data;
                setProfileData(userData);
            })
            .catch((error) => {
                console.error("Error is :", error);
            })
    }, [nickname]);




    // profile image
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
            {profileData && (
                <div className={styles.profileHeader}>
                    <div className={styles.profileImageText}>
                        {profileData.image && (
                            <img
                                src={`data:image/png;base64,${arrayBufferToBase64(profileData.image.data)}`}
                                alt="Profile Image"
                                className={styles.profileImage}
                            />
                        )}
                        <div className={styles.profileText}>
                            <p className={styles.profilename}>{profileData.nickname}</p>

                            <p className={styles.Profession}>{profileData.profession}</p>
                        </div>
                    </div>
                    <div className={styles.profileAboutText}>
                        <p className={styles.About}>{profileData.about}</p>
                    </div>
                </div>
            )}



            {/* there would be a place where we would make sure there are more data avaliable */}
            <p className={styles.ProfileBlogHeader}>Previous Blogs</p>
            {profileBlogData.length > 0 && (
                <div className={styles.profileBlogs}>


                    {profileBlogData.map((blogPost) => (
                        <div key={blogPost.id} className={styles.profileBlog}>
                           
                            <div className={styles.profileBlogData}>
                                <img
                                    src={`data:image/png;base64,${arrayBufferToBase64(blogPost.image.data)}`}
                                    alt="Blog Post Image"
                                    className={styles.profileBlogImgae}
                                />
                                <p className={styles.ProfileBlogHeading}>{blogPost.heading}</p>
                                <div className={styles.static}>
                                    <div className={styles.profileBlogViews}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="27" height="25" viewBox="0 0 27 25" fill="none">
                                            <path d="M13.0761 9.375C12.2141 9.375 11.3874 9.70424 10.7779 10.2903C10.1684 10.8763 9.82593 11.6712 9.82593 12.5C9.82593 13.3288 10.1684 14.1237 10.7779 14.7097C11.3874 15.2958 12.2141 15.625 13.0761 15.625C13.9381 15.625 14.7649 15.2958 15.3744 14.7097C15.9839 14.1237 16.3264 13.3288 16.3264 12.5C16.3264 11.6712 15.9839 10.8763 15.3744 10.2903C14.7649 9.70424 13.9381 9.375 13.0761 9.375ZM13.0761 17.7083C11.6395 17.7083 10.2616 17.1596 9.24573 16.1828C8.22984 15.2061 7.65912 13.8813 7.65912 12.5C7.65912 11.1187 8.22984 9.7939 9.24573 8.81715C10.2616 7.8404 11.6395 7.29167 13.0761 7.29167C14.5128 7.29167 15.8907 7.8404 16.9066 8.81715C17.9224 9.7939 18.4932 11.1187 18.4932 12.5C18.4932 13.8813 17.9224 15.2061 16.9066 16.1828C15.8907 17.1596 14.5128 17.7083 13.0761 17.7083ZM13.0761 4.6875C7.65912 4.6875 3.03298 7.92708 1.15869 12.5C3.03298 17.0729 7.65912 20.3125 13.0761 20.3125C18.4932 20.3125 23.1193 17.0729 24.9936 12.5C23.1193 7.92708 18.4932 4.6875 13.0761 4.6875Z" fill="#699BF7" />
                                        </svg>
                                        <p>{blogPost.views}</p>
                                    </div>
                                    <div className={styles.profileBloglikes}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="27" height="25" viewBox="0 0 27 25" fill="none">
                                            <path d="M18.4932 4.6875C16.218 4.6875 14.2137 5.78125 13.0761 7.5C11.9386 5.78125 9.93427 4.6875 7.65912 4.6875C4.08388 4.6875 1.15869 7.5 1.15869 10.9375C1.15869 17.1354 13.0761 23.4375 13.0761 23.4375C13.0761 23.4375 24.9936 17.1875 24.9936 10.9375C24.9936 7.5 22.0684 4.6875 18.4932 4.6875Z" fill="#F44336" />
                                        </svg>
                                        <p>{blogPost.likes}</p>
                                    </div>

                                </div>
                            </div>
                            {/* You can render other properties as needed */}
                        </div>
                    ))}

                </div>
            )}

        </div>
    );
}

export default Profile;
