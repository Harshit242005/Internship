
import React, { useEffect, useState, useContext } from 'react';
import Navbar from './Navbar';
import axios from 'axios';
import "./Saved.css";
import { IdContext } from './IdContext';

function Saved() {
  const [savedData, setSavedData] = useState([]);
  const { id } = useContext(IdContext);
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('http://localhost:5000/savedCards', { params: { id } });
        setSavedData(response.data);
      } catch (error) {
        console.error('Error fetching saved data:', error);
      }
    };

    fetchData();
  }, []);

  const deleteCard = (cardNumber) => {
    axios.post('http://localhost:5000/delete', { cardNumber })
      .then(response => {
        if (response.status === 200) {
          // Delete the card from the UI
          setSavedData(savedData.filter(card => card.Number !== cardNumber));
          console.log('Number deleted successfully');
        } else {
          console.error('Failed to delete number');
        }
      })
      .catch(error => {
        console.error('Error deleting number:', error);
      });
  };

  return (
    <div>
      <Navbar />
      <h1>Saved Cards</h1>
      <div className='cards'>
        {savedData.map(card => (
          <div className='card-data' key={card.Number}>
            <h3>{card.Heading}</h3>
            <p>{card.Description}</p>
            <button className='delete-button' onClick={() => deleteCard(card.Number)}>Remove</button>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Saved;
