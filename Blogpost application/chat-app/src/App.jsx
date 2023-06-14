
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Home from './Home';
import SignUp from './Signup';
import Login from './Login';
import Cards from './Cards';
import Saved from './Saved';
import CreateCard from './CreateCard';
import { IdContext } from './IdContext';
import Profile from './Profile';
import ChangeProfile from './ChangeProfile';
import VerifyCode from './VerifyCode';


const App = () => {
  const [id, setId] = useState(null);
  return (
    <Router>
      <IdContext.Provider value={{ id, setId}}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/signup" element={<SignUp />} />
        <Route path="/login" element={<Login />} />
        <Route path="/Cards" element={<Cards />} />
        <Route path="/Create" element={<CreateCard />} />
        <Route path="/Saved" element={<Saved />}/>
        <Route path="/Profile/:id" element={<Profile />}/>
        <Route path="/ChangeProfile/:id" element={<ChangeProfile />} />
        <Route path="/Verify" element={<VerifyCode />} />
      </Routes>
      </IdContext.Provider>
    </Router>
  );
};

export default App;
