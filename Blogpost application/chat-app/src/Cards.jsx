


import React, { useState, useEffect, useContext } from 'react';
import './Cards.css';
import Navbar from './Navbar';
import axios from 'axios';
import { IdContext } from './IdContext';

function Cards() {
  const [data, setData] = useState([]);
  const { id } = useContext(IdContext);
  const [likedCards, setLikedCards] = useState([]);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await axios.get('http://localhost:5000/cards');
      setData(response.data);
      response.data.forEach(item => {
        checkLikeStatus(item.Number); // Call checkLikeStatus for each item
      });
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };


  const handleBookmarkClick = (event, number) => {
    const bookmarkedIcon = event.target;
    if (!bookmarkedIcon.classList.contains('bookmarked')) {
      bookmarkedIcon.classList.add('bookmarked');
    }

    axios
      .post('http://localhost:5000/bookmark', { number, id })
      .then(response => {
        console.log(response.data.message);
      })
      .catch(error => {
        console.error('Error:', error.message);
      });
  };

  const handleHeartClick = (number) => {
    if (!likedCards.includes(number)) {
      axios
        .post('http://localhost:5000/like', { number, id })
        .then(response => {
          console.log(response.data.message);
          setLikedCards(prevLikedCards => [...prevLikedCards, number]);
        })
        .catch(error => {
          console.error('Error:', error.message);
        });
    }
  };

  const checkLikeStatus = (number) => {
    axios
      .get('http://localhost:5000/checkLike', {
        params: {
          number: number,
          userId: id
        }
      })
      .then(response => {
        const likeId = response.data.likeId;
        if (likeId === 1) {
          setLikedCards(prevLikedCards => [...prevLikedCards, number]);
        }
      })
      .catch(error => {
        console.error('Error:', error.message);
      });
  };

  const isBookmarked = (number) => {
    // Add your logic to determine if the card is bookmarked
    // Return true or false accordingly
  };

  const isLiked = (number) => {
    return likedCards.includes(number);
  };

  return (
    <div>
      <Navbar />
      <div className='Cards-all'>

        {data.map(item => (
          <div className='card' key={item.Number}>
            <div className='card-data-all'>
              <div className='card-data'>
                <h2>{item.Heading}</h2>
                <p>{item.Description}</p>
              </div>
              <div className='like'>
                <i
                  className={`fa fa-heart-o ${isLiked(item.Number) ? 'liked' : ''}`}
                  onClick={() => handleHeartClick(item.Number)}
                  disabled={isLiked(item.Number)}
                ></i>
                <i
                  className={`fa fa-bookmark-o ${isBookmarked(item.Number) ? 'bookmarked' : ''}`}
                  style={{ color: isBookmarked(item.Number) ? 'green' : '' }}
                  onClick={(event) => handleBookmarkClick(event, item.Number)}
                ></i>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Cards;

