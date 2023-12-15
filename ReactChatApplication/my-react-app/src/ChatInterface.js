import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useLocation } from 'react-router-dom';
import styles from './ChatInterface.module.css';
function ChatInterface() {
  const [inputText, setInputText] = useState('');
  const handleInputChange = (event) => {
    setInputText(event.target.value);
  };
  const handleSendClick = () => {
    // Create the JSON object with the required data
    const messageData = {
      type: 'userMessage',
      UserFrom: userMe,
      UserTo: selectedUser,
      Message: inputText,
      Time: new Date().toISOString(), // Use current timestamp
    };
    // Send the JSON object to the server using the WebSocket
    socket.send(JSON.stringify(messageData));
    // Clear the input text after sending the message
    setInputText('');
  };
  const [userData, setUserData] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [fetchedData, setFetchedData] = useState([]);
  const [userStatusMap, setUserStatusMap] = useState(new Map());
  const fetchDataForUser = async (username) => {
    try {
      // Prepare the data to send in the request
      const requestData = {
        userFrom: username,
        userTo: userMe,
      };
      console.log(requestData.userFrom, requestData.userTo);
      console.log(`request data for getting chat messages: ${requestData}`);
      // Make a POST request to your backend endpoint
      const response = await axios.post('http://localhost:5000/fetchUserChat', requestData);
      // Assuming the response data is a JSON object
      const responseData = response.data;
      console.log(`fetched response data as the chat is: ${responseData}`);
      // Update state with the new data
      // Update state with the new data
      setFetchedData(responseData);

    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };
  // useEffect to log the fetchedData whenever it changes
  useEffect(() => {
    console.log('Fetched Data:', fetchedData);
  }, [fetchedData]);
  const location = useLocation();
  const { state } = location;
  // Destructure email with a default value
  const { email = '', userMe } = state || {};
  useEffect(() => {
    // Access email and username from the state
    console.log('Email:', email);
    console.log('Username:', userMe);
  }, [email, userMe]);
  // Establish WebSocket connection
  const socket = new WebSocket('ws://localhost:5000');
  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await axios.get('http://localhost:5000/GetUsers', {
          params: { email: email },
        });
        const users = response.data;
        setUserData(users);
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };
    fetchUserData();
  }, [email]);
  const handleUserClick = (username) => {
    setSelectedUser(username);
    fetchDataForUser(username);
    console.log(`selected user is ${username} and fecting messages between ${userMe} and ${username}`);
  };
  // Handle WebSocket events
  useEffect(() => {
    // Event listener for when the WebSocket connection is opened
    const handleWebSocketOpen = () => {
      console.log('WebSocket connection opened:', socket);
      const myInfoMessage = {
        type: 'myInfo',
        myUserName: userMe
      }
      socket.send(JSON.stringify(myInfoMessage));

      // Send user information to the server when the connection is opened
      if (selectedUser) {
        const messageData = {
          type: 'userInfo',
          username: selectedUser,
        };
        socket.send(JSON.stringify(messageData));
      }
    };
    // Event listener for incoming messages
    const handleWebSocketMessage = (event) => {
      const message = JSON.parse(event.data);
      console.log('Message received:', message);
      // Handle different message types
      switch (message.type) {
        case 'acknowledgment':
          console.log('Server acknowledgment:', message);
          break;
        case 'userMessage':
          // Handle userMessage type
          handleUserMessage(message);
          break;
        case 'userStatus':
          // Handle userStatus type
          handleUserStatus(message);
          break;
        // Handle other message types as needed
        default:
          break;
      }
    };
    // Attach event listeners
    socket.addEventListener('open', handleWebSocketOpen);
    socket.addEventListener('message', handleWebSocketMessage);

    // Cleanup the WebSocket connection on component unmount
    return () => {
      socket.close();
    };
  }, [selectedUser]);

  function handleUserStatus(message) {
    const { username, status } = message;

    // Update the user's status in the userStatusMap
    setUserStatusMap((prevMap) => {
      const newMap = new Map(prevMap);
      newMap.set(username, status);
      console.log(`User Status Map ${newMap}`);
      return newMap;
    });
  }


  function handleUserMessage(messageData) {
    // Extract data from the userMessage
    const { UserFrom, UserTo, Message, Time } = messageData;

    // Display the message in the ChatMessages div only if it's intended for the currently selected user
    if ((UserTo === userMe && UserFrom === selectedUser) || (UserTo === selectedUser && UserFrom === userMe)) {
      const chatMessagesDiv = document.querySelector(`.${styles.ChatMessages}`);
      const messageElement = document.createElement('div');

      // Determine the CSS class based on the condition
      const cssClass = UserFrom === userMe ? styles.chatMe : styles.chatOther;

      // Apply the CSS class to the main div element
      messageElement.className = cssClass;

      // Add the inner HTML content to the div
      messageElement.innerHTML = `
      <p>${Message}</p>
      <p><em>${new Date(Time).toLocaleString()}</em></p>
    `;

      // Append the main div element to the ChatMessages div
      chatMessagesDiv.appendChild(messageElement);

      // Optionally, you can scroll to the bottom to show the latest message
      chatMessagesDiv.scrollTop = chatMessagesDiv.scrollHeight;
    }
  }

  const renderMessages = () => {
    if (!Array.isArray(fetchedData) || fetchedData.length === 0) {
      return <p>No messages available.</p>;
    }

    return fetchedData.map((message) => (
      <div key={message._id} className={message.UserFrom === userMe ? styles.chatMe : styles.chatOther}>
        {/* <p>{`From: ${message.UserFrom}`}</p> */}
        <p>{`Message: ${message.Message}`}</p>
        <p>{`Time: ${new Date(message.Time).toLocaleString()}`}</p>
        {/* Add more details as needed */}
      </div>
    ));
  };

  const [searchText, setSearchText] = useState('');
  const [filteredUserData, setFilteredUserData] = useState([]);

  const handleSearchInputChange = async (e) => {
    const text = e.target.value;
    setSearchText(text);

    // Make a fetch (axios) request to your server
    try {
      console.log(searchText);
      const response = await axios.get('http://localhost:5000/GetSpecificUsers', { params: { Search: searchText }, });

      // Assuming your server sends back an array of user data
      console.log(response.data);
      setFilteredUserData(response.data);
    } catch (error) {
      console.error('Error fetching user data:', error);
    }
  };

  const usersToDisplay = searchText.trim() !== '' ? filteredUserData : userData;

  return (
    <div className={styles.ChatInterface}>
      <div className={styles.Users}>
        <input
          className={styles.searchUser}
          type='text'
          placeholder='Type username...'
          value={searchText}
          onChange={handleSearchInputChange}
        />
        {usersToDisplay.map((user) => (
          <div
            key={user.Username}
            className={`${styles.User} ${selectedUser === user.Username ? styles.SelectedUser : ''
              } ${userStatusMap.get(user.Username) === 'Online' && selectedUser === user.Username ? styles.UserOnline : ''}`}
            onClick={() => handleUserClick(user.Username)}
          >
            {user.Image && (
              <img
                src={`data:image/jpeg;base64,${user.Image.toString('base64')}`}
                className={styles.UserImage}
                alt="user"
              />
            )}
            <p>{user.Username}</p>
          </div>
        ))}

      </div>
      <div className={styles.Chat}>
        <div className={styles.ChatMessages}>
          {renderMessages()}
        </div>
        <div className={styles.ChatInputs}>
          <input type="text" className={styles.ChatInput} value={inputText}
            onChange={handleInputChange} placeholder="type message..." />
          <button onClick={handleSendClick} className={styles.ChatMessageSendButton}>
            <svg xmlns="http://www.w3.org/2000/svg" height="50" width="50" viewBox="0 0 512 512"><path fill="#00ffe1" d="M440 6.5L24 246.4c-34.4 19.9-31.1 70.8 5.7 85.9L144 379.6V464c0 46.4 59.2 65.5 86.6 28.6l43.8-59.1 111.9 46.2c5.9 2.4 12.1 3.6 18.3 3.6 8.2 0 16.3-2.1 23.6-6.2 12.8-7.2 21.6-20 23.9-34.5l59.4-387.2c6.1-40.1-36.9-68.8-71.5-48.9zM192 464v-64.6l36.6 15.1L192 464zm212.6-28.7l-153.8-63.5L391 169.5c10.7-15.5-9.5-33.5-23.7-21.2L155.8 332.6 48 288 464 48l-59.4 387.3z" /></svg>
          </button>
        </div>
      </div>
    </div>
  );
}
export default ChatInterface;