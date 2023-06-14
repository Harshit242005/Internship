import React, { useState, useContext } from 'react'
import Navbar from './Navbar'
import "./Create.css"
import { useNavigate } from 'react-router-dom'
import { IdContext } from './IdContext';

import axios from 'axios';


function CreateCard() {

    const navigate = useNavigate();

    const [heading, setHeading] = useState('');
    const [description, setDescription] = useState('');

    const { id } = useContext(IdContext);

    console.log(id);

    const handleInsert = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post(`http://localhost:5000/create/${id}`, {
                heading,
                description,
            });

            if (response.status === 200) {
                navigate('/Cards');
            } else {
                throw new Error('Creation of blogpost is failed');
            }
        }
        catch (error) {
            console.error('Error in blogpost creation', error);
        }

    };
    return (
        <div>
            <Navbar />
            <form onSubmit={handleInsert}>
                <div className='Heading'>
                    <label htmlFor="heading">Heading</label>
                    <input type='text' name="heading" placeholder='Enter the heading of the blog' autoComplete='off' value={heading} onChange={(e) => setHeading(e.target.value)} />
                </div>
                <div className='Description'>
                    <label>Description</label>
                    <textarea htmlFor="description" name="description" value={description} onChange={(e) => setDescription(e.target.value)} placeholder='Enter description fo the blog'></textarea>
                </div>
                <button type='submit' className='Post'>Post</button>
            </form>
        </div>
    )
}

export default CreateCard