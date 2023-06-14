import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import Navbar from './Navbar';
import "./Change.css";

function ChangeProfile() {
  const [imageSrc, setImageSrc] = useState('');
  const { id } = useParams();
  const [userData, setUserData] = useState(null);
  const [blogPosts, setBlogPosts] = useState([]);
  const navigate = useNavigate();
  useEffect(() => {
    const fetchBlogPosts = async () => {
      try {
        const response = await axios.get(`http://localhost:5000/blogposts/${id}`);
        setBlogPosts(response.data);
      } catch (error) {
        console.error('Error fetching blog posts:', error);
      }
    };

    fetchBlogPosts();
  }, [id]);

  const handleDelete = async (number) => {
    try {
      await axios.delete(`http://localhost:5000/blogposts/${number}`);
      // Refresh the blog posts after successful deletion

      navigate(`/ChangeProfile/${id}`);
    } catch (error) {
      console.error('Error deleting blog post:', error);
    }
  };


  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(`http://localhost:5000/User/${id}`);
        setUserData(response.data);
      } catch (error) {
        console.error('Error fetching user data:', error);
      }


    };

    fetchData();
  }, [id]);

  useEffect(() => {
    const fetchImage = async () => {
      try {
        const response = await axios.get(`http://localhost:5000/getImage/${id}`, {
          responseType: 'arraybuffer',
        });
        const imageBlob = new Blob([response.data], { type: 'image/jpeg' });
        const imageUrl = URL.createObjectURL(imageBlob);
        setImageSrc(imageUrl);
      } catch (error) {
        console.error('Error fetching user image:', error);
      }
    };

    fetchImage();
  }, [id]);

  return (
    <div>
      <Navbar />
      <div className='userData'>
        <div className='userData-image'>
          <img src={imageSrc} alt="profile picture" />
        </div>
        <div className='userData-text'>
          {userData && userData.length > 0 ? (
            <>
              <p>Email: {userData[0].Email}</p>
              <p>Nickname: {userData[0].Nickname}</p>
            </>
          ) : (
            <p>Loading user data...</p>
          )}
        </div>

        <div className="blogPosts">

          <div className="scrollableDiv">
            {blogPosts.map((post, index) => (
              <div className='blog' key={index}>
                <h3>{post.Heading}</h3>
                <p>{post.Description}</p>
                <p>Likes: {post.Likes}</p>

                <a>
                  <button onClick={() => handleDelete(post.Number)}>Delete</button>
                </a>
              </div>
            ))}
          </div>

        </div>
      </div>

    </div>
  );
}

export default ChangeProfile;
