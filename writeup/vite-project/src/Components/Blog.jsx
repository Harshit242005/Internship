import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import axios from 'axios';
import styles from './Styles/Blog.module.css';


function Blog() {
  const { number, nickname } = useParams();
  const [blogData, setBlogData] = useState({});
  const [extraData, setExtraData] = useState([]);
  // this would help us as a hook 
  const [checkLike, setCheckLikeStatus] = useState(false);
  const [checkBookmark, setCheckBookmarkStatus] = useState(false);
  // to get the status of the liked 
  const checkLikeStatus = () => {
    // Make an Axios GET request to fetch the like status
    axios
      .get(`http://localhost:3000/blog/likeStatus/${number}/${nickname}`)
      .then((response) => {

        setCheckLikeStatus(response.data.isLikedStatus); // Update the like status based on the response
       
      })
      .catch((error) => {
        console.error('Error fetching like status:', error);
      });
  };

  const checkBookmarkStatus = () => {
    // Make an Axios GET request to fetch the like status
    axios
      .get(`http://localhost:3000/blog/bookmarkStatus/${number}/${nickname}`)
      .then((response) => {

        setCheckBookmarkStatus(response.data.isBookmarkStatus); // Update the like status based on the response
       
      })
      .catch((error) => {
        console.error('Error fetching like status:', error);
      });
  };

  // Combine the two useEffect hooks into one
  useEffect(() => {
    // Fetch the blog data
    axios.get(`http://localhost:3000/blog/${number}`)
      .then((response) => {
        setBlogData(response.data);
        
      })
      .catch((error) => {
        console.error('Error fetching blog data:', error);
      });

    // Fetch the extra data
    axios.get(`http://localhost:3000/ExtraBlog/${nickname}`)
      .then((response) => {
        setExtraData(response.data);
      })
      .catch((error) => {
        console.error('Error fetching extra data:', error);
      });
    checkLikeStatus();
    checkBookmarkStatus();
  }, [number, nickname]);


  // Function to convert buffer to data URL
  function bufferToDataURL(buffer) {
    const arrayBufferView = new Uint8Array(buffer.data);
    const blob = new Blob([arrayBufferView], { type: 'image/jpeg' });
    const urlCreator = window.URL || window.webkitURL;
    return urlCreator.createObjectURL(blob);
  }
  console.log(extraData.image);
  // Check if there's image data before rendering the image
  const imageSrc = blogData.image ? bufferToDataURL(blogData.image) : '';

  // states for changing the like and bookmarked
  const [isLiked, setIsLiked] = useState(false);
  const [isBookmarked, setIsBookmarked] = useState(false);
  
  

  const toggleLike = () => {
    // Check if the user has already liked the blog post
    if (checkLike) {
      // Send a POST request to decrease likes
      axios.post(`http://localhost:3000/blog/like/decrease/${number}/${nickname}`)
        .then((response) => {
          // Handle the response if needed
          console.log('Decrease Like Response:', response);
          // Set isLiked to false since the user unliked the blog post
          setIsLiked(false);
          checkLike(false);
        })
        .catch((error) => {
          // Handle errors if needed
          console.error('Error decreasing like:', error);
        });
    } else {
      // Send a POST request to increase likes
      axios.post(`http://localhost:3000/blog/like/increase/${number}/${nickname}`)
        .then((response) => {
          // Handle the response if needed
          console.log('Increase Like Response:', response);
          // Set isLiked to true since the user liked the blog post
          setIsLiked(true);
          checkLike(true);
        })
        .catch((error) => {
          // Handle errors if needed
          console.error('Error increasing like:', error);
        });
    }
  };


  const toggleBookmark = () => {
    // queries to change values in the databases
    if (isBookmarked) {
      // Send a POST request to remove the bookmark
      axios.post(`http://localhost:3000/blog/bookmark/remove/${number}/${nickname}`)
        .then((response) => {
          // Handle the response if needed
          console.log('Remove Bookmark Response:', response);
        })
        .catch((error) => {
          // Handle errors if needed
          console.error('Error removing bookmark:', error);
        });
    } else {
      // Send a POST request to add the bookmark
      axios.post(`http://localhost:3000/blog/bookmark/add/${number}/${nickname}`)
        .then((response) => {
          // Handle the response if needed
          console.log('Add Bookmark Response:', response);
        })
        .catch((error) => {
          // Handle errors if needed
          console.error('Error adding bookmark:', error);
        });
    }
    setIsBookmarked((prevState) => !prevState);
  };


  return (
    <div>
      {/* here there would be a back button */}
      <Link to={`/Home/${nickname}`}>,<button className={styles.BackButton}>Back</button></Link>
      <div className={styles.blogData}>
        <div className={styles.image}>
          {/* Render the image */}
          {imageSrc && <img src={imageSrc} alt="Blog" className={styles.BlogImage} />}
        </div>
        <div className={styles.textData}>
          <p className={styles.heading1}>{blogData.heading}</p>
          <p>{blogData.content}</p>
        </div>
        <div className={styles.LikeAndBookmarked}>
          {checkLike ? (
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="25"
              height="25"
              viewBox="0 0 25 25"
              fill="#F44336"
              onClick={toggleLike}
            >
              <path d="M17.7083 4.6875C15.5208 4.6875 13.5938 5.78125 12.5 7.5C11.4063 5.78125 9.47917 4.6875 7.29167 4.6875C3.85417 4.6875 1.04167 7.5 1.04167 10.9375C1.04167 17.1354 12.5 23.4375 12.5 23.4375C12.5 23.4375 23.9583 17.1875 23.9583 10.9375C23.9583 7.5 21.1458 4.6875 17.7083 4.6875Z" fill="#F44336" />            </svg>
          ) : (
            <i className="fa fa-heart-o like" id="like" onClick={toggleLike}></i>
          )}

          {checkBookmark ? (
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="25"
              height="25"
              viewBox="0 0 25 25"
              fill="#4ECB71"
              onClick={toggleBookmark}
            >
              <path d="M5.20834 21.875V5.20833C5.20834 4.63542 5.41251 4.14479 5.82084 3.73646C6.22918 3.32813 6.71945 3.12431 7.29168 3.125H17.7083C18.2813 3.125 18.7719 3.32917 19.1802 3.7375C19.5886 4.14584 19.7924 4.63611 19.7917 5.20833V21.875L12.5 18.75L5.20834 21.875Z" fill="#4ECB71" />
            </svg>
          ) : (
            <i className="fa fa-bookmark-o bookmark" id="bookmark" onClick={toggleBookmark}></i>
          )}
        </div>

      </div>


      {/* more data from that same person */}
      <div className={styles.more}>
        <p className={styles.moreText}>More from {nickname}</p>
        <div className={styles.blogs}>
          {extraData.map((extraBlog) => (
            <Link className={styles.blogLink} to={`/Blog/${extraBlog.number}/${nickname}`}>
              <div className='ExtraBlogData' key={extraBlog.id}>
                {/* Render the image for each extraBlog */}
                {imageSrc && <img src={bufferToDataURL(extraBlog.image)} alt="Blog" className={styles.extraBlogImage} />}

                <p className={styles.ExtraBlogDataHeading}>{extraBlog.heading}</p>

              </div>
            </Link>
          ))}
        </div>
      </div>

    </div>
  );
}

export default Blog;
