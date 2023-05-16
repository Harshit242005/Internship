
import React from 'react'

import { BrowserRouter, Routes, Route } from 'react-router-dom'
import FirstPage from './FirstPage'
import SecondPage from './SecondPage'
import ApiData from './ApiData'
import ChoicesData from './ChoicesData'

function App() {
  

  return (
    <BrowserRouter>
    <Routes>
      <Route path='/' element={<FirstPage />}/>
      <Route path='/second-page' element={<SecondPage />} />
      <Route path='/api-data' element={<ApiData />} />
      <Route path='/choices' element={<ChoicesData />} />
    </Routes>
    </BrowserRouter>
  )
}

export default App
